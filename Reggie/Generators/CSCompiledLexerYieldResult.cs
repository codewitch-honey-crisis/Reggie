using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledLexerYieldResult(TextWriter Response, IDictionary<string, object> Arguments, string symbol, int symbolId) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
dynamic a= Arguments;
if(""==(string)a.token) {

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write("yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: ");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write(symbol);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write(", Value: sb.ToString()");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write(((bool)a.lines)?", Line: line, Column: column":"");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write(");");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"

} else {

            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write("yield return new ");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write(a.token);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write("(absoluteIndex, (int)(absi - absoluteIndex), position, (int)(cursorPos - position), ");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write(symbol);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write(", sb.ToString()");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write(((bool)a.lines)?", line, column":"");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write(");");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"

}
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerYieldResult.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
