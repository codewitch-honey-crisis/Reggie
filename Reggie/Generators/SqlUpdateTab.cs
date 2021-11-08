using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlUpdateTab(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateTab.template"
dynamic a = Arguments; if((bool)a.lines) { 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateTab.template"
            Response.Write("SET @cc = (((@cc - 1) / @tabWidth) + 1) * @tabWidth + 1");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateTab.template"

}
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateTab.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
