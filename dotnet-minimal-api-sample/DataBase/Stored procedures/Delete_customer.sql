SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.DeleteCustomer
	@DocumentNumber NVARCHAR(30),
	@DeletedByUserId INT,
	@DeletedDate DATETIME2
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS(SELECT * FROM dbo.Customers WHERE DocumentNumber = @DocumentNumber AND State = 1)
	BEGIN
		RAISERROR('No existe un cliente con ese número de documento.',16,1);
		RETURN -1;
	END

	UPDATE Customers SET State = 0, DeletedDate = @DeletedDate, DeletedByUserId = @DeletedByUserId WHERE DocumentNumber = @DocumentNumber AND State = 1;
END
GO
