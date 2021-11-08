using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableCheckerIsDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableCheckerIsDeclarations.template"
            Response.Write("int adv;\r\nvar cursor = text.GetEnumerator();\r\nint tlen;\r\nint tto;\r\nint prlen;\r\nint pmin;\r\nint pmax;\r\nint i;\r\nint j;\r\nint ch;\r\nint state = 0;\r\nbool matched;\r\nbool done;\r\nint acc;\r\n");
            Response.Flush();
        }
    }
}
