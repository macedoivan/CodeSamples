ALTER proc [dbo].[Help_SelectByWebsiteId]
			@WebsiteId int
as 

BEGIN 

	SELECT [Id],
			[Question],
			[Answer],
			[SortOrder],
			[WebsiteId],
			[DateAdded],
			[DateModified]

	FROM dbo.Help

	WHERE WebsiteId = @WebsiteId

	ORDER BY SortOrder asc

	
END