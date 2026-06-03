SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.UpdateCustomer
	@FirstName NVARCHAR(100),
	@MiddleName NVARCHAR(100) = NULL,
	@LastName NVARCHAR(100),
	@SecondLastName NVARCHAR(100) = NULL,
	@DocumentType NVARCHAR(20),
	@DocumentNumber NVARCHAR(30),
	@Email NVARCHAR(200) = NULL,
	@Phone NVARCHAR(20) = NULL,
	@City NVARCHAR(100) = NULL,
	@Address NVARCHAR(255) = NULL,
	@UpdateDate DATETIME2,
	@UpdatedByUserId INT
AS
BEGIN
	SET NOCOUNT ON;
	IF (SELECT COUNT(*) FROM dbo.Customers WHERE DocumentNumber = @DocumentNumber AND State = 1) = 0
	BEGIN
		RAISERROR('No existe un cliente con ese número de documento.',16,1);
		RETURN -1;
	END

	UPDATE Customers SET 
		FirstName = @FirstName, 
		MiddleName = @MiddleName, 
		LastName = @LastName, 
		SecondLastName = @SecondLastName, 
		DocumentType = @DocumentType, 
		DocumentNumber = @DocumentNumber, 
		Email = @Email, 
		Phone = @Phone, 
		City = @City, 
		Address = @Address, 
		UpdateDate = @UpdateDate, 
		UpdatedByUserId = @UpdatedByUserId
	WHERE DocumentNumber = @DocumentNumber AND State = 1;
END
GO
