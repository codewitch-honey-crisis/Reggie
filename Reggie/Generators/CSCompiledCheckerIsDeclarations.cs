using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledCheckerIsDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledCheckerIsDeclarations.template"
            Response.Write("int adv;\r\nvar cursor = text.GetEnumerator();\r\nint ch;\r\n");
            Response.Flush();
        }
    }
}
