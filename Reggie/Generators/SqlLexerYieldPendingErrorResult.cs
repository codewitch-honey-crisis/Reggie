using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlLexerYieldPendingErrorResult(TextWriter Response, IDictionary<string, object> Arguments, bool isFinal, bool skipErrorCheck) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
if(!skipErrorCheck) { 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("IF @hasError = 1");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write(isFinal?" AND DATALENGTH(@capture) > 0":"");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("\r\nBEGIN\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
a._indent = ((int)a._indent) + 1; }
if(!isFinal) {
	if(!skipErrorCheck) {
		
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("SET @ai = CAST((@absoluteIndex - @errorIndex) AS INT)\r\nSET @tmp = SUBSTRING(@capture,1,@ai)\r\nINSERT INTO #Results SELECT @errorIndex AS [AbsolutePosition], @ai AS [AbsoluteLength], @errorPos AS [Position], CAST((@position - @errorPos) AS INT) AS [Length], -1 AS [SymbolId], @tmp AS [Value]");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write(((bool)a.lines)?", @errorLine AS [Line], @errorColumn AS [Column]":"");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("\r\nSET @capture = SUBSTRING(@capture,@ai+1,(DATALENGTH(@capture)/2)-(@ai))\r\nSET @hasError = 0\r\n");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"

	} else {

            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], DATALENGTH(@capture) / 2 AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], -1 AS [SymbolId], @capture AS [Value]");
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write(((bool)a.lines)?", @line AS [Line], @column AS [Column]":"");
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("\r\n");
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"

	}
} else { // is final
	
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("INSERT INTO #Results SELECT @errorIndex AS [AbsolutePosition], DATALENGTH(@capture) / 2 AS [AbsoluteLength], @errorPos AS [Position], CAST((@cursorPos - @errorPos) AS INT) AS [Length], -1 AS [SymbolId], @capture AS [Value]");
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write(((bool)a.lines)?", @errorLine AS [Line], @errorColumn AS [Column]":"");
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("\r\n");
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"

}
if(!skipErrorCheck) {
a._indent = ((int)a._indent) - 1;
            #line 21 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("\r\nEND\r\n");
            #line 23 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
} if(skipErrorCheck) {
            #line 23 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
            Response.Write("\r\n");
            #line 24 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerYieldPendingErrorResult.template"
}
            Response.Flush();
        }
    }
}
