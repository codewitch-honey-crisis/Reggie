USE [test]
GO


DECLARE	@return_value Int

CREATE TABLE #Temp (
	[SymbolId] INT NOT NULL,
	[Value] NVARCHAR(MAX) NOT NULL,
	[Position] BIGINT NOT NULL,
	[Length] INT NOT NULL,
	[Line] INT NOT NULL,
	[Column] INT NOT NULL
)
INSERT INTO #Temp EXEC 	[dbo].[Example_Tokenize]
		@value = N'		/*  */ baz  12343 foo	123.22 bar' 
SELECT [dbo].[ExampleSymbol].[Id] as [Id], [dbo].[ExampleSymbol].[SymbolName] AS [Name],[#Temp].[Value] AS [Value],[#Temp].[Position] AS [Position], [#Temp].[Length] AS [Length], [#Temp].[Line] as [Line], [#Temp].[Column] AS [Column] FROM #Temp INNER JOIN [dbo].[ExampleSymbol] ON [dbo].[ExampleSymbol].[Id] = [#Temp].[SymbolId]
DROP TABLE #Temp
--EXEC 	[dbo].[Example_Tokenize] @value = N'/*  */ baz  12343 foo 123.22 bar' 
GO
