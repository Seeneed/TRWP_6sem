using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("CelebritiesDb")
    ?? throw new InvalidOperationException("Connection string 'CelebritiesDb' is not set.");

builder.Services.AddScoped(_ => new SqlConnection(connectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
