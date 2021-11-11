using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlUpdateLineFeed(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateLineFeed.template"
dynamic a = Arguments; if((bool)a.lines) { 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateLineFeed.template"
            Response.Write("SET @cc = 1\r\nSET @lc = @lc + 1");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateLineFeed.template"

}
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateLineFeed.template"
            Response.Write("\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateLineFeed.template"
            Response.Flush();
        }
    }
}
