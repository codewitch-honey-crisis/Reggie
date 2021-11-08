using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSMatcherMatchReturn(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchReturn.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchReturn.template"
            Response.Write("System.Collections.Generic.IEnumerable<");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchReturn.template"
            Response.Write(""==(string)a.token?("(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value"+(((bool)a.lines)?", int Line, int Column":"")+")"):(string)a.token);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchReturn.template"
            Response.Write(">");
            Response.Flush();
        }
    }
}
