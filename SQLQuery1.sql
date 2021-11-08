USE [Test]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[Example_Tokenize]
		@value = N'/**/'

SELECT	@return_value as 'Return Value'

GO
