SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.UpdateProduct
	@Name NVARCHAR(200),
	@Code NVARCHAR(50), 
	@Description NVARCHAR(MAX) = NULL, 
	@Price DECIMAL(18,2), 
	@Stock INT, 
	@UpdateDate DATETIME2,
	@UpdatedByUserId NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	-- verificar si existe el producto.
	IF (SELECT COUNT(*) FROM dbo.Products WHERE Code = @Code AND State = 1) = 0
	BEGIN
		RAISERROR('No existe un producto con ese código.',16,1);
		RETURN -1;
	END

	-- Actualizamos el producto si existe
	UPDATE Products SET Name= @Name, Description = @Description, Price = @Price, Stock = @Stock, UpdateDate = @UpdateDate, UpdatedByUserId = @UpdatedByUserId
	WHERE Code = @Code AND State = 1;
END
GO
