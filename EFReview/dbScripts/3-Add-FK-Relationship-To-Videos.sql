/*
   Tuesday, June 26, 20183:01:51 PM
   User: 
   Server: (localdb)\ProjectsV13
   Database: Vidzy
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Genres SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Videos
	(
	Id int NOT NULL IDENTITY (1, 1),
	Name varchar(255) NOT NULL,
	ReleaseDate datetime NOT NULL,
	GenreId tinyint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Videos SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Videos ON
GO
IF EXISTS(SELECT * FROM dbo.Videos)
	 EXEC('INSERT INTO dbo.Tmp_Videos (Id, Name, ReleaseDate, GenreId)
		SELECT Id, Name, ReleaseDate, GenreId FROM dbo.Videos WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Videos OFF
GO
ALTER TABLE dbo.VideoGenres
	DROP CONSTRAINT FK_VideoGenres_Videos
GO
DROP TABLE dbo.Videos
GO
EXECUTE sp_rename N'dbo.Tmp_Videos', N'Videos', 'OBJECT' 
GO
ALTER TABLE dbo.Videos ADD CONSTRAINT
	PK_Videos PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Videos ADD CONSTRAINT
	FK_Videos_Genres FOREIGN KEY
	(
	GenreId
	) REFERENCES dbo.Genres
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.VideoGenres SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
