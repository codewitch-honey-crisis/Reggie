using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSMatcherYieldResult(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
dynamic a= Arguments;
if(""==(string)a.token) {

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
            Response.Write("yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), Value: sb.ToString()");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
            Response.Write(((bool)a.lines)?", Line: line, Column: column":"");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
            Response.Write(");");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"

} else {

            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
            Response.Write("yield return new ");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
            Response.Write(a.token);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
            Response.Write("(absoluteIndex, (int)(absi - absoluteIndex), position, (int)(cursorPos - position), sb.ToString()");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
            Response.Write(((bool)a.lines)?", line, column":"");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
            Response.Write(");");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
}
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherYieldResult.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
