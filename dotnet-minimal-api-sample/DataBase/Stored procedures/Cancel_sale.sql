SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.CancelSale
    @InvoiceNumber NVARCHAR(50),
    @CancellationDate DATETIME2,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SaleId INT;

    --1.Buscar el Id de la venta y verificar que exista
    SELECT @SaleId = Id FROM dbo.Sales WHERE InvoiceNumber = @InvoiceNumber;

    IF @SaleId IS NULL
    BEGIN
        RAISERROR('La factura especificada no existe.', 16, 1);
    RETURN -1;
    END

    -- 2.Verificar si la factura ya está cancelada
    IF EXISTS (SELECT 1 FROM dbo.Sales WHERE Id = @SaleId AND State = 0)
    BEGIN
        RAISERROR('La factura ya se encuentra cancelada.', 16, 1);
    RETURN -1;
    END

    -- 3.Ejecutar la cancelación con auditoría
    UPDATE dbo.Sales
    SET State = 0,
    CancellationDate = @CancellationDate,
    CancellationUserId = @UserId
    WHERE Id = @SaleId;

END
GO