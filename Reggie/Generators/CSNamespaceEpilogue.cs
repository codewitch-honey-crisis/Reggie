using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSNamespaceEpilogue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespaceEpilogue.template"

dynamic a=Arguments;
if(""!=a.@namespace) {
a._indent=((int)a._indent)-1;

            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespaceEpilogue.template"
            Response.Write("}\r\n");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespaceEpilogue.template"
}
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespaceEpilogue.template"
            Response.Flush();
        }
    }
}
