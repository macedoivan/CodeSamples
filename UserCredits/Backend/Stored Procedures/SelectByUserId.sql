ALTER proc [dbo].[UserCredits_SelectByUserIdDash]
			@UserId nvarchar(128)


as

BEGIN
 
	SELECT SUM(Amount) AS Total
		   
	FROM dbo.UserCredits

	WHERE UserId = @UserId

	GROUP BY UserId

	---

	SELECT  [uC].[Id],
			[uC].[UserId],
			[uC].[Amount],
			[uC].[TransactionId],
			[uC].[JobId],
			[uC].[DateAdded],
			--
			[uP].[FirstName],
			[uP].[LastName],
			--
			[anU].[Email],
			[anU].[PhoneNumber],
			--
			[m].[Url]
		   
	 FROM [dbo].[UserCredits] as uC 
		 left join [dbo].[UserProfiles] as uP
		 on  [uC].[UserId] = [uP].[UserId]

		 left join [dbo].[AspNetUsers] as anU
		 on [uP].[UserId] = [anU].[Id] 

		 left join [dbo].[Media] as m
		 on [uP].[MediaId] = [m].[Id]

	WHERE [uC].[UserId] = @UserId

	ORDER BY [uC].[DateAdded] desc

END	