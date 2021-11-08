using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableLexerYieldResult(TextWriter Response, IDictionary<string, object> Arguments, bool isBlockEnd) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
dynamic a= Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write("if(0==(TokenizeSymbolFlags[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(isBlockEnd?"sacc":"acc");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write("] & 1)) {");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
a._indent =((int)a._indent)+1;
if(""==(string)a.token) {

            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write("\r\nyield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: ");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(isBlockEnd?"sacc":"acc");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(", Value: sb.ToString()");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(((bool)a.lines)?", Line: line, Column: column":"");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(");");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"

} else {

            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write("\r\nyield return new ");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(a.token);
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write("(absoluteIndex, (int)(absi - absoluteIndex), position, (int)(cursorPos - position), ");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(isBlockEnd?"sacc":"acc");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(", sb.ToString()");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(((bool)a.lines)?", line, column":"");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write(");");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"

}
a._indent =((int)a._indent)-1;

            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerYieldResult.template"
            Response.Write("\r\n}\r\n");
            Response.Flush();
        }
    }
}
