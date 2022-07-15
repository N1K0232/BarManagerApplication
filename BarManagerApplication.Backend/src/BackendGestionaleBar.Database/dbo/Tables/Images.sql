CREATE TABLE [dbo].[Images]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT newid(),
	[Path] NVARCHAR(256) NOT NULL,
	[Length] BIGINT NOT NULL,
	[Description] NVARCHAR(512) NULL,
	[CreatedDate] DATETIME NOT NULL,
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL,
    [LastModifiedDate] DATETIME NULL,
    [UpdatedBy] UNIQUEIDENTIFIER NULL,

	PRIMARY KEY(Id),
	FOREIGN KEY(CreatedBy) REFERENCES [dbo].[AspNetUsers](Id),
    FOREIGN KEY(UpdatedBy) REFERENCES [dbo].[AspNetUsers](Id)
)