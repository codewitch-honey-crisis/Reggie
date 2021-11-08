-- This file was generated using Reggie 0.9.6.0 from the
-- Test.rgg specification file on 11/8/2021 6:24:04 AM UTC
use [Test]
GO
-- <summary>Represents a matcher for the regular expressions in Test.rgg</summary>
DROP PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchVerbatimStringLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by VerbatimStringLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <param name="line">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="column">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="tabWidth">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchVerbatimStringLiteral] @value NVARCHAR(MAX), @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS
BEGIN
    DECLARE @adv INT
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
    DECLARE @index INT = 0
    DECLARE @absi BIGINT = 0
    DECLARE @ch BIGINT
    DECLARE @ch1 NCHAR
    DECLARE @ch2 NCHAR
    DECLARE @tch BIGINT
    DECLARE @state INT = 0
    DECLARE @toState INT = -1
    DECLARE @accept INT = -1
    DECLARE @capture NVARCHAR(MAX)
    DECLARE @blockEndId INT
    DECLARE @cursorPos BIGINT = @position
    DECLARE @absoluteIndex INT
    DECLARE @result INT = 0
    DECLARE @len INT = 0
    DECLARE @newIndex INT
    DECLARE @newCursorPos INT
    DECLARE @newCapture NVARCHAR(MAX)
    DECLARE @newCh BIGINT
    DECLARE @newTch BIGINT
    DECLARE @newAbsi BIGINT
    DECLARE @newCh1 NCHAR
    DECLARE @newCh2 NCHAR
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @newLC INT
    DECLARE @newCC INT
    CREATE TABLE #Results (
    	[AbsolutePosition] BIGINT NOT NULL,
    	[AbsoluteLength] INT NOT NULL,
    	[Position] BIGINT NOT NULL,
    	[Length] INT NOT NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [Line] INT NOT NULL,
        [Column] INT NOT NULL)
    
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
        
        SET @position = @cursorPos
        SET @absoluteIndex = @absi
        SET @line = @lc
        SET @column = @cc
    -- q0
        IF @ch = 64
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        GOTO next
    q1:
        IF @ch = 34
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        GOTO next
    q2:
        IF @ch = 9
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 10
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            SET @lc = @lc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 13
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            
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
            GOTO q2
        END -- IF {range match}
        IF (@ch >= 0 AND @ch <= 8) OR @ch = 11 OR @ch = 12 OR (@ch >= 14 AND @ch <= 33) OR (@ch >= 35 AND @ch <= 1114111)
        BEGIN
            SET @capture = @capture + @ch1
            IF @tch < 2048 SET @capture = @capture + @ch2
            IF @ch > 31 SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 34
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        GOTO next
    q3:
        IF @ch = 34
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
    next:
        
        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
        SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
        IF @ch>31 SET @cc = @cc + 1
        
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
    END -- WHILE input loop
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchStringLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by StringLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <param name="line">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="column">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="tabWidth">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchStringLiteral] @value NVARCHAR(MAX), @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS
BEGIN
    DECLARE @adv INT
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
    DECLARE @index INT = 0
    DECLARE @absi BIGINT = 0
    DECLARE @ch BIGINT
    DECLARE @ch1 NCHAR
    DECLARE @ch2 NCHAR
    DECLARE @tch BIGINT
    DECLARE @state INT = 0
    DECLARE @toState INT = -1
    DECLARE @accept INT = -1
    DECLARE @capture NVARCHAR(MAX)
    DECLARE @blockEndId INT
    DECLARE @cursorPos BIGINT = @position
    DECLARE @absoluteIndex INT
    DECLARE @result INT = 0
    DECLARE @len INT = 0
    DECLARE @newIndex INT
    DECLARE @newCursorPos INT
    DECLARE @newCapture NVARCHAR(MAX)
    DECLARE @newCh BIGINT
    DECLARE @newTch BIGINT
    DECLARE @newAbsi BIGINT
    DECLARE @newCh1 NCHAR
    DECLARE @newCh2 NCHAR
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @newLC INT
    DECLARE @newCC INT
    CREATE TABLE #Results (
    	[AbsolutePosition] BIGINT NOT NULL,
    	[AbsoluteLength] INT NOT NULL,
    	[Position] BIGINT NOT NULL,
    	[Length] INT NOT NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [Line] INT NOT NULL,
        [Column] INT NOT NULL)
    
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
        
        SET @position = @cursorPos
        SET @absoluteIndex = @absi
        SET @line = @lc
        SET @column = @cc
    -- q0
        IF @ch = 34
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        GOTO next
    q1:
        IF @ch = 9
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 10
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            SET @lc = @lc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 13
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            
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
            GOTO q1
        END -- IF {range match}
        IF (@ch >= 0 AND @ch <= 8) OR @ch = 11 OR @ch = 12 OR (@ch >= 14 AND @ch <= 33) OR (@ch >= 35 AND @ch <= 91) OR (@ch >= 93 AND @ch <= 1114111)
        BEGIN
            SET @capture = @capture + @ch1
            IF @tch < 2048 SET @capture = @capture + @ch2
            IF @ch > 31 SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 34
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 92
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        GOTO next
    q2:
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q3:
        IF @ch = 9
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 10
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            SET @lc = @lc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 13
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            
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
            GOTO q1
        END -- IF {range match}
        IF (@ch >= 0 AND @ch <= 8) OR @ch = 11 OR @ch = 12 OR (@ch >= 14 AND @ch <= 33) OR (@ch >= 35 AND @ch <= 91) OR (@ch >= 93 AND @ch <= 1114111)
        BEGIN
            SET @capture = @capture + @ch1
            IF @tch < 2048 SET @capture = @capture + @ch2
            IF @ch > 31 SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 34
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q4
        END -- IF {range match}
        IF @ch = 92
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        GOTO next
    q4:
        IF @ch = 9
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 10
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            SET @lc = @lc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 13
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            
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
            GOTO q1
        END -- IF {range match}
        IF (@ch >= 0 AND @ch <= 8) OR @ch = 11 OR @ch = 12 OR (@ch >= 14 AND @ch <= 33) OR (@ch >= 35 AND @ch <= 91) OR (@ch >= 93 AND @ch <= 1114111)
        BEGIN
            SET @capture = @capture + @ch1
            IF @tch < 2048 SET @capture = @capture + @ch2
            IF @ch > 31 SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 34
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 92
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
    next:
        
        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
        SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
        IF @ch>31 SET @cc = @cc + 1
        
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
    END -- WHILE input loop
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchCharacterLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by CharacterLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <param name="line">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="column">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="tabWidth">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchCharacterLiteral] @value NVARCHAR(MAX), @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS
BEGIN
    DECLARE @adv INT
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
    DECLARE @index INT = 0
    DECLARE @absi BIGINT = 0
    DECLARE @ch BIGINT
    DECLARE @ch1 NCHAR
    DECLARE @ch2 NCHAR
    DECLARE @tch BIGINT
    DECLARE @state INT = 0
    DECLARE @toState INT = -1
    DECLARE @accept INT = -1
    DECLARE @capture NVARCHAR(MAX)
    DECLARE @blockEndId INT
    DECLARE @cursorPos BIGINT = @position
    DECLARE @absoluteIndex INT
    DECLARE @result INT = 0
    DECLARE @len INT = 0
    DECLARE @newIndex INT
    DECLARE @newCursorPos INT
    DECLARE @newCapture NVARCHAR(MAX)
    DECLARE @newCh BIGINT
    DECLARE @newTch BIGINT
    DECLARE @newAbsi BIGINT
    DECLARE @newCh1 NCHAR
    DECLARE @newCh2 NCHAR
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @newLC INT
    DECLARE @newCC INT
    CREATE TABLE #Results (
    	[AbsolutePosition] BIGINT NOT NULL,
    	[AbsoluteLength] INT NOT NULL,
    	[Position] BIGINT NOT NULL,
    	[Length] INT NOT NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [Line] INT NOT NULL,
        [Column] INT NOT NULL)
    
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
        
        SET @position = @cursorPos
        SET @absoluteIndex = @absi
        SET @line = @lc
        SET @column = @cc
    -- q0
        IF @ch = 39
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        GOTO next
    q1:
        IF @ch = 9
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 10
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            SET @lc = @lc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 13
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            
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
            GOTO q2
        END -- IF {range match}
        IF (@ch >= 0 AND @ch <= 8) OR @ch = 11 OR @ch = 12 OR (@ch >= 14 AND @ch <= 38) OR (@ch >= 40 AND @ch <= 91) OR (@ch >= 93 AND @ch <= 1114111)
        BEGIN
            SET @capture = @capture + @ch1
            IF @tch < 2048 SET @capture = @capture + @ch2
            IF @ch > 31 SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 92
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q4
        END -- IF {range match}
        GOTO next
    q2:
        IF @ch = 39
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        GOTO next
    q3:
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q4:
        IF @ch = 9
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 10
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            SET @lc = @lc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 13
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            
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
            GOTO q2
        END -- IF {range match}
        IF (@ch >= 0 AND @ch <= 8) OR @ch = 11 OR @ch = 12 OR (@ch >= 14 AND @ch <= 38) OR (@ch >= 40 AND @ch <= 1114111)
        BEGIN
            SET @capture = @capture + @ch1
            IF @tch < 2048 SET @capture = @capture + @ch2
            IF @ch > 31 SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 39
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        GOTO next
    q5:
        IF @ch = 39
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
    next:
        
        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
        SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
        IF @ch>31 SET @cc = @cc + 1
        
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
    END -- WHILE input loop
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchIntegerLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by IntegerLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <param name="line">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="column">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="tabWidth">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchIntegerLiteral] @value NVARCHAR(MAX), @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS
BEGIN
    DECLARE @adv INT
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
    DECLARE @index INT = 0
    DECLARE @absi BIGINT = 0
    DECLARE @ch BIGINT
    DECLARE @ch1 NCHAR
    DECLARE @ch2 NCHAR
    DECLARE @tch BIGINT
    DECLARE @state INT = 0
    DECLARE @toState INT = -1
    DECLARE @accept INT = -1
    DECLARE @capture NVARCHAR(MAX)
    DECLARE @blockEndId INT
    DECLARE @cursorPos BIGINT = @position
    DECLARE @absoluteIndex INT
    DECLARE @result INT = 0
    DECLARE @len INT = 0
    DECLARE @newIndex INT
    DECLARE @newCursorPos INT
    DECLARE @newCapture NVARCHAR(MAX)
    DECLARE @newCh BIGINT
    DECLARE @newTch BIGINT
    DECLARE @newAbsi BIGINT
    DECLARE @newCh1 NCHAR
    DECLARE @newCh2 NCHAR
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @newLC INT
    DECLARE @newCC INT
    CREATE TABLE #Results (
    	[AbsolutePosition] BIGINT NOT NULL,
    	[AbsoluteLength] INT NOT NULL,
    	[Position] BIGINT NOT NULL,
    	[Length] INT NOT NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [Line] INT NOT NULL,
        [Column] INT NOT NULL)
    
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
        
        SET @position = @cursorPos
        SET @absoluteIndex = @absi
        SET @line = @lc
        SET @column = @cc
    -- q0
        IF @ch = 48
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch >= 49 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        GOTO next
    q1:
        IF @ch >= 48 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF @ch = 120
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q6
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q2:
        IF @ch >= 48 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q3:
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q4
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q4:
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q5:
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q4
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q6:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q7
        END -- IF {range match}
        GOTO next
    q7:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q8
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q8:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q9
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q9:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q10
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q10:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q11
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q11:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q12
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q12:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q13
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q13:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q14
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q14:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q15
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q15:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q16
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q16:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q17
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q17:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q18
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q18:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q19
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q19:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q20
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q20:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q21
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q21:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 70) OR (@ch >= 97 AND @ch <= 102)
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q22
        END -- IF {range match}
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q22:
        IF @ch = 76 OR @ch = 108
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 85 OR @ch = 117
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
    next:
        
        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
        SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
        IF @ch>31 SET @cc = @cc + 1
        
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
    END -- WHILE input loop
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchFloatLiteral]
GO
-- <summary>Returns all occurances of the expression indicated by FloatLiteral within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <param name="line">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="column">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="tabWidth">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchFloatLiteral] @value NVARCHAR(MAX), @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS
BEGIN
    DECLARE @adv INT
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
    DECLARE @index INT = 0
    DECLARE @absi BIGINT = 0
    DECLARE @ch BIGINT
    DECLARE @ch1 NCHAR
    DECLARE @ch2 NCHAR
    DECLARE @tch BIGINT
    DECLARE @state INT = 0
    DECLARE @toState INT = -1
    DECLARE @accept INT = -1
    DECLARE @capture NVARCHAR(MAX)
    DECLARE @blockEndId INT
    DECLARE @cursorPos BIGINT = @position
    DECLARE @absoluteIndex INT
    DECLARE @result INT = 0
    DECLARE @len INT = 0
    DECLARE @newIndex INT
    DECLARE @newCursorPos INT
    DECLARE @newCapture NVARCHAR(MAX)
    DECLARE @newCh BIGINT
    DECLARE @newTch BIGINT
    DECLARE @newAbsi BIGINT
    DECLARE @newCh1 NCHAR
    DECLARE @newCh2 NCHAR
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @newLC INT
    DECLARE @newCC INT
    CREATE TABLE #Results (
    	[AbsolutePosition] BIGINT NOT NULL,
    	[AbsoluteLength] INT NOT NULL,
    	[Position] BIGINT NOT NULL,
    	[Length] INT NOT NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [Line] INT NOT NULL,
        [Column] INT NOT NULL)
    
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
        
        SET @position = @cursorPos
        SET @absoluteIndex = @absi
        SET @line = @lc
        SET @column = @cc
    -- q0
        IF @ch = 46
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch >= 48 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q7
        END -- IF {range match}
        GOTO next
    q1:
        IF @ch >= 48 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        GOTO next
    q2:
        IF @ch >= 48 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        IF @ch = 68 OR @ch = 70 OR @ch = 77 OR @ch = 100 OR @ch = 102 OR @ch = 109
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 69 OR @ch = 101
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q4
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q3:
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q4:
        IF @ch = 43 OR @ch = 45
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q5
        END -- IF {range match}
        IF @ch >= 48 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q6
        END -- IF {range match}
        GOTO next
    q5:
        IF @ch >= 48 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q6
        END -- IF {range match}
        GOTO next
    q6:
        IF @ch >= 48 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q6
        END -- IF {range match}
        IF @ch = 68 OR @ch = 70 OR @ch = 77 OR @ch = 100 OR @ch = 102 OR @ch = 109
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
        GOTO next
    q7:
        IF @ch = 46
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch >= 48 AND @ch <= 57
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q7
        END -- IF {range match}
        IF @ch = 68 OR @ch = 70 OR @ch = 77 OR @ch = 100 OR @ch = 102 OR @ch = 109
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q3
        END -- IF {range match}
        IF @ch = 69 OR @ch = 101
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q4
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
    next:
        
        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
        SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
        IF @ch>31 SET @cc = @cc + 1
        
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
    END -- WHILE input loop
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchWhitespace]
GO
-- <summary>Returns all occurances of the expression indicated by Whitespace within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <param name="line">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="column">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="tabWidth">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchWhitespace] @value NVARCHAR(MAX), @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS
BEGIN
    DECLARE @adv INT
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
    DECLARE @index INT = 0
    DECLARE @absi BIGINT = 0
    DECLARE @ch BIGINT
    DECLARE @ch1 NCHAR
    DECLARE @ch2 NCHAR
    DECLARE @tch BIGINT
    DECLARE @state INT = 0
    DECLARE @toState INT = -1
    DECLARE @accept INT = -1
    DECLARE @capture NVARCHAR(MAX)
    DECLARE @blockEndId INT
    DECLARE @cursorPos BIGINT = @position
    DECLARE @absoluteIndex INT
    DECLARE @result INT = 0
    DECLARE @len INT = 0
    DECLARE @newIndex INT
    DECLARE @newCursorPos INT
    DECLARE @newCapture NVARCHAR(MAX)
    DECLARE @newCh BIGINT
    DECLARE @newTch BIGINT
    DECLARE @newAbsi BIGINT
    DECLARE @newCh1 NCHAR
    DECLARE @newCh2 NCHAR
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @newLC INT
    DECLARE @newCC INT
    CREATE TABLE #Results (
    	[AbsolutePosition] BIGINT NOT NULL,
    	[AbsoluteLength] INT NOT NULL,
    	[Position] BIGINT NOT NULL,
    	[Length] INT NOT NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [Line] INT NOT NULL,
        [Column] INT NOT NULL)
    
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
        
        SET @position = @cursorPos
        SET @absoluteIndex = @absi
        SET @line = @lc
        SET @column = @cc
    -- q0
        IF @ch = 9
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 10
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            SET @lc = @lc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 13
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 11 OR @ch = 12 OR @ch = 32
        BEGIN
            SET @capture = @capture + @ch1
            IF @ch > 31 SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        GOTO next
    q1:
        IF @ch = 9
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 10
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            SET @lc = @lc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 13
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = 1
            
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
            GOTO q1
        END -- IF {range match}
        IF @ch = 11 OR @ch = 12 OR @ch = 32
        BEGIN
            SET @capture = @capture + @ch1
            IF @ch > 31 SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
    next:
        
        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
        SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
        IF @ch>31 SET @cc = @cc + 1
        
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
    END -- WHILE input loop
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchIdentifier]
GO
-- <summary>Returns all occurances of the expression indicated by Identifier within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <param name="line">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="column">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="tabWidth">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchIdentifier] @value NVARCHAR(MAX), @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS
BEGIN
    DECLARE @adv INT
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
    DECLARE @index INT = 0
    DECLARE @absi BIGINT = 0
    DECLARE @ch BIGINT
    DECLARE @ch1 NCHAR
    DECLARE @ch2 NCHAR
    DECLARE @tch BIGINT
    DECLARE @state INT = 0
    DECLARE @toState INT = -1
    DECLARE @accept INT = -1
    DECLARE @capture NVARCHAR(MAX)
    DECLARE @blockEndId INT
    DECLARE @cursorPos BIGINT = @position
    DECLARE @absoluteIndex INT
    DECLARE @result INT = 0
    DECLARE @len INT = 0
    DECLARE @newIndex INT
    DECLARE @newCursorPos INT
    DECLARE @newCapture NVARCHAR(MAX)
    DECLARE @newCh BIGINT
    DECLARE @newTch BIGINT
    DECLARE @newAbsi BIGINT
    DECLARE @newCh1 NCHAR
    DECLARE @newCh2 NCHAR
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @newLC INT
    DECLARE @newCC INT
    CREATE TABLE #Results (
    	[AbsolutePosition] BIGINT NOT NULL,
    	[AbsoluteLength] INT NOT NULL,
    	[Position] BIGINT NOT NULL,
    	[Length] INT NOT NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [Line] INT NOT NULL,
        [Column] INT NOT NULL)
    
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
        
        SET @position = @cursorPos
        SET @absoluteIndex = @absi
        SET @line = @lc
        SET @column = @cc
    -- q0
        IF (@ch >= 65 AND @ch <= 90) OR (@ch >= 97 AND @ch <= 122) OR @ch = 170 OR @ch = 181 OR @ch = 186 OR (@ch >= 192 AND @ch <= 214) OR (@ch >= 216 AND @ch <= 246) OR (@ch >= 248 AND @ch <= 705) OR (@ch >= 710 AND @ch <= 721) OR (@ch >= 736 AND @ch <= 740) OR @ch = 748 OR 
                @ch = 750 OR (@ch >= 880 AND @ch <= 884) OR @ch = 886 OR @ch = 887 OR (@ch >= 890 AND @ch <= 893) OR @ch = 895 OR @ch = 902 OR (@ch >= 904 AND @ch <= 906) OR @ch = 908 OR (@ch >= 910 AND @ch <= 929) OR (@ch >= 931 AND @ch <= 1013) OR 
                (@ch >= 1015 AND @ch <= 1153) OR (@ch >= 1162 AND @ch <= 1327) OR (@ch >= 1329 AND @ch <= 1366) OR @ch = 1369 OR (@ch >= 1377 AND @ch <= 1415) OR (@ch >= 1488 AND @ch <= 1514) OR (@ch >= 1520 AND @ch <= 1522) OR (@ch >= 1568 AND @ch <= 1610) OR @ch = 1646 OR @ch = 1647 OR (@ch >= 1649 AND @ch <= 1747) OR 
                @ch = 1749 OR @ch = 1765 OR @ch = 1766 OR @ch = 1774 OR @ch = 1775 OR (@ch >= 1786 AND @ch <= 1788) OR @ch = 1791 OR @ch = 1808 OR (@ch >= 1810 AND @ch <= 1839) OR (@ch >= 1869 AND @ch <= 1957) OR @ch = 1969 OR (@ch >= 1994 AND @ch <= 2026) OR 
                @ch = 2036 OR @ch = 2037 OR @ch = 2042 OR (@ch >= 2048 AND @ch <= 2069) OR @ch = 2074 OR @ch = 2084 OR @ch = 2088 OR (@ch >= 2112 AND @ch <= 2136) OR (@ch >= 2208 AND @ch <= 2228) OR (@ch >= 2308 AND @ch <= 2361) OR @ch = 2365 OR 
                @ch = 2384 OR (@ch >= 2392 AND @ch <= 2401) OR (@ch >= 2417 AND @ch <= 2432) OR (@ch >= 2437 AND @ch <= 2444) OR @ch = 2447 OR @ch = 2448 OR (@ch >= 2451 AND @ch <= 2472) OR (@ch >= 2474 AND @ch <= 2480) OR @ch = 2482 OR (@ch >= 2486 AND @ch <= 2489) OR @ch = 2493 OR 
                @ch = 2510 OR @ch = 2524 OR @ch = 2525 OR (@ch >= 2527 AND @ch <= 2529) OR @ch = 2544 OR @ch = 2545 OR (@ch >= 2565 AND @ch <= 2570) OR @ch = 2575 OR @ch = 2576 OR (@ch >= 2579 AND @ch <= 2600) OR (@ch >= 2602 AND @ch <= 2608) OR @ch = 2610 OR @ch = 2611 OR @ch = 2613 OR @ch = 2614 OR 
                @ch = 2616 OR @ch = 2617 OR (@ch >= 2649 AND @ch <= 2652) OR @ch = 2654 OR (@ch >= 2674 AND @ch <= 2676) OR (@ch >= 2693 AND @ch <= 2701) OR (@ch >= 2703 AND @ch <= 2705) OR (@ch >= 2707 AND @ch <= 2728) OR (@ch >= 2730 AND @ch <= 2736) OR @ch = 2738 OR @ch = 2739 OR (@ch >= 2741 AND @ch <= 2745) OR 
                @ch = 2749 OR @ch = 2768 OR @ch = 2784 OR @ch = 2785 OR @ch = 2809 OR (@ch >= 2821 AND @ch <= 2828) OR @ch = 2831 OR @ch = 2832 OR (@ch >= 2835 AND @ch <= 2856) OR (@ch >= 2858 AND @ch <= 2864) OR @ch = 2866 OR @ch = 2867 OR (@ch >= 2869 AND @ch <= 2873) OR 
                @ch = 2877 OR @ch = 2908 OR @ch = 2909 OR (@ch >= 2911 AND @ch <= 2913) OR @ch = 2929 OR @ch = 2947 OR (@ch >= 2949 AND @ch <= 2954) OR (@ch >= 2958 AND @ch <= 2960) OR (@ch >= 2962 AND @ch <= 2965) OR @ch = 2969 OR @ch = 2970 OR @ch = 2972 OR 
                @ch = 2974 OR @ch = 2975 OR @ch = 2979 OR @ch = 2980 OR (@ch >= 2984 AND @ch <= 2986) OR (@ch >= 2990 AND @ch <= 3001) OR @ch = 3024 OR (@ch >= 3077 AND @ch <= 3084) OR (@ch >= 3086 AND @ch <= 3088) OR (@ch >= 3090 AND @ch <= 3112) OR (@ch >= 3114 AND @ch <= 3129) OR @ch = 3133 OR 
                (@ch >= 3160 AND @ch <= 3162) OR @ch = 3168 OR @ch = 3169 OR (@ch >= 3205 AND @ch <= 3212) OR (@ch >= 3214 AND @ch <= 3216) OR (@ch >= 3218 AND @ch <= 3240) OR (@ch >= 3242 AND @ch <= 3251) OR (@ch >= 3253 AND @ch <= 3257) OR @ch = 3261 OR @ch = 3294 OR @ch = 3296 OR @ch = 3297 OR 
                @ch = 3313 OR @ch = 3314 OR (@ch >= 3333 AND @ch <= 3340) OR (@ch >= 3342 AND @ch <= 3344) OR (@ch >= 3346 AND @ch <= 3386) OR @ch = 3389 OR @ch = 3406 OR (@ch >= 3423 AND @ch <= 3425) OR (@ch >= 3450 AND @ch <= 3455) OR (@ch >= 3461 AND @ch <= 3478) OR (@ch >= 3482 AND @ch <= 3505) OR 
                (@ch >= 3507 AND @ch <= 3515) OR @ch = 3517 OR (@ch >= 3520 AND @ch <= 3526) OR (@ch >= 3585 AND @ch <= 3632) OR @ch = 3634 OR @ch = 3635 OR (@ch >= 3648 AND @ch <= 3654) OR @ch = 3713 OR @ch = 3714 OR @ch = 3716 OR @ch = 3719 OR @ch = 3720 OR @ch = 3722 OR 
                @ch = 3725 OR (@ch >= 3732 AND @ch <= 3735) OR (@ch >= 3737 AND @ch <= 3743) OR (@ch >= 3745 AND @ch <= 3747) OR @ch = 3749 OR @ch = 3751 OR @ch = 3754 OR @ch = 3755 OR (@ch >= 3757 AND @ch <= 3760) OR @ch = 3762 OR @ch = 3763 OR @ch = 3773 OR 
                (@ch >= 3776 AND @ch <= 3780) OR @ch = 3782 OR (@ch >= 3804 AND @ch <= 3807) OR @ch = 3840 OR (@ch >= 3904 AND @ch <= 3911) OR (@ch >= 3913 AND @ch <= 3948) OR (@ch >= 3976 AND @ch <= 3980) OR (@ch >= 4096 AND @ch <= 4138) OR @ch = 4159 OR (@ch >= 4176 AND @ch <= 4181) OR 
                (@ch >= 4186 AND @ch <= 4189) OR @ch = 4193 OR @ch = 4197 OR @ch = 4198 OR (@ch >= 4206 AND @ch <= 4208) OR (@ch >= 4213 AND @ch <= 4225) OR @ch = 4238 OR (@ch >= 4256 AND @ch <= 4293) OR @ch = 4295 OR @ch = 4301 OR (@ch >= 4304 AND @ch <= 4346) OR 
                (@ch >= 4348 AND @ch <= 4680) OR (@ch >= 4682 AND @ch <= 4685) OR (@ch >= 4688 AND @ch <= 4694) OR @ch = 4696 OR (@ch >= 4698 AND @ch <= 4701) OR (@ch >= 4704 AND @ch <= 4744) OR (@ch >= 4746 AND @ch <= 4749) OR (@ch >= 4752 AND @ch <= 4784) OR (@ch >= 4786 AND @ch <= 4789) OR (@ch >= 4792 AND @ch <= 4798) OR 
                @ch = 4800 OR (@ch >= 4802 AND @ch <= 4805) OR (@ch >= 4808 AND @ch <= 4822) OR (@ch >= 4824 AND @ch <= 4880) OR (@ch >= 4882 AND @ch <= 4885) OR (@ch >= 4888 AND @ch <= 4954) OR (@ch >= 4992 AND @ch <= 5007) OR (@ch >= 5024 AND @ch <= 5109) OR (@ch >= 5112 AND @ch <= 5117) OR (@ch >= 5121 AND @ch <= 5740) OR 
                (@ch >= 5743 AND @ch <= 5759) OR (@ch >= 5761 AND @ch <= 5786) OR (@ch >= 5792 AND @ch <= 5866) OR (@ch >= 5873 AND @ch <= 5880) OR (@ch >= 5888 AND @ch <= 5900) OR (@ch >= 5902 AND @ch <= 5905) OR (@ch >= 5920 AND @ch <= 5937) OR (@ch >= 5952 AND @ch <= 5969) OR (@ch >= 5984 AND @ch <= 5996) OR (@ch >= 5998 AND @ch <= 6000) OR 
                (@ch >= 6016 AND @ch <= 6067) OR @ch = 6103 OR @ch = 6108 OR (@ch >= 6176 AND @ch <= 6263) OR (@ch >= 6272 AND @ch <= 6312) OR @ch = 6314 OR (@ch >= 6320 AND @ch <= 6389) OR (@ch >= 6400 AND @ch <= 6430) OR (@ch >= 6480 AND @ch <= 6509) OR (@ch >= 6512 AND @ch <= 6516) OR 
                (@ch >= 6528 AND @ch <= 6571) OR (@ch >= 6576 AND @ch <= 6601) OR (@ch >= 6656 AND @ch <= 6678) OR (@ch >= 6688 AND @ch <= 6740) OR @ch = 6823 OR (@ch >= 6917 AND @ch <= 6963) OR (@ch >= 6981 AND @ch <= 6987) OR (@ch >= 7043 AND @ch <= 7072) OR @ch = 7086 OR @ch = 7087 OR (@ch >= 7098 AND @ch <= 7141) OR 
                (@ch >= 7168 AND @ch <= 7203) OR (@ch >= 7245 AND @ch <= 7247) OR (@ch >= 7258 AND @ch <= 7293) OR (@ch >= 7401 AND @ch <= 7404) OR (@ch >= 7406 AND @ch <= 7409) OR @ch = 7413 OR @ch = 7414 OR (@ch >= 7424 AND @ch <= 7615) OR (@ch >= 7680 AND @ch <= 7957) OR (@ch >= 7960 AND @ch <= 7965) OR (@ch >= 7968 AND @ch <= 8005) OR 
                (@ch >= 8008 AND @ch <= 8013) OR (@ch >= 8016 AND @ch <= 8023) OR @ch = 8025 OR @ch = 8027 OR @ch = 8029 OR (@ch >= 8031 AND @ch <= 8061) OR (@ch >= 8064 AND @ch <= 8116) OR (@ch >= 8118 AND @ch <= 8124) OR @ch = 8126 OR (@ch >= 8130 AND @ch <= 8132) OR 
                (@ch >= 8134 AND @ch <= 8140) OR (@ch >= 8144 AND @ch <= 8147) OR (@ch >= 8150 AND @ch <= 8155) OR (@ch >= 8160 AND @ch <= 8172) OR (@ch >= 8178 AND @ch <= 8180) OR (@ch >= 8182 AND @ch <= 8188) OR @ch = 8305 OR @ch = 8319 OR (@ch >= 8336 AND @ch <= 8348) OR @ch = 8450 OR 
                @ch = 8455 OR (@ch >= 8458 AND @ch <= 8467) OR @ch = 8469 OR (@ch >= 8473 AND @ch <= 8477) OR @ch = 8484 OR @ch = 8486 OR @ch = 8488 OR (@ch >= 8490 AND @ch <= 8493) OR (@ch >= 8495 AND @ch <= 8505) OR (@ch >= 8508 AND @ch <= 8511) OR 
                (@ch >= 8517 AND @ch <= 8521) OR @ch = 8526 OR @ch = 8579 OR @ch = 8580 OR (@ch >= 11264 AND @ch <= 11310) OR (@ch >= 11312 AND @ch <= 11358) OR (@ch >= 11360 AND @ch <= 11492) OR (@ch >= 11499 AND @ch <= 11502) OR @ch = 11506 OR @ch = 11507 OR (@ch >= 11520 AND @ch <= 11557) OR @ch = 11559 OR 
                @ch = 11565 OR (@ch >= 11568 AND @ch <= 11623) OR @ch = 11631 OR (@ch >= 11648 AND @ch <= 11670) OR (@ch >= 11680 AND @ch <= 11686) OR (@ch >= 11688 AND @ch <= 11694) OR (@ch >= 11696 AND @ch <= 11702) OR (@ch >= 11704 AND @ch <= 11710) OR (@ch >= 11712 AND @ch <= 11718) OR (@ch >= 11720 AND @ch <= 11726) OR 
                (@ch >= 11728 AND @ch <= 11734) OR (@ch >= 11736 AND @ch <= 11742) OR @ch = 11823 OR @ch = 12293 OR @ch = 12294 OR (@ch >= 12337 AND @ch <= 12341) OR @ch = 12347 OR @ch = 12348 OR (@ch >= 12353 AND @ch <= 12438) OR (@ch >= 12445 AND @ch <= 12447) OR (@ch >= 12449 AND @ch <= 12538) OR (@ch >= 12540 AND @ch <= 12543) OR 
                (@ch >= 12549 AND @ch <= 12589) OR (@ch >= 12593 AND @ch <= 12686) OR (@ch >= 12704 AND @ch <= 12730) OR (@ch >= 12784 AND @ch <= 12799) OR (@ch >= 13312 AND @ch <= 19893) OR (@ch >= 19968 AND @ch <= 40917) OR (@ch >= 40960 AND @ch <= 42124) OR (@ch >= 42192 AND @ch <= 42237) OR (@ch >= 42240 AND @ch <= 42508) OR (@ch >= 42512 AND @ch <= 42527) OR 
                @ch = 42538 OR @ch = 42539 OR (@ch >= 42560 AND @ch <= 42606) OR (@ch >= 42623 AND @ch <= 42653) OR (@ch >= 42656 AND @ch <= 42725) OR (@ch >= 42775 AND @ch <= 42783) OR (@ch >= 42786 AND @ch <= 42888) OR (@ch >= 42891 AND @ch <= 42925) OR (@ch >= 42928 AND @ch <= 42935) OR (@ch >= 42999 AND @ch <= 43009) OR (@ch >= 43011 AND @ch <= 43013) OR 
                (@ch >= 43015 AND @ch <= 43018) OR (@ch >= 43020 AND @ch <= 43042) OR (@ch >= 43072 AND @ch <= 43123) OR (@ch >= 43138 AND @ch <= 43187) OR (@ch >= 43250 AND @ch <= 43255) OR @ch = 43259 OR @ch = 43261 OR (@ch >= 43274 AND @ch <= 43301) OR (@ch >= 43312 AND @ch <= 43334) OR (@ch >= 43360 AND @ch <= 43388) OR 
                (@ch >= 43396 AND @ch <= 43442) OR @ch = 43471 OR (@ch >= 43488 AND @ch <= 43492) OR (@ch >= 43494 AND @ch <= 43503) OR (@ch >= 43514 AND @ch <= 43518) OR (@ch >= 43520 AND @ch <= 43560) OR (@ch >= 43584 AND @ch <= 43586) OR (@ch >= 43588 AND @ch <= 43595) OR (@ch >= 43616 AND @ch <= 43638) OR @ch = 43642 OR 
                (@ch >= 43646 AND @ch <= 43695) OR @ch = 43697 OR @ch = 43701 OR @ch = 43702 OR (@ch >= 43705 AND @ch <= 43709) OR @ch = 43712 OR @ch = 43714 OR (@ch >= 43739 AND @ch <= 43741) OR (@ch >= 43744 AND @ch <= 43754) OR (@ch >= 43762 AND @ch <= 43764) OR (@ch >= 43777 AND @ch <= 43782) OR 
                (@ch >= 43785 AND @ch <= 43790) OR (@ch >= 43793 AND @ch <= 43798) OR (@ch >= 43808 AND @ch <= 43814) OR (@ch >= 43816 AND @ch <= 43822) OR (@ch >= 43824 AND @ch <= 43866) OR (@ch >= 43868 AND @ch <= 43877) OR (@ch >= 43888 AND @ch <= 44002) OR (@ch >= 44032 AND @ch <= 55203) OR (@ch >= 55216 AND @ch <= 55238) OR (@ch >= 55243 AND @ch <= 55291) OR 
                (@ch >= 63744 AND @ch <= 64109) OR (@ch >= 64112 AND @ch <= 64217) OR (@ch >= 64256 AND @ch <= 64262) OR (@ch >= 64275 AND @ch <= 64279) OR @ch = 64285 OR (@ch >= 64287 AND @ch <= 64296) OR (@ch >= 64298 AND @ch <= 64310) OR (@ch >= 64312 AND @ch <= 64316) OR @ch = 64318 OR @ch = 64320 OR @ch = 64321 OR 
                @ch = 64323 OR @ch = 64324 OR (@ch >= 64326 AND @ch <= 64433) OR (@ch >= 64467 AND @ch <= 64829) OR (@ch >= 64848 AND @ch <= 64911) OR (@ch >= 64914 AND @ch <= 64967) OR (@ch >= 65008 AND @ch <= 65019) OR (@ch >= 65136 AND @ch <= 65140) OR (@ch >= 65142 AND @ch <= 65276) OR (@ch >= 65313 AND @ch <= 65338) OR (@ch >= 65345 AND @ch <= 65370) OR 
                (@ch >= 65382 AND @ch <= 65470) OR (@ch >= 65474 AND @ch <= 65479) OR (@ch >= 65482 AND @ch <= 65487) OR (@ch >= 65490 AND @ch <= 65495) OR (@ch >= 65498 AND @ch <= 65500) OR (@ch >= 65536 AND @ch <= 65547) OR (@ch >= 65549 AND @ch <= 65574) OR (@ch >= 65576 AND @ch <= 65594) OR @ch = 65596 OR @ch = 65597 OR (@ch >= 65599 AND @ch <= 65613) OR 
                (@ch >= 65616 AND @ch <= 65629) OR (@ch >= 65664 AND @ch <= 65786) OR (@ch >= 66176 AND @ch <= 66204) OR (@ch >= 66208 AND @ch <= 66256) OR (@ch >= 66304 AND @ch <= 66335) OR (@ch >= 66352 AND @ch <= 66368) OR (@ch >= 66370 AND @ch <= 66377) OR (@ch >= 66384 AND @ch <= 66421) OR (@ch >= 66432 AND @ch <= 66461) OR (@ch >= 66464 AND @ch <= 66499) OR 
                (@ch >= 66504 AND @ch <= 66511) OR (@ch >= 66560 AND @ch <= 66717) OR (@ch >= 66816 AND @ch <= 66855) OR (@ch >= 66864 AND @ch <= 66915) OR (@ch >= 67072 AND @ch <= 67382) OR (@ch >= 67392 AND @ch <= 67413) OR (@ch >= 67424 AND @ch <= 67431) OR (@ch >= 67584 AND @ch <= 67589) OR @ch = 67592 OR (@ch >= 67594 AND @ch <= 67637) OR 
                @ch = 67639 OR @ch = 67640 OR @ch = 67644 OR (@ch >= 67647 AND @ch <= 67669) OR (@ch >= 67680 AND @ch <= 67702) OR (@ch >= 67712 AND @ch <= 67742) OR (@ch >= 67808 AND @ch <= 67826) OR @ch = 67828 OR @ch = 67829 OR (@ch >= 67840 AND @ch <= 67861) OR (@ch >= 67872 AND @ch <= 67897) OR (@ch >= 67968 AND @ch <= 68023) OR 
                @ch = 68030 OR @ch = 68031 OR @ch = 68096 OR (@ch >= 68112 AND @ch <= 68115) OR (@ch >= 68117 AND @ch <= 68119) OR (@ch >= 68121 AND @ch <= 68147) OR (@ch >= 68192 AND @ch <= 68220) OR (@ch >= 68224 AND @ch <= 68252) OR (@ch >= 68288 AND @ch <= 68295) OR (@ch >= 68297 AND @ch <= 68324) OR (@ch >= 68352 AND @ch <= 68405) OR 
                (@ch >= 68416 AND @ch <= 68437) OR (@ch >= 68448 AND @ch <= 68466) OR (@ch >= 68480 AND @ch <= 68497) OR (@ch >= 68608 AND @ch <= 68680) OR (@ch >= 68736 AND @ch <= 68786) OR (@ch >= 68800 AND @ch <= 68850) OR (@ch >= 69635 AND @ch <= 69687) OR (@ch >= 69763 AND @ch <= 69807) OR (@ch >= 69840 AND @ch <= 69864) OR (@ch >= 69891 AND @ch <= 69926) OR 
                (@ch >= 69968 AND @ch <= 70002) OR @ch = 70006 OR (@ch >= 70019 AND @ch <= 70066) OR (@ch >= 70081 AND @ch <= 70084) OR @ch = 70106 OR @ch = 70108 OR (@ch >= 70144 AND @ch <= 70161) OR (@ch >= 70163 AND @ch <= 70187) OR (@ch >= 70272 AND @ch <= 70278) OR @ch = 70280 OR 
                (@ch >= 70282 AND @ch <= 70285) OR (@ch >= 70287 AND @ch <= 70301) OR (@ch >= 70303 AND @ch <= 70312) OR (@ch >= 70320 AND @ch <= 70366) OR (@ch >= 70405 AND @ch <= 70412) OR @ch = 70415 OR @ch = 70416 OR (@ch >= 70419 AND @ch <= 70440) OR (@ch >= 70442 AND @ch <= 70448) OR @ch = 70450 OR @ch = 70451 OR (@ch >= 70453 AND @ch <= 70457) OR 
                @ch = 70461 OR @ch = 70480 OR (@ch >= 70493 AND @ch <= 70497) OR (@ch >= 70784 AND @ch <= 70831) OR @ch = 70852 OR @ch = 70853 OR @ch = 70855 OR (@ch >= 71040 AND @ch <= 71086) OR (@ch >= 71128 AND @ch <= 71131) OR (@ch >= 71168 AND @ch <= 71215) OR @ch = 71236 OR 
                (@ch >= 71296 AND @ch <= 71338) OR (@ch >= 71424 AND @ch <= 71449) OR (@ch >= 71840 AND @ch <= 71903) OR @ch = 71935 OR (@ch >= 72384 AND @ch <= 72440) OR (@ch >= 73728 AND @ch <= 74649) OR (@ch >= 74880 AND @ch <= 75075) OR (@ch >= 77824 AND @ch <= 78894) OR (@ch >= 82944 AND @ch <= 83526) OR (@ch >= 92160 AND @ch <= 92728) OR 
                (@ch >= 92736 AND @ch <= 92766) OR (@ch >= 92880 AND @ch <= 92909) OR (@ch >= 92928 AND @ch <= 92975) OR (@ch >= 92992 AND @ch <= 92995) OR (@ch >= 93027 AND @ch <= 93047) OR (@ch >= 93053 AND @ch <= 93071) OR (@ch >= 93952 AND @ch <= 94020) OR @ch = 94032 OR (@ch >= 94099 AND @ch <= 94111) OR @ch = 110592 OR @ch = 110593 OR 
                (@ch >= 113664 AND @ch <= 113770) OR (@ch >= 113776 AND @ch <= 113788) OR (@ch >= 113792 AND @ch <= 113800) OR (@ch >= 113808 AND @ch <= 113817) OR (@ch >= 119808 AND @ch <= 119892) OR (@ch >= 119894 AND @ch <= 119964) OR @ch = 119966 OR @ch = 119967 OR @ch = 119970 OR @ch = 119973 OR @ch = 119974 OR (@ch >= 119977 AND @ch <= 119980) OR 
                (@ch >= 119982 AND @ch <= 119993) OR @ch = 119995 OR (@ch >= 119997 AND @ch <= 120003) OR (@ch >= 120005 AND @ch <= 120069) OR (@ch >= 120071 AND @ch <= 120074) OR (@ch >= 120077 AND @ch <= 120084) OR (@ch >= 120086 AND @ch <= 120092) OR (@ch >= 120094 AND @ch <= 120121) OR (@ch >= 120123 AND @ch <= 120126) OR (@ch >= 120128 AND @ch <= 120132) OR 
                @ch = 120134 OR (@ch >= 120138 AND @ch <= 120144) OR (@ch >= 120146 AND @ch <= 120485) OR (@ch >= 120488 AND @ch <= 120512) OR (@ch >= 120514 AND @ch <= 120538) OR (@ch >= 120540 AND @ch <= 120570) OR (@ch >= 120572 AND @ch <= 120596) OR (@ch >= 120598 AND @ch <= 120628) OR (@ch >= 120630 AND @ch <= 120654) OR (@ch >= 120656 AND @ch <= 120686) OR 
                (@ch >= 120688 AND @ch <= 120712) OR (@ch >= 120714 AND @ch <= 120744) OR (@ch >= 120746 AND @ch <= 120770) OR (@ch >= 120772 AND @ch <= 120779) OR (@ch >= 124928 AND @ch <= 125124) OR (@ch >= 126464 AND @ch <= 126467) OR (@ch >= 126469 AND @ch <= 126495) OR @ch = 126497 OR @ch = 126498 OR @ch = 126500 OR @ch = 126503 OR 
                (@ch >= 126505 AND @ch <= 126514) OR (@ch >= 126516 AND @ch <= 126519) OR @ch = 126521 OR @ch = 126523 OR @ch = 126530 OR @ch = 126535 OR @ch = 126537 OR @ch = 126539 OR (@ch >= 126541 AND @ch <= 126543) OR @ch = 126545 OR @ch = 126546 OR 
                @ch = 126548 OR @ch = 126551 OR @ch = 126553 OR @ch = 126555 OR @ch = 126557 OR @ch = 126559 OR @ch = 126561 OR @ch = 126562 OR @ch = 126564 OR (@ch >= 126567 AND @ch <= 126570) OR (@ch >= 126572 AND @ch <= 126578) OR 
                (@ch >= 126580 AND @ch <= 126583) OR (@ch >= 126585 AND @ch <= 126588) OR @ch = 126590 OR (@ch >= 126592 AND @ch <= 126601) OR (@ch >= 126603 AND @ch <= 126619) OR (@ch >= 126625 AND @ch <= 126627) OR (@ch >= 126629 AND @ch <= 126633) OR (@ch >= 126635 AND @ch <= 126651) OR (@ch >= 131072 AND @ch <= 173782) OR (@ch >= 173824 AND @ch <= 177972) OR 
                (@ch >= 177984 AND @ch <= 178205) OR (@ch >= 178208 AND @ch <= 183969) OR (@ch >= 194560 AND @ch <= 195101)
        BEGIN
            SET @capture = @capture + @ch1
            IF @tch < 2048 SET @capture = @capture + @ch2
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        GOTO next
    q1:
        IF (@ch >= 48 AND @ch <= 57) OR (@ch >= 65 AND @ch <= 90) OR (@ch >= 97 AND @ch <= 122) OR @ch = 170 OR @ch = 181 OR @ch = 186 OR (@ch >= 192 AND @ch <= 214) OR (@ch >= 216 AND @ch <= 246) OR (@ch >= 248 AND @ch <= 705) OR (@ch >= 710 AND @ch <= 721) OR (@ch >= 736 AND @ch <= 740) OR 
                @ch = 748 OR @ch = 750 OR (@ch >= 880 AND @ch <= 884) OR @ch = 886 OR @ch = 887 OR (@ch >= 890 AND @ch <= 893) OR @ch = 895 OR @ch = 902 OR (@ch >= 904 AND @ch <= 906) OR @ch = 908 OR (@ch >= 910 AND @ch <= 929) OR 
                (@ch >= 931 AND @ch <= 1013) OR (@ch >= 1015 AND @ch <= 1153) OR (@ch >= 1162 AND @ch <= 1327) OR (@ch >= 1329 AND @ch <= 1366) OR @ch = 1369 OR (@ch >= 1377 AND @ch <= 1415) OR (@ch >= 1488 AND @ch <= 1514) OR (@ch >= 1520 AND @ch <= 1522) OR (@ch >= 1568 AND @ch <= 1610) OR (@ch >= 1632 AND @ch <= 1641) OR 
                @ch = 1646 OR @ch = 1647 OR (@ch >= 1649 AND @ch <= 1747) OR @ch = 1749 OR @ch = 1765 OR @ch = 1766 OR (@ch >= 1774 AND @ch <= 1788) OR @ch = 1791 OR @ch = 1808 OR (@ch >= 1810 AND @ch <= 1839) OR (@ch >= 1869 AND @ch <= 1957) OR @ch = 1969 OR 
                (@ch >= 1984 AND @ch <= 2026) OR @ch = 2036 OR @ch = 2037 OR @ch = 2042 OR (@ch >= 2048 AND @ch <= 2069) OR @ch = 2074 OR @ch = 2084 OR @ch = 2088 OR (@ch >= 2112 AND @ch <= 2136) OR (@ch >= 2208 AND @ch <= 2228) OR (@ch >= 2308 AND @ch <= 2361) OR 
                @ch = 2365 OR @ch = 2384 OR (@ch >= 2392 AND @ch <= 2401) OR (@ch >= 2406 AND @ch <= 2415) OR (@ch >= 2417 AND @ch <= 2432) OR (@ch >= 2437 AND @ch <= 2444) OR @ch = 2447 OR @ch = 2448 OR (@ch >= 2451 AND @ch <= 2472) OR (@ch >= 2474 AND @ch <= 2480) OR @ch = 2482 OR 
                (@ch >= 2486 AND @ch <= 2489) OR @ch = 2493 OR @ch = 2510 OR @ch = 2524 OR @ch = 2525 OR (@ch >= 2527 AND @ch <= 2529) OR (@ch >= 2534 AND @ch <= 2545) OR (@ch >= 2565 AND @ch <= 2570) OR @ch = 2575 OR @ch = 2576 OR (@ch >= 2579 AND @ch <= 2600) OR (@ch >= 2602 AND @ch <= 2608) OR 
                @ch = 2610 OR @ch = 2611 OR @ch = 2613 OR @ch = 2614 OR @ch = 2616 OR @ch = 2617 OR (@ch >= 2649 AND @ch <= 2652) OR @ch = 2654 OR (@ch >= 2662 AND @ch <= 2671) OR (@ch >= 2674 AND @ch <= 2676) OR (@ch >= 2693 AND @ch <= 2701) OR (@ch >= 2703 AND @ch <= 2705) OR (@ch >= 2707 AND @ch <= 2728) OR 
                (@ch >= 2730 AND @ch <= 2736) OR @ch = 2738 OR @ch = 2739 OR (@ch >= 2741 AND @ch <= 2745) OR @ch = 2749 OR @ch = 2768 OR @ch = 2784 OR @ch = 2785 OR (@ch >= 2790 AND @ch <= 2799) OR @ch = 2809 OR (@ch >= 2821 AND @ch <= 2828) OR @ch = 2831 OR @ch = 2832 OR 
                (@ch >= 2835 AND @ch <= 2856) OR (@ch >= 2858 AND @ch <= 2864) OR @ch = 2866 OR @ch = 2867 OR (@ch >= 2869 AND @ch <= 2873) OR @ch = 2877 OR @ch = 2908 OR @ch = 2909 OR (@ch >= 2911 AND @ch <= 2913) OR (@ch >= 2918 AND @ch <= 2927) OR @ch = 2929 OR @ch = 2947 OR 
                (@ch >= 2949 AND @ch <= 2954) OR (@ch >= 2958 AND @ch <= 2960) OR (@ch >= 2962 AND @ch <= 2965) OR @ch = 2969 OR @ch = 2970 OR @ch = 2972 OR @ch = 2974 OR @ch = 2975 OR @ch = 2979 OR @ch = 2980 OR (@ch >= 2984 AND @ch <= 2986) OR (@ch >= 2990 AND @ch <= 3001) OR @ch = 3024 OR 
                (@ch >= 3046 AND @ch <= 3055) OR (@ch >= 3077 AND @ch <= 3084) OR (@ch >= 3086 AND @ch <= 3088) OR (@ch >= 3090 AND @ch <= 3112) OR (@ch >= 3114 AND @ch <= 3129) OR @ch = 3133 OR (@ch >= 3160 AND @ch <= 3162) OR @ch = 3168 OR @ch = 3169 OR (@ch >= 3174 AND @ch <= 3183) OR (@ch >= 3205 AND @ch <= 3212) OR 
                (@ch >= 3214 AND @ch <= 3216) OR (@ch >= 3218 AND @ch <= 3240) OR (@ch >= 3242 AND @ch <= 3251) OR (@ch >= 3253 AND @ch <= 3257) OR @ch = 3261 OR @ch = 3294 OR @ch = 3296 OR @ch = 3297 OR (@ch >= 3302 AND @ch <= 3311) OR @ch = 3313 OR @ch = 3314 OR (@ch >= 3333 AND @ch <= 3340) OR 
                (@ch >= 3342 AND @ch <= 3344) OR (@ch >= 3346 AND @ch <= 3386) OR @ch = 3389 OR @ch = 3406 OR (@ch >= 3423 AND @ch <= 3425) OR (@ch >= 3430 AND @ch <= 3439) OR (@ch >= 3450 AND @ch <= 3455) OR (@ch >= 3461 AND @ch <= 3478) OR (@ch >= 3482 AND @ch <= 3505) OR (@ch >= 3507 AND @ch <= 3515) OR 
                @ch = 3517 OR (@ch >= 3520 AND @ch <= 3526) OR (@ch >= 3558 AND @ch <= 3567) OR (@ch >= 3585 AND @ch <= 3632) OR @ch = 3634 OR @ch = 3635 OR (@ch >= 3648 AND @ch <= 3654) OR (@ch >= 3664 AND @ch <= 3673) OR @ch = 3713 OR @ch = 3714 OR @ch = 3716 OR @ch = 3719 OR @ch = 3720 OR 
                @ch = 3722 OR @ch = 3725 OR (@ch >= 3732 AND @ch <= 3735) OR (@ch >= 3737 AND @ch <= 3743) OR (@ch >= 3745 AND @ch <= 3747) OR @ch = 3749 OR @ch = 3751 OR @ch = 3754 OR @ch = 3755 OR (@ch >= 3757 AND @ch <= 3760) OR @ch = 3762 OR @ch = 3763 OR 
                @ch = 3773 OR (@ch >= 3776 AND @ch <= 3780) OR @ch = 3782 OR (@ch >= 3792 AND @ch <= 3801) OR (@ch >= 3804 AND @ch <= 3807) OR @ch = 3840 OR (@ch >= 3872 AND @ch <= 3881) OR (@ch >= 3904 AND @ch <= 3911) OR (@ch >= 3913 AND @ch <= 3948) OR (@ch >= 3976 AND @ch <= 3980) OR 
                (@ch >= 4096 AND @ch <= 4138) OR (@ch >= 4159 AND @ch <= 4169) OR (@ch >= 4176 AND @ch <= 4181) OR (@ch >= 4186 AND @ch <= 4189) OR @ch = 4193 OR @ch = 4197 OR @ch = 4198 OR (@ch >= 4206 AND @ch <= 4208) OR (@ch >= 4213 AND @ch <= 4225) OR @ch = 4238 OR (@ch >= 4240 AND @ch <= 4249) OR 
                (@ch >= 4256 AND @ch <= 4293) OR @ch = 4295 OR @ch = 4301 OR (@ch >= 4304 AND @ch <= 4346) OR (@ch >= 4348 AND @ch <= 4680) OR (@ch >= 4682 AND @ch <= 4685) OR (@ch >= 4688 AND @ch <= 4694) OR @ch = 4696 OR (@ch >= 4698 AND @ch <= 4701) OR (@ch >= 4704 AND @ch <= 4744) OR 
                (@ch >= 4746 AND @ch <= 4749) OR (@ch >= 4752 AND @ch <= 4784) OR (@ch >= 4786 AND @ch <= 4789) OR (@ch >= 4792 AND @ch <= 4798) OR @ch = 4800 OR (@ch >= 4802 AND @ch <= 4805) OR (@ch >= 4808 AND @ch <= 4822) OR (@ch >= 4824 AND @ch <= 4880) OR (@ch >= 4882 AND @ch <= 4885) OR (@ch >= 4888 AND @ch <= 4954) OR 
                (@ch >= 4992 AND @ch <= 5007) OR (@ch >= 5024 AND @ch <= 5109) OR (@ch >= 5112 AND @ch <= 5117) OR (@ch >= 5121 AND @ch <= 5740) OR (@ch >= 5743 AND @ch <= 5759) OR (@ch >= 5761 AND @ch <= 5786) OR (@ch >= 5792 AND @ch <= 5866) OR (@ch >= 5873 AND @ch <= 5880) OR (@ch >= 5888 AND @ch <= 5900) OR (@ch >= 5902 AND @ch <= 5905) OR 
                (@ch >= 5920 AND @ch <= 5937) OR (@ch >= 5952 AND @ch <= 5969) OR (@ch >= 5984 AND @ch <= 5996) OR (@ch >= 5998 AND @ch <= 6000) OR (@ch >= 6016 AND @ch <= 6067) OR @ch = 6103 OR @ch = 6108 OR (@ch >= 6112 AND @ch <= 6121) OR (@ch >= 6160 AND @ch <= 6169) OR (@ch >= 6176 AND @ch <= 6263) OR 
                (@ch >= 6272 AND @ch <= 6312) OR @ch = 6314 OR (@ch >= 6320 AND @ch <= 6389) OR (@ch >= 6400 AND @ch <= 6430) OR (@ch >= 6470 AND @ch <= 6509) OR (@ch >= 6512 AND @ch <= 6516) OR (@ch >= 6528 AND @ch <= 6571) OR (@ch >= 6576 AND @ch <= 6601) OR (@ch >= 6608 AND @ch <= 6617) OR (@ch >= 6656 AND @ch <= 6678) OR 
                (@ch >= 6688 AND @ch <= 6740) OR (@ch >= 6784 AND @ch <= 6793) OR (@ch >= 6800 AND @ch <= 6809) OR @ch = 6823 OR (@ch >= 6917 AND @ch <= 6963) OR (@ch >= 6981 AND @ch <= 6987) OR (@ch >= 6992 AND @ch <= 7001) OR (@ch >= 7043 AND @ch <= 7072) OR (@ch >= 7086 AND @ch <= 7141) OR (@ch >= 7168 AND @ch <= 7203) OR 
                (@ch >= 7232 AND @ch <= 7241) OR (@ch >= 7245 AND @ch <= 7293) OR (@ch >= 7401 AND @ch <= 7404) OR (@ch >= 7406 AND @ch <= 7409) OR @ch = 7413 OR @ch = 7414 OR (@ch >= 7424 AND @ch <= 7615) OR (@ch >= 7680 AND @ch <= 7957) OR (@ch >= 7960 AND @ch <= 7965) OR (@ch >= 7968 AND @ch <= 8005) OR (@ch >= 8008 AND @ch <= 8013) OR 
                (@ch >= 8016 AND @ch <= 8023) OR @ch = 8025 OR @ch = 8027 OR @ch = 8029 OR (@ch >= 8031 AND @ch <= 8061) OR (@ch >= 8064 AND @ch <= 8116) OR (@ch >= 8118 AND @ch <= 8124) OR @ch = 8126 OR (@ch >= 8130 AND @ch <= 8132) OR (@ch >= 8134 AND @ch <= 8140) OR 
                (@ch >= 8144 AND @ch <= 8147) OR (@ch >= 8150 AND @ch <= 8155) OR (@ch >= 8160 AND @ch <= 8172) OR (@ch >= 8178 AND @ch <= 8180) OR (@ch >= 8182 AND @ch <= 8188) OR @ch = 8305 OR @ch = 8319 OR (@ch >= 8336 AND @ch <= 8348) OR @ch = 8450 OR @ch = 8455 OR 
                (@ch >= 8458 AND @ch <= 8467) OR @ch = 8469 OR (@ch >= 8473 AND @ch <= 8477) OR @ch = 8484 OR @ch = 8486 OR @ch = 8488 OR (@ch >= 8490 AND @ch <= 8493) OR (@ch >= 8495 AND @ch <= 8505) OR (@ch >= 8508 AND @ch <= 8511) OR (@ch >= 8517 AND @ch <= 8521) OR 
                @ch = 8526 OR @ch = 8579 OR @ch = 8580 OR (@ch >= 11264 AND @ch <= 11310) OR (@ch >= 11312 AND @ch <= 11358) OR (@ch >= 11360 AND @ch <= 11492) OR (@ch >= 11499 AND @ch <= 11502) OR @ch = 11506 OR @ch = 11507 OR (@ch >= 11520 AND @ch <= 11557) OR @ch = 11559 OR @ch = 11565 OR 
                (@ch >= 11568 AND @ch <= 11623) OR @ch = 11631 OR (@ch >= 11648 AND @ch <= 11670) OR (@ch >= 11680 AND @ch <= 11686) OR (@ch >= 11688 AND @ch <= 11694) OR (@ch >= 11696 AND @ch <= 11702) OR (@ch >= 11704 AND @ch <= 11710) OR (@ch >= 11712 AND @ch <= 11718) OR (@ch >= 11720 AND @ch <= 11726) OR (@ch >= 11728 AND @ch <= 11734) OR 
                (@ch >= 11736 AND @ch <= 11742) OR @ch = 11823 OR @ch = 12293 OR @ch = 12294 OR (@ch >= 12337 AND @ch <= 12341) OR @ch = 12347 OR @ch = 12348 OR (@ch >= 12353 AND @ch <= 12438) OR (@ch >= 12445 AND @ch <= 12447) OR (@ch >= 12449 AND @ch <= 12538) OR (@ch >= 12540 AND @ch <= 12543) OR (@ch >= 12549 AND @ch <= 12589) OR 
                (@ch >= 12593 AND @ch <= 12686) OR (@ch >= 12704 AND @ch <= 12730) OR (@ch >= 12784 AND @ch <= 12799) OR (@ch >= 13312 AND @ch <= 19893) OR (@ch >= 19968 AND @ch <= 40917) OR (@ch >= 40960 AND @ch <= 42124) OR (@ch >= 42192 AND @ch <= 42237) OR (@ch >= 42240 AND @ch <= 42508) OR (@ch >= 42512 AND @ch <= 42539) OR (@ch >= 42560 AND @ch <= 42606) OR 
                (@ch >= 42623 AND @ch <= 42653) OR (@ch >= 42656 AND @ch <= 42725) OR (@ch >= 42775 AND @ch <= 42783) OR (@ch >= 42786 AND @ch <= 42888) OR (@ch >= 42891 AND @ch <= 42925) OR (@ch >= 42928 AND @ch <= 42935) OR (@ch >= 42999 AND @ch <= 43009) OR (@ch >= 43011 AND @ch <= 43013) OR (@ch >= 43015 AND @ch <= 43018) OR (@ch >= 43020 AND @ch <= 43042) OR 
                (@ch >= 43072 AND @ch <= 43123) OR (@ch >= 43138 AND @ch <= 43187) OR (@ch >= 43216 AND @ch <= 43225) OR (@ch >= 43250 AND @ch <= 43255) OR @ch = 43259 OR @ch = 43261 OR (@ch >= 43264 AND @ch <= 43301) OR (@ch >= 43312 AND @ch <= 43334) OR (@ch >= 43360 AND @ch <= 43388) OR (@ch >= 43396 AND @ch <= 43442) OR 
                (@ch >= 43471 AND @ch <= 43481) OR (@ch >= 43488 AND @ch <= 43492) OR (@ch >= 43494 AND @ch <= 43518) OR (@ch >= 43520 AND @ch <= 43560) OR (@ch >= 43584 AND @ch <= 43586) OR (@ch >= 43588 AND @ch <= 43595) OR (@ch >= 43600 AND @ch <= 43609) OR (@ch >= 43616 AND @ch <= 43638) OR @ch = 43642 OR (@ch >= 43646 AND @ch <= 43695) OR 
                @ch = 43697 OR @ch = 43701 OR @ch = 43702 OR (@ch >= 43705 AND @ch <= 43709) OR @ch = 43712 OR @ch = 43714 OR (@ch >= 43739 AND @ch <= 43741) OR (@ch >= 43744 AND @ch <= 43754) OR (@ch >= 43762 AND @ch <= 43764) OR (@ch >= 43777 AND @ch <= 43782) OR (@ch >= 43785 AND @ch <= 43790) OR 
                (@ch >= 43793 AND @ch <= 43798) OR (@ch >= 43808 AND @ch <= 43814) OR (@ch >= 43816 AND @ch <= 43822) OR (@ch >= 43824 AND @ch <= 43866) OR (@ch >= 43868 AND @ch <= 43877) OR (@ch >= 43888 AND @ch <= 44002) OR (@ch >= 44016 AND @ch <= 44025) OR (@ch >= 44032 AND @ch <= 55203) OR (@ch >= 55216 AND @ch <= 55238) OR (@ch >= 55243 AND @ch <= 55291) OR 
                (@ch >= 63744 AND @ch <= 64109) OR (@ch >= 64112 AND @ch <= 64217) OR (@ch >= 64256 AND @ch <= 64262) OR (@ch >= 64275 AND @ch <= 64279) OR @ch = 64285 OR (@ch >= 64287 AND @ch <= 64296) OR (@ch >= 64298 AND @ch <= 64310) OR (@ch >= 64312 AND @ch <= 64316) OR @ch = 64318 OR @ch = 64320 OR @ch = 64321 OR 
                @ch = 64323 OR @ch = 64324 OR (@ch >= 64326 AND @ch <= 64433) OR (@ch >= 64467 AND @ch <= 64829) OR (@ch >= 64848 AND @ch <= 64911) OR (@ch >= 64914 AND @ch <= 64967) OR (@ch >= 65008 AND @ch <= 65019) OR (@ch >= 65136 AND @ch <= 65140) OR (@ch >= 65142 AND @ch <= 65276) OR (@ch >= 65296 AND @ch <= 65305) OR (@ch >= 65313 AND @ch <= 65338) OR 
                (@ch >= 65345 AND @ch <= 65370) OR (@ch >= 65382 AND @ch <= 65470) OR (@ch >= 65474 AND @ch <= 65479) OR (@ch >= 65482 AND @ch <= 65487) OR (@ch >= 65490 AND @ch <= 65495) OR (@ch >= 65498 AND @ch <= 65500) OR (@ch >= 65536 AND @ch <= 65547) OR (@ch >= 65549 AND @ch <= 65574) OR (@ch >= 65576 AND @ch <= 65594) OR @ch = 65596 OR @ch = 65597 OR 
                (@ch >= 65599 AND @ch <= 65613) OR (@ch >= 65616 AND @ch <= 65629) OR (@ch >= 65664 AND @ch <= 65786) OR (@ch >= 66176 AND @ch <= 66204) OR (@ch >= 66208 AND @ch <= 66256) OR (@ch >= 66304 AND @ch <= 66335) OR (@ch >= 66352 AND @ch <= 66368) OR (@ch >= 66370 AND @ch <= 66377) OR (@ch >= 66384 AND @ch <= 66421) OR (@ch >= 66432 AND @ch <= 66461) OR 
                (@ch >= 66464 AND @ch <= 66499) OR (@ch >= 66504 AND @ch <= 66511) OR (@ch >= 66560 AND @ch <= 66717) OR (@ch >= 66720 AND @ch <= 66729) OR (@ch >= 66816 AND @ch <= 66855) OR (@ch >= 66864 AND @ch <= 66915) OR (@ch >= 67072 AND @ch <= 67382) OR (@ch >= 67392 AND @ch <= 67413) OR (@ch >= 67424 AND @ch <= 67431) OR (@ch >= 67584 AND @ch <= 67589) OR 
                @ch = 67592 OR (@ch >= 67594 AND @ch <= 67637) OR @ch = 67639 OR @ch = 67640 OR @ch = 67644 OR (@ch >= 67647 AND @ch <= 67669) OR (@ch >= 67680 AND @ch <= 67702) OR (@ch >= 67712 AND @ch <= 67742) OR (@ch >= 67808 AND @ch <= 67826) OR @ch = 67828 OR @ch = 67829 OR (@ch >= 67840 AND @ch <= 67861) OR 
                (@ch >= 67872 AND @ch <= 67897) OR (@ch >= 67968 AND @ch <= 68023) OR @ch = 68030 OR @ch = 68031 OR @ch = 68096 OR (@ch >= 68112 AND @ch <= 68115) OR (@ch >= 68117 AND @ch <= 68119) OR (@ch >= 68121 AND @ch <= 68147) OR (@ch >= 68192 AND @ch <= 68220) OR (@ch >= 68224 AND @ch <= 68252) OR (@ch >= 68288 AND @ch <= 68295) OR 
                (@ch >= 68297 AND @ch <= 68324) OR (@ch >= 68352 AND @ch <= 68405) OR (@ch >= 68416 AND @ch <= 68437) OR (@ch >= 68448 AND @ch <= 68466) OR (@ch >= 68480 AND @ch <= 68497) OR (@ch >= 68608 AND @ch <= 68680) OR (@ch >= 68736 AND @ch <= 68786) OR (@ch >= 68800 AND @ch <= 68850) OR (@ch >= 69635 AND @ch <= 69687) OR (@ch >= 69734 AND @ch <= 69743) OR 
                (@ch >= 69763 AND @ch <= 69807) OR (@ch >= 69840 AND @ch <= 69864) OR (@ch >= 69872 AND @ch <= 69881) OR (@ch >= 69891 AND @ch <= 69926) OR (@ch >= 69942 AND @ch <= 69951) OR (@ch >= 69968 AND @ch <= 70002) OR @ch = 70006 OR (@ch >= 70019 AND @ch <= 70066) OR (@ch >= 70081 AND @ch <= 70084) OR (@ch >= 70096 AND @ch <= 70106) OR 
                @ch = 70108 OR (@ch >= 70144 AND @ch <= 70161) OR (@ch >= 70163 AND @ch <= 70187) OR (@ch >= 70272 AND @ch <= 70278) OR @ch = 70280 OR (@ch >= 70282 AND @ch <= 70285) OR (@ch >= 70287 AND @ch <= 70301) OR (@ch >= 70303 AND @ch <= 70312) OR (@ch >= 70320 AND @ch <= 70366) OR (@ch >= 70384 AND @ch <= 70393) OR 
                (@ch >= 70405 AND @ch <= 70412) OR @ch = 70415 OR @ch = 70416 OR (@ch >= 70419 AND @ch <= 70440) OR (@ch >= 70442 AND @ch <= 70448) OR @ch = 70450 OR @ch = 70451 OR (@ch >= 70453 AND @ch <= 70457) OR @ch = 70461 OR @ch = 70480 OR (@ch >= 70493 AND @ch <= 70497) OR (@ch >= 70784 AND @ch <= 70831) OR 
                @ch = 70852 OR @ch = 70853 OR @ch = 70855 OR (@ch >= 70864 AND @ch <= 70873) OR (@ch >= 71040 AND @ch <= 71086) OR (@ch >= 71128 AND @ch <= 71131) OR (@ch >= 71168 AND @ch <= 71215) OR @ch = 71236 OR (@ch >= 71248 AND @ch <= 71257) OR (@ch >= 71296 AND @ch <= 71338) OR (@ch >= 71360 AND @ch <= 71369) OR 
                (@ch >= 71424 AND @ch <= 71449) OR (@ch >= 71472 AND @ch <= 71481) OR (@ch >= 71840 AND @ch <= 71913) OR @ch = 71935 OR (@ch >= 72384 AND @ch <= 72440) OR (@ch >= 73728 AND @ch <= 74649) OR (@ch >= 74880 AND @ch <= 75075) OR (@ch >= 77824 AND @ch <= 78894) OR (@ch >= 82944 AND @ch <= 83526) OR (@ch >= 92160 AND @ch <= 92728) OR 
                (@ch >= 92736 AND @ch <= 92766) OR (@ch >= 92768 AND @ch <= 92777) OR (@ch >= 92880 AND @ch <= 92909) OR (@ch >= 92928 AND @ch <= 92975) OR (@ch >= 92992 AND @ch <= 92995) OR (@ch >= 93008 AND @ch <= 93017) OR (@ch >= 93027 AND @ch <= 93047) OR (@ch >= 93053 AND @ch <= 93071) OR (@ch >= 93952 AND @ch <= 94020) OR @ch = 94032 OR 
                (@ch >= 94099 AND @ch <= 94111) OR @ch = 110592 OR @ch = 110593 OR (@ch >= 113664 AND @ch <= 113770) OR (@ch >= 113776 AND @ch <= 113788) OR (@ch >= 113792 AND @ch <= 113800) OR (@ch >= 113808 AND @ch <= 113817) OR (@ch >= 119808 AND @ch <= 119892) OR (@ch >= 119894 AND @ch <= 119964) OR @ch = 119966 OR @ch = 119967 OR @ch = 119970 OR 
                @ch = 119973 OR @ch = 119974 OR (@ch >= 119977 AND @ch <= 119980) OR (@ch >= 119982 AND @ch <= 119993) OR @ch = 119995 OR (@ch >= 119997 AND @ch <= 120003) OR (@ch >= 120005 AND @ch <= 120069) OR (@ch >= 120071 AND @ch <= 120074) OR (@ch >= 120077 AND @ch <= 120084) OR (@ch >= 120086 AND @ch <= 120092) OR (@ch >= 120094 AND @ch <= 120121) OR 
                (@ch >= 120123 AND @ch <= 120126) OR (@ch >= 120128 AND @ch <= 120132) OR @ch = 120134 OR (@ch >= 120138 AND @ch <= 120144) OR (@ch >= 120146 AND @ch <= 120485) OR (@ch >= 120488 AND @ch <= 120512) OR (@ch >= 120514 AND @ch <= 120538) OR (@ch >= 120540 AND @ch <= 120570) OR (@ch >= 120572 AND @ch <= 120596) OR (@ch >= 120598 AND @ch <= 120628) OR 
                (@ch >= 120630 AND @ch <= 120654) OR (@ch >= 120656 AND @ch <= 120686) OR (@ch >= 120688 AND @ch <= 120712) OR (@ch >= 120714 AND @ch <= 120744) OR (@ch >= 120746 AND @ch <= 120770) OR (@ch >= 120772 AND @ch <= 120779) OR (@ch >= 120782 AND @ch <= 120831) OR (@ch >= 124928 AND @ch <= 125124) OR (@ch >= 126464 AND @ch <= 126467) OR (@ch >= 126469 AND @ch <= 126495) OR 
                @ch = 126497 OR @ch = 126498 OR @ch = 126500 OR @ch = 126503 OR (@ch >= 126505 AND @ch <= 126514) OR (@ch >= 126516 AND @ch <= 126519) OR @ch = 126521 OR @ch = 126523 OR @ch = 126530 OR @ch = 126535 OR @ch = 126537 OR 
                @ch = 126539 OR (@ch >= 126541 AND @ch <= 126543) OR @ch = 126545 OR @ch = 126546 OR @ch = 126548 OR @ch = 126551 OR @ch = 126553 OR @ch = 126555 OR @ch = 126557 OR @ch = 126559 OR @ch = 126561 OR @ch = 126562 OR 
                @ch = 126564 OR (@ch >= 126567 AND @ch <= 126570) OR (@ch >= 126572 AND @ch <= 126578) OR (@ch >= 126580 AND @ch <= 126583) OR (@ch >= 126585 AND @ch <= 126588) OR @ch = 126590 OR (@ch >= 126592 AND @ch <= 126601) OR (@ch >= 126603 AND @ch <= 126619) OR (@ch >= 126625 AND @ch <= 126627) OR (@ch >= 126629 AND @ch <= 126633) OR 
                (@ch >= 126635 AND @ch <= 126651) OR (@ch >= 131072 AND @ch <= 173782) OR (@ch >= 173824 AND @ch <= 177972) OR (@ch >= 177984 AND @ch <= 178205) OR (@ch >= 178208 AND @ch <= 183969) OR (@ch >= 194560 AND @ch <= 195101)
        BEGIN
            SET @capture = @capture + @ch1
            IF @tch < 2048 SET @capture = @capture + @ch2
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        IF DATALENGTH(@capture) > 0
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END
    next:
        
        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
        SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
        IF @ch>31 SET @cc = @cc + 1
        
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
    END -- WHILE input loop
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE
GO
-- Matches the block end for CommentBlock
DROP PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchCommentBlockBlockEnd]
GO
CREATE PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchCommentBlockBlockEnd] @value NVARCHAR(MAX), @ch BIGINT, @tch BIGINT, @ch1 NCHAR, @ch2 NCHAR, @cursorPos BIGINT, @index INT, @absi BIGINT, @valueEnd INT, @capture NVARCHAR(MAX), @lc INT, @cc INT, @tabWidth INT, @newIndex INT OUTPUT, @newCursorPos BIGINT OUTPUT, @newAbsi BIGINT OUTPUT, @newCapture NVARCHAR(MAX) OUTPUT, @newCh BIGINT OUTPUT, @newTch BIGINT OUTPUT, @newCh1 NCHAR OUTPUT, @newCh2 NCHAR OUTPUT, @newLC INT OUTPUT, @newCC INT OUTPUT
AS
BEGIN
    DECLARE @adv INT
    WHILE @ch <> -1
    BEGIN
    -- q0
        IF @ch = 42
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        GOTO next
    q1:
        IF @ch = 47
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        GOTO next
    q2:
        SET @newCh = @ch
        SET @newCapture = @capture
        SET @newTch = @tch
        SET @newCh1 = @ch1
        SET @newCh2 = @ch2
        SET @newIndex = @index
        SET @newCursorPos = @cursorPos
        SET @newAbsi = @absi
        SET @newLC = @lc
        SET @newCC = @cc
        RETURN 1
    next:
        
        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
        SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
        IF @ch>31 SET @cc = @cc + 1
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
    END -- WHILE input loop
    SET @newCh = @ch
    SET @newCapture = @capture
    SET @newTch = @tch
    SET @newCh1 = @ch1
    SET @newCh2 = @ch2
    SET @newIndex = @index
    SET @newCursorPos = @cursorPos
    SET @newAbsi = @absi
    SET @newLC = @lc
    SET @newCC = @cc
    RETURN 0
