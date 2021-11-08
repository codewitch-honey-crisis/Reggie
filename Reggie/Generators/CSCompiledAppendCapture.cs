using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledAppendCapture(TextWriter Response, IDictionary<string, object> Arguments, bool ascii) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledAppendCapture.template"

if(ascii) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledAppendCapture.template"
            Response.Write("sb.Append(unchecked((char)ch));");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledAppendCapture.template"

} else {
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledAppendCapture.template"
            Response.Write("sb.Append(char.ConvertFromUtf32(ch));");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledAppendCapture.template"

}
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledAppendCapture.template"
            Response.Write("\r\n");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledAppendCapture.template"
            Response.Flush();
        }
    }
}
