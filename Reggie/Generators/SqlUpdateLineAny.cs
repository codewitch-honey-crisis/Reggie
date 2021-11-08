using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlUpdateLineAny(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateLineAny.template"
dynamic a = Arguments;
if((bool)a.lines) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateLineAny.template"
            Response.Write("\r\nSET @cc = CASE @ch WHEN 9 THEN (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1 WHEN 10 THEN 1 WHEN 13 THEN 1 ELSE @cc END\r\nSET @lc = CASE @ch WHEN 10 THEN @lc+1 ELSE @lc END\r\nIF @ch>31 SET @cc = @cc + 1");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateLineAny.template"

}
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateLineAny.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
