SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.ListUser
	@UserName NVARCHAR(200),
	@Password NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;
	-- verificar si ya existe el usuario.
	IF NOT EXISTS(SELECT * FROM dbo.Users WHERE UserName = @UserName)
	BEGIN
		RAISERROR('El usuario no existe.',16,1);
		RETURN -1;
	END

	DECLARE @PasswordEncrypt VARBINARY(200)
	DECLARE @PasswordDecrypt NVARCHAR(200)

	SELECT @PasswordEncrypt = Password FROM Users WHERE UserName = @UserName
	SET @PasswordDecrypt = DECRYPTBYPASSPHRASE('Cifrado', @PasswordEncrypt)

	IF(@PasswordDecrypt = @Password)
	BEGIN
		SELECT UserName, EMail FROM Users WHERE UserName = @UserName
	END
	ELSE
	BEGIN
		RAISERROR('Credenciales no válidas',16,1);
		RETURN -1;
	END
END
GO