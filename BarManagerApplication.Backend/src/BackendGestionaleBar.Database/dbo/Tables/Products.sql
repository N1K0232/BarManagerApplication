CREATE TABLE [dbo].[Products] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [CategoryId]     UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (256)   NOT NULL,
    [Price]          DECIMAL (10, 2)  NOT NULL,
    [ExpirationDate] DATE             NOT NULL,
    [Quantity]       INT              NOT NULL,
    [CreatedDate]    DATETIME         NOT NULL,
    [LastModifiedDate] DATETIME       NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
);

