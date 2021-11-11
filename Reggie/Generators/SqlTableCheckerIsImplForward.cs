using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableCheckerIsImplForward(TextWriter Response, IDictionary<string, object> Arguments, string symbol, int symbolId) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplForward.template"

dynamic a = Arguments;
var be = ((int[][])a._blockEndDfas)[symbolId];

            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplForward.template"
            Response.Write("DECLARE @result INT\r\nEXEC @result = ");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplForward.template"
            Response.Write(a.@class);
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplForward.template"
            Response.Write("_TableIs @symbolId = ");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplForward.template"
            Response.Write(symbolId);
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplForward.template"
            Response.Write(", @value = @value\r\nRETURN @result\r\n");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplForward.template"
            Response.Flush();
        }
    }
}
