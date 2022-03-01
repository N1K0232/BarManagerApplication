CREATE TABLE [dbo].[Clienti] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Nome]          NVARCHAR (MAX)   NOT NULL,
    [Cognome]       NVARCHAR (MAX)   NOT NULL,
    [DataNascita]   DATE             NOT NULL,
    [CodiceFiscale] NVARCHAR (50)    NOT NULL,
    [Telefono]      NVARCHAR (10)    NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([CodiceFiscale] ASC),
    UNIQUE NONCLUSTERED ([Telefono] ASC)
);

