using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableRejectPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableRejectPrologue.template"
            Response.Write("else {\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableRejectPrologue.template"
dynamic a = Arguments; a._indent=((int)a._indent) +1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableRejectPrologue.template"
            Response.Flush();
        }
    }
}
