CREATE TABLE [dbo].[OrderDetails]
(
	[OrderId] UNIQUEIDENTIFIER NOT NULL,
	[ProductId] UNIQUEIDENTIFIER NOT NULL,
	[CreatedDate] DATE NOT NULL,
	[LastModifiedDate] DATE NULL,
	
	PRIMARY KEY([OrderId],[ProductId]),
	FOREIGN KEY([OrderId]) REFERENCES Orders(Id),
	FOREIGN KEY([ProductId]) REFERENCES Products(Id)
)