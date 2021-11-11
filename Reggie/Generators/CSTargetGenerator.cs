using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTargetGenerator(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTargetGenerator.template"

dynamic a = Arguments;
a.MainFile();
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTargetGenerator.template"
            Response.Flush();
        }
    }
}
