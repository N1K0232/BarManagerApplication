CREATE TABLE [dbo].[Categories] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (256)   NOT NULL,
    [Description] NVARCHAR (512)   NULL,
    [CreatedDate] DATETIME NOT NULL,
	[LastModifiedDate] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

