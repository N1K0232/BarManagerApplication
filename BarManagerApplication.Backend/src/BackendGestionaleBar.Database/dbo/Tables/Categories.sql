CREATE TABLE [dbo].[Categories]
(
	[IdCategory] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(256) NOT NULL,
	[Description] NVARCHAR(512),

	PRIMARY KEY(IdCategory)
)