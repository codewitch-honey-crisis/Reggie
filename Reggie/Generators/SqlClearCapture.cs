using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlClearCapture(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClearCapture.template"
            Response.Write("SET @capture = N\'\'\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClearCapture.template"
            Response.Flush();
        }
    }
}
