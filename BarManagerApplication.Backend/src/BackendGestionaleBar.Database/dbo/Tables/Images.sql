﻿CREATE TABLE [dbo].[Images]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT newid(),
	[Path] NVARCHAR(256) NOT NULL,
	[Length] BIGINT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastModifiedDate] DATETIME NULL,

	PRIMARY KEY(Id)
)