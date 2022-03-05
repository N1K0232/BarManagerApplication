CREATE PROCEDURE SP_RegisterCliente
@IdCliente UNIQUEIDENTIFIER,
@Nome NVARCHAR(MAX),
@Cognome NVARCHAR(MAX),
@DataNascita DATE,
@Telefono NVARCHAR(10),
@Identity INT OUTPUT
AS
BEGIN
INSERT INTO Clienti(IdCliente,Nome,Cognome,DataNascita,Telefono)
VALUES(@IdCliente,@Nome,@Cognome,@DataNascita,@Telefono);
IF @@ERROR <> 0
	SET @Identity = 0;
	RETURN -101;
SELECT @Identity=SCOPE_IDENTITY();
RETURN 0;
END