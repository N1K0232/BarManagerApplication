CREATE TABLE [dbo].[Credenziali] (
    [IdUtente]           UNIQUEIDENTIFIER NOT NULL,
    [Email]              NVARCHAR (150)   NOT NULL,
    [Password]           NVARCHAR (150)   NOT NULL,
    [Role]               NVARCHAR (10)    NOT NULL,
    [DataRegistrazione]  DATE             NOT NULL,
    [DataCambioPassword] DATE             NULL,
    [DataUltimoAccesso]  DATE             NULL,
    PRIMARY KEY CLUSTERED ([Email] ASC),
    FOREIGN KEY ([IdUtente]) REFERENCES [dbo].[Clienti] ([Id])
);

