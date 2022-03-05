CREATE TABLE [dbo].[Clienti] (
    [IdCliente]            UNIQUEIDENTIFIER NOT NULL,
    [Nome]          NVARCHAR (MAX)   NOT NULL,
    [Cognome]       NVARCHAR (MAX)   NOT NULL,
    [DataNascita]   DATE             NOT NULL,
    [CodiceFiscale] NVARCHAR (50)    NOT NULL,
    [Telefono]      NVARCHAR (10)    NOT NULL,
    PRIMARY KEY CLUSTERED ([IdCliente] ASC),
    UNIQUE NONCLUSTERED ([CodiceFiscale] ASC),
    UNIQUE NONCLUSTERED ([Telefono] ASC)
);

