CREATE TABLE [dbo].[Orders]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
	[UmbrellaId] UNIQUEIDENTIFIER NOT NULL,
	[OrderDate] DATE NOT NULL,
	[OrderStatus] NVARCHAR(50) NOT NULL,
	[CreatedDate]       DATETIME         NOT NULL,
    [LastModifiedDate]  DATETIME         NULL,
    [IsDeleted]         BIT              NOT NULL,
    [DeletedDate]       DATETIME         NULL,

	PRIMARY KEY(Id),
	FOREIGN KEY(UserId) REFERENCES AspNetUsers(Id),
	FOREIGN KEY(UmbrellaId) REFERENCES Umbrellas(Id)
)