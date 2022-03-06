﻿CREATE TABLE [dbo].[Products]
(
	[IdProduct] UNIQUEIDENTIFIER NOT NULL,
	[IdCategory] INTEGER NOT NULL,
	[Name] NVARCHAR(256) NOT NULL,
	[Price] DECIMAL(10,2) NOT NULL,

	PRIMARY KEY(IdProduct),
	FOREIGN KEY(IdCategory) REFERENCES Categories(IdCategory)
)