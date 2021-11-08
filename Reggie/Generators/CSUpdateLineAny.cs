using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSUpdateLineAny(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
dynamic a = Arguments;
if((bool)a.lines) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("switch (ch) {\r\ncase \'\\t\':");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a._indent = ((int)a._indent) +1;
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a.UpdateTab();
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("break;");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a._indent = ((int)a._indent) -1;
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("\r\ncase \'\\r\':");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a._indent = ((int)a._indent) +1;
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("\r\n");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a.UpdateCarriageReturn();
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("break;");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a._indent = ((int)a._indent) -1;
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("\r\ncase \'\\n\':");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a._indent = ((int)a._indent) +1;
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("\r\n");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a.UpdateLineFeed();
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("break;");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a._indent = ((int)a._indent) -1;
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("\r\ndefault:");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a._indent = ((int)a._indent) +1;
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("\r\n");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a.UpdateNonControl(true);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("break;");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
a._indent = ((int)a._indent) -1;
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
            Response.Write("\r\n}\r\n");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSUpdateLineAny.template"
}
            Response.Flush();
        }
    }
}
