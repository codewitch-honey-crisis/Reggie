using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableMachineLoopPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMachineLoopPrologue.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMachineLoopPrologue.template"
            Response.Write("SET @acc = -1\r\nSET @done = 0\r\nWHILE @done = 0\r\nBEGIN");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMachineLoopPrologue.template"
a._indent=((int)a._indent) +1;
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMachineLoopPrologue.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
