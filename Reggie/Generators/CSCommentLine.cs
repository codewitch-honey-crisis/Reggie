using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCommentLine(TextWriter Response, IDictionary<string, object> Arguments, string text) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCommentLine.template"
            Response.Write("// ");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCommentLine.template"
            Response.Write(text);
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCommentLine.template"
            Response.Write("\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCommentLine.template"
            Response.Flush();
        }
    }
}
