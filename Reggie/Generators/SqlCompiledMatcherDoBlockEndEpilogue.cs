using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledMatcherDoBlockEndEpilogue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndEpilogue.template"
 dynamic a = Arguments;
a._indent=((int)a._indent)-1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndEpilogue.template"
            Response.Write("END -- IF matched block end\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndEpilogue.template"
            Response.Flush();
        }
    }
}
