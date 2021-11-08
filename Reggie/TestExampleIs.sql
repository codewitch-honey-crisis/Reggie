USE [test]
GO
DECLARE @result INT
EXEC @result = [dbo].[Example_IsCommentBlock] @value = N'/***/'
SELECT @result as [Return Value]
--EXEC @result = [dbo].[Example_IsIdentifier] @value = N'foo'
--SELECT @result as [Return Value]
GO
