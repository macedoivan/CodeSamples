ALTER proc [dbo].[UserCredits_SelectAll]
	    @CurrentPage int = 1
      , @ItemsPerPage int = 20
	  , @Query nvarchar(128) = null


as

BEGIN

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

	WHERE (@Query  IS NULL OR
		  (uP.FirstName LIKE @Query+'%') OR
		  (uP.LastName LIKE @Query+'%') OR
		  (anU.Email LIKE @Query+'%'))
		

	ORDER BY [uC].[DateAdded] desc
		OFFSET ((@CurrentPage - 1) * @ItemsPerPage) ROWS
        FETCH NEXT  @ItemsPerPage ROWS ONLY

	SELECT COUNT('UserId')

		FROM [dbo].[UserCredits] as uC 
		 left join [dbo].[UserProfiles] as uP
		 on  [uC].[UserId] = [uP].[UserId]

		 left join [dbo].[AspNetUsers] as anU
		 on [uP].[UserId] = [anU].[Id] 

		 left join [dbo].[Media] as m
		 on [uP].[MediaId] = [m].[Id]

	WHERE (@Query  IS NULL OR
		  (uP.FirstName LIKE @Query+'%') OR
		  (uP.LastName LIKE @Query+'%') OR
		  (anU.Email LIKE @Query+'%'))
END