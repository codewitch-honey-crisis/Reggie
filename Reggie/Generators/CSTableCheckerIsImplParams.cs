using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableCheckerIsImplParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableCheckerIsImplParams.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableCheckerIsImplParams.template"
            Response.Write("int[] dfa, int[] blockEnd, System.Collections.Generic.IEnumerable<char> text");
            Response.Flush();
        }
    }
}
