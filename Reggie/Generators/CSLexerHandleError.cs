using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSLexerHandleError(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerHandleError.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerHandleError.template"
            Response.Write("if(!hasError) {");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerHandleError.template"
a._indent=((int)a._indent)+1;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerHandleError.template"
            Response.Write("\r\nerrorPos = position;\r\nerrorIndex = absoluteIndex;");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerHandleError.template"

if((bool)a.lines) {
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerHandleError.template"
            Response.Write("\r\nerrorColumn = column;\r\nerrorLine = line;");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerHandleError.template"
}
a._indent=((int)a._indent)-1;

            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerHandleError.template"
            Response.Write("\r\n}\r\nhasError = true;\r\n");
            Response.Flush();
        }
    }
}
