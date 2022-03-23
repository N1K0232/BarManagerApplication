CREATE TABLE [dbo].[Products] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [IdCategory]     UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (256)   NOT NULL,
    [Price]          DECIMAL (10, 2)  NOT NULL,
    [ExpirationDate] DATE             NOT NULL,
    [Quantity]       INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdCategory]) REFERENCES [dbo].[Categories] ([Id])
);

