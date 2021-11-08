using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSLexerResetMatch(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerResetMatch.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerResetMatch.template"
            Response.Write("position = cursorPos;\r\nabsoluteIndex = absi;");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerResetMatch.template"

if((bool)a.lines) {
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerResetMatch.template"
            Response.Write("\r\nline = lc;\r\ncolumn = cc;");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerResetMatch.template"

}
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerResetMatch.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
