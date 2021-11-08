using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCheckerAccept(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCheckerAccept.template"
            Response.Write("RETURN CASE @ch WHEN -1 THEN 1 ELSE 0 END\r\n");
            Response.Flush();
        }
    }
}
