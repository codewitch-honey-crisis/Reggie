USE [Test]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[SqlCompiledLexer_Tokenize]
		@value = N'.... foo /* bar */.. foo...'

GO
DECLARE	@return_value Int
EXEC	@return_value = [dbo].[SqlTableLexer_Tokenize]
		@value = N'.... foo /* bar */.. foo...'


GO
