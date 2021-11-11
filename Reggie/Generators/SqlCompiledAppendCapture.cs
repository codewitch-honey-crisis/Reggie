using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledAppendCapture(TextWriter Response, IDictionary<string, object> Arguments, bool ascii) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledAppendCapture.template"

if(ascii) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledAppendCapture.template"
            Response.Write("SET @capture = @capture + @ch1");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledAppendCapture.template"

} else {
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledAppendCapture.template"
            Response.Write("SET @capture = @capture + @ch1\r\nIF @tch < 2048 SET @capture = @capture + @ch2");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledAppendCapture.template"

}
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledAppendCapture.template"
            Response.Write("\r\n");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledAppendCapture.template"
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledAppendCapture.template"
            Response.Flush();
        }
    }
}
