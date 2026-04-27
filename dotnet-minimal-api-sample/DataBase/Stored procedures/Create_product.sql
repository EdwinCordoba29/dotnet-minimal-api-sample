SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.CreateProduct
	@Name NVARCHAR(200),
	@Code NVARCHAR(50), 
	@Description NVARCHAR(MAX) = NULL, 
	@Price DECIMAL(18,2), 
	@Stock INT, 
	@State BIT, 
	@CreationDate DATETIME2,
    @CreatedByUserId NVARCHAR(50) 

AS
BEGIN
	SET NOCOUNT ON;
	-- verificar si ya existe un producto con el mismo código.
	IF EXISTS(SELECT * FROM dbo.Products WHERE Code = @Code)
	BEGIN
		RAISERROR('Ya existe un producto con el mismo código.',16,1);
		RETURN -1;
	END

	-- Insertamos el producto si el código no existe
	INSERT INTO Products (Name, Code, Description, Price, Stock, State, CreationDate, CreatedByUserId) VALUES
		(@Name, 
		@Code, 
		@Description, 
		@Price, 
		@Stock, 
		@State, 
		@CreationDate,
		@CreatedByUserId);
END
GO