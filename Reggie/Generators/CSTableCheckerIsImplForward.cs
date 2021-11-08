using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableCheckerIsImplForward(TextWriter Response, IDictionary<string, object> Arguments, string symbol, int symbolId) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableCheckerIsImplForward.template"

dynamic a = Arguments;
var be = ((int[][])a._blockEndDfas)[symbolId];

            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableCheckerIsImplForward.template"
            Response.Write("return TableIs(");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableCheckerIsImplForward.template"
            Response.Write(symbol);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableCheckerIsImplForward.template"
            Response.Write("DfaStateTable, ");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableCheckerIsImplForward.template"
            Response.Write((be==null||be.Length==0)?"null":symbol+"BlockEndDfaStateTable");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableCheckerIsImplForward.template"
            Response.Write(", text);\r\n");
            Response.Flush();
        }
    }
}
