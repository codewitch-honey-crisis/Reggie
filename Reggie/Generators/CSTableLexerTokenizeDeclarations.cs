using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableLexerTokenizeDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"
dynamic a = Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"
            Response.Write("int adv;\r\nvar sb = new System.Text.StringBuilder();\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"
if(!(bool)a.textreader) { 
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"
            Response.Write("var cursor = text.GetEnumerator();\r\n");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"
}
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"
            Response.Write("int tlen;\r\nint tto;\r\nint prlen;\r\nint pmin;\r\nint pmax;\r\nint i;\r\nint j;\r\nvar hasError = false;\r\nbool matched;\r\nvar errorPos = position;\r\nvar absoluteIndex = 0L;\r\nvar errorIndex = absoluteIndex;\r\nvar cursorPos = position;\r\nvar absi = 0L;");
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"
if((bool)a.lines) {
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"
            Response.Write("\r\nvar lc = line;\r\nvar cc = column;\r\nvar errorLine = line;\r\nvar errorColumn = column;");
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"

}
            #line 23 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeDeclarations.template"
            Response.Write("\r\nint ch;\r\nvar state = 0;\r\nbool done;\r\nint sacc;\r\nvar acc = -1;\r\nint ai;\r\n");
            Response.Flush();
        }
    }
}
