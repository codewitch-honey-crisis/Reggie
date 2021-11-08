using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableMachineLoopPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMachineLoopPrologue.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMachineLoopPrologue.template"
            Response.Write("acc = -1;\r\ndone = false;\r\nwhile(!done) {\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMachineLoopPrologue.template"
a._indent=((int)a._indent) +1;
            Response.Flush();
        }
    }
}
