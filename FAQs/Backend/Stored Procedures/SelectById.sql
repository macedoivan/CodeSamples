ALTER proc [dbo].[Help_SelectById]
			@Id int
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
	WHERE Id = @Id

END