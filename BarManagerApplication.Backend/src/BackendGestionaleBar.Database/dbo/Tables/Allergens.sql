CREATE TABLE [dbo].[Allergens]
(
	[IdAllergen] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[Description] NVARCHAR(512),

	PRIMARY KEY(IdAllergen)
)