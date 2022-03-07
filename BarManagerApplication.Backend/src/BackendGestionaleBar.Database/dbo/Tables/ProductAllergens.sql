CREATE TABLE [dbo].[ProductAllergens]
(
	[IdAllergen] INTEGER NOT NULL,
	[IdProduct] UNIQUEIDENTIFIER NOT NULL,

    PRIMARY KEY(IdAllergen,IdProduct),
	FOREIGN KEY(IdAllergen) REFERENCES Allergens(IdAllergen),
	FOREIGN KEY(IdProduct) REFERENCES Products(IdProduct)
)