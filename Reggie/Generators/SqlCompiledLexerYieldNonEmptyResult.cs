using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledLexerYieldNonEmptyResult(TextWriter Response, IDictionary<string, object> Arguments, string symbol, int symbolId) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldNonEmptyResult.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldNonEmptyResult.template"
            Response.Write("IF DATALENGTH(@capture) > 0\r\nBEGIN");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldNonEmptyResult.template"
a._indent = ((int)a._indent) + 1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldNonEmptyResult.template"
            Response.Write("\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldNonEmptyResult.template"
a.CompiledLexerYieldResult(symbol, symbolId);
a._indent = ((int)a._indent) - 1;
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldNonEmptyResult.template"
            Response.Write("END\r\n");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldNonEmptyResult.template"
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerYieldNonEmptyResult.template"
            Response.Flush();
        }
    }
}
