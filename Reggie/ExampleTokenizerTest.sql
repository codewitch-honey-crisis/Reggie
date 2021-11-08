USE [Test]
GO

EXEC	[dbo].[SqlCompiledTokenizerWithLines_Tokenize]
		@value = N'foo bar /* baz */ 123 -456.78...'

GO
