using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSReadCodepoint(TextWriter Response, IDictionary<string, object> Arguments, bool isChecker) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSReadCodepoint.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSReadCodepoint.template"
            Response.Write("ch = ReadUtf32(");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSReadCodepoint.template"
            Response.Write((bool)a.textreader?"text":"cursor");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSReadCodepoint.template"
            Response.Write(", out adv);\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSReadCodepoint.template"
            Response.Flush();
        }
    }
}