END -- CREATE PROCEDURE
GO
DROP PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchCommentBlock]
GO
-- <summary>Returns all occurances of the expression indicated by CommentBlock within <paramref name="value"/></summary>
-- <param name="value">The text to search</param>
-- <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
-- <param name="line">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="column">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>
-- <param name="tabWidth">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>
-- <remarks>The matches contain both the absolute native character position within <paramref name="value"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
CREATE PROCEDURE [dbo].[SqlCompiledMatcherWithLines_MatchCommentBlock] @value NVARCHAR(MAX), @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4
AS
BEGIN
    DECLARE @adv INT
    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1
    DECLARE @index INT = 0
    DECLARE @absi BIGINT = 0
    DECLARE @ch BIGINT
    DECLARE @ch1 NCHAR
    DECLARE @ch2 NCHAR
    DECLARE @tch BIGINT
    DECLARE @state INT = 0
    DECLARE @toState INT = -1
    DECLARE @accept INT = -1
    DECLARE @capture NVARCHAR(MAX)
    DECLARE @blockEndId INT
    DECLARE @cursorPos BIGINT = @position
    DECLARE @absoluteIndex INT
    DECLARE @result INT = 0
    DECLARE @len INT = 0
    DECLARE @newIndex INT
    DECLARE @newCursorPos INT
    DECLARE @newCapture NVARCHAR(MAX)
    DECLARE @newCh BIGINT
    DECLARE @newTch BIGINT
    DECLARE @newAbsi BIGINT
    DECLARE @newCh1 NCHAR
    DECLARE @newCh2 NCHAR
    DECLARE @lc INT = @line
    DECLARE @cc INT = @column
    DECLARE @newLC INT
    DECLARE @newCC INT
    CREATE TABLE #Results (
    	[AbsolutePosition] BIGINT NOT NULL,
    	[AbsoluteLength] INT NOT NULL,
    	[Position] BIGINT NOT NULL,
    	[Length] INT NOT NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [Line] INT NOT NULL,
        [Column] INT NOT NULL)
    
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
        
        SET @position = @cursorPos
        SET @absoluteIndex = @absi
        SET @line = @lc
        SET @column = @cc
    -- q0
        IF @ch = 47
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q1
        END -- IF {range match}
        GOTO next
    q1:
        IF @ch = 42
        BEGIN
            SET @capture = @capture + @ch1
            SET @cc = @cc + 1
            
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
            GOTO q2
        END -- IF {range match}
        GOTO next
    q2:
        EXEC @len = [dbo].[SqlCompiledMatcherWithLines_MatchCommentBlockBlockEnd] @value = @value, @ch = @ch,  @tch = @tch, @ch1 = @ch1, @ch2 = @ch2, @cursorPos = @cursorPos, @index = @index, @absi = @absi, @valueEnd = @valueEnd, @capture = @capture, @lc = @lc, @cc = @cc, @tabwidth = @tabWidth, @newIndex = @newIndex OUTPUT, @newAbsi = @newAbsi OUTPUT, @newCursorPos = @newCursorPos OUTPUT, @newCapture = @newCapture OUTPUT, @newCh = @newCh OUTPUT, @newTch = @newTch OUTPUT, @newCh1 = @newCh1 OUTPUT, @newCh2 = @newCh2 OUTPUT, @newLC = @newLC OUTPUT, @newCC = @newCC OUTPUT
        SET @index = @newIndex
        SET @absi = @newAbsi
        SET @cursorPos = @newCursorPos
        SET @absi = @newAbsi
        SET @capture = @newCapture
        SET @ch = @newCh
        SET @tch = @newTch
        SET @ch1 = @newCh1
        SET @ch2 = @newCh2
        SET @lc = @newLC
        SET @cc = @newCC
        IF @len = 1
        BEGIN
            INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value], @line AS [Line], @column AS [Column]
        END -- IF matched block end
        CONTINUE
    next:
        
        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END
        SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END
        IF @ch>31 SET @cc = @cc + 1
        
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
    END -- WHILE input loop
    SELECT * FROM #Results
    DROP TABLE #Results
END -- CREATE PROCEDURE
GO

