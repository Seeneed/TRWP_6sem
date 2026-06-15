using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TDWA06_01.Models;

namespace TDWA06_01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CelebritiesController : ControllerBase
{
    private readonly SqlConnection _db;

    public CelebritiesController(SqlConnection db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Celebrity>>> GetAll()
    {
        await OpenAsync();
        var list = new List<Celebrity>();

        await using var cmd = new SqlCommand(
            "SELECT Id, Fullname, Nationality, ReqPhotoPath FROM dbo.Celebrities ORDER BY Id", _db);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(ReadCelebrity(reader));
        }

        return list;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Celebrity>> GetById(int id)
    {
        await OpenAsync();

        await using var cmd = new SqlCommand(
            "SELECT Id, Fullname, Nationality, ReqPhotoPath FROM dbo.Celebrities WHERE Id = @id", _db);
        cmd.Parameters.AddWithValue("@id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return NotFound();
        }

        return ReadCelebrity(reader);
    }

    [HttpPost]
    public async Task<ActionResult<Celebrity>> Create([FromBody] Celebrity celebrity)
    {
        await OpenAsync();

        await using var cmd = new SqlCommand(
            @"INSERT INTO dbo.Celebrities (Fullname, Nationality, ReqPhotoPath)
              VALUES (@fullname, @nationality, @photo);
              SELECT CAST(SCOPE_IDENTITY() AS int);", _db);
        cmd.Parameters.AddWithValue("@fullname", celebrity.Fullname);
        cmd.Parameters.AddWithValue("@nationality", celebrity.Nationality);
        cmd.Parameters.AddWithValue("@photo", (object?)celebrity.ReqPhotoPath ?? DBNull.Value);

        var newId = (int)(await cmd.ExecuteScalarAsync() ?? 0);
        celebrity.Id = newId;

        return CreatedAtAction(nameof(GetById), new { id = newId }, celebrity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Celebrity celebrity)
    {
        await OpenAsync();

        await using var cmd = new SqlCommand(
            @"UPDATE dbo.Celebrities
              SET Fullname = @fullname, Nationality = @nationality, ReqPhotoPath = @photo
              WHERE Id = @id", _db);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@fullname", celebrity.Fullname);
        cmd.Parameters.AddWithValue("@nationality", celebrity.Nationality);
        cmd.Parameters.AddWithValue("@photo", (object?)celebrity.ReqPhotoPath ?? DBNull.Value);

        var rows = await cmd.ExecuteNonQueryAsync();
        return rows == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await OpenAsync();

        await using var cmd = new SqlCommand("DELETE FROM dbo.Celebrities WHERE Id = @id", _db);
        cmd.Parameters.AddWithValue("@id", id);

        var rows = await cmd.ExecuteNonQueryAsync();
        return rows == 0 ? NotFound() : NoContent();
    }

    private async Task OpenAsync()
    {
        if (_db.State != System.Data.ConnectionState.Open)
        {
            await _db.OpenAsync();
        }
    }

    private static Celebrity ReadCelebrity(SqlDataReader reader) => new()
    {
        Id = reader.GetInt32(0),
        Fullname = reader.GetString(1),
        Nationality = reader.GetString(2),
        ReqPhotoPath = reader.IsDBNull(3) ? null : reader.GetString(3)
    };
}
