using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledCheckerIsBlockEndDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledCheckerIsBlockEndDeclarations.template"
            Response.Write("int adv;\r\nbool matched;\r\n");
            Response.Flush();
        }
    }
}