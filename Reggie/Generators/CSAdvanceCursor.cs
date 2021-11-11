using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSAdvanceCursor(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSAdvanceCursor.template"
            Response.Write("absi += adv;\r\n++cursorPos;\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSAdvanceCursor.template"
            Response.Flush();
        }
    }
}
