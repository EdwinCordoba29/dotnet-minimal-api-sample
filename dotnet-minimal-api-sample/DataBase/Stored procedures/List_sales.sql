SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.ListSales
	@CustomerDocumentNumber NVARCHAR(30) = NULL,
    @InvoiceNumber NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
    SELECT 
        S.Id AS SaleId,
        S.InvoiceNumber,
        S.SaleDateTime,
        S.Observations,
        S.State,
        C.FirstName + ' ' + C.LastName AS CustomerName,
        C.DocumentNumber AS CustomerDocument,
        SI.ProductId,
        P.Code AS ProductCode,
        P.Name AS ProductName,
        SI.Quantity,
        SI.UnitPrice,
        SI.Discount
    FROM dbo.Sales S
    INNER JOIN dbo.Customers C ON S.CustomerId = C.Id
    INNER JOIN dbo.SaleItems SI ON S.Id = SI.SaleId
    INNER JOIN dbo.Products P ON SI.ProductId = P.Id
    WHERE 
        (@CustomerDocumentNumber IS NULL OR C.DocumentNumber = @CustomerDocumentNumber)
        AND 
        (@InvoiceNumber IS NULL OR S.InvoiceNumber = @InvoiceNumber)
    ORDER BY S.SaleDateTime DESC, S.Id;
END
GO
