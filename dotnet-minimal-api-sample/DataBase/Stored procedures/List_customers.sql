SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.ListCustomers
	@DocumentNumber NVARCHAR(30) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT FirstName, MiddleName, LastName, SecondLastName, DocumentType, DocumentNumber, Email, Phone, City, Address
	FROM dbo.Customers WHERE (@DocumentNumber IS NULL OR DocumentNumber = @DocumentNumber) AND State = 1;
END
GO
