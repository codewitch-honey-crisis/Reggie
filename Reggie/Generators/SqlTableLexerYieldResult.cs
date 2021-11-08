using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableLexerYieldResult(TextWriter Response, IDictionary<string, object> Arguments, bool isBlockEnd) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldResult.template"
dynamic a= Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldResult.template"
            Response.Write("IF (@flags & 1) = 0 INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], ");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldResult.template"
            Response.Write(isBlockEnd?"@sacc":"@acc");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldResult.template"
            Response.Write(" AS [SymbolId], @capture AS [Value]");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldResult.template"
            Response.Write(((bool)a.lines)?", @line AS [Line], @column AS [Column]":"");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldResult.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
