using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableLexerYieldNonEmptyResult(TextWriter Response, IDictionary<string, object> Arguments, bool isBlockEnd) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldNonEmptyResult.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldNonEmptyResult.template"
            Response.Write("if(sb.Length > 0) {");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldNonEmptyResult.template"
a._indent = ((int)a._indent) + 1;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldNonEmptyResult.template"
            Response.Write("\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldNonEmptyResult.template"
a.TableLexerYieldResult(isBlockEnd);
a._indent = ((int)a._indent) - 1;
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldNonEmptyResult.template"
            Response.Write("}\r\n");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldNonEmptyResult.template"
            Response.Flush();
        }
    }
}
