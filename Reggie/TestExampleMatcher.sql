USE [test]
GO
DECLARE @result INT = -1
EXEC @result = [dbo].[Example_IsWhitespace] N'  '
SELECT @result as [Result]
GO

EXEC [dbo].[Example_MatchCommentBlock] @value = N'		/*  */ baz  12343 foo	123.22 bar'
EXEC [dbo].[Example_MatchWhitespace] @value = N'		/*  */ baz  12343 foo	123.22 bar'

GO
