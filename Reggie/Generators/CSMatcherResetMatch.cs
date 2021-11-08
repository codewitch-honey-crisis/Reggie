using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSMatcherResetMatch(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherResetMatch.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherResetMatch.template"
            Response.Write("sb.Clear();\r\nposition = cursorPos;\r\nabsoluteIndex = absi;");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherResetMatch.template"

if((bool)a.lines) {
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherResetMatch.template"
            Response.Write("\r\nline = lc;\r\ncolumn = cc;");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherResetMatch.template"

}
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherResetMatch.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
