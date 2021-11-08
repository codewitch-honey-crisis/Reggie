using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledCheckerIsBlockEndParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledCheckerIsBlockEndParams.template"
            Response.Write("System.Collections.Generic.IEnumerator<char> cursor, int ch");
            Response.Flush();
        }
    }
}
