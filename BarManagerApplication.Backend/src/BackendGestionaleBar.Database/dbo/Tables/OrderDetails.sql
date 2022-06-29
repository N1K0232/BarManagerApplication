CREATE TABLE [dbo].[OrderDetails]
(
	[OrderId] UNIQUEIDENTIFIER NOT NULL,
	[ProductId] UNIQUEIDENTIFIER NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastModifiedDate] DATETIME NULL,
	
	PRIMARY KEY([OrderId],[ProductId]),
	FOREIGN KEY([OrderId]) REFERENCES Orders(Id),
	FOREIGN KEY([ProductId]) REFERENCES Products(Id)
)