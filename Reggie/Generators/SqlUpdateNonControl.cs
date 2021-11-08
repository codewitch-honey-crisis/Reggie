using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlUpdateNonControl(TextWriter Response, IDictionary<string, object> Arguments, bool check) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateNonControl.template"
dynamic a = Arguments; if((bool)a.lines) { 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateNonControl.template"
            Response.Write(check?"IF @ch > 31 SET @cc = @cc + 1":"SET @cc = @cc + 1");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateNonControl.template"

}
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateNonControl.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
