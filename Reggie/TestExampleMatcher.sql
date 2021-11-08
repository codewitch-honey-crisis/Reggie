USE [test]
GO
EXEC [dbo].[Example_MatchCommentBlock] @value = N'  /*fubar*/ baz /**/ 12343 /* foo /* 	 */bar 123.22 bar/**//*'
--EXEC [dbo].[Example_MatchWhitespace] @value = N' /*fubar*/ baz /**/ 12343 foo /* 	 */bar 123.22 bar/**/'
--EXEC [dbo].[Example_MatchWhitespace] @value = N'		/*  */ baz  12343 foo	123.22 bar'

GO
