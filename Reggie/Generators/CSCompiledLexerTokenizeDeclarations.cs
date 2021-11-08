using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledLexerTokenizeDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"
dynamic a = Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"
            Response.Write("int adv;\r\nvar sb = new System.Text.StringBuilder();\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"
if(!(bool)a.textreader) { 
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"
            Response.Write("var cursor = text.GetEnumerator();\r\n");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"
}
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"
            Response.Write("var hasError = false;\r\nbool matched;\r\nvar errorPos = position;\r\nvar absoluteIndex = 0L;\r\nvar errorIndex = absoluteIndex;\r\nvar cursorPos = position;\r\nvar absi = 0L;");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"
if((bool)a.lines) {
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"
            Response.Write("\r\nvar lc = line;\r\nvar cc = column;\r\nvar errorLine = line;\r\nvar errorColumn = column;");
            #line 15 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"

}
            #line 16 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeDeclarations.template"
            Response.Write("\r\nint ai;\r\nint ch;\r\n");
            Response.Flush();
        }
    }
}
