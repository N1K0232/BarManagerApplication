CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [LoginProvider] NVARCHAR (128)   NOT NULL,
    [Name]          NVARCHAR (128)   NOT NULL,
    [Value]         NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC)
);

