CREATE TABLE [dbo].[Umbrellas]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Coordinates] NVARCHAR(10) NOT NULL,
	[CreatedDate]       DATETIME         NOT NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NOT NULL,
    [LastModifiedDate]  DATETIME         NULL,
    [UpdatedBy]         UNIQUEIDENTIFIER NULL,
    [IsDeleted]         BIT              NOT NULL,
    [DeletedDate]       DATETIME         NULL,
    [DeletedBy]         UNIQUEIDENTIFIER NULL,

	PRIMARY KEY(Id),
    FOREIGN KEY(CreatedBy) REFERENCES AspNetUsers(Id),
    FOREIGN KEY(UpdatedBy) REFERENCES AspNetUsers(Id),
    FOREIGN KEY(DeletedBy) REFERENCES AspNetUsers(Id)
)