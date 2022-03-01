CREATE PROCEDURE [dbo].[SP_RegisterCredenziali]
	@IdUtente uniqueidentifier,
	@Email nvarchar(150),
	@Password nvarchar(150),
	@Role nvarchar(10),
	@DataRegistrazione date,
	@Identity INT OUTPUT
AS
	DECLARE @count INT;
	SELECT @count=COUNT(*) FROM Credenziali WHERE Email=@Email;
	IF @count > 0 RETURN -1;
	BEGIN
	INSERT INTO Credenziali(IdUtente,Email,Password,Role,DataRegistrazione)
	VALUES (@IdUtente,@Email,@Password,@Role,@DataRegistrazione);
	IF @@ERROR <> 0 RETURN -101;
	SELECT @Identity=SCOPE_IDENTITY();
	RETURN 0;
	END