using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSUpdateNonControl(TextWriter Response, IDictionary<string, object> Arguments, bool check) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateNonControl.template"
dynamic a = Arguments; if((bool)a.lines) { 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateNonControl.template"
            Response.Write(check?"if(ch > 31) ++cc;":"++cc;");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateNonControl.template"
            Response.Write("\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateNonControl.template"
}
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateNonControl.template"
            Response.Flush();
        }
    }
}
