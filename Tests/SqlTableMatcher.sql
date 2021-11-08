-- This file was generated using Reggie 0.9.6.0 from the
-- Test.rgg specification file on 11/8/2021 6:24:05 AM UTC
use [Test]
GO
-- <summary>Represents a matcher for the regular expressions in Test.rgg</summary>
DROP TABLE [dbo].[SqlTableMatcherState]
GO

CREATE TABLE [dbo].[SqlTableMatcherState] (
    [SymbolId]  INT NOT NULL,
    [StateId]  INT NOT NULL,
    [Accepts] INT NOT NULL DEFAULT 0,
    [BlockEndId] INT NOT NULL DEFAULT -1
    CONSTRAINT [PK_SqlTableMatcherState] PRIMARY KEY ([SymbolId], [StateId], [BlockEndId])
)
GO

DROP TABLE [dbo].[SqlTableMatcherStateTransition]
GO

CREATE TABLE [dbo].[SqlTableMatcherStateTransition]
(
    [SymbolId]  INT NOT NULL,
	[StateId] INT NOT NULL , 
    [BlockEndId] INT NOT NULL , 
	[ToStateId] INT NOT NULL,
    [Min] BIGINT NOT NULL, 
    [Max] BIGINT NOT NULL, 
    CONSTRAINT [PK_SqlTableMatcherStateTransition] PRIMARY KEY ([SymbolId], [StateId], [BlockEndId], [Min], [Max]) 
)
GO
DROP TABLE [dbo].[SqlTableMatcherSymbol]
GO

