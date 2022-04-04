CREATE TABLE [dbo].[OrderDetails]
(
	[IdOrder] UNIQUEIDENTIFIER NOT NULL,
	[IdProduct] UNIQUEIDENTIFIER NOT NULL,
	[CreatedDate] DATE NOT NULL,
	[LastModifiedDate] DATE NULL,
	
	PRIMARY KEY(IdOrder,IdProduct),
	FOREIGN KEY(IdOrder) REFERENCES Orders(Id),
	FOREIGN KEY(IdProduct) REFERENCES Products(Id)
)