SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.UpdateProduct
	@Id INT,
	@Name NVARCHAR(200),
	@Code NVARCHAR(50), 
	@Description NVARCHAR(MAX) = NULL, 
	@Price DECIMAL(18,2), 
	@Stock INT, 
	@State BIT, 
	@UpdateDate DATETIME2 = NULL
AS
BEGIN
	SET NOCOUNT ON;
	-- verificar si existe el producto.
	IF (SELECT COUNT(*) FROM dbo.Products WHERE Id = @Id) = 0
	BEGIN
		RAISERROR('No existe un producto con ese código.',16,1);
		RETURN -1;
	END

	-- Actualizamos el producto si existe
	UPDATE Products SET Name= @Name, Code = @Code, Description = @Description, Price = @Price, Stock = @Stock, State = @State, UpdateDate = @UpdateDate
	WHERE Id = @Id;
END
GO
