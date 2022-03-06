CREATE TABLE [dbo].[Categories]
(
	[IdCategory] INTEGER NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(256) NOT NULL,
	[Description] NVARCHAR(512),

	PRIMARY KEY(IdCategory)
)