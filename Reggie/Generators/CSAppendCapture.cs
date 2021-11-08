using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSAppendCapture(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSAppendCapture.template"
            Response.Write("sb.Append(char.ConvertFromUtf32(ch));\r\n");
            Response.Flush();
        }
    }
}
