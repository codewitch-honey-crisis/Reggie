using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableRejectPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableRejectPrologue.template"
            Response.Write("ELSE -- IF not accept\r\nBEGIN");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableRejectPrologue.template"
dynamic a = Arguments; a._indent=((int)a._indent) +1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableRejectPrologue.template"
            Response.Write("\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableRejectPrologue.template"
            Response.Flush();
        }
    }
}