CREATE TABLE [dbo].[SqlTableMatcherSymbol] (
    [Id] INT NOT NULL,
    [Flags] INT NOT NULL DEFAULT 0,
    [SymbolName] NVARCHAR(MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
-- Matches text based on a DFA table and block end DFA table
DROP PROCEDURE [dbo].[SqlTableMatcher_TableMatch]
GO
CREATE PROCEDURE [dbo].[SqlTableMatcher_TableMatch] @symbolId INT, @value NVARCHAR(MAX), @position BIGINT = 0
AS
BEGIN
    CREATE TABLE #Results (
    	[AbsolutePosition] BIGINT NOT NULL,
    	[AbsoluteLength] INT NOT NULL,
    	[Position] BIGINT NOT NULL,
    	[Length] INT NOT NULL,
        [Value] NVARCHAR(MAX) NOT NULL)
    DECLARE @adv INT
    DECLARE @capture NVARCHAR(MAX) = N''
    DECLARE @index INT = 0
    DECLARE @newIndex INT
    DECLARE @valueEnd INT = DATALENGTH(@value) / 2 + 1
    DECLARE @tch INT
    DECLARE @newTch INT
    DECLARE @ch1 NCHAR
    DECLARE @newCh1 NCHAR
    DECLARE @ch2 NCHAR
    DECLARE @newCh2 NCHAR
    DECLARE @absi BIGINT = 0
    DECLARE @newAbsi BIGINT
    DECLARE @toState INT
    DECLARE @blockId INT
    DECLARE @tto INT
    DECLARE @hasError INT = 0
    DECLARE @absoluteIndex BIGINT = 0
    DECLARE @cursorPos BIGINT = @position
    DECLARE @flags INT
    DECLARE @ch BIGINT
    DECLARE @state INT = 0
    DECLARE @done INT = 0
    DECLARE @sacc INT
    DECLARE @acc INT = -1
    DECLARE @ai INT
    
    SET @index = @index + 1
    SET @adv = 1
    IF @index < @valueEnd
    BEGIN
    	SET @ch1 = SUBSTRING(@value, @index, 1)
    	SET @ch = UNICODE(@ch1)
    	SET @tch = @ch - 0xd800
    	IF @tch < 0 SET @tch = @tch + 2147483648
    	IF @tch < 2048
    	BEGIN
    		SET @ch = @ch * 1024
    		SET @index = @index + 1
    		SET @adv = 2
    		IF @index >= @valueEnd RETURN -1
    		SET @ch2 = SUBSTRING(@value, @index, 1)
    		SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
    	END
    END
    ELSE
    BEGIN
    	SET @ch = -1
    END
    WHILE @ch <> -1
    BEGIN
        SET @capture = N''
        SET @blockId = -1
        SET @position = @cursorPos
        SET @absoluteIndex = @absi
        SET @acc = -1
        SET @done = 0
        WHILE @done = 0
        BEGIN
        start_dfa:
            SET @done = 1
            SET @toState = -1
            SELECT @toState = [dbo].[SqlTableMatcherStateTransition].[ToStateId] FROM [dbo].[SqlTableMatcherState] INNER JOIN [dbo].[SqlTableMatcherStateTransition] ON [dbo].[SqlTableMatcherState].[StateId]=[dbo].[SqlTableMatcherStateTransition].[StateId] AND [dbo].[SqlTableMatcherState].[SymbolId]=[dbo].[SqlTableMatcherStateTransition].[SymbolId] AND [dbo].[SqlTableMatcherStateTransition].[BlockEndId]=[dbo].[SqlTableMatcherState].[BlockEndId] WHERE [dbo].[SqlTableMatcherState].[SymbolId] = @symbolId AND [dbo].[SqlTableMatcherState].[StateId] = @state AND [dbo].[SqlTableMatcherState].[BlockEndId] = @blockId AND [dbo].[SqlTableMatcherStateTransition].[SymbolId] = @symbolId AND @ch BETWEEN [dbo].[SqlTableMatcherStateTransition].[Min] AND [dbo].[SqlTableMatcherStateTransition].[Max]
            IF @toState <> -1
            BEGIN
                SET @state = @toState
                SET @done = 0
                SET @capture = @capture + @ch1
                IF @tch < 2048 SET @capture = @capture + @ch2
                
                SET @index = @index + 1
                SET @adv = 1
                IF @index < @valueEnd
                BEGIN
                	SET @ch1 = SUBSTRING(@value, @index, 1)
                	SET @ch = UNICODE(@ch1)
                	SET @tch = @ch - 0xd800
                	IF @tch < 0 SET @tch = @tch + 2147483648
                	IF @tch < 2048
                	BEGIN
                		SET @ch = @ch * 1024
                		SET @index = @index + 1
                		SET @adv = 2
                		IF @index >= @valueEnd RETURN -1
                		SET @ch2 = SUBSTRING(@value, @index, 1)
                		SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
                	END
                END
                ELSE
                BEGIN
                	SET @ch = -1
                END
                SET @absi = @absi + @adv;
                SET @cursorPos = @cursorPos + 1
                GOTO start_dfa
            END
        END -- WHILE table machine loop
        SET @acc = -1
        SELECT @acc = [dbo].[SqlTableMatcherState].[SymbolId] FROM [dbo].[SqlTableMatcherState] WHERE [dbo].[SqlTableMatcherState].[SymbolId] = @symbolId AND [dbo].[SqlTableMatcherState].[StateId] = @state AND [dbo].[SqlTableMatcherState].[BlockEndId] = @blockId AND [dbo].[SqlTableMatcherState].[Accepts] = 1
        IF @acc <> -1
        BEGIN
            SET @blockId = -1
            SELECT TOP 1 @blockId = [dbo].[SqlTableMatcherState].[BlockEndId] FROM [dbo].[SqlTableMatcherState] WHERE [dbo].[SqlTableMatcherState].[SymbolId]=@symbolId AND [dbo].[SqlTableMatcherState].[BlockEndId] <> -1
            IF @blockId <> -1
            BEGIN
                SET @state = 0
                WHILE @ch <> -1
                BEGIN
                    SET @acc = -1
                    SET @done = 0
                    WHILE @done = 0
                    BEGIN
                    start_block_end:
                        SET @done = 1
                        SET @toState = -1
                        SELECT @toState = [dbo].[SqlTableMatcherStateTransition].[ToStateId] FROM [dbo].[SqlTableMatcherState] INNER JOIN [dbo].[SqlTableMatcherStateTransition] ON [dbo].[SqlTableMatcherState].[StateId]=[dbo].[SqlTableMatcherStateTransition].[StateId] AND [dbo].[SqlTableMatcherState].[SymbolId]=[dbo].[SqlTableMatcherStateTransition].[SymbolId] AND [dbo].[SqlTableMatcherStateTransition].[BlockEndId]=[dbo].[SqlTableMatcherState].[BlockEndId] WHERE [dbo].[SqlTableMatcherState].[SymbolId] = @symbolId AND [dbo].[SqlTableMatcherState].[StateId] = @state AND [dbo].[SqlTableMatcherState].[BlockEndId] = @blockId AND [dbo].[SqlTableMatcherStateTransition].[SymbolId] = @symbolId AND @ch BETWEEN [dbo].[SqlTableMatcherStateTransition].[Min] AND [dbo].[SqlTableMatcherStateTransition].[Max]
                        IF @toState <> -1
                        BEGIN
                            SET @state = @toState
                            SET @done = 0
                            SET @capture = @capture + @ch1
                            IF @tch < 2048 SET @capture = @capture + @ch2
                            
                            SET @index = @index + 1
                            SET @adv = 1
                            IF @index < @valueEnd
                            BEGIN
                            	SET @ch1 = SUBSTRING(@value, @index, 1)
                            	SET @ch = UNICODE(@ch1)
                            	SET @tch = @ch - 0xd800
                            	IF @tch < 0 SET @tch = @tch + 2147483648
                            	IF @tch < 2048
                            	BEGIN
                            		SET @ch = @ch * 1024
                            		SET @index = @index + 1
                            		SET @adv = 2
                            		IF @index >= @valueEnd RETURN -1
                            		SET @ch2 = SUBSTRING(@value, @index, 1)
                            		SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
                            	END
                            END
                            ELSE
                            BEGIN
                            	SET @ch = -1
                            END
                            SET @absi = @absi + @adv;
                            SET @cursorPos = @cursorPos + 1
                            GOTO start_block_end
                        END
                    END -- WHILE table machine loop
                    SET @acc = -1
                    SELECT @acc = [dbo].[SqlTableMatcherState].[SymbolId] FROM [dbo].[SqlTableMatcherState] WHERE [dbo].[SqlTableMatcherState].[SymbolId] = @symbolId AND [dbo].[SqlTableMatcherState].[StateId] = @state AND [dbo].[SqlTableMatcherState].[BlockEndId] = @blockId AND [dbo].[SqlTableMatcherState].[Accepts] = 1
                    IF @acc <> -1
                    BEGIN
                        INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value]
                        BREAK
                    END -- IF accept
                    ELSE -- IF not accept
                    BEGIN
                        
                        SET @capture = @capture + @ch1
                        IF @tch < 2048 SET @capture = @capture + @ch2
                        
                        SET @index = @index + 1
                        SET @adv = 1
                        IF @index < @valueEnd
                        BEGIN
                        	SET @ch1 = SUBSTRING(@value, @index, 1)
                        	SET @ch = UNICODE(@ch1)
                        	SET @tch = @ch - 0xd800
                        	IF @tch < 0 SET @tch = @tch + 2147483648
                        	IF @tch < 2048
                        	BEGIN
                        		SET @ch = @ch * 1024
                        		SET @index = @index + 1
                        		SET @adv = 2
                        		IF @index >= @valueEnd RETURN -1
                        		SET @ch2 = SUBSTRING(@value, @index, 1)
                        		SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
                        	END
                        END
                        ELSE
                        BEGIN
                        	SET @ch = -1
                        END
                        SET @absi = @absi + @adv;
                        SET @cursorPos = @cursorPos + 1
                    END -- IF not accept
                    SET @state = 0
                END -- WHILE input loop
                SET @state = 0
                CONTINUE
            END -- IF block end
            ELSE -- IF not block end
            BEGIN
                IF DATALENGTH(@capture) > 0
                BEGIN
                    INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value]
                END
            END -- IF not block end
        END -- IF accept
        
        
        SET @index = @index + 1
        SET @adv = 1
        IF @index < @valueEnd
        BEGIN
        	SET @ch1 = SUBSTRING(@value, @index, 1)
        	SET @ch = UNICODE(@ch1)
        	SET @tch = @ch - 0xd800
        	IF @tch < 0 SET @tch = @tch + 2147483648
        	IF @tch < 2048
        	BEGIN
        		SET @ch = @ch * 1024
        		SET @index = @index + 1
        		SET @adv = 2
        		IF @index >= @valueEnd RETURN -1
        		SET @ch2 = SUBSTRING(@value, @index, 1)
        		SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
        	END
        END
        ELSE
        BEGIN
        	SET @ch = -1
        END
        SET @absi = @absi + @adv;
        SET @cursorPos = @cursorPos + 1
        SET @state = 0
    END -- WHILE input loop
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlTableMatcher_MatchVerbatimStringLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by VerbatimStringLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlTableMatcher_MatchVerbatimStringLiteral] @value NVARCHAR(MAX), @position BIGINT = 0
AS
BEGIN
    EXEC SqlTableMatcher_TableMatch @symbolId = 0, @value = @value, @position = @position
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlTableMatcher_MatchStringLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by StringLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlTableMatcher_MatchStringLiteral] @value NVARCHAR(MAX), @position BIGINT = 0
AS
BEGIN
    EXEC SqlTableMatcher_TableMatch @symbolId = 1, @value = @value, @position = @position
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlTableMatcher_MatchCharacterLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by CharacterLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlTableMatcher_MatchCharacterLiteral] @value NVARCHAR(MAX), @position BIGINT = 0
AS
BEGIN
    EXEC SqlTableMatcher_TableMatch @symbolId = 2, @value = @value, @position = @position
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlTableMatcher_MatchIntegerLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by IntegerLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlTableMatcher_MatchIntegerLiteral] @value NVARCHAR(MAX), @position BIGINT = 0
AS
BEGIN
    EXEC SqlTableMatcher_TableMatch @symbolId = 3, @value = @value, @position = @position
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlTableMatcher_MatchFloatLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by FloatLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlTableMatcher_MatchFloatLiteral] @value NVARCHAR(MAX), @position BIGINT = 0
AS
BEGIN
    EXEC SqlTableMatcher_TableMatch @symbolId = 4, @value = @value, @position = @position
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlTableMatcher_MatchWhitespace]
GO
-- <summary>Returns all occurances of the expression indicated by Whitespace within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlTableMatcher_MatchWhitespace] @value NVARCHAR(MAX), @position BIGINT = 0
AS
BEGIN
    EXEC SqlTableMatcher_TableMatch @symbolId = 5, @value = @value, @position = @position
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlTableMatcher_MatchIdentifier]
GO
-- <summary>Returns all occurances of the expression indicated by Identifier within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlTableMatcher_MatchIdentifier] @value NVARCHAR(MAX), @position BIGINT = 0
AS
BEGIN
    EXEC SqlTableMatcher_TableMatch @symbolId = 6, @value = @value, @position = @position
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlTableMatcher_MatchCommentBlock]
GO
-- <summary>Returns all occurances of the expression indicated by CommentBlock within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlTableMatcher_MatchCommentBlock] @value NVARCHAR(MAX), @position BIGINT = 0
AS
BEGIN
    EXEC SqlTableMatcher_TableMatch @symbolId = 7, @value = @value, @position = @position
