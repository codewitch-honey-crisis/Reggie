USE [test]
GO


DECLARE	@return_value Int

EXEC 	[dbo].[SqlTableTokenizerWithLines_Tokenize]
		@value = N'		/* a*/ baz  12343 foo	123.22 bar....' 
--EXEC 	[dbo].[Example_Tokenize] @value = N'/*  */ baz  12343 foo 123.22 bar' 
GO
