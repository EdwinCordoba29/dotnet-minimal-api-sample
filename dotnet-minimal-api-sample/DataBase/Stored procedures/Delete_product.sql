SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.DeleteProduct
	@Code NVARCHAR(50),
	@DeletedByUserId NVARCHAR(50),
    @DeletedDate DATETIME2
AS
BEGIN
	SET NOCOUNT ON;
	-- verificar si existe el producto.
	IF NOT EXISTS(SELECT * FROM dbo.Products WHERE Code = @Code AND State = 1)
	BEGIN
		RAISERROR('No existe un producto con ese código.',16,1);
		RETURN -1;
	END

	-- Actualizamdo el estado del producto si existe
	UPDATE Products SET State = 0, DeletedDate = @DeletedDate, DeletedByUserId = @DeletedByUserId WHERE Code = @Code AND State = 1;
END
GO