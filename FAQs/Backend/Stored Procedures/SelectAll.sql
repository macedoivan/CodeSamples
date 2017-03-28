ALTER proc [dbo].[Help_SelectAll]
/*
*/
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
	ORDER BY SortOrder asc

END