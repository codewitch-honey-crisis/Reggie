using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSLexerYieldPendingErrorResult(TextWriter Response, IDictionary<string, object> Arguments, bool isFinal, bool skipErrorCheck) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
if(!skipErrorCheck) { 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("if(hasError");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(isFinal?" && sb.Length > 0":"");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(") {\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
a._indent = ((int)a._indent) + 1; }
if(!isFinal) {
	if(!skipErrorCheck) {
		
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("ai = (int)(absoluteIndex - errorIndex);\r\n");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"

		if(""==(string)a.token) {
			
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai)");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(((bool)a.lines)?", Line: errorLine, Column: errorColumn":"");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(");\r\n");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"

		} else {
			
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("yield return new ");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(a.token);
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("(errorIndex, ai, errorPos, (int)(position - errorPos), sb.ToString(0, ai)");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(((bool)a.lines)?", errorLine, errorColumn":"");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(");\r\n");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"

		}
			
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("sb.Remove(0, ai);\r\nhasError = false;\r\n");
            #line 16 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"

	} else {
		if(""==(string)a.token) {
			
            #line 19 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: sb.Length, Position: position, Length: (int)(cursorPos - position), SymbolId: ERROR, Value: sb.ToString()");
            #line 19 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(((bool)a.lines)?", Line: line, Column: column":"");
            #line 19 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(");\r\n");
            #line 20 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"

		} else {
			
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("yield return new ");
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(a.token);
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("(absoluteIndex, sb.Length, position, (int)(cursorPos - position), ERROR, sb.ToString()");
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(((bool)a.lines)?", line, column":"");
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(");\r\n");
            #line 23 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"

		}
		
            #line 25 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("\r\n");
            #line 26 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"

	}
} else { // is final
	if(""==(string)a.token) {
			
            #line 30 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("yield return (AbsolutePosition: errorIndex, AbsoluteLength: sb.Length, Position: errorPos, Length: (int)(cursorPos - errorPos), SymbolId: ERROR, Value: sb.ToString()");
            #line 30 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(((bool)a.lines)?", Line: errorLine, Column: errorColumn":"");
            #line 30 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(");\r\n");
            #line 31 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"

	} else {
			
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("yield return new ");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(a.token);
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("(errorIndex, sb.Length, errorPos, (int)(cursorPos - errorPos), sb.ToString()");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(((bool)a.lines)?", errorLine, errorColumn":"");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write(");\r\n");
            #line 34 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"

	}
}
if(!skipErrorCheck) {
a._indent = ((int)a._indent) - 1;
            #line 38 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("\r\n}\r\n");
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
} if(skipErrorCheck) {
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
            Response.Write("\r\n");
            #line 41 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerYieldPendingErrorResult.template"
}
            Response.Flush();
        }
    }
}
