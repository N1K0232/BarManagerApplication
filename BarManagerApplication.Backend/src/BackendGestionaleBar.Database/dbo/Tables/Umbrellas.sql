CREATE TABLE [dbo].[Umbrellas]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Coordinates] NVARCHAR(10) NOT NULL,
	[CreatedDate]       DATETIME         NOT NULL,
    [LastModifiedDate]  DATETIME         NULL,
    [IsDeleted]         BIT              NOT NULL,
    [DeletedDate]       DATETIME         NULL,

	PRIMARY KEY(Id)
)