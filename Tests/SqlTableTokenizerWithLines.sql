
USE [Test]
GO
DROP TABLE [dbo].[SqlTableTokenizerWithLinesState]
GO

CREATE TABLE [dbo].[SqlTableTokenizerWithLinesState] (
    [StateId]  INT NOT NULL,
    [AcceptId] INT NOT NULL DEFAULT -1,
    [BlockEndId] INT NOT NULL DEFAULT -1
    CONSTRAINT [PK_SqlTableTokenizerWithLinesState] PRIMARY KEY ([StateId], [BlockEndId])
)
GO

DROP TABLE [dbo].[SqlTableTokenizerWithLinesStateTransition]
GO

CREATE TABLE [dbo].[SqlTableTokenizerWithLinesStateTransition]
(
    [StateId] INT NOT NULL , 
    [BlockEndId] INT NOT NULL , 
	[ToStateId] INT NOT NULL,
    [Min] BIGINT NOT NULL, 
    [Max] BIGINT NOT NULL, 
    CONSTRAINT [PK_SqlTableTokenizerWithLinesStateTransition] PRIMARY KEY ([StateId], [BlockEndId], [Min], [Max]) 
)
GO
DROP TABLE [dbo].[SqlTableTokenizerWithLinesSymbol]
GO

CREATE TABLE [dbo].[SqlTableTokenizerWithLinesSymbol] (
    [Id] INT NOT NULL,
    [Flags] INT NOT NULL DEFAULT 0,
    [BlockEndId] INT NOT NULL DEFAULT -1,
    [SymbolName] NVARCHAR(MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

DROP PROCEDURE [dbo].[SqlTableTokenizerWithLines_Tokenize]
GO
CREATE PROCEDURE [dbo].[SqlTableTokenizerWithLines_Tokenize] @value NVARCHAR(MAX), @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS BEGIN
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
	DECLARE @index INT = 1
	DECLARE @absoluteIndex BIGINT = 0
	DECLARE @ch BIGINT
	DECLARE @ch1 NCHAR
	DECLARE @ch2 NCHAR
	DECLARE @t BIGINT
	DECLARE @state INT = 0
	DECLARE @toState INT = -1
	DECLARE @accept INT = -1
	DECLARE @cursorPos BIGINT = @position
	DECLARE @capture NVARCHAR(MAX) = N''
	DECLARE @tmp NVARCHAR(MAX) = N''
	DECLARE @blockId INT
	DECLARE @result INT = 0
	DECLARE @len INT = 0
	DECLARE @flags INT = 0
	DECLARE @matched INT = 0
    DECLARE @errorIndex BIGINT
    DECLARE @errorPos BIGINT
    DECLARE @hasError INT = 0
	DECLARE @ai INT
	DECLARE @done INT = 0
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @errorLine INT
    DECLARE @errorColumn INT
    CREATE TABLE #Results (
	[AbsolutePosition] BIGINT NOT NULL,
	[AbsoluteLength] INT NOT NULL,
	[Position] BIGINT NOT NULL,
	[Length] INT NOT NULL,
    [SymbolId] INT NOT NULL,
    [Value] NVARCHAR(MAX) NOT NULL,
    [Line] INT NOT NULL,
    [Column] INT NOT NULL)
	IF @index >= @valueEnd
	BEGIN 
		SET @ch = -1
	END
	ELSE
	BEGIN
		SET @ch1 = SUBSTRING(@value,@index,1)
		SET @ch = UNICODE(@ch1)
		SET @t = @ch - 0xd800
		IF @t < 0 SET @t = @t + 2147483648
		IF @t < 2048
		BEGIN
			SET @ch = @ch * 1024
			SET @index = @index + 1
			IF @index >= @valueEnd RETURN -1
			SET @ch2 = SUBSTRING(@value,@index,1)
			SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
		END
	END
    SET @cursorPos = @position
	SET @lc = @line
	SET @cc = @column        
    WHILE @ch <> -1
	BEGIN
        SET @position = @cursorPos
        SET @absoluteIndex = @index-1
        SET @line = @lc
        SET @column = @cc     
        SET @done = 0
        SET @state = 0
        WHILE @done = 0
        BEGIN
            SET @done = 1
			SET @toState = -1
			SELECT @toState = [dbo].[SqlTableTokenizerWithLinesStateTransition].[ToStateId] FROM [dbo].[SqlTableTokenizerWithLinesState] INNER JOIN [dbo].[SqlTableTokenizerWithLinesStateTransition] ON [dbo].[SqlTableTokenizerWithLinesState].[StateId]=[dbo].[SqlTableTokenizerWithLinesStateTransition].[StateId] AND [dbo].[SqlTableTokenizerWithLinesStateTransition].[BlockEndId]=-1 AND [dbo].[SqlTableTokenizerWithLinesState].[StateId]=@state AND [dbo].[SqlTableTokenizerWithLinesState].[BlockEndId] = -1 AND @ch BETWEEN [dbo].[SqlTableTokenizerWithLinesStateTransition].[Min] AND [dbo].[SqlTableTokenizerWithLinesStateTransition].[Max]
			IF @toState <> -1
			BEGIN
				SET @done = 0
				SET @state = @toState
				SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
				SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
				IF @ch>31 SET @cc = @cc + 1
				SET @capture = @capture + @ch1
				IF @t < 2048 SET @capture = @capture + @ch2
				SET @index = @index + 1
				IF @index >= @valueEnd
				BEGIN
					SET @ch = -1
					SET @done = 1
				END
				ELSE
				BEGIN
					SET @ch1 = SUBSTRING(@value,@index,1)
					SET @ch = UNICODE(@ch1)
					SET @t = @ch - 0xd800
					IF @t < 0 SET @t = @t + 2147483648
					IF @t < 2048
					BEGIN
						SET @ch = @ch * 1024
						SET @index = @index + 1
						IF @index >= @valueEnd RETURN -1
						SET @ch2 = SUBSTRING(@value,@index,1)
						SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
					END
					SET @cursorPos = @cursorPos + 1
				END
			END
		END
		SET @accept = -1
		SELECT @accept = [dbo].[SqlTableTokenizerWithLinesState].[AcceptId], @flags = [dbo].[SqlTableTokenizerWithLinesSymbol].[Flags] FROM [dbo].[SqlTableTokenizerWithLinesState] INNER JOIN [dbo].[SqlTableTokenizerWithLinesSymbol] ON [dbo].[SqlTableTokenizerWithLinesState].[AcceptId] = [dbo].[SqlTableTokenizerWithLinesSymbol].[Id] WHERE [dbo].[SqlTableTokenizerWithLinesState].[StateId] = @state AND [dbo].[SqlTableTokenizerWithLinesState].[BlockEndId] = -1 AND [dbo].[SqlTableTokenizerWithLinesState].[AcceptId] <> -1
		IF @accept <> -1 
		BEGIN
			IF @hasError = 1
			BEGIN
				SET @hasError = 0
				SET @ai = CAST((@absoluteIndex - @errorIndex) AS INT)
				SET @tmp = SUBSTRING(@capture,1,@ai)
				SET @capture = SUBSTRING(@capture,@ai+1,(DATALENGTH(@capture)/2)-(@ai+1))
                INSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp, @errorLine, @errorColumn)
			END -- IF @hasError = 1
			SELECT @blockId = [dbo].[SqlTableTokenizerWithLinesSymbol].[BlockEndId] FROM [dbo].[SqlTableTokenizerWithLinesSymbol] WHERE [dbo].[SqlTableTokenizerWithLinesSymbol].[Id]=@accept
			IF @blockId <> -1 
			BEGIN
				SET @state = 0
                WHILE @ch <> -1
				BEGIN
                    SET @done = 0
                    WHILE @done = 0
					BEGIN
                        SET @done = 1
						SET @toState = -1
						SELECT @toState = [dbo].[SqlTableTokenizerWithLinesStateTransition].[ToStateId] FROM [dbo].[SqlTableTokenizerWithLinesState] INNER JOIN [dbo].[SqlTableTokenizerWithLinesStateTransition] ON [dbo].[SqlTableTokenizerWithLinesState].[StateId]=[dbo].[SqlTableTokenizerWithLinesStateTransition].[StateId] AND [dbo].[SqlTableTokenizerWithLinesStateTransition].[BlockEndId]=@blockId AND [dbo].[SqlTableTokenizerWithLinesState].[StateId]=@state AND [dbo].[SqlTableTokenizerWithLinesState].[BlockEndId] = @blockId AND @ch BETWEEN [dbo].[SqlTableTokenizerWithLinesStateTransition].[Min] AND [dbo].[SqlTableTokenizerWithLinesStateTransition].[Max]
						IF @toState <> -1
						BEGIN
							SET @state = @toState
							SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
							SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
							IF @ch>31 SET @cc = @cc + 1
							SET @capture = @capture + @ch1
							IF @t < 2048 SET @capture = @capture + @ch2
							SET @index = @index + 1
							SET @done = 0
							IF @index >= @valueEnd
							BEGIN
								SET @ch = -1
							END
							ELSE -- IF @index >= @valueEnd
							BEGIN
								SET @ch1 = SUBSTRING(@value,@index,1)
								SET @ch = UNICODE(@ch1)
								SET @t = @ch - 0xd800
								IF @t < 0 SET @t = @t + 2147483648
								IF @t < 2048
								BEGIN
									SET @ch = @ch * 1024
									SET @index = @index + 1
									IF @index >= @valueEnd RETURN -1
									SET @ch2 = SUBSTRING(@value,@index,1)
									SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
								END
								SET @cursorPos = @cursorPos + 1
							END -- IF @index >= @valueEnd
						END -- IF @toState <> -1
					END -- WHILE @done = 0
					SET @accept = -1
					SELECT @accept = [dbo].[SqlTableTokenizerWithLinesState].[AcceptId], @flags = [dbo].[SqlTableTokenizerWithLinesSymbol].[Flags] FROM [dbo].[SqlTableTokenizerWithLinesState] INNER JOIN [dbo].[SqlTableTokenizerWithLinesSymbol] ON [dbo].[SqlTableTokenizerWithLinesState].[AcceptId] = [dbo].[SqlTableTokenizerWithLinesSymbol].[Id] WHERE [dbo].[SqlTableTokenizerWithLinesState].[StateId] = @state AND [dbo].[SqlTableTokenizerWithLinesState].[BlockEndId] = @blockId
					IF @accept <> -1
					BEGIN
						IF (@flags & 1) = 0 -- symbol isn't hidden
						BEGIN
							-- HACK
							IF @ch = -1 SET @cursorPos = @cursorPos + 1
							SET @len = @index-@absoluteIndex-1
							IF(@len>0) INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @accept AS [SymbolId], @capture AS [Value], @line AS [Line], @column AS [Column]
						END
						SET @capture = N''
						BREAK
					END -- IF @accept <> -1
					SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
					SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
					IF @ch>31 SET @cc = @cc + 1
					SET @state = 0
					SET @capture = @capture + @ch1
					IF @t < 2048 SET @capture = @capture + @ch2
					SET @index = @index + 1
					IF @index >= @valueEnd
					BEGIN
						SET @ch = -1
						SET @done = 1
					END
					ELSE
					BEGIN
						SET @ch1 = SUBSTRING(@value,@index,1)
						SET @ch = UNICODE(@ch1)
						SET @t = @ch - 0xd800
						IF @t < 0 SET @t = @t + 2147483648
						IF @t < 2048
						BEGIN
							SET @ch = @ch * 1024
							SET @index = @index + 1
							IF @index >= @valueEnd RETURN -1
							SET @ch2 = SUBSTRING(@value,@index,1)
							SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
						END
						SET @cursorPos = @cursorPos + 1
					END			
				END  -- WHILE @ch <> -1
				CONTINUE
			END
			ELSE -- IF @blockId <> -1 
			BEGIN
				-- HACK:
				IF @ch = -1 SET @cursorPos = @cursorPos +1
				IF @hasError = 1
                BEGIN
					SET @hasError = 0
					SET @ai = CAST((@absoluteIndex - @errorIndex) AS INT)
					SET @tmp = SUBSTRING(@capture,1,@ai)
					SET @capture = SUBSTRING(@capture,@ai+1,(DATALENGTH(@capture)/2)-(@ai+1))
					INSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp, @errorLine, @errorColumn)       
                END
				IF (@flags & 1) = 0 -- symbol isn't hidden
				BEGIN
					SET @len = @index-@absoluteIndex-1
					
					IF(@len>0) INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @accept AS [SymbolId], @capture AS [Value], @line AS [Line], @column AS [Column]
				END
				SET @capture = N''
			END -- IF @blockId <> -1
		END
		ELSE -- IF @accept <> -1 
		BEGIN 
			-- handle the errors
			IF @hasError = 0
			BEGIN
				SET @errorPos = @position
				SET @errorIndex = @absoluteIndex
				SET @errorColumn = @column
				SET @errorLine = @line
			END
			SET @hasError = 1
		END -- IF @accept <> -1 
	END -- WHILE @ch <> -1
	IF @hasError =1 AND DATALENGTH(@capture) > 0
    BEGIN
		INSERT INTO #Results SELECT @errorIndex AS [AbsolutePosition], DATALENGTH(@capture)/2 AS [AbsoluteLength], @errorPos AS [Position], CAST((@position- @errorPos +1) AS INT) AS [Length], -1 AS [SymbolId], @capture AS [Value], @errorLine AS [Line], @errorColumn AS [Column]
    END
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE Tokenize
GO
DROP PROCEDURE [dbo].[SqlTableTokenizerWithLines_TextTokenize]
GO
CREATE PROCEDURE [dbo].[SqlTableTokenizerWithLines_TextTokenize] @value NTEXT, @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS BEGIN
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
	DECLARE @index INT = 1
	DECLARE @absoluteIndex BIGINT = 0
	DECLARE @ch BIGINT
	DECLARE @ch1 NCHAR
	DECLARE @ch2 NCHAR
	DECLARE @t BIGINT
	DECLARE @state INT = 0
	DECLARE @toState INT = -1
	DECLARE @accept INT = -1
	DECLARE @cursorPos BIGINT = @position
	DECLARE @capture NVARCHAR(MAX) = N''
	DECLARE @tmp NVARCHAR(MAX) = N''
	DECLARE @blockId INT
	DECLARE @result INT = 0
	DECLARE @len INT = 0
	DECLARE @flags INT = 0
	DECLARE @matched INT = 0
    DECLARE @errorIndex BIGINT
    DECLARE @errorPos BIGINT
    DECLARE @hasError INT = 0
	DECLARE @ai INT
	DECLARE @done INT = 0
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @errorLine INT
    DECLARE @errorColumn INT
    CREATE TABLE #Results (
	[AbsolutePosition] BIGINT NOT NULL,
	[AbsoluteLength] INT NOT NULL,
	[Position] BIGINT NOT NULL,
	[Length] INT NOT NULL,
    [SymbolId] INT NOT NULL,
    [Value] NVARCHAR(MAX) NOT NULL,
    [Line] INT NOT NULL,
    [Column] INT NOT NULL)
	IF @index >= @valueEnd
	BEGIN 
		SET @ch = -1
	END
	ELSE
	BEGIN
		SET @ch1 = SUBSTRING(@value,@index,1)
		SET @ch = UNICODE(@ch1)
		SET @t = @ch - 0xd800
		IF @t < 0 SET @t = @t + 2147483648
		IF @t < 2048
		BEGIN
			SET @ch = @ch * 1024
			SET @index = @index + 1
			IF @index >= @valueEnd RETURN -1
			SET @ch2 = SUBSTRING(@value,@index,1)
			SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
		END
	END
    SET @cursorPos = @position
	SET @lc = @line
	SET @cc = @column        
    WHILE @ch <> -1
	BEGIN
        SET @position = @cursorPos
        SET @absoluteIndex = @index-1
        SET @line = @lc
        SET @column = @cc     
        SET @done = 0
        SET @state = 0
        WHILE @done = 0
        BEGIN
            SET @done = 1
			SET @toState = -1
			SELECT @toState = [dbo].[SqlTableTokenizerWithLinesStateTransition].[ToStateId] FROM [dbo].[SqlTableTokenizerWithLinesState] INNER JOIN [dbo].[SqlTableTokenizerWithLinesStateTransition] ON [dbo].[SqlTableTokenizerWithLinesState].[StateId]=[dbo].[SqlTableTokenizerWithLinesStateTransition].[StateId] AND [dbo].[SqlTableTokenizerWithLinesStateTransition].[BlockEndId]=-1 AND [dbo].[SqlTableTokenizerWithLinesState].[StateId]=@state AND [dbo].[SqlTableTokenizerWithLinesState].[BlockEndId] = -1 AND @ch BETWEEN [dbo].[SqlTableTokenizerWithLinesStateTransition].[Min] AND [dbo].[SqlTableTokenizerWithLinesStateTransition].[Max]
			IF @toState <> -1
			BEGIN
				SET @done = 0
				SET @state = @toState
				SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
				SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
				IF @ch>31 SET @cc = @cc + 1
				SET @capture = @capture + @ch1
				IF @t < 2048 SET @capture = @capture + @ch2
				SET @index = @index + 1
				IF @index >= @valueEnd
				BEGIN
					SET @ch = -1
					SET @done = 1
				END
				ELSE
				BEGIN
					SET @ch1 = SUBSTRING(@value,@index,1)
					SET @ch = UNICODE(@ch1)
					SET @t = @ch - 0xd800
					IF @t < 0 SET @t = @t + 2147483648
					IF @t < 2048
					BEGIN
						SET @ch = @ch * 1024
						SET @index = @index + 1
						IF @index >= @valueEnd RETURN -1
						SET @ch2 = SUBSTRING(@value,@index,1)
						SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
					END
					SET @cursorPos = @cursorPos + 1
				END
			END
		END
		SET @accept = -1
		SELECT @accept = [dbo].[SqlTableTokenizerWithLinesState].[AcceptId], @flags = [dbo].[SqlTableTokenizerWithLinesSymbol].[Flags] FROM [dbo].[SqlTableTokenizerWithLinesState] INNER JOIN [dbo].[SqlTableTokenizerWithLinesSymbol] ON [dbo].[SqlTableTokenizerWithLinesState].[AcceptId] = [dbo].[SqlTableTokenizerWithLinesSymbol].[Id] WHERE [dbo].[SqlTableTokenizerWithLinesState].[StateId] = @state AND [dbo].[SqlTableTokenizerWithLinesState].[BlockEndId] = -1 AND [dbo].[SqlTableTokenizerWithLinesState].[AcceptId] <> -1
		IF @accept <> -1 
		BEGIN
			IF @hasError = 1
			BEGIN
				SET @hasError = 0
				SET @ai = CAST((@absoluteIndex - @errorIndex) AS INT)
				SET @tmp = SUBSTRING(@capture,1,@ai)
				SET @capture = SUBSTRING(@capture,@ai+1,(DATALENGTH(@capture)/2)-(@ai+1))
                INSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp, @errorLine, @errorColumn)
			END -- IF @hasError = 1
			SELECT @blockId = [dbo].[SqlTableTokenizerWithLinesSymbol].[BlockEndId] FROM [dbo].[SqlTableTokenizerWithLinesSymbol] WHERE [dbo].[SqlTableTokenizerWithLinesSymbol].[Id]=@accept
			IF @blockId <> -1 
			BEGIN
				SET @state = 0
                WHILE @ch <> -1
				BEGIN
                    SET @done = 0
                    WHILE @done = 0
					BEGIN
                        SET @done = 1
						SET @toState = -1
						SELECT @toState = [dbo].[SqlTableTokenizerWithLinesStateTransition].[ToStateId] FROM [dbo].[SqlTableTokenizerWithLinesState] INNER JOIN [dbo].[SqlTableTokenizerWithLinesStateTransition] ON [dbo].[SqlTableTokenizerWithLinesState].[StateId]=[dbo].[SqlTableTokenizerWithLinesStateTransition].[StateId] AND [dbo].[SqlTableTokenizerWithLinesStateTransition].[BlockEndId]=@blockId AND [dbo].[SqlTableTokenizerWithLinesState].[StateId]=@state AND [dbo].[SqlTableTokenizerWithLinesState].[BlockEndId] = @blockId AND @ch BETWEEN [dbo].[SqlTableTokenizerWithLinesStateTransition].[Min] AND [dbo].[SqlTableTokenizerWithLinesStateTransition].[Max]
						IF @toState <> -1
						BEGIN
							SET @state = @toState
							SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
							SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
							IF @ch>31 SET @cc = @cc + 1
							SET @capture = @capture + @ch1
							IF @t < 2048 SET @capture = @capture + @ch2
							SET @index = @index + 1
							SET @done = 0
							IF @index >= @valueEnd
							BEGIN
								SET @ch = -1
							END
							ELSE -- IF @index >= @valueEnd
							BEGIN
								SET @ch1 = SUBSTRING(@value,@index,1)
								SET @ch = UNICODE(@ch1)
								SET @t = @ch - 0xd800
								IF @t < 0 SET @t = @t + 2147483648
								IF @t < 2048
								BEGIN
									SET @ch = @ch * 1024
									SET @index = @index + 1
									IF @index >= @valueEnd RETURN -1
									SET @ch2 = SUBSTRING(@value,@index,1)
									SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
								END
								SET @cursorPos = @cursorPos + 1
							END -- IF @index >= @valueEnd
						END -- IF @toState <> -1
					END -- WHILE @done = 0
					SET @accept = -1
					SELECT @accept = [dbo].[SqlTableTokenizerWithLinesState].[AcceptId], @flags = [dbo].[SqlTableTokenizerWithLinesSymbol].[Flags] FROM [dbo].[SqlTableTokenizerWithLinesState] INNER JOIN [dbo].[SqlTableTokenizerWithLinesSymbol] ON [dbo].[SqlTableTokenizerWithLinesState].[AcceptId] = [dbo].[SqlTableTokenizerWithLinesSymbol].[Id] WHERE [dbo].[SqlTableTokenizerWithLinesState].[StateId] = @state AND [dbo].[SqlTableTokenizerWithLinesState].[BlockEndId] = @blockId
					IF @accept <> -1
					BEGIN
						IF (@flags & 1) = 0 -- symbol isn't hidden
						BEGIN
							-- HACK
							IF @ch = -1 SET @cursorPos = @cursorPos + 1
							SET @len = @index-@absoluteIndex-1
							IF(@len>0) INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @accept AS [SymbolId], @capture AS [Value], @line AS [Line], @column AS [Column]
						END
						SET @capture = N''
						BREAK
					END -- IF @accept <> -1
					SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
					SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
					IF @ch>31 SET @cc = @cc + 1
					SET @state = 0
					SET @capture = @capture + @ch1
					IF @t < 2048 SET @capture = @capture + @ch2
					SET @index = @index + 1
					IF @index >= @valueEnd
					BEGIN
						SET @ch = -1
						SET @done = 1
					END
					ELSE
					BEGIN
						SET @ch1 = SUBSTRING(@value,@index,1)
						SET @ch = UNICODE(@ch1)
						SET @t = @ch - 0xd800
						IF @t < 0 SET @t = @t + 2147483648
						IF @t < 2048
						BEGIN
							SET @ch = @ch * 1024
							SET @index = @index + 1
							IF @index >= @valueEnd RETURN -1
							SET @ch2 = SUBSTRING(@value,@index,1)
							SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00
						END
						SET @cursorPos = @cursorPos + 1
					END			
				END  -- WHILE @ch <> -1
				CONTINUE
			END
			ELSE -- IF @blockId <> -1 
			BEGIN
				-- HACK:
				IF @ch = -1 SET @cursorPos = @cursorPos +1
				IF @hasError = 1
                BEGIN
					SET @hasError = 0
					SET @ai = CAST((@absoluteIndex - @errorIndex) AS INT)
					SET @tmp = SUBSTRING(@capture,1,@ai)
					SET @capture = SUBSTRING(@capture,@ai+1,(DATALENGTH(@capture)/2)-(@ai+1))
					INSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp, @errorLine, @errorColumn)       
                END
				IF (@flags & 1) = 0 -- symbol isn't hidden
				BEGIN
					SET @len = @index-@absoluteIndex-1
					
					IF(@len>0) INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @accept AS [SymbolId], @capture AS [Value], @line AS [Line], @column AS [Column]
				END
				SET @capture = N''
			END -- IF @blockId <> -1
		END
		ELSE -- IF @accept <> -1 
		BEGIN 
			-- handle the errors
			IF @hasError = 0
			BEGIN
				SET @errorPos = @position
				SET @errorIndex = @absoluteIndex
				SET @errorColumn = @column
				SET @errorLine = @line
			END
			SET @hasError = 1
		END -- IF @accept <> -1 
	END -- WHILE @ch <> -1
	IF @hasError =1 AND DATALENGTH(@capture) > 0
    BEGIN
		INSERT INTO #Results SELECT @errorIndex AS [AbsolutePosition], DATALENGTH(@capture)/2 AS [AbsoluteLength], @errorPos AS [Position], CAST((@position- @errorPos +1) AS INT) AS [Length], -1 AS [SymbolId], @capture AS [Value], @errorLine AS [Line], @errorColumn AS [Column]
    END
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE Tokenize
GO
TRUNCATE TABLE [dbo].[SqlTableTokenizerWithLinesSymbol]
TRUNCATE TABLE [dbo].[SqlTableTokenizerWithLinesStateTransition]
TRUNCATE TABLE [dbo].[SqlTableTokenizerWithLinesState]
GO
BEGIN TRANSACTION
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(0, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,1,9,13)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,1,32,32)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,2,34,34)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,6,39,39)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,11,46,46)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,17,47,47)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,19,48,48)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,20,49,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,41,64,64)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65,90)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,97,122)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,170,170)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,181,181)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,186,186)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,192,214)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,216,246)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,248,705)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,710,721)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,736,740)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,748,748)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,750,750)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,880,884)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,886,887)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,890,893)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,895,895)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,902,902)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,904,906)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,908,908)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,910,929)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,931,1013)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1015,1153)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1162,1327)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1329,1366)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1369,1369)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1377,1415)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1488,1514)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1520,1522)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1568,1610)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1646,1647)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1649,1747)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1749,1749)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1765,1766)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1774,1775)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1786,1788)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1791,1791)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1808,1808)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1810,1839)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1869,1957)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1969,1969)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,1994,2026)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2036,2037)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2042,2042)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2048,2069)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2074,2074)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2084,2084)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2088,2088)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2112,2136)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2208,2228)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2308,2361)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2365,2365)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2384,2384)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2392,2401)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2417,2432)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2437,2444)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2447,2448)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2451,2472)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2474,2480)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2482,2482)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2486,2489)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2493,2493)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2510,2510)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2524,2525)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2527,2529)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2544,2545)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2565,2570)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2575,2576)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2579,2600)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2602,2608)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2610,2611)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2613,2614)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2616,2617)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2649,2652)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2654,2654)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2674,2676)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2693,2701)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2703,2705)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2707,2728)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2730,2736)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2738,2739)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2741,2745)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2749,2749)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2768,2768)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2784,2785)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2809,2809)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2821,2828)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2831,2832)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2835,2856)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2858,2864)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2866,2867)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2869,2873)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2877,2877)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2908,2909)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2911,2913)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2929,2929)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2947,2947)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2949,2954)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2958,2960)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2962,2965)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2969,2970)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2972,2972)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2974,2975)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2979,2980)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2984,2986)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,2990,3001)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3024,3024)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3077,3084)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3086,3088)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3090,3112)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3114,3129)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3133,3133)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3160,3162)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3168,3169)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3205,3212)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3214,3216)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3218,3240)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3242,3251)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3253,3257)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3261,3261)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3294,3294)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3296,3297)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3313,3314)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3333,3340)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3342,3344)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3346,3386)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3389,3389)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3406,3406)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3423,3425)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3450,3455)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3461,3478)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3482,3505)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3507,3515)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3517,3517)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3520,3526)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3585,3632)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3634,3635)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3648,3654)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3713,3714)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3716,3716)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3719,3720)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3722,3722)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3725,3725)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3732,3735)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3737,3743)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3745,3747)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3749,3749)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3751,3751)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3754,3755)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3757,3760)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3762,3763)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3773,3773)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3776,3780)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3782,3782)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3804,3807)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3840,3840)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3904,3911)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3913,3948)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,3976,3980)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4096,4138)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4159,4159)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4176,4181)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4186,4189)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4193,4193)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4197,4198)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4206,4208)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4213,4225)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4238,4238)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4256,4293)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4295,4295)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4301,4301)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4304,4346)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4348,4680)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4682,4685)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4688,4694)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4696,4696)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4698,4701)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4704,4744)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4746,4749)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4752,4784)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4786,4789)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4792,4798)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4800,4800)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4802,4805)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4808,4822)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4824,4880)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4882,4885)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4888,4954)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,4992,5007)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5024,5109)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5112,5117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5121,5740)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5743,5759)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5761,5786)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5792,5866)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5873,5880)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5888,5900)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5902,5905)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5920,5937)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5952,5969)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5984,5996)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,5998,6000)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6016,6067)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6103,6103)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6108,6108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6176,6263)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6272,6312)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6314,6314)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6320,6389)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6400,6430)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6480,6509)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6512,6516)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6528,6571)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6576,6601)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6656,6678)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6688,6740)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6823,6823)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6917,6963)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,6981,6987)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7043,7072)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7086,7087)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7098,7141)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7168,7203)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7245,7247)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7258,7293)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7401,7404)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7406,7409)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7413,7414)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7424,7615)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7680,7957)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7960,7965)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,7968,8005)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8008,8013)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8016,8023)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8025,8025)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8027,8027)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8029,8029)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8031,8061)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8064,8116)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8118,8124)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8126,8126)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8130,8132)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8134,8140)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8144,8147)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8150,8155)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8160,8172)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8178,8180)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8182,8188)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8305,8305)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8319,8319)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8336,8348)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8450,8450)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8455,8455)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8458,8467)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8469,8469)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8473,8477)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8484,8484)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8486,8486)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8488,8488)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8490,8493)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8495,8505)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8508,8511)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8517,8521)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8526,8526)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,8579,8580)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11264,11310)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11312,11358)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11360,11492)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11499,11502)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11506,11507)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11520,11557)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11559,11559)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11565,11565)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11568,11623)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11631,11631)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11648,11670)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11680,11686)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11688,11694)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11696,11702)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11704,11710)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11712,11718)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11720,11726)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11728,11734)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11736,11742)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,11823,11823)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12293,12294)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12337,12341)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12347,12348)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12353,12438)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12445,12447)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12449,12538)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12540,12543)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12549,12589)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12593,12686)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12704,12730)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,12784,12799)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,13312,19893)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,19968,40917)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,40960,42124)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42192,42237)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42240,42508)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42512,42527)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42538,42539)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42560,42606)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42623,42653)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42656,42725)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42775,42783)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42786,42888)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42891,42925)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42928,42935)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,42999,43009)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43011,43013)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43015,43018)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43020,43042)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43072,43123)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43138,43187)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43250,43255)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43259,43259)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43261,43261)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43274,43301)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43312,43334)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43360,43388)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43396,43442)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43471,43471)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43488,43492)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43494,43503)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43514,43518)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43520,43560)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43584,43586)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43588,43595)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43616,43638)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43642,43642)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43646,43695)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43697,43697)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43701,43702)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43705,43709)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43712,43712)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43714,43714)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43739,43741)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43744,43754)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43762,43764)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43777,43782)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43785,43790)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43793,43798)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43808,43814)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43816,43822)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43824,43866)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43868,43877)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,43888,44002)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,44032,55203)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,55216,55238)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,55243,55291)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,63744,64109)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64112,64217)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64256,64262)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64275,64279)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64285,64285)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64287,64296)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64298,64310)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64312,64316)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64318,64318)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64320,64321)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64323,64324)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64326,64433)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64467,64829)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64848,64911)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,64914,64967)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65008,65019)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65136,65140)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65142,65276)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65313,65338)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65345,65370)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65382,65470)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65474,65479)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65482,65487)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65490,65495)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65498,65500)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65536,65547)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65549,65574)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65576,65594)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65596,65597)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65599,65613)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65616,65629)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,65664,65786)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66176,66204)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66208,66256)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66304,66335)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66352,66368)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66370,66377)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66384,66421)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66432,66461)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66464,66499)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66504,66511)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66560,66717)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66816,66855)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,66864,66915)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67072,67382)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67392,67413)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67424,67431)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67584,67589)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67592,67592)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67594,67637)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67639,67640)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67644,67644)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67647,67669)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67680,67702)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67712,67742)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67808,67826)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67828,67829)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67840,67861)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67872,67897)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,67968,68023)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68030,68031)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68096,68096)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68112,68115)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68117,68119)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68121,68147)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68192,68220)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68224,68252)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68288,68295)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68297,68324)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68352,68405)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68416,68437)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68448,68466)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68480,68497)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68608,68680)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68736,68786)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,68800,68850)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,69635,69687)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,69763,69807)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,69840,69864)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,69891,69926)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,69968,70002)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70006,70006)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70019,70066)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70081,70084)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70106,70106)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70108,70108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70144,70161)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70163,70187)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70272,70278)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70280,70280)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70282,70285)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70287,70301)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70303,70312)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70320,70366)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70405,70412)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70415,70416)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70419,70440)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70442,70448)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70450,70451)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70453,70457)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70461,70461)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70480,70480)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70493,70497)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70784,70831)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70852,70853)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,70855,70855)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,71040,71086)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,71128,71131)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,71168,71215)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,71236,71236)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,71296,71338)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,71424,71449)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,71840,71903)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,71935,71935)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,72384,72440)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,73728,74649)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,74880,75075)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,77824,78894)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,82944,83526)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,92160,92728)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,92736,92766)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,92880,92909)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,92928,92975)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,92992,92995)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,93027,93047)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,93053,93071)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,93952,94020)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,94032,94032)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,94099,94111)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,110592,110593)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,113664,113770)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,113776,113788)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,113792,113800)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,113808,113817)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,119808,119892)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,119894,119964)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,119966,119967)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,119970,119970)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,119973,119974)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,119977,119980)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,119982,119993)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,119995,119995)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,119997,120003)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120005,120069)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120071,120074)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120077,120084)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120086,120092)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120094,120121)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120123,120126)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120128,120132)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120134,120134)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120138,120144)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120146,120485)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120488,120512)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120514,120538)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120540,120570)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120572,120596)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120598,120628)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120630,120654)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120656,120686)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120688,120712)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120714,120744)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120746,120770)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,120772,120779)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,124928,125124)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126464,126467)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126469,126495)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126497,126498)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126500,126500)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126503,126503)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126505,126514)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126516,126519)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126521,126521)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126523,126523)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126530,126530)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126535,126535)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126537,126537)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126539,126539)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126541,126543)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126545,126546)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126548,126548)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126551,126551)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126553,126553)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126555,126555)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126557,126557)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126559,126559)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126561,126562)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126564,126564)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126567,126570)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126572,126578)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126580,126583)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126585,126588)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126590,126590)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126592,126601)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126603,126619)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126625,126627)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126629,126633)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,126635,126651)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,131072,173782)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,173824,177972)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,177984,178205)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,178208,183969)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,-1,44,194560,195101)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(1, 5, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(1,-1,1,9,13)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(1,-1,1,32,32)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(2, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(2,-1,2,0,33)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(2,-1,2,35,91)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(2,-1,2,93,1114111)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(2,-1,3,34,34)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(2,-1,4,92,92)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(3, 1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(4, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(4,-1,2,0,33)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(4,-1,2,35,91)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(4,-1,2,93,1114111)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(4,-1,5,34,34)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(4,-1,4,92,92)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(5, 1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(5,-1,2,0,33)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(5,-1,2,35,91)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(5,-1,2,93,1114111)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(5,-1,3,34,34)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(5,-1,4,92,92)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(6, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(6,-1,7,0,38)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(6,-1,7,40,91)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(6,-1,7,93,1114111)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(6,-1,9,92,92)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(7, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(7,-1,8,39,39)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(8, 2, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(9, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(9,-1,7,0,38)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(9,-1,7,40,1114111)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(9,-1,10,39,39)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(10, 2, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(10,-1,8,39,39)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(11, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(11,-1,12,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(12, 4, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(12,-1,12,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(12,-1,13,68,68)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(12,-1,13,70,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(12,-1,13,77,77)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(12,-1,13,100,100)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(12,-1,13,102,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(12,-1,13,109,109)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(12,-1,14,69,69)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(12,-1,14,101,101)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(13, 4, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(14, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(14,-1,15,43,43)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(14,-1,15,45,45)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(14,-1,16,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(15, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(15,-1,16,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(16, 4, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(16,-1,16,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(16,-1,13,68,68)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(16,-1,13,70,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(16,-1,13,77,77)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(16,-1,13,100,100)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(16,-1,13,102,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(16,-1,13,109,109)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(17, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(17,-1,18,42,42)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(18, 40, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(19, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,11,46,46)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,20,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,13,68,68)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,13,70,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,13,77,77)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,13,100,100)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,13,102,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,13,109,109)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,14,69,69)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,14,101,101)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(19,-1,24,120,120)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(20, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,11,46,46)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,20,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,13,68,68)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,13,70,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,13,77,77)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,13,100,100)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,13,102,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,13,109,109)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,14,69,69)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,14,101,101)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(20,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(21, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(21,-1,22,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(21,-1,22,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(22, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(23, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(23,-1,22,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(23,-1,22,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(24, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(24,-1,25,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(24,-1,25,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(24,-1,25,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(25, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(25,-1,26,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(25,-1,26,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(25,-1,26,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(25,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(25,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(25,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(25,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(26, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(26,-1,27,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(26,-1,27,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(26,-1,27,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(26,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(26,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(26,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(26,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(27, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(27,-1,28,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(27,-1,28,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(27,-1,28,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(27,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(27,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(27,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(27,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(28, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(28,-1,29,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(28,-1,29,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(28,-1,29,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(28,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(28,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(28,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(28,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(29, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(29,-1,30,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(29,-1,30,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(29,-1,30,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(29,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(29,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(29,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(29,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(30, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(30,-1,31,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(30,-1,31,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(30,-1,31,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(30,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(30,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(30,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(30,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(31, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(31,-1,32,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(31,-1,32,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(31,-1,32,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(31,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(31,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(31,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(31,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(32, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(32,-1,33,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(32,-1,33,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(32,-1,33,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(32,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(32,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(32,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(32,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(33, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(33,-1,34,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(33,-1,34,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(33,-1,34,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(33,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(33,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(33,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(33,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(34, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(34,-1,35,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(34,-1,35,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(34,-1,35,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(34,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(34,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(34,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(34,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(35, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(35,-1,36,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(35,-1,36,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(35,-1,36,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(35,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(35,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(35,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(35,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(36, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(36,-1,37,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(36,-1,37,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(36,-1,37,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(36,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(36,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(36,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(36,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(37, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(37,-1,38,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(37,-1,38,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(37,-1,38,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(37,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(37,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(37,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(37,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(38, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(38,-1,39,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(38,-1,39,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(38,-1,39,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(38,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(38,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(38,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(38,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(39, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(39,-1,40,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(39,-1,40,65,70)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(39,-1,40,97,102)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(39,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(39,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(39,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(39,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(40, 3, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(40,-1,21,76,76)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(40,-1,21,108,108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(40,-1,23,85,85)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(40,-1,23,117,117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(41, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(41,-1,42,34,34)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(42, -1, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(42,-1,42,0,33)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(42,-1,42,35,1114111)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(42,-1,43,34,34)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(43, 0, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(43,-1,42,34,34)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(44, 6, -1)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,48,57)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65,90)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,97,122)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,170,170)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,181,181)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,186,186)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,192,214)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,216,246)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,248,705)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,710,721)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,736,740)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,748,748)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,750,750)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,880,884)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,886,887)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,890,893)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,895,895)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,902,902)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,904,906)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,908,908)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,910,929)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,931,1013)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1015,1153)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1162,1327)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1329,1366)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1369,1369)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1377,1415)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1488,1514)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1520,1522)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1568,1610)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1632,1641)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1646,1647)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1649,1747)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1749,1749)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1765,1766)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1774,1788)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1791,1791)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1808,1808)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1810,1839)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1869,1957)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1969,1969)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,1984,2026)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2036,2037)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2042,2042)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2048,2069)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2074,2074)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2084,2084)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2088,2088)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2112,2136)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2208,2228)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2308,2361)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2365,2365)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2384,2384)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2392,2401)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2406,2415)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2417,2432)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2437,2444)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2447,2448)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2451,2472)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2474,2480)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2482,2482)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2486,2489)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2493,2493)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2510,2510)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2524,2525)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2527,2529)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2534,2545)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2565,2570)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2575,2576)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2579,2600)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2602,2608)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2610,2611)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2613,2614)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2616,2617)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2649,2652)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2654,2654)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2662,2671)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2674,2676)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2693,2701)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2703,2705)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2707,2728)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2730,2736)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2738,2739)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2741,2745)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2749,2749)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2768,2768)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2784,2785)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2790,2799)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2809,2809)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2821,2828)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2831,2832)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2835,2856)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2858,2864)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2866,2867)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2869,2873)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2877,2877)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2908,2909)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2911,2913)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2918,2927)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2929,2929)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2947,2947)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2949,2954)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2958,2960)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2962,2965)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2969,2970)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2972,2972)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2974,2975)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2979,2980)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2984,2986)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,2990,3001)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3024,3024)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3046,3055)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3077,3084)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3086,3088)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3090,3112)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3114,3129)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3133,3133)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3160,3162)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3168,3169)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3174,3183)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3205,3212)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3214,3216)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3218,3240)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3242,3251)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3253,3257)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3261,3261)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3294,3294)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3296,3297)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3302,3311)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3313,3314)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3333,3340)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3342,3344)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3346,3386)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3389,3389)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3406,3406)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3423,3425)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3430,3439)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3450,3455)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3461,3478)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3482,3505)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3507,3515)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3517,3517)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3520,3526)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3558,3567)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3585,3632)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3634,3635)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3648,3654)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3664,3673)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3713,3714)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3716,3716)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3719,3720)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3722,3722)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3725,3725)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3732,3735)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3737,3743)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3745,3747)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3749,3749)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3751,3751)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3754,3755)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3757,3760)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3762,3763)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3773,3773)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3776,3780)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3782,3782)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3792,3801)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3804,3807)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3840,3840)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3872,3881)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3904,3911)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3913,3948)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,3976,3980)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4096,4138)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4159,4169)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4176,4181)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4186,4189)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4193,4193)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4197,4198)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4206,4208)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4213,4225)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4238,4238)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4240,4249)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4256,4293)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4295,4295)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4301,4301)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4304,4346)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4348,4680)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4682,4685)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4688,4694)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4696,4696)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4698,4701)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4704,4744)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4746,4749)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4752,4784)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4786,4789)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4792,4798)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4800,4800)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4802,4805)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4808,4822)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4824,4880)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4882,4885)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4888,4954)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,4992,5007)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5024,5109)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5112,5117)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5121,5740)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5743,5759)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5761,5786)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5792,5866)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5873,5880)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5888,5900)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5902,5905)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5920,5937)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5952,5969)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5984,5996)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,5998,6000)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6016,6067)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6103,6103)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6108,6108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6112,6121)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6160,6169)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6176,6263)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6272,6312)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6314,6314)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6320,6389)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6400,6430)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6470,6509)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6512,6516)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6528,6571)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6576,6601)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6608,6617)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6656,6678)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6688,6740)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6784,6793)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6800,6809)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6823,6823)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6917,6963)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6981,6987)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,6992,7001)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7043,7072)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7086,7141)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7168,7203)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7232,7241)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7245,7293)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7401,7404)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7406,7409)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7413,7414)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7424,7615)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7680,7957)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7960,7965)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,7968,8005)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8008,8013)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8016,8023)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8025,8025)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8027,8027)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8029,8029)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8031,8061)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8064,8116)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8118,8124)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8126,8126)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8130,8132)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8134,8140)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8144,8147)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8150,8155)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8160,8172)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8178,8180)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8182,8188)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8305,8305)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8319,8319)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8336,8348)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8450,8450)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8455,8455)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8458,8467)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8469,8469)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8473,8477)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8484,8484)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8486,8486)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8488,8488)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8490,8493)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8495,8505)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8508,8511)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8517,8521)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8526,8526)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,8579,8580)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11264,11310)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11312,11358)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11360,11492)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11499,11502)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11506,11507)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11520,11557)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11559,11559)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11565,11565)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11568,11623)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11631,11631)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11648,11670)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11680,11686)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11688,11694)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11696,11702)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11704,11710)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11712,11718)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11720,11726)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11728,11734)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11736,11742)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,11823,11823)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12293,12294)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12337,12341)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12347,12348)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12353,12438)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12445,12447)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12449,12538)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12540,12543)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12549,12589)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12593,12686)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12704,12730)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,12784,12799)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,13312,19893)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,19968,40917)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,40960,42124)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42192,42237)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42240,42508)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42512,42539)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42560,42606)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42623,42653)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42656,42725)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42775,42783)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42786,42888)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42891,42925)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42928,42935)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,42999,43009)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43011,43013)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43015,43018)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43020,43042)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43072,43123)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43138,43187)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43216,43225)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43250,43255)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43259,43259)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43261,43261)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43264,43301)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43312,43334)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43360,43388)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43396,43442)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43471,43481)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43488,43492)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43494,43518)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43520,43560)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43584,43586)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43588,43595)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43600,43609)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43616,43638)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43642,43642)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43646,43695)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43697,43697)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43701,43702)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43705,43709)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43712,43712)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43714,43714)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43739,43741)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43744,43754)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43762,43764)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43777,43782)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43785,43790)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43793,43798)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43808,43814)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43816,43822)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43824,43866)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43868,43877)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,43888,44002)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,44016,44025)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,44032,55203)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,55216,55238)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,55243,55291)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,63744,64109)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64112,64217)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64256,64262)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64275,64279)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64285,64285)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64287,64296)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64298,64310)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64312,64316)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64318,64318)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64320,64321)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64323,64324)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64326,64433)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64467,64829)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64848,64911)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,64914,64967)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65008,65019)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65136,65140)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65142,65276)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65296,65305)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65313,65338)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65345,65370)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65382,65470)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65474,65479)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65482,65487)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65490,65495)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65498,65500)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65536,65547)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65549,65574)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65576,65594)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65596,65597)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65599,65613)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65616,65629)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,65664,65786)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66176,66204)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66208,66256)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66304,66335)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66352,66368)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66370,66377)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66384,66421)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66432,66461)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66464,66499)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66504,66511)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66560,66717)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66720,66729)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66816,66855)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,66864,66915)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67072,67382)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67392,67413)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67424,67431)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67584,67589)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67592,67592)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67594,67637)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67639,67640)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67644,67644)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67647,67669)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67680,67702)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67712,67742)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67808,67826)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67828,67829)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67840,67861)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67872,67897)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,67968,68023)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68030,68031)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68096,68096)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68112,68115)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68117,68119)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68121,68147)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68192,68220)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68224,68252)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68288,68295)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68297,68324)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68352,68405)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68416,68437)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68448,68466)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68480,68497)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68608,68680)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68736,68786)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,68800,68850)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,69635,69687)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,69734,69743)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,69763,69807)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,69840,69864)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,69872,69881)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,69891,69926)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,69942,69951)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,69968,70002)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70006,70006)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70019,70066)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70081,70084)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70096,70106)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70108,70108)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70144,70161)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70163,70187)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70272,70278)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70280,70280)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70282,70285)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70287,70301)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70303,70312)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70320,70366)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70384,70393)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70405,70412)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70415,70416)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70419,70440)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70442,70448)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70450,70451)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70453,70457)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70461,70461)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70480,70480)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70493,70497)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70784,70831)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70852,70853)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70855,70855)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,70864,70873)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71040,71086)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71128,71131)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71168,71215)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71236,71236)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71248,71257)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71296,71338)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71360,71369)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71424,71449)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71472,71481)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71840,71913)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,71935,71935)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,72384,72440)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,73728,74649)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,74880,75075)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,77824,78894)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,82944,83526)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,92160,92728)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,92736,92766)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,92768,92777)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,92880,92909)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,92928,92975)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,92992,92995)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,93008,93017)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,93027,93047)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,93053,93071)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,93952,94020)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,94032,94032)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,94099,94111)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,110592,110593)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,113664,113770)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,113776,113788)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,113792,113800)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,113808,113817)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,119808,119892)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,119894,119964)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,119966,119967)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,119970,119970)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,119973,119974)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,119977,119980)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,119982,119993)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,119995,119995)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,119997,120003)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120005,120069)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120071,120074)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120077,120084)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120086,120092)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120094,120121)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120123,120126)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120128,120132)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120134,120134)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120138,120144)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120146,120485)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120488,120512)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120514,120538)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120540,120570)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120572,120596)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120598,120628)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120630,120654)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120656,120686)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120688,120712)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120714,120744)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120746,120770)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120772,120779)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,120782,120831)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,124928,125124)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126464,126467)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126469,126495)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126497,126498)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126500,126500)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126503,126503)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126505,126514)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126516,126519)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126521,126521)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126523,126523)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126530,126530)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126535,126535)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126537,126537)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126539,126539)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126541,126543)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126545,126546)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126548,126548)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126551,126551)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126553,126553)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126555,126555)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126557,126557)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126559,126559)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126561,126562)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126564,126564)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126567,126570)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126572,126578)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126580,126583)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126585,126588)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126590,126590)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126592,126601)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126603,126619)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126625,126627)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126629,126633)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,126635,126651)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,131072,173782)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,173824,177972)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,177984,178205)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,178208,183969)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(44,-1,44,194560,195101)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesSymbol] VALUES(0, 0, -1, N'VerbatimStringLiteral')
INSERT INTO [dbo].[SqlTableTokenizerWithLinesSymbol] VALUES(1, 0, -1, N'StringLiteral')
INSERT INTO [dbo].[SqlTableTokenizerWithLinesSymbol] VALUES(2, 0, -1, N'CharacterLiteral')
INSERT INTO [dbo].[SqlTableTokenizerWithLinesSymbol] VALUES(3, 0, -1, N'IntegerLiteral')
INSERT INTO [dbo].[SqlTableTokenizerWithLinesSymbol] VALUES(4, 0, -1, N'FloatLiteral')
INSERT INTO [dbo].[SqlTableTokenizerWithLinesSymbol] VALUES(5, 1, -1, N'Whitespace')
INSERT INTO [dbo].[SqlTableTokenizerWithLinesSymbol] VALUES(6, 0, -1, N'Identifier')
INSERT INTO [dbo].[SqlTableTokenizerWithLinesSymbol] VALUES(40, 0, 0, N'CommentBlock')
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(0, -1,0)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(0,0,1,42,42)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(1, -1,0)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesStateTransition] VALUES(1,0,2,47,47)
INSERT INTO [dbo].[SqlTableTokenizerWithLinesState] VALUES(2, 40,0)
COMMIT
GO

