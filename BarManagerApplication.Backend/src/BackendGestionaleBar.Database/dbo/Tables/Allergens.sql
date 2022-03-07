CREATE TABLE [dbo].[Allergens]
(
	[IdAllergen] INTEGER NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL,
	[Description] NVARCHAR(512),

	PRIMARY KEY(IdAllergen)
)