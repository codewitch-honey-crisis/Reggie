using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlUpdateCarriageReturn(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateCarriageReturn.template"
dynamic a = Arguments; if((bool)a.lines) { 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateCarriageReturn.template"
            Response.Write("SET @cc = 1\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateCarriageReturn.template"
}
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlUpdateCarriageReturn.template"
            Response.Flush();
        }
    }
}
