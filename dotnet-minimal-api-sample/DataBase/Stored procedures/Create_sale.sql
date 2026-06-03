SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.CreateSale
	@CustomerDocumentNumber NVARCHAR(30),
	@ProductCode NVARCHAR(50),
	@SaleUserId INT,
	@SaleDateTime DATETIME2,
	@Observations NVARCHAR(500) = NULL,
	@State BIT = 1,
	@Quantity INT,
	@Discount DECIMAL(18,2) = NULL,
	@SaleId INT = NULL

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ProductId INT;
	DECLARE @ProductPrice DECIMAL(18,2);
	DECLARE @CustomerId INT;
	DECLARE @CurrentInvoiceNumber NVARCHAR(50);
	DECLARE @NewSaleId INT;
	SET @ProductId = 0;
	SET @ProductPrice = 0.00;
	SET @CustomerId = 0;

	SELECT @ProductId = Id, @ProductPrice = Price FROM dbo.Products WHERE Code = @ProductCode AND State = 1
	SELECT @CustomerId = Id FROM dbo.Customers WHERE DocumentNumber = @CustomerDocumentNumber AND State = 1

	IF @ProductId = 0
	BEGIN
		RAISERROR('Producto no existe o está inactivo.',16,1);
		RETURN -1;
	END

	IF @CustomerId = 0
	BEGIN
		RAISERROR('Cliente no existe o está inactivo.',16,1);
		RETURN -1;
	END

	IF @SaleId IS NULL
	BEGIN
        UPDATE dbo.InvoiceCounters 
        SET LastNumber = LastNumber + 1 
        WHERE Prefix = 'FAC-';

        DECLARE @LastNum INT;
        SELECT @LastNum = LastNumber FROM dbo.InvoiceCounters WHERE Prefix = 'FAC-';
        SET @CurrentInvoiceNumber = 'FAC-' + RIGHT('0000000' + CAST(@LastNum AS VARCHAR), 7);

		INSERT INTO Sales (InvoiceNumber, CustomerId, SaleUserId, SaleDateTime, Observations, State, CancellationDate, CancellationUserId)
		VALUES
		(@CurrentInvoiceNumber, @CustomerId, @SaleUserId, @SaleDateTime, @Observations, @State, NULL, NULL);
		        
		SET @NewSaleId = SCOPE_IDENTITY();
	END
    ELSE
    BEGIN
        SET @NewSaleId = @SaleId;
    END

    INSERT INTO dbo.SaleItems (SaleId, ProductId, Quantity, UnitPrice, Discount)
    VALUES (@NewSaleId, @ProductId, @Quantity, @ProductPrice, @Discount);

	SELECT @NewSaleId AS SaleId;
END
GO
