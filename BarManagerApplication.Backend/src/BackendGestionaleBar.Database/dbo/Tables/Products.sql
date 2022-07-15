CREATE TABLE [dbo].[Products] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [CategoryId]        UNIQUEIDENTIFIER NOT NULL,
    [Name]              NVARCHAR (256)   NOT NULL,
    [Price]             DECIMAL (10, 2)  NOT NULL,
    [ExpirationDate]    DATE             NOT NULL,
    [Quantity]          INT              NOT NULL,
    [CreatedDate]       DATETIME         NOT NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NOT NULL,
    [LastModifiedDate]  DATETIME         NULL,
    [UpdatedBy]         UNIQUEIDENTIFIER NULL,
    [IsDeleted]         BIT              NOT NULL,
    [DeletedDate]       DATETIME         NULL,
    [DeletedBy]         UNIQUEIDENTIFIER NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers] ([Id])
);