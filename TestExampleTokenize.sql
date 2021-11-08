USE [Test]
GO

EXEC	[dbo].[Example_Tokenize]
		@value = N'foo bar /* baz */ 123 -456.78'

GO
