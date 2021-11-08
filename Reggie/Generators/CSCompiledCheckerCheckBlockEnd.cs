using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledCheckerCheckBlockEnd(TextWriter Response, IDictionary<string, object> Arguments, string symbol) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledCheckerCheckBlockEnd.template"
            Response.Write("return Is");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledCheckerCheckBlockEnd.template"
            Response.Write(symbol);
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledCheckerCheckBlockEnd.template"
            Response.Write("BlockEnd(cursor, ch);\r\n");
            Response.Flush();
        }
    }
}
