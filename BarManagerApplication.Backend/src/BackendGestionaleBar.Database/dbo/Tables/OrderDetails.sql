CREATE TABLE [dbo].[OrderDetails]
(
	[OrderId] UNIQUEIDENTIFIER NOT NULL,
	[ProductId] UNIQUEIDENTIFIER NOT NULL,
	[OrderedQuantity] INTEGER NOT NULL,
	[Price] DECIMAL(10,2) NOT NULL,
	
	PRIMARY KEY([OrderId],[ProductId]),
	FOREIGN KEY([OrderId]) REFERENCES Orders(Id),
	FOREIGN KEY([ProductId]) REFERENCES Products(Id)
)