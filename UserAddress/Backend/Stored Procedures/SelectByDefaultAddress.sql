ALTER PROC [dbo].[UserAddress_SelectByDefaultAddress]
		   @UserId nvarchar(128)
		  ,@DefaultAddress bit = 1
AS
BEGIN	

SELECT  [uA].[DefaultAddress],
	    [a].[AddressId],
		[a].[DateCreated],
		[a].[DateModified],
		[a].[Name],
		[a].[ExternalPlaceId],
		[a].[Line1],
		[a].[Line2],
		[a].[City],
		[a].[State],
		[a].[StateId],
		[a].[ZipCode],
		[a].[Latitude],
		[a].[Longitude],
		[a].[Country]
	
FROM [dbo].[UserAddress] as uA
left join [dbo].[Address] as a
on [uA].[AddressId] = [a].[AddressId]

WHERE uA.UserId = @UserId AND
	  uA.DefaultAddress = @DefaultAddress

END