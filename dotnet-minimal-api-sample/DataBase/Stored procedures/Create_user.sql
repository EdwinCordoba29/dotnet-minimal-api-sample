SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.CreateUser
	@UserName NVARCHAR(200),
	@Password NVARCHAR(200), 
	@Email NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;
	-- verificar si ya existe un usuario.
	IF EXISTS(SELECT * FROM dbo.Users WHERE UserName = @UserName)
	BEGIN
		RAISERROR('Nombre de usuario ya existe.',16,1);
		RETURN -1;
	END

	-- Insertamos el usuario si no existe
	INSERT INTO Users (UserName, Password, Email) VALUES
		(@UserName, @Password, @Email);
END
GO