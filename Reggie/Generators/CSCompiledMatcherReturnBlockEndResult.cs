using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledMatcherReturnBlockEndResult(TextWriter Response, IDictionary<string, object> Arguments, bool success) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherReturnBlockEndResult.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherReturnBlockEndResult.template"
            Response.Write("return ");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherReturnBlockEndResult.template"
            Response.Write(success?"true":"false");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherReturnBlockEndResult.template"
            Response.Write(";\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherReturnBlockEndResult.template"
            Response.Flush();
        }
    }
}