END -- CREATE PROCEDURE
GO
TRUNCATE TABLE [dbo].[SqlTableMatcherSymbol]
TRUNCATE TABLE [dbo].[SqlTableMatcherStateTransition]
TRUNCATE TABLE [dbo].[SqlTableMatcherState]
GO
BEGIN TRANSACTION
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(0, 0, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(0, 0, -1, 1, 64, 64)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(0, 1, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(0, 1, -1, 2, 34, 34)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(0, 2, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(0, 2, -1, 2, 0, 33)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(0, 2, -1, 2, 35, 1114111)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(0, 2, -1, 3, 34, 34)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(0, 3, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(0, 3, -1, 2, 34, 34)
INSERT INTO [dbo].[SqlTableMatcherSymbol] VALUES(0, 0,N'VerbatimStringLiteral')
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(1, 0, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 0, -1, 1, 34, 34)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(1, 1, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 1, -1, 1, 0, 33)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 1, -1, 1, 35, 91)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 1, -1, 1, 93, 1114111)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 1, -1, 2, 34, 34)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 1, -1, 3, 92, 92)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(1, 2, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(1, 3, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 3, -1, 1, 0, 33)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 3, -1, 1, 35, 91)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 3, -1, 1, 93, 1114111)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 3, -1, 4, 34, 34)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 3, -1, 3, 92, 92)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(1, 4, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 4, -1, 1, 0, 33)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 4, -1, 1, 35, 91)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 4, -1, 1, 93, 1114111)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 4, -1, 2, 34, 34)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(1, 4, -1, 3, 92, 92)
INSERT INTO [dbo].[SqlTableMatcherSymbol] VALUES(1, 0,N'StringLiteral')
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(2, 0, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 0, -1, 1, 39, 39)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(2, 1, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 1, -1, 2, 0, 38)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 1, -1, 2, 40, 91)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 1, -1, 2, 93, 1114111)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 1, -1, 4, 92, 92)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(2, 2, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 2, -1, 3, 39, 39)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(2, 3, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(2, 4, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 4, -1, 2, 0, 38)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 4, -1, 2, 40, 1114111)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 4, -1, 5, 39, 39)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(2, 5, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(2, 5, -1, 3, 39, 39)
INSERT INTO [dbo].[SqlTableMatcherSymbol] VALUES(2, 0,N'CharacterLiteral')
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 0, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 0, -1, 1, 48, 48)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 0, -1, 2, 49, 57)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 1, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 1, -1, 2, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 1, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 1, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 1, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 1, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 1, -1, 6, 120, 120)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 2, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 2, -1, 2, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 2, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 2, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 2, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 2, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 3, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 3, -1, 4, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 3, -1, 4, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 4, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 5, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 5, -1, 4, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 5, -1, 4, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 6, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 6, -1, 7, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 6, -1, 7, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 6, -1, 7, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 7, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 7, -1, 8, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 7, -1, 8, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 7, -1, 8, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 7, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 7, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 7, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 7, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 8, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 8, -1, 9, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 8, -1, 9, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 8, -1, 9, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 8, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 8, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 8, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 8, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 9, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 9, -1, 10, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 9, -1, 10, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 9, -1, 10, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 9, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 9, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 9, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 9, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 10, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 10, -1, 11, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 10, -1, 11, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 10, -1, 11, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 10, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 10, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 10, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 10, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 11, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 11, -1, 12, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 11, -1, 12, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 11, -1, 12, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 11, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 11, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 11, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 11, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 12, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 12, -1, 13, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 12, -1, 13, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 12, -1, 13, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 12, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 12, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 12, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 12, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 13, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 13, -1, 14, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 13, -1, 14, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 13, -1, 14, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 13, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 13, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 13, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 13, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 14, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 14, -1, 15, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 14, -1, 15, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 14, -1, 15, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 14, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 14, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 14, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 14, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 15, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 15, -1, 16, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 15, -1, 16, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 15, -1, 16, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 15, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 15, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 15, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 15, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 16, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 16, -1, 17, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 16, -1, 17, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 16, -1, 17, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 16, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 16, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 16, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 16, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 17, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 17, -1, 18, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 17, -1, 18, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 17, -1, 18, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 17, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 17, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 17, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 17, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 18, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 18, -1, 19, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 18, -1, 19, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 18, -1, 19, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 18, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 18, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 18, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 18, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 19, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 19, -1, 20, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 19, -1, 20, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 19, -1, 20, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 19, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 19, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 19, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 19, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 20, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 20, -1, 21, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 20, -1, 21, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 20, -1, 21, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 20, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 20, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 20, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 20, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 21, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 21, -1, 22, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 21, -1, 22, 65, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 21, -1, 22, 97, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 21, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 21, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 21, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 21, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(3, 22, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 22, -1, 3, 76, 76)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 22, -1, 3, 108, 108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 22, -1, 5, 85, 85)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(3, 22, -1, 5, 117, 117)
INSERT INTO [dbo].[SqlTableMatcherSymbol] VALUES(3, 0,N'IntegerLiteral')
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(4, 0, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 0, -1, 1, 46, 46)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 0, -1, 7, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(4, 1, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 1, -1, 2, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(4, 2, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 2, -1, 2, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 2, -1, 3, 68, 68)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 2, -1, 3, 70, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 2, -1, 3, 77, 77)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 2, -1, 3, 100, 100)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 2, -1, 3, 102, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 2, -1, 3, 109, 109)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 2, -1, 4, 69, 69)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 2, -1, 4, 101, 101)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(4, 3, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(4, 4, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 4, -1, 5, 43, 43)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 4, -1, 5, 45, 45)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 4, -1, 6, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(4, 5, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 5, -1, 6, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(4, 6, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 6, -1, 6, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 6, -1, 3, 68, 68)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 6, -1, 3, 70, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 6, -1, 3, 77, 77)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 6, -1, 3, 100, 100)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 6, -1, 3, 102, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 6, -1, 3, 109, 109)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(4, 7, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 1, 46, 46)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 7, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 3, 68, 68)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 3, 70, 70)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 3, 77, 77)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 3, 100, 100)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 3, 102, 102)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 3, 109, 109)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 4, 69, 69)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(4, 7, -1, 4, 101, 101)
INSERT INTO [dbo].[SqlTableMatcherSymbol] VALUES(4, 0,N'FloatLiteral')
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(5, 0, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(5, 0, -1, 1, 9, 13)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(5, 0, -1, 1, 32, 32)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(5, 1, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(5, 1, -1, 1, 9, 13)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(5, 1, -1, 1, 32, 32)
INSERT INTO [dbo].[SqlTableMatcherSymbol] VALUES(5, 0,N'Whitespace')
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(6, 0, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65, 90)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 97, 122)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 170, 170)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 181, 181)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 186, 186)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 192, 214)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 216, 246)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 248, 705)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 710, 721)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 736, 740)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 748, 748)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 750, 750)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 880, 884)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 886, 887)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 890, 893)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 895, 895)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 902, 902)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 904, 906)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 908, 908)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 910, 929)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 931, 1013)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1015, 1153)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1162, 1327)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1329, 1366)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1369, 1369)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1377, 1415)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1488, 1514)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1520, 1522)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1568, 1610)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1646, 1647)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1649, 1747)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1749, 1749)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1765, 1766)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1774, 1775)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1786, 1788)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1791, 1791)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1808, 1808)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1810, 1839)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1869, 1957)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1969, 1969)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 1994, 2026)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2036, 2037)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2042, 2042)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2048, 2069)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2074, 2074)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2084, 2084)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2088, 2088)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2112, 2136)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2208, 2228)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2308, 2361)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2365, 2365)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2384, 2384)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2392, 2401)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2417, 2432)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2437, 2444)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2447, 2448)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2451, 2472)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2474, 2480)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2482, 2482)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2486, 2489)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2493, 2493)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2510, 2510)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2524, 2525)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2527, 2529)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2544, 2545)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2565, 2570)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2575, 2576)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2579, 2600)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2602, 2608)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2610, 2611)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2613, 2614)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2616, 2617)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2649, 2652)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2654, 2654)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2674, 2676)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2693, 2701)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2703, 2705)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2707, 2728)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2730, 2736)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2738, 2739)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2741, 2745)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2749, 2749)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2768, 2768)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2784, 2785)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2809, 2809)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2821, 2828)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2831, 2832)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2835, 2856)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2858, 2864)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2866, 2867)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2869, 2873)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2877, 2877)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2908, 2909)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2911, 2913)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2929, 2929)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2947, 2947)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2949, 2954)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2958, 2960)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2962, 2965)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2969, 2970)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2972, 2972)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2974, 2975)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2979, 2980)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2984, 2986)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 2990, 3001)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3024, 3024)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3077, 3084)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3086, 3088)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3090, 3112)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3114, 3129)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3133, 3133)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3160, 3162)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3168, 3169)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3205, 3212)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3214, 3216)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3218, 3240)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3242, 3251)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3253, 3257)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3261, 3261)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3294, 3294)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3296, 3297)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3313, 3314)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3333, 3340)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3342, 3344)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3346, 3386)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3389, 3389)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3406, 3406)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3423, 3425)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3450, 3455)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3461, 3478)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3482, 3505)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3507, 3515)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3517, 3517)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3520, 3526)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3585, 3632)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3634, 3635)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3648, 3654)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3713, 3714)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3716, 3716)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3719, 3720)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3722, 3722)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3725, 3725)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3732, 3735)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3737, 3743)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3745, 3747)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3749, 3749)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3751, 3751)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3754, 3755)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3757, 3760)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3762, 3763)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3773, 3773)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3776, 3780)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3782, 3782)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3804, 3807)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3840, 3840)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3904, 3911)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3913, 3948)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 3976, 3980)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4096, 4138)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4159, 4159)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4176, 4181)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4186, 4189)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4193, 4193)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4197, 4198)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4206, 4208)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4213, 4225)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4238, 4238)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4256, 4293)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4295, 4295)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4301, 4301)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4304, 4346)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4348, 4680)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4682, 4685)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4688, 4694)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4696, 4696)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4698, 4701)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4704, 4744)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4746, 4749)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4752, 4784)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4786, 4789)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4792, 4798)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4800, 4800)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4802, 4805)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4808, 4822)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4824, 4880)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4882, 4885)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4888, 4954)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 4992, 5007)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5024, 5109)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5112, 5117)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5121, 5740)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5743, 5759)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5761, 5786)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5792, 5866)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5873, 5880)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5888, 5900)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5902, 5905)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5920, 5937)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5952, 5969)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5984, 5996)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 5998, 6000)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6016, 6067)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6103, 6103)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6108, 6108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6176, 6263)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6272, 6312)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6314, 6314)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6320, 6389)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6400, 6430)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6480, 6509)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6512, 6516)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6528, 6571)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6576, 6601)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6656, 6678)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6688, 6740)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6823, 6823)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6917, 6963)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 6981, 6987)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7043, 7072)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7086, 7087)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7098, 7141)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7168, 7203)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7245, 7247)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7258, 7293)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7401, 7404)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7406, 7409)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7413, 7414)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7424, 7615)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7680, 7957)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7960, 7965)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 7968, 8005)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8008, 8013)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8016, 8023)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8025, 8025)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8027, 8027)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8029, 8029)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8031, 8061)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8064, 8116)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8118, 8124)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8126, 8126)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8130, 8132)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8134, 8140)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8144, 8147)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8150, 8155)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8160, 8172)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8178, 8180)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8182, 8188)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8305, 8305)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8319, 8319)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8336, 8348)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8450, 8450)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8455, 8455)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8458, 8467)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8469, 8469)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8473, 8477)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8484, 8484)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8486, 8486)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8488, 8488)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8490, 8493)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8495, 8505)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8508, 8511)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8517, 8521)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8526, 8526)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 8579, 8580)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11264, 11310)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11312, 11358)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11360, 11492)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11499, 11502)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11506, 11507)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11520, 11557)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11559, 11559)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11565, 11565)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11568, 11623)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11631, 11631)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11648, 11670)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11680, 11686)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11688, 11694)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11696, 11702)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11704, 11710)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11712, 11718)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11720, 11726)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11728, 11734)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11736, 11742)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 11823, 11823)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12293, 12294)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12337, 12341)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12347, 12348)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12353, 12438)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12445, 12447)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12449, 12538)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12540, 12543)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12549, 12589)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12593, 12686)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12704, 12730)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 12784, 12799)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 13312, 19893)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 19968, 40917)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 40960, 42124)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42192, 42237)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42240, 42508)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42512, 42527)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42538, 42539)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42560, 42606)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42623, 42653)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42656, 42725)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42775, 42783)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42786, 42888)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42891, 42925)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42928, 42935)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 42999, 43009)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43011, 43013)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43015, 43018)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43020, 43042)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43072, 43123)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43138, 43187)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43250, 43255)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43259, 43259)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43261, 43261)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43274, 43301)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43312, 43334)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43360, 43388)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43396, 43442)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43471, 43471)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43488, 43492)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43494, 43503)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43514, 43518)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43520, 43560)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43584, 43586)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43588, 43595)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43616, 43638)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43642, 43642)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43646, 43695)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43697, 43697)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43701, 43702)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43705, 43709)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43712, 43712)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43714, 43714)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43739, 43741)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43744, 43754)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43762, 43764)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43777, 43782)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43785, 43790)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43793, 43798)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43808, 43814)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43816, 43822)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43824, 43866)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43868, 43877)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 43888, 44002)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 44032, 55203)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 55216, 55238)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 55243, 55291)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 63744, 64109)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64112, 64217)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64256, 64262)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64275, 64279)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64285, 64285)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64287, 64296)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64298, 64310)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64312, 64316)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64318, 64318)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64320, 64321)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64323, 64324)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64326, 64433)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64467, 64829)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64848, 64911)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 64914, 64967)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65008, 65019)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65136, 65140)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65142, 65276)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65313, 65338)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65345, 65370)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65382, 65470)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65474, 65479)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65482, 65487)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65490, 65495)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65498, 65500)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65536, 65547)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65549, 65574)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65576, 65594)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65596, 65597)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65599, 65613)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65616, 65629)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 65664, 65786)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66176, 66204)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66208, 66256)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66304, 66335)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66352, 66368)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66370, 66377)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66384, 66421)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66432, 66461)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66464, 66499)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66504, 66511)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66560, 66717)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66816, 66855)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 66864, 66915)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67072, 67382)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67392, 67413)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67424, 67431)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67584, 67589)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67592, 67592)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67594, 67637)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67639, 67640)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67644, 67644)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67647, 67669)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67680, 67702)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67712, 67742)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67808, 67826)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67828, 67829)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67840, 67861)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67872, 67897)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 67968, 68023)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68030, 68031)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68096, 68096)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68112, 68115)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68117, 68119)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68121, 68147)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68192, 68220)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68224, 68252)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68288, 68295)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68297, 68324)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68352, 68405)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68416, 68437)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68448, 68466)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68480, 68497)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68608, 68680)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68736, 68786)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 68800, 68850)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 69635, 69687)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 69763, 69807)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 69840, 69864)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 69891, 69926)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 69968, 70002)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70006, 70006)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70019, 70066)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70081, 70084)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70106, 70106)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70108, 70108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70144, 70161)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70163, 70187)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70272, 70278)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70280, 70280)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70282, 70285)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70287, 70301)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70303, 70312)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70320, 70366)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70405, 70412)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70415, 70416)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70419, 70440)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70442, 70448)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70450, 70451)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70453, 70457)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70461, 70461)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70480, 70480)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70493, 70497)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70784, 70831)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70852, 70853)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 70855, 70855)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 71040, 71086)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 71128, 71131)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 71168, 71215)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 71236, 71236)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 71296, 71338)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 71424, 71449)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 71840, 71903)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 71935, 71935)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 72384, 72440)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 73728, 74649)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 74880, 75075)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 77824, 78894)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 82944, 83526)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 92160, 92728)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 92736, 92766)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 92880, 92909)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 92928, 92975)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 92992, 92995)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 93027, 93047)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 93053, 93071)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 93952, 94020)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 94032, 94032)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 94099, 94111)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 110592, 110593)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 113664, 113770)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 113776, 113788)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 113792, 113800)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 113808, 113817)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 119808, 119892)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 119894, 119964)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 119966, 119967)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 119970, 119970)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 119973, 119974)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 119977, 119980)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 119982, 119993)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 119995, 119995)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 119997, 120003)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120005, 120069)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120071, 120074)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120077, 120084)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120086, 120092)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120094, 120121)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120123, 120126)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120128, 120132)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120134, 120134)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120138, 120144)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120146, 120485)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120488, 120512)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120514, 120538)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120540, 120570)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120572, 120596)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120598, 120628)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120630, 120654)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120656, 120686)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120688, 120712)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120714, 120744)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120746, 120770)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 120772, 120779)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 124928, 125124)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126464, 126467)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126469, 126495)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126497, 126498)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126500, 126500)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126503, 126503)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126505, 126514)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126516, 126519)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126521, 126521)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126523, 126523)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126530, 126530)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126535, 126535)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126537, 126537)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126539, 126539)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126541, 126543)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126545, 126546)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126548, 126548)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126551, 126551)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126553, 126553)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126555, 126555)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126557, 126557)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126559, 126559)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126561, 126562)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126564, 126564)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126567, 126570)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126572, 126578)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126580, 126583)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126585, 126588)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126590, 126590)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126592, 126601)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126603, 126619)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126625, 126627)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126629, 126633)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 126635, 126651)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 131072, 173782)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 173824, 177972)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 177984, 178205)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 178208, 183969)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 0, -1, 1, 194560, 195101)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(6, 1, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 48, 57)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65, 90)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 97, 122)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 170, 170)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 181, 181)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 186, 186)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 192, 214)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 216, 246)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 248, 705)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 710, 721)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 736, 740)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 748, 748)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 750, 750)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 880, 884)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 886, 887)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 890, 893)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 895, 895)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 902, 902)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 904, 906)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 908, 908)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 910, 929)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 931, 1013)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1015, 1153)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1162, 1327)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1329, 1366)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1369, 1369)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1377, 1415)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1488, 1514)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1520, 1522)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1568, 1610)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1632, 1641)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1646, 1647)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1649, 1747)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1749, 1749)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1765, 1766)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1774, 1788)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1791, 1791)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1808, 1808)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1810, 1839)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1869, 1957)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1969, 1969)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 1984, 2026)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2036, 2037)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2042, 2042)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2048, 2069)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2074, 2074)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2084, 2084)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2088, 2088)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2112, 2136)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2208, 2228)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2308, 2361)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2365, 2365)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2384, 2384)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2392, 2401)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2406, 2415)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2417, 2432)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2437, 2444)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2447, 2448)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2451, 2472)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2474, 2480)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2482, 2482)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2486, 2489)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2493, 2493)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2510, 2510)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2524, 2525)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2527, 2529)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2534, 2545)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2565, 2570)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2575, 2576)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2579, 2600)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2602, 2608)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2610, 2611)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2613, 2614)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2616, 2617)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2649, 2652)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2654, 2654)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2662, 2671)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2674, 2676)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2693, 2701)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2703, 2705)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2707, 2728)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2730, 2736)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2738, 2739)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2741, 2745)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2749, 2749)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2768, 2768)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2784, 2785)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2790, 2799)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2809, 2809)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2821, 2828)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2831, 2832)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2835, 2856)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2858, 2864)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2866, 2867)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2869, 2873)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2877, 2877)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2908, 2909)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2911, 2913)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2918, 2927)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2929, 2929)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2947, 2947)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2949, 2954)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2958, 2960)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2962, 2965)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2969, 2970)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2972, 2972)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2974, 2975)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2979, 2980)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2984, 2986)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 2990, 3001)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3024, 3024)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3046, 3055)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3077, 3084)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3086, 3088)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3090, 3112)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3114, 3129)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3133, 3133)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3160, 3162)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3168, 3169)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3174, 3183)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3205, 3212)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3214, 3216)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3218, 3240)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3242, 3251)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3253, 3257)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3261, 3261)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3294, 3294)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3296, 3297)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3302, 3311)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3313, 3314)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3333, 3340)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3342, 3344)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3346, 3386)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3389, 3389)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3406, 3406)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3423, 3425)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3430, 3439)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3450, 3455)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3461, 3478)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3482, 3505)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3507, 3515)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3517, 3517)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3520, 3526)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3558, 3567)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3585, 3632)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3634, 3635)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3648, 3654)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3664, 3673)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3713, 3714)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3716, 3716)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3719, 3720)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3722, 3722)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3725, 3725)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3732, 3735)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3737, 3743)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3745, 3747)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3749, 3749)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3751, 3751)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3754, 3755)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3757, 3760)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3762, 3763)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3773, 3773)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3776, 3780)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3782, 3782)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3792, 3801)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3804, 3807)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3840, 3840)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3872, 3881)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3904, 3911)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3913, 3948)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 3976, 3980)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4096, 4138)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4159, 4169)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4176, 4181)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4186, 4189)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4193, 4193)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4197, 4198)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4206, 4208)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4213, 4225)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4238, 4238)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4240, 4249)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4256, 4293)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4295, 4295)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4301, 4301)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4304, 4346)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4348, 4680)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4682, 4685)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4688, 4694)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4696, 4696)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4698, 4701)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4704, 4744)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4746, 4749)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4752, 4784)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4786, 4789)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4792, 4798)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4800, 4800)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4802, 4805)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4808, 4822)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4824, 4880)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4882, 4885)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4888, 4954)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 4992, 5007)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5024, 5109)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5112, 5117)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5121, 5740)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5743, 5759)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5761, 5786)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5792, 5866)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5873, 5880)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5888, 5900)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5902, 5905)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5920, 5937)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5952, 5969)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5984, 5996)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 5998, 6000)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6016, 6067)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6103, 6103)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6108, 6108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6112, 6121)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6160, 6169)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6176, 6263)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6272, 6312)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6314, 6314)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6320, 6389)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6400, 6430)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6470, 6509)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6512, 6516)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6528, 6571)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6576, 6601)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6608, 6617)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6656, 6678)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6688, 6740)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6784, 6793)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6800, 6809)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6823, 6823)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6917, 6963)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6981, 6987)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 6992, 7001)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7043, 7072)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7086, 7141)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7168, 7203)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7232, 7241)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7245, 7293)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7401, 7404)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7406, 7409)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7413, 7414)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7424, 7615)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7680, 7957)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7960, 7965)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 7968, 8005)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8008, 8013)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8016, 8023)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8025, 8025)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8027, 8027)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8029, 8029)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8031, 8061)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8064, 8116)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8118, 8124)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8126, 8126)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8130, 8132)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8134, 8140)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8144, 8147)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8150, 8155)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8160, 8172)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8178, 8180)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8182, 8188)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8305, 8305)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8319, 8319)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8336, 8348)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8450, 8450)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8455, 8455)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8458, 8467)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8469, 8469)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8473, 8477)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8484, 8484)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8486, 8486)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8488, 8488)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8490, 8493)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8495, 8505)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8508, 8511)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8517, 8521)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8526, 8526)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 8579, 8580)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11264, 11310)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11312, 11358)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11360, 11492)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11499, 11502)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11506, 11507)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11520, 11557)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11559, 11559)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11565, 11565)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11568, 11623)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11631, 11631)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11648, 11670)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11680, 11686)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11688, 11694)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11696, 11702)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11704, 11710)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11712, 11718)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11720, 11726)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11728, 11734)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11736, 11742)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 11823, 11823)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12293, 12294)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12337, 12341)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12347, 12348)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12353, 12438)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12445, 12447)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12449, 12538)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12540, 12543)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12549, 12589)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12593, 12686)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12704, 12730)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 12784, 12799)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 13312, 19893)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 19968, 40917)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 40960, 42124)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42192, 42237)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42240, 42508)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42512, 42539)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42560, 42606)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42623, 42653)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42656, 42725)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42775, 42783)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42786, 42888)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42891, 42925)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42928, 42935)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 42999, 43009)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43011, 43013)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43015, 43018)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43020, 43042)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43072, 43123)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43138, 43187)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43216, 43225)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43250, 43255)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43259, 43259)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43261, 43261)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43264, 43301)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43312, 43334)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43360, 43388)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43396, 43442)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43471, 43481)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43488, 43492)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43494, 43518)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43520, 43560)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43584, 43586)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43588, 43595)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43600, 43609)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43616, 43638)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43642, 43642)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43646, 43695)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43697, 43697)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43701, 43702)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43705, 43709)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43712, 43712)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43714, 43714)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43739, 43741)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43744, 43754)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43762, 43764)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43777, 43782)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43785, 43790)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43793, 43798)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43808, 43814)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43816, 43822)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43824, 43866)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43868, 43877)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 43888, 44002)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 44016, 44025)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 44032, 55203)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 55216, 55238)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 55243, 55291)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 63744, 64109)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64112, 64217)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64256, 64262)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64275, 64279)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64285, 64285)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64287, 64296)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64298, 64310)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64312, 64316)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64318, 64318)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64320, 64321)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64323, 64324)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64326, 64433)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64467, 64829)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64848, 64911)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 64914, 64967)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65008, 65019)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65136, 65140)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65142, 65276)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65296, 65305)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65313, 65338)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65345, 65370)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65382, 65470)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65474, 65479)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65482, 65487)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65490, 65495)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65498, 65500)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65536, 65547)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65549, 65574)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65576, 65594)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65596, 65597)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65599, 65613)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65616, 65629)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 65664, 65786)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66176, 66204)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66208, 66256)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66304, 66335)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66352, 66368)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66370, 66377)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66384, 66421)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66432, 66461)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66464, 66499)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66504, 66511)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66560, 66717)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66720, 66729)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66816, 66855)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 66864, 66915)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67072, 67382)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67392, 67413)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67424, 67431)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67584, 67589)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67592, 67592)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67594, 67637)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67639, 67640)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67644, 67644)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67647, 67669)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67680, 67702)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67712, 67742)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67808, 67826)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67828, 67829)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67840, 67861)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67872, 67897)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 67968, 68023)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68030, 68031)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68096, 68096)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68112, 68115)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68117, 68119)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68121, 68147)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68192, 68220)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68224, 68252)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68288, 68295)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68297, 68324)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68352, 68405)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68416, 68437)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68448, 68466)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68480, 68497)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68608, 68680)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68736, 68786)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 68800, 68850)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 69635, 69687)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 69734, 69743)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 69763, 69807)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 69840, 69864)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 69872, 69881)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 69891, 69926)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 69942, 69951)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 69968, 70002)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70006, 70006)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70019, 70066)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70081, 70084)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70096, 70106)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70108, 70108)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70144, 70161)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70163, 70187)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70272, 70278)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70280, 70280)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70282, 70285)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70287, 70301)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70303, 70312)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70320, 70366)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70384, 70393)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70405, 70412)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70415, 70416)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70419, 70440)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70442, 70448)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70450, 70451)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70453, 70457)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70461, 70461)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70480, 70480)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70493, 70497)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70784, 70831)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70852, 70853)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70855, 70855)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 70864, 70873)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71040, 71086)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71128, 71131)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71168, 71215)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71236, 71236)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71248, 71257)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71296, 71338)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71360, 71369)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71424, 71449)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71472, 71481)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71840, 71913)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 71935, 71935)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 72384, 72440)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 73728, 74649)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 74880, 75075)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 77824, 78894)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 82944, 83526)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 92160, 92728)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 92736, 92766)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 92768, 92777)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 92880, 92909)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 92928, 92975)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 92992, 92995)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 93008, 93017)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 93027, 93047)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 93053, 93071)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 93952, 94020)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 94032, 94032)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 94099, 94111)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 110592, 110593)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 113664, 113770)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 113776, 113788)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 113792, 113800)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 113808, 113817)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 119808, 119892)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 119894, 119964)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 119966, 119967)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 119970, 119970)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 119973, 119974)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 119977, 119980)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 119982, 119993)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 119995, 119995)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 119997, 120003)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120005, 120069)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120071, 120074)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120077, 120084)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120086, 120092)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120094, 120121)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120123, 120126)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120128, 120132)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120134, 120134)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120138, 120144)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120146, 120485)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120488, 120512)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120514, 120538)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120540, 120570)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120572, 120596)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120598, 120628)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120630, 120654)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120656, 120686)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120688, 120712)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120714, 120744)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120746, 120770)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120772, 120779)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 120782, 120831)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 124928, 125124)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126464, 126467)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126469, 126495)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126497, 126498)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126500, 126500)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126503, 126503)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126505, 126514)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126516, 126519)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126521, 126521)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126523, 126523)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126530, 126530)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126535, 126535)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126537, 126537)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126539, 126539)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126541, 126543)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126545, 126546)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126548, 126548)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126551, 126551)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126553, 126553)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126555, 126555)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126557, 126557)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126559, 126559)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126561, 126562)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126564, 126564)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126567, 126570)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126572, 126578)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126580, 126583)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126585, 126588)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126590, 126590)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126592, 126601)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126603, 126619)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126625, 126627)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126629, 126633)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 126635, 126651)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 131072, 173782)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 173824, 177972)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 177984, 178205)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 178208, 183969)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(6, 1, -1, 1, 194560, 195101)
INSERT INTO [dbo].[SqlTableMatcherSymbol] VALUES(6, 0,N'Identifier')
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(7, 0, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(7, 0, -1, 1, 47, 47)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(7, 1, 0, -1)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(7, 1, -1, 2, 42, 42)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(7, 2, 1, -1)
INSERT INTO [dbo].[SqlTableMatcherSymbol] VALUES(7, 0,N'CommentBlock')
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(7, 0, 0, 0)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(7, 0, 0, 1, 42, 42)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(7, 1, 0, 0)
INSERT INTO [dbo].[SqlTableMatcherStateTransition] VALUES(7, 1, 0, 2, 47, 47)
INSERT INTO [dbo].[SqlTableMatcherState] VALUES(7, 2, 1, 0)
COMMIT
GO

