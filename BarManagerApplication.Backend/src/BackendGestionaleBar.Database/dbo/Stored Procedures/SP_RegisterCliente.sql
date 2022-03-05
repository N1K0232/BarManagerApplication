CREATE PROCEDURE SP_RegisterCliente
@Id UNIQUEIDENTIFIER,
@Nome NVARCHAR(MAX),
@Cognome NVARCHAR(MAX),
@DataNascita DATE,
@CodiceFiscale NVARCHAR(50),
@Telefono NVARCHAR(10),
@Identity INT OUTPUT
AS
DECLARE @count INT
SELECT @count=COUNT(*) FROM Clienti WHERE CodiceFiscale=@CodiceFiscale
IF @count <> 0
	SET @Identity = 0;
	RETURN -1;
BEGIN
INSERT INTO Clienti(Id,Nome,Cognome,DataNascita,CodiceFiscale,Telefono)
VALUES(@Id,@Nome,@Cognome,@DataNascita,@CodiceFiscale,@Telefono);
IF @@ERROR <> 0
	SET @Identity = 0;
	RETURN -101;
SELECT @Identity=SCOPE_IDENTITY();
RETURN 0;
END