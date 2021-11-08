using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableMatcherMatchImplForward(TextWriter Response, IDictionary<string, object> Arguments, string symbol, int symbolId) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchImplForward.template"

dynamic a = Arguments;
var be = ((int[][])a._blockEndDfas)[symbolId];

            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchImplForward.template"
            Response.Write("foreach(var result in TableMatch(");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchImplForward.template"
            Response.Write(symbol);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchImplForward.template"
            Response.Write("DfaStateTable, ");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchImplForward.template"
            Response.Write((be==null||be.Length==0)?"null":symbol+"BlockEndDfaStateTable");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchImplForward.template"
            Response.Write(", text, position");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchImplForward.template"
            Response.Write(((bool)a.lines)?", line, column, tabWidth":"");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchImplForward.template"
            Response.Write(")) {\r\n\tyield return result;\r\n}\r\n");
            Response.Flush();
        }
    }
}
