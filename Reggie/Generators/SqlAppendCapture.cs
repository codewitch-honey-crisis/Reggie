using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlAppendCapture(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlAppendCapture.template"
            Response.Write("SET @capture = @capture + @ch1\r\nIF @tch < 2048 SET @capture = @capture + @ch2\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlAppendCapture.template"
            Response.Flush();
        }
    }
}
