using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableLexerYieldNonEmptyResult(TextWriter Response, IDictionary<string, object> Arguments, bool isBlockEnd) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldNonEmptyResult.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldNonEmptyResult.template"
            Response.Write("IF DATALENGTH(@capture) > 0\r\nBEGIN");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldNonEmptyResult.template"
a._indent = ((int)a._indent) + 1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldNonEmptyResult.template"
            Response.Write("\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldNonEmptyResult.template"
a.TableLexerYieldResult(isBlockEnd);
a._indent = ((int)a._indent) - 1;
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldNonEmptyResult.template"
            Response.Write("END\r\n");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldNonEmptyResult.template"
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerYieldNonEmptyResult.template"
            Response.Flush();
        }
    }
}
