ALTER Proc [dbo].[MailChimp_SelectUsers] 
	    @RoleId nvarchar(128) = 'CCC43F44-3F2A-4E3A-9E5D-C01936E5B59D'
	   ,@Date date


as

BEGIN

DECLARE  @StartDate as datetime2(7) 
	    ,@EndDate as datetime2(7)

SET @StartDate = (CAST(@Date as datetime2(7)))

SET @EndDate = DATEADD(DAY, DATEDIFF(DAY, -1, @Date), 0)

SELECT [nu].[Email]
	  ,[w].[ListId]
	  ,[uR].[RoleId]
	  ,[uW].[WebsiteId]
	  ,[uP].[DateAdded]

FROM   [dbo].[UserProfiles] as uP
	   inner join  [dbo].[AspNetUsers] as nU
	   on [uP].[UserId] = [nU].[Id]
	   inner join [dbo].[AspNetUserRoles] as uR
	   on [nU].[Id] = [uR].[UserId]
	   inner join [dbo].[AspNetRoles] as nR
	   on [uR].[RoleId] = [nR].[Id]
	   inner join [dbo].[UserWebsite] as uW
	   on [nU].[Id] = [uW].[UserId]
	   inner join [dbo].[Website] as w
	   on [uW].[WebsiteId] = [w].[Id]

WHERE [uP].[DateAdded] >= @StartDate AND
	  [uP].[DateAdded] <= @EndDate AND 
	  RoleId = @RoleId

END 