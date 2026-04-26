SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.DeleteProduct
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	-- verificar si existe el producto.
	IF NOT EXISTS(SELECT * FROM dbo.Products WHERE Id = @Id)
	BEGIN
		RAISERROR('No existe un producto con ese código.',16,1);
		RETURN -1;
	END

	-- Eliminamos el producto si existe
	DELETE FROM Products WHERE Id = @Id;
END
GO