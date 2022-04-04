CREATE TABLE [dbo].[Categories] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (256)   NOT NULL,
    [Description] NVARCHAR (512)   NOT NULL,
    [CreatedDate] DATE NOT NULL,
	[LastModifiedDate] DATE NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

