USE [Test]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[Example_TableIs] @symbolId = 7, @value = N'/***/'

SELECT	@return_value as 'Return Value'

GO
