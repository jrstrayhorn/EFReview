USE [Vidzy]
GO

/****** Object:  StoredProcedure [dbo].[spAddVideo]    Script Date: 6/26/2018 3:04:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spAddVideo]
(
	@Name varchar(255),
	@ReleaseDate datetime,
	@Genre varchar(255)
)
AS

	DECLARE @GenreId tinyint
	SET @GenreId = (SELECT Id FROM Genres WHERE Name = @Genre)

	INSERT INTO Videos (Name, ReleaseDate, GenreId)
	VALUES (@Name, @ReleaseDate, @GenreId)

GO


