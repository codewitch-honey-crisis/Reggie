using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSBreakInputLoop(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSBreakInputLoop.template"
            Response.Write("break;\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSBreakInputLoop.template"
            Response.Flush();
        }
    }
}
