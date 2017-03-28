ALTER proc  [dbo].[Help_UpdateOrder]
			@Id int 
			,@SortOrder int
as 

BEGIN 
	UPDATE dbo.Help
	SET
		[SortOrder] = @SortOrder
	WHERE [Id] = @Id

END