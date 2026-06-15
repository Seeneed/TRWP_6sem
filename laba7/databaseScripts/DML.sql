USE [Celebrities];
GO

INSERT INTO [dbo].[Celebrities] ([Fullname], [Nationality], [ReqPhotoPath])
VALUES
    (N'Leonardo DiCaprio', N'US', N'/photos/dicaprio.jpg'),
    (N'Marion Cotillard', N'FR', NULL),
    (N'Brad Pitt', N'US', N'/photos/pitt.jpg');
GO

SELECT * FROM [dbo].[Celebrities];
GO

UPDATE [dbo].[Celebrities]
SET [ReqPhotoPath] = N'/photos/updated.jpg'
WHERE [Fullname] = N'Brad Pitt';
GO

SELECT * FROM [dbo].[Celebrities] WHERE [Fullname] = N'Brad Pitt';
GO

DELETE FROM [dbo].[Celebrities]
WHERE [Fullname] = N'Marion Cotillard';
GO

SELECT * FROM [dbo].[Celebrities];
GO