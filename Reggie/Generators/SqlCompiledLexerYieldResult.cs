using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledLexerYieldResult(TextWriter Response, IDictionary<string, object> Arguments, string symbol, int symbolId) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldResult.template"
dynamic a= Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldResult.template"
            Response.Write("INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], ");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldResult.template"
            Response.Write(symbolId);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldResult.template"
            Response.Write(" AS [SymbolId], @capture AS [Value]");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldResult.template"
            Response.Write(((bool)a.lines)?", @line AS [Line], @column AS [Column]":"");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldResult.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
