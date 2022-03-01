CREATE TABLE [dbo].[Prodotti] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [IdCategoria]         INT              NOT NULL,
    [Nome]                NVARCHAR (MAX)   NOT NULL,
    [QuantitaDisponibile] INT              NOT NULL,
    [Prezzo]              NUMERIC (7, 2)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdCategoria]) REFERENCES [dbo].[Categorie] ([Id])
);

