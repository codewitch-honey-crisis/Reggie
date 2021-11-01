using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class SqlCompiledMatcherGenerator {
        public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {

var rules = (IList<LexRule>)Arguments["rules"];
var ignoreCase = (bool)Arguments["ignorecase"];
var inputFile = (string)Arguments["inputfile"];
var outputFile = (string)Arguments["outputfile"];
var codeclass = (string)Arguments["codeclass"];
var codenamespace = (string)Arguments["codenamespace"];
var stderr = (TextWriter)Arguments["stderr"];
var dot = (bool)Arguments["dot"];
var jpg = (bool)Arguments["jpg"];
var cwd = Path.GetDirectoryName(outputFile!=null?outputFile:inputFile);
var blockEnds = BuildBlockEnds(rules,inputFile,ignoreCase);
int[][] blockEndDfas = new int[blockEnds.Length][];
//var hasBlockEnds = false;
for(var i = 0;i<blockEnds.Length;++i) {
    var be = blockEnds[i];
    if(be!=null) {
        blockEndDfas[i] = ToDfaTable(be);
       // hasBlockEnds = true;
    }
}
foreach(var rule in rules) {
    var fa = ParseToFA(rule,inputFile,ignoreCase);
    var dfa = ToDfaTable(fa);
    if(dot) {
        var opts = new F.FFA.DotGraphOptions();
        opts.HideAcceptSymbolIds = true;
        var fn = Path.Combine(cwd, rule.Symbol + ".dot");
        stderr.WriteLine("Writing {0}...",fn);
        using(var sw=new StreamWriter(fn)) {
            fa.WriteDotTo(sw,opts);
        }        
    }
    if(jpg) {
        var opts = new F.FFA.DotGraphOptions();
        opts.HideAcceptSymbolIds = true;
        var fn = Path.Combine(cwd, rule.Symbol + ".jpg");
        stderr.WriteLine("Writing {0}...",fn);
        try {
            fa.RenderToFile(fn,opts);
        }  
        catch {}
    }

    var dfaMap = GetDfaStateTransitionMap(dfa);
    var bedfa = blockEndDfas[rule.Id];
    var bedfaMap = bedfa!=null?GetDfaStateTransitionMap(bedfa):(int[])null;
    var isQ0reffed = IsQ0Reffed(dfa);
    var isBEQ0reffed = bedfa!=null && IsQ0Reffed(bedfa);
    int si, sid;
	for(var k=0;k<2;++k) {
		var istext = k==1;
            Response.Write("\r\nDROP PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("_");
            Response.Write(istext?"Text":"");
            Response.Write("Match");
            Response.Write(rule.Symbol);
            Response.Write("]\r\nGO\r\nCREATE PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("_");
            Response.Write(istext?"Text":"");
            Response.Write("Match");
            Response.Write(rule.Symbol);
            Response.Write("] @value ");
            Response.Write(istext?"NTEXT":"NVARCHAR(MAX)");
            Response.Write(", @position BIGINT = 0\r\nAS\r\nBEGIN\r\n\tDECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\n\tDECLARE @index INT = 1\r\n\tDECLARE @ch BIGINT\r\n\tDECLARE @ch1 NCHAR\r\n\tDECLARE @ch2 NCHAR\r\n\tDECLARE @tch BIGINT\r\n\tDECLARE @state INT = 0\r\n\tDECLARE @toState INT = -1\r\n\tDECLARE @accept INT = -1\r\n\tDECLARE @capture NVARCHAR(MAX)\r\n\tDECLARE @blockEndId INT\r\n\tDECLARE @cursorPos BIGINT = @position\r\n\tDECLARE @absoluteIndex INT\r\n\tDECLARE @result INT = 0\r\n\tDECLARE @len INT = 0\r\n\tDECLARE @newIndex INT\r\n\tDECLARE @newCursorPos INT\r\n\tDECLARE @newCapture NVARCHAR(MAX)\r\n    DECLARE @newCh BIGINT\r\n    DECLARE @newTch BIGINT\r\n    DECLARE @newCh1 NCHAR\r\n    DECLARE @newCh2 NCHAR\r\n\tCREATE TABLE #Results (\r\n    [AbsolutePosition]  BIGINT NOT NULL,\r\n\t[AbsoluteLength] INT NOT NULL,\r\n\t[Position] BIGINT NOT NULL,\r\n\t[Length] INT NOT NULL,\r\n    [Value] NVARCHAR(MAX) NOT NULL\r\n\t)\r\n\tIF @index >= @valueEnd\r\n\tBEGIN \r\n\t\tSET @ch = -1\r\n\tEND\r\n\tELSE\r\n\tBEGIN\r\n\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\tSET @ch = UNICODE(@ch1)\r\n\t\tSET @tch = @ch - 0xd800\r\n\t\tIF @tch < 0 ");
            Response.Write("SET @tch = @tch + 2147483648\r\n\t\tIF @tch < 2048\r\n\t\tBEGIN\r\n\t\t\tSET @ch = @ch * 1024\r\n\t\t\tSET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\tSET @ch2 = SUBSTRING(@value,@index,1);\r\n\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\tEND\r\n\tEND\r\n\tWHILE @ch <> -1\r\n\tBEGIN\r\n\t\tSET @capture = N\'\'\r\n\t\tSET @position = @cursorPos\r\n\t\tSET @absoluteIndex = @index -1");
 
si = 0;
sid = 0;
while(si<dfa.Length) {
    var sacc = dfa[si++];
    if(sid==0) {
        if(isQ0reffed) {
            Response.Write("\r\n    q0:");

        } else {
            Response.Write("\r\n    -- q0:");

        }
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = dfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = dfa[si++];
        var exts = GetTransitionExtents(dfa,si);
            Response.Write("\r\n        IF ");
WriteSqlRangeCharMatchTests(dfa, si, 2, Response);
            Response.Write("\r\n\t\tBEGIN\r\n\t\t\tSET @capture = @capture + @ch1");

		if(exts.Value>127) {
            Response.Write("\r\n\t\t\tIF @tch < 2048 SET @capture = @capture + @ch2");

        }
            Response.Write("\r\n\t\t\tSET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos+1\r\n\t\t\tEND\r\n            GOTO q");
            Response.Write(dfaMap[tto]);
            Response.Write("\r\n        END -- IF {range match}");

        // skip the packed ranges
        var prlen = dfa[si++];
        si+=prlen*2;
    } // for .. tlen
    if(sacc!=-1) { // accepting
        if(blockEnds[sacc]==null) {
            Response.Write("\r\n        -- HACK:\r\n        IF @ch = -1 SET @cursorPos = @cursorPos + 1\r\n        IF DATALENGTH(@capture)>0 INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @index-@absoluteIndex-1 AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @capture AS [Value]");

            if(si<dfa.Length) { // are there more states? 
            Response.Write("\r\n        GOTO next");

            }
        } else {
            Response.Write("\r\n\t\tEXEC @len = [dbo].[");
            Response.Write(codeclass);
            Response.Write("__Match");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd] @value = @value, @ch = @ch,  @tch = @tch, @ch1 = @ch1, @ch2 = @ch2, @cursorPos = @cursorPos, @index = @index, @valueEnd = @valueEnd, @capture = @capture, @newIndex = @newIndex OUTPUT, @newCursorPos = @newCursorPos OUTPUT, @newCapture = @newCapture OUTPUT, @newCh = @newCh OUTPUT, @newTch = @newTch OUTPUT, @newCh1 = @newCh1 OUTPUT, @newCh2 = @newCh2 OUTPUT\r\n\t\tSET @index = @newIndex\r\n\t\tSET @cursorPos = @newCursorPos\r\n\t\tSET @capture = @newCapture\r\n        SET @ch = @newCh\r\n        SET @tch = @newTch\r\n        SET @ch1 = @newCh1\r\n        SET @ch2 = @newCh2\r\n\t\tIF @len = 1 INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], @index-@absoluteIndex-1 AS [AbsoluteLength], @position AS [Position], @cursorPos-@position AS [Length], @capture AS [Value]\r\n        CONTINUE");

        }
    } else {// not accepting
        if(si<dfa.Length) { // are there more states? 
            Response.Write("\r\n        GOTO next");

        }
    }
    ++sid; // we're on the next state now
} // while(si<dfa.Length) 

            Response.Write("\r\n    next:\r\n        SET @index = @index + 1\r\n\t\tIF @index >= @valueEnd\r\n\t\tBEGIN\r\n\t\t\tSET @ch = -1\r\n\t\tEND\r\n\t\tELSE\r\n\t\tBEGIN\r\n\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\tIF @tch < 2048\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\tSET @index = @index + 1\r\n\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\tEND\r\n\t\t\tSET @cursorPos = @cursorPos + 1\r\n\t\tEND\r\n    END -- WHILE @ch <> -1\r\n\tSELECT * FROM #Results\r\n\tDROP TABLE #Results\r\nEND\r\nGO");

if(blockEnds[rule.Id]!=null) {
            Response.Write("\r\nDROP PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("__");
            Response.Write(istext?"Text":"");
            Response.Write("Match");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd]\r\nGO\r\nCREATE PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("__");
            Response.Write(istext?"Text":"");
            Response.Write("Match");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd] @value ");
            Response.Write(istext?"NTEXT":"NVARCHAR(MAX)");
            Response.Write(", @ch BIGINT, @tch BIGINT, @ch1 NCHAR, @ch2 NCHAR, @cursorPos BIGINT, @index INT, @valueEnd INT, @capture NVARCHAR(MAX), @newIndex INT OUTPUT, @newCursorPos BIGINT OUTPUT, @newCapture NVARCHAR(MAX) OUTPUT, @newCh BIGINT OUTPUT, @newTch BIGINT OUTPUT, @newCh1 NCHAR OUTPUT, @newCh2 NCHAR OUTPUT\r\nAS\r\nBEGIN\r\n    WHILE @ch <> -1\r\n    BEGIN");
 
si = 0;
sid = 0;
while(si<bedfa.Length) {
    var sacc = bedfa[si++];
    if(sid==0) {
        if(isBEQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    -- q0:");

        }
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = bedfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = bedfa[si++];
        var exts = GetTransitionExtents(bedfa,si);
            Response.Write("\r\n        IF ");
WriteSqlRangeCharMatchTests(bedfa, si, 2, Response);
            Response.Write("\r\n        BEGIN\r\n            SET @capture = @capture + @ch1");

        if(exts.Value>127) {
            Response.Write("\r\n            IF @tch < 2048 SET @capture = @capture + @ch2");

        }
            Response.Write("\r\n            SET @index = @index + 1\r\n\t\t\tIF @index >= @valueEnd\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = -1\r\n\t\t\tEND\r\n\t\t\tELSE\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t\tIF @tch < 2048\r\n\t\t\t\tBEGIN\r\n\t\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\t\tSET @index = @index + 1\r\n\t\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\t\tEND\r\n\t\t\t\tSET @cursorPos = @cursorPos+1\r\n\t\t\tEND\r\n            GOTO q");
            Response.Write(bedfaMap[tto]);
            Response.Write("\r\n        END");

        // skip the packed ranges
        var prlen = bedfa[si++];
        si+=prlen*2;
    } // for .. tlen
    if(sacc!=-1) { // accepting
            Response.Write("\r\n        -- HACK:\r\n        IF @ch = -1 SET @cursorPos = @cursorPos + 1\r\n        SET @newCh = @ch\r\n        SET @newTch = @tch\r\n        SET @newCh1 = @ch1\r\n        SET @newCh2 = @ch2\r\n        SET @newCursorPos = @cursorPos\r\n        SET @newIndex = @index\r\n        SET @newCapture = @capture\r\n        RETURN 1");

    } else {// not accepting
            Response.Write("\r\n        GOTO next");

    }
    ++sid; // we're on the next state now
} // while(si<bedfa.Length) 

            Response.Write("\r\n    next:\r\n        SET @capture = @capture + @ch1\r\n        IF @tch < 2048 SET @capture = @capture + @ch2\r\n        SET @index = @index + 1\r\n\t\tIF @index >= @valueEnd\r\n\t\tBEGIN\r\n\t\t\tSET @ch = -1\r\n\t\tEND\r\n\t\tELSE\r\n\t\tBEGIN\r\n\t\t\tSET @ch1 = SUBSTRING(@value,@index,1)\r\n\t\t\tSET @ch = UNICODE(@ch1)\r\n\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\tIF @tch < 2048\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\tSET @index = @index + 1\r\n\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\tSET @ch2 = SUBSTRING(@value,@index,1)\r\n\t\t\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00\r\n\t\t\tEND\r\n\t\tEND\r\n        SET @cursorPos = @cursorPos+1\r\n    END -- WHILE @ch <> -1\r\n    SET @newCh = @ch\r\n    SET @newTch = @tch\r\n    SET @newCh1 = @ch1\r\n    SET @newCh2 = @ch2\r\n    SET @newCursorPos = @cursorPos\r\n    SET @newIndex = @index\r\n    SET @newCapture = @capture\r\n    RETURN 0\r\nEND\r\nGO");

}
            Response.Write("\r\nDROP PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("_");
            Response.Write(istext?"Text":"");
            Response.Write("Is");
            Response.Write(rule.Symbol);
            Response.Write("]\r\nGO\r\nCREATE PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("_");
            Response.Write(istext?"Text":"");
            Response.Write("Is");
            Response.Write(rule.Symbol);
            Response.Write("] @value ");
            Response.Write(istext?"NTEXT":"NVARCHAR(MAX)");
            Response.Write("\r\nAS\r\nBEGIN\r\n    DECLARE @ch BIGINT\r\n    DECLARE @tch BIGINT\r\n    DECLARE @index INT = 1\r\n    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\n    DECLARE @result INT\r\n    IF @index >= @valueEnd\r\n    BEGIN\r\n        SET @ch = -1\r\n    END\r\n    ELSE\r\n    BEGIN\r\n        SET @ch = UNICODE(SUBSTRING(@value,@index,1))\r\n\t    SET @tch = @ch - 0xd800\r\n\t    IF @tch < 0 SET @tch = @tch + 2147483648\r\n\t    IF @tch < 2048\r\n\t    BEGIN\r\n\t\t    SET @ch = @ch * 1024\r\n\t\t    SET @index = @index + 1\r\n\t\t    IF @index >= @valueEnd RETURN -1\r\n\t\t    SET @ch = @ch + UNICODE(SUBSTRING(@value,@index,1)) - 0x35fdc00\r\n\t    END\r\n    END\r\n    WHILE @ch <> -1\r\n    BEGIN");

        si = 0;
        sid = 0;
        while(si<dfa.Length) {
            var sacc = dfa[si++];
            if(sid==0) {
                if(isQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    -- q0:");

                }
            } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

            } // if(sid==0)
            var tlen = dfa[si++];
            for(var j=0;j<tlen;++j) {
                var tto = dfa[si++];
                var exts = GetTransitionExtents(dfa,si);
            Response.Write("\r\n        IF ");
WriteSqlRangeCharMatchTests(dfa, si, 2, Response);
            Response.Write("\r\n        BEGIN\r\n            SET @index = @index + 1\r\n            IF @index >= @valueEnd\r\n            BEGIN\r\n                SET @ch = -1\r\n            END\r\n            ELSE\r\n            BEGIN\r\n                SET @ch = UNICODE(SUBSTRING(@value,@index,1))\r\n\t            SET @tch = @ch - 0xd800\r\n\t            IF @tch < 0 SET @tch = @tch + 2147483648\r\n\t            IF @tch < 2048\r\n\t            BEGIN\r\n\t\t            SET @ch = @ch * 1024\r\n\t\t            SET @index = @index + 1\r\n\t\t            IF @index >= @valueEnd RETURN -1\r\n\t\t            SET @ch = @ch + UNICODE(SUBSTRING(@value,@index,1)) - 0x35fdc00\r\n\t            END\r\n            END                    \r\n            GOTO q");
            Response.Write(dfaMap[tto]);
            Response.Write("\r\n        END");

                // skip the packed ranges
                var prlen = dfa[si++];
                si+=prlen*2;
            } // for .. tlen
            if(sacc!=-1) { // accepting
                if(blockEnds[sacc]==null) {
            Response.Write("\r\n        RETURN CASE @ch WHEN -1 THEN 1 ELSE 0 END");

                } else {
            Response.Write("\r\n        EXEC @result = ");
            Response.Write(codeclass);
            Response.Write("__Is");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd @value, @index, @ch\r\n        RETURN @result");

                }
            } else {// not accepting
            Response.Write("\r\n        RETURN 0");

            }
            ++sid; // we're on the next state now
        } // while(si<dfa.Length) 
            Response.Write("\r\n    END -- WHILE @ch <> -1\r\n    RETURN 0\r\nEND\r\nGO");

        if(bedfa!=null) {
            Response.Write("\r\nDROP PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("__");
            Response.Write(istext?"Text":"");
            Response.Write("Is");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd]\r\nGO\r\nCREATE PROCEDURE [dbo].[");
            Response.Write(codeclass);
            Response.Write("__");
            Response.Write(istext?"Text":"");
            Response.Write("Is");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd] @value ");
            Response.Write(istext?"NTEXT":"NVARCHAR(MAX)");
            Response.Write(", @index INT, @ch INT\r\nAS\r\nBEGIN\r\n    DECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\n\tDECLARE @tch BIGINT\r\n\tDECLARE @accept INT = -1\r\n\tDECLARE @result INT = 0\r\n    WHILE @ch <> -1\r\n    BEGIN");

            si = 0;
            sid = 0;
            while(si<bedfa.Length) {
                var sacc = bedfa[si++];
                if(sid==0) {
                    if(isBEQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    -- q0:");

                    }
                } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

                } // if(sid==0)
                var tlen = bedfa[si++];
                for(var j=0;j<tlen;++j) {
                    var tto = bedfa[si++];
            Response.Write("\r\n        IF ");
WriteSqlRangeCharMatchTests(bedfa, si, 2, Response);
            Response.Write("\r\n        BEGIN\r\n            SET @index = @index + 1\r\n            IF @index >= @valueEnd\r\n            BEGIN\r\n                SET @ch = -1\r\n            END\r\n            ELSE\r\n            BEGIN\r\n                SET @ch = UNICODE(SUBSTRING(@value,@index,1))\r\n\t\t\t    SET @tch = @ch - 0xd800\r\n\t\t\t    IF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\t    IF @tch < 2048\r\n\t\t\t    BEGIN\r\n\t\t\t\t    SET @ch = @ch * 1024\r\n\t\t\t\t    SET @index = @index + 1\r\n\t\t\t\t    IF @index >= @valueEnd RETURN -1\r\n\t\t\t\t    SET @ch = @ch + UNICODE(SUBSTRING(@value,@index,1)) - 0x35fdc00\r\n\t\t\t    END\r\n            END\r\n            GOTO q");
            Response.Write(bedfaMap[tto]);
            Response.Write("\r\n        END -- IF(range tests...");

                        // skip the packed ranges
                        var prlen = bedfa[si++];
                        si+=prlen*2;
                    } // for .. tlen
                    if(sacc!=-1) { // accepting
            Response.Write("\r\n        RETURN CASE @ch WHEN -1 THEN 1 ELSE 0 END");

                    } else {// not accepting
            Response.Write("\r\n        GOTO next");

                    }
                ++sid; // we're on the next state now
                } // while(si<bedfa.Length)
            Response.Write("\r\n    next:\r\n        SET @index = @index + 1\r\n        IF @index >= @valueEnd\r\n        BEGIN\r\n            SET @ch = -1\r\n        END\r\n        ELSE\r\n        BEGIN\r\n            SET @ch = UNICODE(SUBSTRING(@value,@index,1))\r\n\t\t\tSET @tch = @ch - 0xd800\r\n\t\t\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\t\t\tIF @tch < 2048\r\n\t\t\tBEGIN\r\n\t\t\t\tSET @ch = @ch * 1024\r\n\t\t\t\tSET @index = @index + 1\r\n\t\t\t\tIF @index >= @valueEnd RETURN -1\r\n\t\t\t\tSET @ch = @ch + UNICODE(SUBSTRING(@value,@index,1)) - 0x35fdc00\r\n\t\t\tEND\r\n        END\r\n    END -- WHILE @ch <> -1\r\n    RETURN 0\r\nEND -- CREATE PROCEDURE\r\nGO");

        }
	}
}
            Response.Flush();
        }
    }
}
