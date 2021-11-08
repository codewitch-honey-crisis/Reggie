using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSUpdateTab(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateTab.template"
dynamic a = Arguments; if((bool)a.lines) { 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateTab.template"
            Response.Write("cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateTab.template"
}
            Response.Flush();
        }
    }
}
