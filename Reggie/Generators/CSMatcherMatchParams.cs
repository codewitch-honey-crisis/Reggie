using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSMatcherMatchParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchParams.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchParams.template"
            Response.Write((bool)a.textreader?"System.IO.TextReader":"System.Collections.Generic.IEnumerable<char>");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchParams.template"
            Response.Write(" text, long position = 0");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchParams.template"
            Response.Write(((bool)a.lines)?", int line = 1, int column = 1, int tabWidth = 4":"");
            Response.Flush();
        }
    }
}
