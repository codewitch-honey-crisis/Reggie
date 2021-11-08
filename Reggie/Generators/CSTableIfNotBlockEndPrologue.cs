using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableIfNotBlockEndPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableIfNotBlockEndPrologue.template"
            Response.Write("else {\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableIfNotBlockEndPrologue.template"
dynamic a = Arguments; a._indent=((int)a._indent) +1;
            Response.Flush();
        }
    }
}
