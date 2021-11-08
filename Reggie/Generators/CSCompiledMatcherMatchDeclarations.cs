using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledMatcherMatchDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"
dynamic a = Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"
            Response.Write("int adv;\r\nvar sb = new System.Text.StringBuilder();\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"
if(!(bool)a.textreader) { 
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"
            Response.Write("var cursor = text.GetEnumerator();\r\n");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"
}
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"
            Response.Write("var absoluteIndex = 0L;\r\nvar cursorPos = position;\r\nvar absi = 0L;");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"
if((bool)a.lines) {
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"
            Response.Write("\r\nvar lc = line;\r\nvar cc = column;");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"

}
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchDeclarations.template"
            Response.Write("\r\nint ch;\r\n");
            Response.Flush();
        }
    }
}
