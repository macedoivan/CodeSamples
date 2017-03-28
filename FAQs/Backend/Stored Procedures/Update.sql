ALTER proc [dbo].[Help_Update]
			@Id int 
			,@Question nvarchar(200)
			,@Answer nvarchar(MAX)
			,@WebsiteId int
as 

BEGIN 
	UPDATE dbo.Help
	SET
		[Question] = @Question,
		[Answer] = @Answer,
		[WebsiteId] = @WebsiteId
		
	WHERE [Id] = @Id

END