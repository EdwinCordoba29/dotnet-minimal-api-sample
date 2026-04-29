SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.CreateUser
	@UserName NVARCHAR(200),
	@Password NVARCHAR(200), 
	@EMail NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;
	-- verificar si ya existe un usuario.
	IF EXISTS(SELECT * FROM dbo.Users WHERE UserName = @UserName)
	BEGIN
		RAISERROR('Nombre de usuario ya existe.',16,1);
		RETURN -1;
	END

	DECLARE @PasswordEncrypt VARBINARY(200)
	SET @PasswordEncrypt = ENCRYPTBYPASSPHRASE('Cifrado', @Password)

	-- Insertamos el usuario si no existe
	INSERT INTO Users (UserName, Password, EMail) VALUES
		(@UserName, @PasswordEncrypt, @EMail);
END
GO