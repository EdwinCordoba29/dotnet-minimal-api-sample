SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.CreateCustomer
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
	@State BIT, 
	@CreationDate DATETIME2,
	@CreatedByUserId INT 
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM dbo.Customers WHERE DocumentNumber = @DocumentNumber AND State = 1)
	BEGIN
		RAISERROR('Ya existe un cliente con el mismo número de documento.',16,1);
		RETURN -1;
	END

	INSERT INTO Customers (FirstName, MiddleName, LastName, SecondLastName, DocumentType, DocumentNumber, Email, Phone, City, Address, State, CreationDate, CreatedByUserId) VALUES
		(@FirstName, 
		@MiddleName, 
		@LastName, 
		@SecondLastName, 
		@DocumentType, 
		@DocumentNumber, 
		@Email, 
		@Phone, 
		@City, 
		@Address, 
		@State, 
		@CreationDate,
		@CreatedByUserId);
END
GO
