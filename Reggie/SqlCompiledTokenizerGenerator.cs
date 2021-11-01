using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class SqlCompiledTokenizerGenerator {
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
var symbolTable = BuildSymbolTable(rules);
var symbolFlags = BuildSymbolFlags(rules);
var lexer = BuildLexer(rules,inputFile,ignoreCase);
var dfa = ToDfaTable(lexer);
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
var dfaMap = GetDfaStateTransitionMap(dfa);
var isQ0reffed = IsQ0Reffed(dfa);
int si, sid;
int[][] blockEndDfas = new int[blockEnds.Length][];
//var hasBlockEnds = false;
for(var i = 0;i<blockEnds.Length;++i) {
    var be = blockEnds[i];
    if(be!=null) {
        blockEndDfas[i] = ToDfaTable(be);
       // hasBlockEnds = true;
    }
}
for(var k = 0;k<2;++k) { 
    var istext = k==1;
for(var sacc = 0;sacc<blockEnds.Length;++sacc) {
    var be = blockEnds[sacc];
    if(be!=null) {
            Response.Write("\r\nDROP PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("__");
            Response.Write(istext?"Text":"");
            Response.Write("Tokenize");
            Response.Write(symbolTable[sacc]);
            Response.Write("BlockEnd]\r\nGO\r\nCREATE PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("__");
            Response.Write(istext?"Text":"");
            Response.Write("Tokenize");
            Response.Write(symbolTable[sacc]);
            Response.Write("BlockEnd] @value ");
            Response.Write(istext?"NTEXT":"NVARCHAR(MAX)");
            Response.Write(", @ch BIGINT, @tch BIGINT, @ch1 NCHAR, @ch2 NCHAR, @cursorPos BIGINT, @index INT, @valueEnd INT, @capture NVARCHAR(MAX)");
            Response.Write(lineCounted?", @lc INT, @cc INT, @tabWidth INT":"");
            Response.Write(", @newIndex INT OUTPUT, @newCursorPos BIGINT OUTPUT, @newCapture NVARCHAR(MAX) OUTPUT, @newCh BIGINT OUTPUT, @newTch BIGINT OUTPUT, @newCh1 NCHAR OUTPUT, @newCh2 NCHAR OUTPUT");
            Response.Write(lineCounted?", @newLC INT OUTPUT, @newCC INT OUTPUT":"");
            Response.Write("\r\nAS\r\nBEGIN");

    if(lineCounted)
    {
            Response.Write("\r\n    SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END\r\n\tSET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END\r\n\tIF @ch > 31 SET @cc = @cc + 1");
     
    } // if(lineCounted)...
            Response.Write("\r\n    SET @capture = @capture + @ch1\r\n    IF @tch < 2048 SET @capture = @capture + @ch2\r\n    SET @index = @index + 1\r\n\tIF @index >= @valueEnd\r\n\tBEGIN\r\n\t\tSET @ch = -1\r\n\tEND\r\n\tELSE\r\n\tBEGIN\r\n\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\tSET @ch = UNICODE(@ch1)\r\n\t\tSET @tch = @ch - 0xd800\r\n\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\tIF @tch < 2048\r\n\t\tBEGIN\r\n\t\t\tSET @ch = @ch * 1024\r\n\t\t\tSET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\tEND\r\n\t\tSET @cursorPos = @cursorPos + 1\r\n\tEND\r\n    WHILE @ch <> -1\r\n    BEGIN");
 
si = 0;
sid = 0;
var bedfa = ToDfaTable(be);
var bedfaMap = GetDfaStateTransitionMap(bedfa);
var beQ0reffed = IsQ0Reffed(bedfa);
while(si<bedfa.Length) {
    var acc = bedfa[si++];
    if(sid==0) {
        if(beQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    -- q0:");

        } // if(beQ0reffed)...
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = bedfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = bedfa[si++];
        var rstart = si;
        var ranges = bedfa;
        if(lineCounted) {
            var lclist = new List<int>(10);
            if (DfaRangesContains('\n',ranges,rstart)) {
                lclist.Add('\n');
                ranges = DfaExcludeFromRanges('\n',ranges,rstart);
                rstart = 0;        
            }
            if (DfaRangesContains('\r',ranges, rstart)) {
                lclist.Add('\r');
                ranges = DfaExcludeFromRanges('\r',ranges,rstart);
                rstart = 0; 
            }
            if (DfaRangesContains('\t',ranges, rstart)) {
                lclist.Add('\t');
                ranges = DfaExcludeFromRanges('\t',ranges,rstart);
                rstart = 0;
            }
            if(lclist.Contains('\t')) {
            Response.Write("\r\n        IF @ch = 9\r\n        BEGIN\r\n            SET @capture = @capture + @ch1\r\n            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1\r\n            SET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\tEND\r\n            GOTO q");
            Response.Write(bedfaMap[tto]);
            Response.Write("\r\n        END");

            }
            if (lclist.Contains('\n')) {
            Response.Write("\r\n        IF @ch = 10\r\n        BEGIN\r\n            SET @capture = @capture + @ch1\r\n            SET @cc = 1\r\n            SET @lc = @lc + 1\r\n            SET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\tEND\r\n            GOTO q");
            Response.Write(bedfaMap[tto]);
            Response.Write("\r\n        END");

            }
            if (lclist.Contains('\r')) {
            Response.Write("\r\n        IF @ch = 13\r\n        BEGIN\r\n            SET @capture = @capture + @ch1\r\n            SET @cc = 1\r\n            SET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\tEND\r\n            GOTO q");
            Response.Write(bedfaMap[tto]);
            Response.Write("\r\n        END");

            }
        }
        var exts = GetTransitionExtents(ranges,rstart);
            Response.Write("\r\n                IF ");
WriteSqlRangeCharMatchTests(ranges, rstart, 2, Response);
            Response.Write("\r\n        BEGIN\r\n            SET @capture = @capture + @ch1");

        if(exts.Value>127) {
            Response.Write("\r\n            IF @tch < 2048 SET @capture = @capture + @ch2");

        } 
         if(lineCounted) {
            if(exts.Key>31) {
            Response.Write("\r\n            SET @cc = @cc + 1");

            } else if(exts.Value>31) {
            Response.Write("\r\n            IF @ch > 31 SET @cc = @cc + 1");

            }
        }
            Response.Write("\r\n            SET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\tEND\r\n            goto q");
            Response.Write(bedfaMap[tto]);
            Response.Write("\r\n        END -- IF {range test}");

        // skip the packed ranges
        var prlen = bedfa[si++];
        si+=prlen*2;
    } // for .. tlen
    if(acc!=-1) { // accepting
            Response.Write("\r\n        IF @ch = -1 SET @cursorPos = @cursorPos + 1\r\n        SET @newCh = @ch\r\n        SET @newTch = @tch\r\n        SET @newCh1 = @ch1\r\n        SET @newCh2 = @ch2\r\n        SET @newCursorPos = @cursorPos\r\n        SET @newIndex = @index\r\n        SET @newCapture = @capture");

        if(lineCounted) {
            Response.Write("\r\n        SET @newLC = @lc\r\n        SET @newCC = @cc");

        }
            Response.Write("\r\n        RETURN 1");

    } else {// not accepting
        if(lineCounted) {
            Response.Write("\r\n        SET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END\r\n\t    SET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END\r\n\t    IF @ch>31 SET @cc = @cc + 1");
     
        } // if(lineCounted)...
            Response.Write("\r\n        SET @capture = @capture + @ch1\r\n        IF @tch < 2048 SET @capture = @capture + @ch2\r\n        SET @index = @index + 1\r\n\t    IF @index >= @valueEnd\r\n\t    BEGIN\r\n\t\t    SET @ch = -1\r\n\t    END\r\n\t    ELSE\r\n\t    BEGIN\r\n\t\t    SET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t    SET @ch = UNICODE(@ch1)\r\n\t\t    SET @tch = @ch - 0xd800\r\n\t\t    IF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t    IF @tch < 2048\r\n\t\t    BEGIN\r\n\t\t\t    SET @ch = @ch * 1024\r\n\t\t\t    SET @index = @index + 1\r\n\t\t\t    IF @index >= @valueEnd RETURN -1\r\n\t\t\t    SET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t    SET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t    END\r\n\t\t    SET @cursorPos = @cursorPos + 1\r\n\t    END");

        if(si<bedfa.Length) {
            Response.Write("\r\n        CONTINUE -- WHILE @ch <> -1");

        }
    } // if(acc!=-1)
    ++sid; // we're on the next state now
} // while(si<bedfa.Length) 
            Response.Write("\r\n    END -- WHILE @ch <> -1\r\n    IF @ch = -1 SET @cursorPos = @cursorPos + 1\r\n    SET @newCh = @ch\r\n    SET @newTch = @tch\r\n    SET @newCh1 = @ch1\r\n    SET @newCh2 = @ch2\r\n    SET @newCursorPos = @cursorPos\r\n    SET @newIndex = @index\r\n    SET @newCapture = @capture");

    if(lineCounted) {
            Response.Write("\r\n    SET @newLC = @lc\r\n    SET @newCC = @cc");

    }
            Response.Write("\r\n    RETURN 0\r\nEND\r\nGO\r\n");

} // if(be!=null) ...
} // for(var sacc=0) ...
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
            Response.Write("\r\nAS\r\nBEGIN\r\n    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\n\tDECLARE @index INT = 1\r\n\tDECLARE @absoluteIndex BIGINT = 0\r\n\tDECLARE @ch BIGINT\r\n\tDECLARE @ch1 NCHAR\r\n\tDECLARE @ch2 NCHAR\r\n\tDECLARE @tch BIGINT\r\n\tDECLARE @state INT = 0\r\n\tDECLARE @toState INT = -1\r\n\tDECLARE @accept INT = -1\r\n\tDECLARE @cursorPos BIGINT = @position\r\n\tDECLARE @capture NVARCHAR(MAX) = N\'\'\r\n\tDECLARE @tmp NVARCHAR(MAX) = N\'\'\r\n\tDECLARE @blockId INT\r\n\tDECLARE @result INT = 0\r\n\tDECLARE @len INT = 0\r\n\tDECLARE @flags INT = 0\r\n\tDECLARE @matched INT = 0\r\n    DECLARE @errorIndex BIGINT\r\n    DECLARE @errorPos BIGINT\r\n    DECLARE @hasError INT = 0\r\n\tDECLARE @ai INT\r\n    DECLARE @newIndex INT\r\n\tDECLARE @newCursorPos INT\r\n\tDECLARE @newCapture NVARCHAR(MAX)\r\n    DECLARE @newCh BIGINT\r\n    DECLARE @newTch BIGINT\r\n    DECLARE @newCh1 NCHAR\r\n    DECLARE @newCh2 NCHAR");
 
    if(lineCounted) {
            Response.Write("\r\n    DECLARE @lc INT = @line\r\n    DECLARE @cc INT = @column\r\n    DECLARE @newLC INT\r\n    DECLARE @newCC INT\r\n    DECLARE @errorLine INT\r\n    DECLARE @errorColumn INT");

    } // if(lineCounted) ... 
            Response.Write("\r\n    CREATE TABLE #Results (\r\n\t[AbsolutePosition] BIGINT NOT NULL,\r\n\t[AbsoluteLength] INT NOT NULL,\r\n\t[Position] BIGINT NOT NULL,\r\n\t[Length] INT NOT NULL,\r\n    [SymbolId] INT NOT NULL,");

    if(lineCounted) {
            Response.Write("\r\n    [Value] NVARCHAR(MAX) NOT NULL,\r\n    [Line] INT NOT NULL,\r\n    [Column] INT NOT NULL");

    } else {
            Response.Write("\r\n    [Value] NVARCHAR(MAX) NOT NULL");

    }
            Response.Write(")\r\n\tIF @index >= @valueEnd\r\n\tBEGIN \r\n\t\tSET @ch = -1\r\n\tEND\r\n\tELSE\r\n\tBEGIN\r\n\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\tSET @ch = UNICODE(@ch1)\r\n\t\tSET @tch = @ch - 0xd800\r\n\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\tIF @tch < 2048\r\n\t\tBEGIN\r\n\t\t\tSET @ch = @ch * 1024\r\n\t\t\tSET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\tEND\r\n\tEND\r\n    SET @cursorPos = @position");

if(lineCounted) {
            Response.Write("\r\n\tSET @lc = @line\r\n\tSET @cc = @column");

}
            Response.Write("        \r\n    WHILE @ch <> -1\r\n\tBEGIN\r\n        SET @position = @cursorPos\r\n        SET @absoluteIndex = @index-1");

if(lineCounted) {
            Response.Write("\r\n        SET @line = @lc\r\n        SET @column = @cc");

}     
si = 0;
sid = 0;
while(si<dfa.Length) {
    var sacc = dfa[si++];
    if(sid==0) {
        if(isQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    -- q0:");

        } // if(isQ0reffed)...
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = dfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = dfa[si++];
        var rstart = si;
        var ranges = dfa;
        if(lineCounted) {
            var lclist = new List<int>(10);
            if (DfaRangesContains('\n',ranges,rstart)) {
                lclist.Add('\n');
                ranges = DfaExcludeFromRanges('\n',ranges,rstart);
                rstart = 0;        
            }
            if (DfaRangesContains('\r',ranges, rstart)) {
                lclist.Add('\r');
                ranges = DfaExcludeFromRanges('\r',ranges,rstart);
                rstart = 0; 
            }
            if (DfaRangesContains('\t',ranges, rstart)) {
                lclist.Add('\t');
                ranges = DfaExcludeFromRanges('\t',ranges,rstart);
                rstart = 0;
            }
            if(lclist.Contains('\t')) {
            Response.Write("\r\n        IF @ch = 9\r\n        BEGIN\r\n            SET @capture = @capture + @ch1\r\n            SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1\r\n            SET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\tEND\r\n            GOTO q");
            Response.Write(dfaMap[tto]);
            Response.Write("\r\n        END");

            }
            if (lclist.Contains('\n')) {
            Response.Write("\r\n        IF @ch = 10\r\n        BEGIN\r\n            SET @capture = @capture + @ch1\r\n            SET @cc = 1\r\n            SET @lc = @lc + 1\r\n            SET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\tEND\r\n            GOTO q");
            Response.Write(dfaMap[tto]);
            Response.Write("\r\n        END");

            }
            if (lclist.Contains('\r')) {
            Response.Write("\r\n        IF @ch = 13\r\n        BEGIN\r\n            SET @capture = @capture + @ch1\r\n            SET @cc = 1\r\n            SET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\tEND\r\n            GOTO q");
            Response.Write(dfaMap[tto]);
            Response.Write("\r\n        END");

            }
        }
        var exts = GetTransitionExtents(ranges,rstart);
            Response.Write("\r\n        IF ");
WriteSqlRangeCharMatchTests(ranges, rstart, 2, Response);
            Response.Write("\r\n        BEGIN\r\n            SET @capture = @capture + @ch1");

        if(exts.Value>127) {
            Response.Write("\r\n            IF @tch < 2048 SET @capture = @capture + @ch2");

        } 
         if(lineCounted) {
            if(exts.Key>31) {
            Response.Write("\r\n            SET @cc = @cc + 1");

            } else if(exts.Value>31) {
            Response.Write("\r\n            IF @ch > 31 SET @cc = @cc + 1");

            }
        }
            Response.Write("\r\n            SET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\t\tEND\r\n            goto q");
            Response.Write(dfaMap[tto]);
            Response.Write("\r\n        END -- IF {range test}");

        // skip the packed ranges
        var prlen = dfa[si++];
        si+=prlen*2;
    } // for .. tlen
    if(sacc!=-1) { // accepting
            Response.Write("\r\n        IF @hasError = 1\r\n        BEGIN\r\n            SET @hasError = 0\r\n            SET @ai = CAST((@absoluteIndex - @errorIndex) AS INT)\r\n\t\t\tSET @tmp = SUBSTRING(@capture,1,@ai)\r\n\t\t\tSET @capture = SUBSTRING(@capture,@ai+1,(DATALENGTH(@capture)/2)-(@ai+1))");

        if(lineCounted) {
            Response.Write("\r\n            INSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp, @errorLine, @errorColumn)");

        } else {
            Response.Write("\r\n            INSERT INTO #Results VALUES(@errorIndex, @ai, @errorPos, CAST((@position - @errorPos) AS INT), -1, @tmp)");

        }
            Response.Write("                \r\n        END");

        if(blockEnds[sacc]==null) {
            Response.Write("       \r\n        IF @ch = -1 SET @cursorPos = @cursorPos + 1");

            if(0==(symbolFlags[sacc]&1)) {
            Response.Write("\r\n\t\tSET @len = @index-@absoluteIndex-1");

                if(lineCounted) {
            Response.Write("\r\n        IF @len>0 INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos-@position) AS INT) AS [Length], @accept AS [SymbolId], @capture AS [Value], @line AS [Line], @column AS [Column]");

		        } else { 
            Response.Write("\r\n        IF @len>0 INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos-@position) AS INT) AS [Length], @accept AS [SymbolId], @capture AS [Value]");

		        }
            }
            Response.Write("\r\n\t\tSET @capture = N\'\'\r\n\t\tCONTINUE -- WHILE @ch <> -1");

        } else { // if(blockEnds[sacc]==null) ... {
            Response.Write("\r\n        EXEC @len = [dbo].[");
            Response.Write(codeclass);
            Response.Write("__");
            Response.Write(istext?"Text":"");
            Response.Write("Tokenize");
            Response.Write(symbolTable[sacc]);
            Response.Write("BlockEnd] @value = @value, @ch = @ch,  @tch = @tch, @ch1 = @ch1, @ch2 = @ch2, @cursorPos = @cursorPos, @index = @index, @valueEnd = @valueEnd, @capture = @capture");
            Response.Write(lineCounted?", @lc = @lc, @cc = @cc, @tabwidth = @tabWidth":"");
            Response.Write(", @newIndex = @newIndex OUTPUT, @newCursorPos = @newCursorPos OUTPUT, @newCapture = @newCapture OUTPUT, @newCh = @newCh OUTPUT, @newTch = @newTch OUTPUT, @newCh1 = @newCh1 OUTPUT, @newCh2 = @newCh2 OUTPUT");
            Response.Write(lineCounted?", @newLC = @newLC OUTPUT, @newCC = @newCC OUTPUT":"");
            Response.Write("\r\n        SET @index = @newIndex\r\n\t\tSET @cursorPos = @newCursorPos\r\n\t\tSET @capture = @newCapture\r\n        SET @ch = @newCh\r\n        SET @tch = @newTch\r\n        SET @ch1 = @newCh1\r\n        SET @ch2 = @newCh2");

            if(lineCounted) {
            Response.Write("\r\n        SET @lc = @newLC\r\n        SET @cc = @newCC");

            }
            Response.Write("\r\n        IF @len = 1 INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@index-@absoluteIndex-1) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos-@position) AS INT) AS [Length], ");
            Response.Write(sacc);
            Response.Write(" AS [SymbolId], @capture AS [Value]");
            Response.Write(lineCounted?", @line AS [Line], @column AS [Column]":"");
            Response.Write("\r\n        CONTINUE");

        } // if(blockEnds[sacc]==null) ...
    } else {// if(sacc!=-1) ...
            Response.Write("\r\n        GOTO error");

    } // if(sacc!=-1) ...
    ++sid; // we're on the next state now
} // while(si<dfa.Length) ...
            Response.Write(" \r\n    error:\r\n        IF @hasError = 1\r\n        BEGIN\r\n            SET @errorPos = @position\r\n            SET @errorIndex = @absoluteIndex");

if(lineCounted) {
            Response.Write("\r\n            SET @errorColumn = @column\r\n            SET @errorLine = @line");

}
            Response.Write("\r\n        END\r\n        SET @hasError = 1    \r\n    END -- WHILE ch <> -1 \r\n    SET @len = DATALENGTH(@capture)/2\r\n    IF @hasError = 1 AND @len <> 0 INSERT INTO #Results SELECT @errorIndex AS [AbsolutePosition], @len AS [AbsoluteLength], @errorPos AS [Position], CAST((@position-@errorPos+1) AS INT) AS [Length], -1 AS [SymbolId], @capture AS [Value]");
            Response.Write(lineCounted?", @errorLine AS [Line], @errorColumn AS [Column]":"");
            Response.Write("\r\n    SELECT * FROM #Results\r\n    DROP TABLE #Results\r\nEND -- Tokenize ...\r\nGO");

} // for(var k = 0;k<2;++k) ... 

            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
