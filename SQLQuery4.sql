USE [Test]
GO
--SELECT [dbo].[ExampleStateTransition].[ToStateId] FROM [dbo].[ExampleState] INNER JOIN [dbo].[ExampleStateTransition] ON [dbo].[ExampleState].[StateId]=[dbo].[ExampleStateTransition].[StateId] AND [dbo].[ExampleState].[SymbolId]= 4 AND [dbo].[ExampleStateTransition].[SymbolId] = 4 AND [dbo].[ExampleStateTransition].[BlockEndId]=-1 AND [dbo].[ExampleState].[StateId]=0 AND [dbo].[ExampleState].[BlockEndId] = -1 AND 32 BETWEEN [dbo].[ExampleStateTransition].[Min] AND [dbo].[ExampleStateTransition].[Max]
--GO
--SELECT * FROM [dbo].[ExampleState] WHERE [dbo].[ExampleState].[SymbolId] = 4 AND [dbo].[ExampleState].[StateId] = 1 --AND [dbo].[ExampleState].[BlockEndId] = -1 AND [dbo].[ExampleState].[Accepts] = 1
--GO

EXEC	[dbo].[Example_MatchCommentBlock]
		@value = N' 123 345 -- /* bar */ -34 33 '


GO
