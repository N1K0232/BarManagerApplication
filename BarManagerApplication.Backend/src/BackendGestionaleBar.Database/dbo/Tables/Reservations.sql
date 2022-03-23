﻿CREATE TABLE [dbo].[Reservations]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
    [IdUser] UNIQUEIDENTIFIER NOT NULL, 
    [Date] DATE NOT NULL, 
    [Time] TIME NOT NULL,

    PRIMARY KEY(Id),
    FOREIGN KEY(IdUser) REFERENCES AspNetUsers(Id)
)