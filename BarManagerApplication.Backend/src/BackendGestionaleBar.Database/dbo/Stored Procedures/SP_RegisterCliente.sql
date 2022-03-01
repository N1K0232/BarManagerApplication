CREATE PROCEDURE SP_RegisterCliente
@Id UNIQUEIDENTIFIER,
@Nome NVARCHAR(MAX),
@Cognome NVARCHAR(MAX),
@DataNascita DATE,
@CodiceFiscale NVARCHAR(50),
@Telefono NVARCHAR(10),
@Identity INT OUTPUT,
@Result INT OUTPUT
AS
DECLARE @count INT
SELECT @count=COUNT(*) FROM Clienti WHERE Id=@Id AND Nome=@Nome AND Cognome=@Cognome AND DataNascita=@DataNascita AND CodiceFiscale=@CodiceFiscale AND Telefono=@Telefono
IF @count <> 0
	SET @Result = -1;
	SET @Identity = 0;
    RETURN @Result;
BEGIN
INSERT INTO Clienti(Id,Nome,Cognome,DataNascita,CodiceFiscale,Telefono) VALUES(@Id,@Nome,@Cognome,@DataNascita,@CodiceFiscale,@Telefono);
IF @@ERROR <> 0
	SET @Result=-101;
	SET @Identity = 0;
	RETURN @Result;
SELECT @Identity=SCOPE_IDENTITY();
SET @Result = 0;
RETURN @Result;
END