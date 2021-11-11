using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableIfBlockEndPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableIfBlockEndPrologue.template"
            Response.Write("if(blockEnd != null) {");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableIfBlockEndPrologue.template"
dynamic a = Arguments; a._indent=((int)a._indent) +1;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableIfBlockEndPrologue.template"
            Response.Write("\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableIfBlockEndPrologue.template"
            Response.Flush();
        }
    }
}
