using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableMatcherMatchDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"
dynamic a = Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"
            Response.Write("int adv;\r\nvar sb = new System.Text.StringBuilder();\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"
if(!(bool)a.textreader) { 
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"
            Response.Write("var cursor = text.GetEnumerator();\r\n");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"
}
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"
            Response.Write("int tlen;\r\nint tto;\r\nint prlen;\r\nint pmin;\r\nint pmax;\r\nint i;\r\nint j;\r\nvar absoluteIndex = 0L;\r\nvar cursorPos = position;\r\nvar absi = 0L;");
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"
if((bool)a.lines) {
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"
            Response.Write("\r\nvar lc = line;\r\nvar cc = column;");
            #line 16 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"

}
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMatcherMatchDeclarations.template"
            Response.Write("\r\nint ch;\r\nvar state = 0;\r\nbool done;\r\nvar acc = -1;\r\n");
            Response.Flush();
        }
    }
}
