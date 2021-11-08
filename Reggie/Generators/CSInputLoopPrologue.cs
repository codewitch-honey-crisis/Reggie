using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSInputLoopPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSInputLoopPrologue.template"
            Response.Write("while(ch != -1) {\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSInputLoopPrologue.template"
dynamic a = Arguments; a._indent=((int)a._indent) +1;
            Response.Flush();
        }
    }
}
