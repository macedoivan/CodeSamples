ALTER proc [dbo].[Help_Delete]
			@Id int

as 

BEGIN 

DELETE dbo.Help
WHERE Id = @Id

END