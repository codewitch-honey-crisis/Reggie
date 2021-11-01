using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class SqlTableTokenizerGenerator {
        public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {

var rules = (IList<LexRule>)Arguments["rules"];
var ignoreCase = (bool)Arguments["ignorecase"];
var lineCounted = (bool)Arguments["lines"];
var inputFile = (string)Arguments["inputfile"];
var outputFile = (string)Arguments["outputfile"];
var stderr = (TextWriter)Arguments["stderr"];
var codeclass = (string)Arguments["codeclass"];
var codetoken = (string)Arguments["codetoken"];
var dot = (bool)Arguments["dot"];
var jpg = (bool)Arguments["jpg"];
var cwd = Path.GetDirectoryName(outputFile!=null?outputFile:inputFile);
var blockEnds = BuildBlockEnds(rules,inputFile,ignoreCase);
Arguments["blockEnds"]=blockEnds;
var symbolTable = BuildSymbolTable(rules);
Arguments["symbolTable"]=symbolTable;
var symbolFlags = BuildSymbolFlags(rules);
Arguments["symbolFlags"]=symbolFlags;
var lexer = BuildLexer(rules,inputFile,ignoreCase);
Arguments["lexer"]=lexer;
if(dot) {
    var opts = new F.FFA.DotGraphOptions();
    var fn = Path.Combine(cwd, codeclass + ".dot");
    stderr.WriteLine("Writing {0}...",fn);
    using(var sw=new StreamWriter(fn)) {
        lexer.WriteDotTo(sw,opts);
    }        
}
if(jpg) {
    var opts = new F.FFA.DotGraphOptions();
    var fn = Path.Combine(cwd, codeclass + ".jpg");
    stderr.WriteLine("Writing {0}...",fn);
    lexer.RenderToFile(fn,opts);
}
Run("SqlTableTokenizerCreateGenerator",Arguments,Response,0);
for(var k = 0;k<2;++k) { 
    bool istext = k==1;
            Response.Write("\r\nDROP PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("_");
            Response.Write(istext?"Text":"");
            Response.Write("Tokenize]\r\nGO");

    if(!lineCounted) {
            Response.Write("\r\nCREATE PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("_");
            Response.Write(istext?"Text":"");
            Response.Write("Tokenize] @value ");
            Response.Write(istext?"NTEXT":"NVARCHAR(MAX)");
            Response.Write(", @position BIGINT = 0");

    } else {
            Response.Write("\r\nCREATE PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("_");
            Response.Write(istext?"Text":"");
            Response.Write("Tokenize] @value ");
            Response.Write(istext?"NTEXT":"NVARCHAR(MAX)");
            Response.Write(", @position BIGINT = 0, @line INT = 1, @column INT = 1, @tabWidth INT = 4");

    }
            Response.Write("\r\nAS BEGIN\r\n    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\n\tDECLARE @index INT = 1\r\n\tDECLARE @absoluteIndex BIGINT = 0\r\n\tDECLARE @ch BIGINT\r\n\tDECLARE @ch1 NCHAR\r\n\tDECLARE @ch2 NCHAR\r\n\tDECLARE @t BIGINT\r\n\tDECLARE @state INT = 0\r\n\tDECLARE @toState INT = -1\r\n\tDECLARE @accept INT = -1\r\n\tDECLARE @cursorPos BIGINT = @position\r\n\tDECLARE @capture NVARCHAR(MAX) = N\'\'\r\n\tDECLARE @tmp NVARCHAR(MAX) = N\'\'\r\n\tDECLARE @blockId INT\r\n\tDECLARE @result INT = 0\r\n\tDECLARE @len INT = 0\r\n\tDECLARE @flags INT = 0\r\n\tDECLARE @matched INT = 0\r\n    DECLARE @errorIndex BIGINT\r\n    DECLARE @errorPos BIGINT\r\n    DECLARE @hasError INT = 0\r\n\tDECLARE @ai INT\r\n\tDECLARE @done INT = 0");
 
    if(lineCounted) {
            Response.Write("\r\n    DECLARE @lc INT = @line\r\n    DECLARE @cc INT = @column\r\n    DECLARE @errorLine INT\r\n    DECLARE @errorColumn INT");

    } // if(lineCounted) ... 
            Response.Write("\r\n    CREATE TABLE #Results (\r\n\t[AbsolutePosition] BIGINT NOT NULL,\r\n\t[AbsoluteLength] INT NOT NULL,\r\n\t[Position] BIGINT NOT NULL,\r\n\t[Length] INT NOT NULL,\r\n    [SymbolId] INT NOT NULL,");

    if(lineCounted) {
            Response.Write("\r\n    [Value] NVARCHAR(MAX) NOT NULL,\r\n    [Line] INT NOT NULL,\r\n    [Column] INT NOT NULL");

    } else {
            Response.Write("\r\n    [Value] NVARCHAR(MAX) NOT NULL");

    }
            Response.Write(")\r\n\tIF @index >= @valueEnd\r\n\tBEGIN \r\n\t\tSET @ch = -1\r\n\tEND\r\n\tELSE\r\n\tBEGIN\r\n\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\tSET @ch = UNICODE(@ch1)\r\n\t\tSET @t = @ch - 0xd800\r\n\t\tIF @t < 0 SET @t = @t + 2147483648\r\n\t\tIF @t < 2048\r\n\t\tBEGIN\r\n\t\t\tSET @ch = @ch * 1024\r\n\t\t\tSET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\tEND\r\n\tEND\r\n    SET @cursorPos = @position");

if(lineCounted) {
            Response.Write("\r\n\tSET @lc = @line\r\n\tSET @cc = @column");

}
            Response.Write("        \r\n    WHILE @ch <> -1\r\n\tBEGIN\r\n        SET @position = @cursorPos\r\n        SET @absoluteIndex = @index-1");

if(lineCounted) {
            Response.Write("\r\n        SET @line = @lc\r\n        SET @column = @cc");

}
            Response.Write("     \r\n        SET @done = 0\r\n        SET @state = 0\r\n        WHILE @done = 0\r\n        BEGIN\r\n            SET @done = 1\r\n\t\t\tSET @toState = -1\r\n\t\t\tSELECT @toState = [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[ToStateId] FROM [dbo].[");
            Response.Write(codeclass);
            Response.Write("State] INNER JOIN [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition] ON [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[StateId]=[dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[StateId] AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[BlockEndId]=-1 AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[StateId]=@state AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[BlockEndId] = -1 AND @ch BETWEEN [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[Min] AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[Max]\r\n\t\t\tIF @toState <> -1\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @done = 0\r\n\t\t\t\tSET @state = @toState");

		if(lineCounted) {
            Response.Write("\r\n\t\t\t\tSET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END\r\n\t\t\t\tSET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END\r\n\t\t\t\tIF @ch>31 SET @cc = @cc + 1");

		} // if(lineCounted) ...
            Response.Write("\r\n\t\t\t\tSET @capture = @capture + @ch1\r\n\t\t\t\tIF @t < 2048 SET @capture = @capture + @ch2\r\n\t\t\t\tSET @index = @index + 1\r\n\t\t\t\tIF @index >= @valueEnd\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = -1\r\n\t\t\t\t\tSET @done = 1\r\n\t\t\t\tEND\r\n\t\t\t\tELSE\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\t\tSET @t = @ch - 0xd800\r\n\t\t\t\t\tIF @t < 0 SET @t = @t + 2147483648\r\n\t\t\t\t\tIF @t < 2048\r\n\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\t\tEND\r\n\t\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\t\tEND\r\n\t\t\tEND\r\n\t\tEND\r\n\t\tSET @accept = -1\r\n\t\tSELECT @accept = [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[AcceptId], @flags = [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol].[Flags] FROM [dbo].[");
            Response.Write(codeclass);
            Response.Write("State] INNER JOIN [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol] ON [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[AcceptId] = [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol].[Id] WHERE [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[StateId] = @state AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[BlockEndId] = -1 AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[AcceptId] <> -1\r\n\t\tIF @accept <> -1 \r\n\t\tBEGIN\r\n\t\t\tIF @hasError = 1\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @hasError = 0\r\n\t\t\t\tSET @ai = CAST((@absoluteIndex - @errorIndex) AS INT)\r\n\t\t\t\tSET @tmp = SUBSTRING(@capture,1,@ai)\r\n\t\t\t\tSET @capture = SUBSTRING(@capture,@ai+1,(DATALENGTH(@capture)/2)-(@ai+1))");

if(lineCounted) {
            Response.Write("\r\n                INSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp, @errorLine, @errorColumn)");

} else {
            Response.Write("\r\n                INSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp)");

}
            Response.Write("\r\n\t\t\tEND -- IF @hasError = 1\r\n\t\t\tSELECT @blockId = [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol].[BlockEndId] FROM [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol] WHERE [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol].[Id]=@accept\r\n\t\t\tIF @blockId <> -1 \r\n\t\t\tBEGIN\r\n\t\t\t\tSET @state = 0\r\n                WHILE @ch <> -1\r\n\t\t\t\tBEGIN\r\n                    SET @done = 0\r\n                    WHILE @done = 0\r\n\t\t\t\t\tBEGIN\r\n                        SET @done = 1\r\n\t\t\t\t\t\tSET @toState = -1\r\n\t\t\t\t\t\tSELECT @toState = [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[ToStateId] FROM [dbo].[");
            Response.Write(codeclass);
            Response.Write("State] INNER JOIN [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition] ON [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[StateId]=[dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[StateId] AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[BlockEndId]=@blockId AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[StateId]=@state AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[BlockEndId] = @blockId AND @ch BETWEEN [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[Min] AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition].[Max]\r\n\t\t\t\t\t\tIF @toState <> -1\r\n\t\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\t\tSET @state = @toState");
 
		if(lineCounted) {
            Response.Write("\r\n\t\t\t\t\t\t\tSET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END\r\n\t\t\t\t\t\t\tSET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END\r\n\t\t\t\t\t\t\tIF @ch>31 SET @cc = @cc + 1");

		} // if(lineCounted) ...
            Response.Write("\r\n\t\t\t\t\t\t\tSET @capture = @capture + @ch1\r\n\t\t\t\t\t\t\tIF @t < 2048 SET @capture = @capture + @ch2\r\n\t\t\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\t\t\tSET @done = 0\r\n\t\t\t\t\t\t\tIF @index >= @valueEnd\r\n\t\t\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\t\t\tSET @ch = -1\r\n\t\t\t\t\t\t\tEND\r\n\t\t\t\t\t\t\tELSE -- IF @index >= @valueEnd\r\n\t\t\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\t\t\t\t\tSET @t = @ch - 0xd800\r\n\t\t\t\t\t\t\t\tIF @t < 0 SET @t = @t + 2147483648\r\n\t\t\t\t\t\t\t\tIF @t < 2048\r\n\t\t\t\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\t\t\t\t\tEND\r\n\t\t\t\t\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\t\t\t\t\tEND -- IF @index >= @valueEnd\r\n\t\t\t\t\t\tEND -- IF @toState <> -1\r\n\t\t\t\t\tEND -- WHILE @done = 0\r\n\t\t\t\t\tSET @accept = -1\r\n\t\t\t\t\tSELECT @accept = [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[AcceptId], @flags = [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol].[Flags] FROM [dbo].[");
            Response.Write(codeclass);
            Response.Write("State] INNER JOIN [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol] ON [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[AcceptId] = [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol].[Id] WHERE [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[StateId] = @state AND [dbo].[");
            Response.Write(codeclass);
            Response.Write("State].[BlockEndId] = @blockId\r\n\t\t\t\t\tIF @accept <> -1\r\n\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\tIF (@flags & 1) = 0 -- symbol isn\'t hidden\r\n\t\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\t\t-- HACK\r\n\t\t\t\t\t\t\tIF @ch = -1 SET @cursorPos = @cursorPos + 1\r\n\t\t\t\t\t\t\tSET @len = @index-@absoluteIndex-1");

		if(lineCounted) {
            Response.Write("\r\n\t\t\t\t\t\t\tIF(@len>0) INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @accept AS [SymbolId], @capture AS [Value], @line AS [Line], @column AS [Column]");

		} else { 
            Response.Write("\r\n\t\t\t\t\t\t\tIF(@len>0) INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @accept AS [SymbolId], @capture AS [Value]");

		}
            Response.Write("\r\n\t\t\t\t\t\tEND\r\n\t\t\t\t\t\tSET @capture = N\'\'\r\n\t\t\t\t\t\tBREAK\r\n\t\t\t\t\tEND -- IF @accept <> -1");

		if(lineCounted) {
            Response.Write("\r\n\t\t\t\t\tSET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END\r\n\t\t\t\t\tSET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END\r\n\t\t\t\t\tIF @ch>31 SET @cc = @cc + 1");

		} // if(lineCounted) ...
            Response.Write("\r\n\t\t\t\t\tSET @state = 0\r\n\t\t\t\t\tSET @capture = @capture + @ch1\r\n\t\t\t\t\tIF @t < 2048 SET @capture = @capture + @ch2\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd\r\n\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\tSET @ch = -1\r\n\t\t\t\t\t\tSET @done = 1\r\n\t\t\t\t\tEND\r\n\t\t\t\t\tELSE\r\n\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\t\t\tSET @t = @ch - 0xd800\r\n\t\t\t\t\t\tIF @t < 0 SET @t = @t + 2147483648\r\n\t\t\t\t\t\tIF @t < 2048\r\n\t\t\t\t\t\tBEGIN\r\n\t\t\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\t\t\tEND\r\n\t\t\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\t\t\tEND\t\t\t\r\n\t\t\t\tEND  -- WHILE @ch <> -1\r\n\t\t\t\tCONTINUE\r\n\t\t\tEND\r\n\t\t\tELSE -- IF @blockId <> -1 \r\n\t\t\tBEGIN\r\n\t\t\t\t-- HACK:\r\n\t\t\t\tIF @ch = -1 SET @cursorPos = @cursorPos +1\r\n\t\t\t\tIF @hasError = 1\r\n                BEGIN\r\n\t\t\t\t\tSET @hasError = 0\r\n\t\t\t\t\tSET @ai = CAST((@absoluteIndex - @errorIndex) AS INT)\r\n\t\t\t\t\tSET @tmp = SUBSTRING(@capture,1,@ai)\r\n\t\t");
            Response.Write("\t\t\tSET @capture = SUBSTRING(@capture,@ai+1,(DATALENGTH(@capture)/2)-(@ai+1))");

if(lineCounted) {
            Response.Write("\r\n\t\t\t\t\tINSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp, @errorLine, @errorColumn)");

} else {
            Response.Write("\r\n\t\t\t\t\tINSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp)");

}
            Response.Write("       \r\n                END\r\n\t\t\t\tIF (@flags & 1) = 0 -- symbol isn\'t hidden\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @len = @index-@absoluteIndex-1\r\n\t\t\t\t\t");

		if(lineCounted) {
            Response.Write("\r\n\t\t\t\t\tIF(@len>0) INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @accept AS [SymbolId], @capture AS [Value], @line AS [Line], @column AS [Column]");

		} else { 
            Response.Write("\r\n\t\t\t\t\tIF(@len>0) INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @accept AS [SymbolId], @capture AS [Value]");

		}
            Response.Write("\r\n\t\t\t\tEND\r\n\t\t\t\tSET @capture = N\'\'\r\n\t\t\tEND -- IF @blockId <> -1\r\n\t\tEND\r\n\t\tELSE -- IF @accept <> -1 \r\n\t\tBEGIN \r\n\t\t\t-- handle the errors\r\n\t\t\tIF @hasError = 0\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @errorPos = @position\r\n\t\t\t\tSET @errorIndex = @absoluteIndex");

	if(lineCounted) {
            Response.Write("\r\n\t\t\t\tSET @errorColumn = @column\r\n\t\t\t\tSET @errorLine = @line");

	}
            Response.Write("\r\n\t\t\tEND\r\n\t\t\tSET @hasError = 1\r\n\t\tEND -- IF @accept <> -1 \r\n\tEND -- WHILE @ch <> -1\r\n\tIF @hasError =1 AND DATALENGTH(@capture) > 0\r\n    BEGIN");

if(lineCounted) {
            Response.Write("\r\n\t\tINSERT INTO #Results SELECT @errorIndex AS [AbsolutePosition], DATALENGTH(@capture)/2 AS [AbsoluteLength], @errorPos AS [Position], CAST((@position- @errorPos +1) AS INT) AS [Length], -1 AS [SymbolId], @capture AS [Value], @errorLine AS [Line], @errorColumn AS [Column]");

} else { 
            Response.Write("\r\n\t\tINSERT INTO #Results SELECT @errorIndex AS [AbsolutePosition], DATALENGTH(@capture)/2 AS [AbsoluteLength], @errorPos AS [Position], CAST((@position- @errorPos +1) AS INT) AS [Length], -1 AS [SymbolId], @capture AS [Value]");

}
            Response.Write("\r\n    END\r\n    SELECT * FROM #Results\r\n    DROP TABLE #Results\r\nEND -- CREATE PROCEDURE Tokenize\r\nGO");

}
Run("SqlTableTokenizerFillerGenerator",Arguments,Response,0);
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
