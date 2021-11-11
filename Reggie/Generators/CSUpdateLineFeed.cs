using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSUpdateLineFeed(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineFeed.template"
dynamic a = Arguments; if((bool)a.lines) { 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineFeed.template"
            Response.Write("cc = 1;\r\n++lc;\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineFeed.template"
}
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineFeed.template"
            Response.Flush();
        }
    }
}
