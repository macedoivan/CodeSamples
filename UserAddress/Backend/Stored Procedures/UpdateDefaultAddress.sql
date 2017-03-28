ALTER PROC [dbo].[UserAddress_UpdateDefaultAddress]
			@UserId nvarchar(128)
		   ,@AddressId int
AS

BEGIN

	UPDATE dbo.UserAddress

	SET DefaultAddress = 0

	WHERE UserId = @UserId

	UPDATE dbo.UserAddress

	SET DefaultAddress = 1 

	WHERE UserId = @UserId AND
		  AddressId = @AddressId

END