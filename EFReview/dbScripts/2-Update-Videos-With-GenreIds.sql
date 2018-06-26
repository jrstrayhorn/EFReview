USE [Vidzy]
GO

UPDATE Videos
SET GenreId = (select Id from Genres where Name = 'Comedy')
WHERE Name = 'Wedding Crashers'

UPDATE Videos
SET GenreId = (select Id from Genres where Name = 'Action')
WHERE Name = 'Black Panther'