using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableIfNotMatchedBlockEndEpilogue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableIfNotMatchedBlockEndEpilogue.template"

dynamic a=Arguments;
a._indent=((int)a._indent)-1;

            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableIfNotMatchedBlockEndEpilogue.template"
            Response.Write("}\r\n");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableIfNotMatchedBlockEndEpilogue.template"
            Response.Flush();
        }
    }
}
