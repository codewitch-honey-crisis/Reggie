USE [Test]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[SqlTableMatcher_TableMatch]
		@symbolId = 5,
		@value = N'  foo   bar baz    '

SELECT	@return_value as 'Return Value'

GO
