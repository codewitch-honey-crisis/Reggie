using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledLexerYieldNonEmptyResult(TextWriter Response, IDictionary<string, object> Arguments, string symbol, int symbolId) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldNonEmptyResult.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldNonEmptyResult.template"
            Response.Write("if(sb.Length > 0) {");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldNonEmptyResult.template"
a._indent = ((int)a._indent) + 1;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldNonEmptyResult.template"
            Response.Write("\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldNonEmptyResult.template"
a.CompiledLexerYieldResult(symbol, symbolId);
a._indent = ((int)a._indent) - 1;
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldNonEmptyResult.template"
            Response.Write("}\r\n");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldNonEmptyResult.template"
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldNonEmptyResult.template"
            Response.Flush();
        }
    }
}
