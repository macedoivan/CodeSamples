ALTER proc [dbo].[Help_Insert] 
			@Id int OUTPUT
			,@SortOrder int
			,@Question nvarchar(200)
			,@Answer nvarchar(MAX)
			,@WebsiteId int
as 

BEGIN 

	INSERT INTO dbo.Help
		([SortOrder]
		,[Question] 
		,[Answer]
		,[WebsiteId]
		,[DateAdded]
		,[DateModified])
	VALUES		
		(@SortOrder
		,@Question
		,@Answer
		,@WebsiteId
		,GETUTCDATE()
		,GETUTCDATE())	
END