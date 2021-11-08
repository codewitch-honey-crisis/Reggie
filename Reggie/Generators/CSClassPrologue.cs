using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSClassPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
 dynamic a = Arguments;
string s = "matcher and checker";
if((bool)a.lexer) {
	s = "lexer/tokenizer";
} else if((bool)a.matcher!=(bool)a.checker) {
	s = ((bool)a.matcher)?"matcher":"checker";
}

            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write("/// <summary>Represents a ");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write(s);
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write(" for the regular expressions in ");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write(Path.GetFileName((string)a.input));
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write("</summary>\r\n[System.CodeDom.Compiler.GeneratedCodeAttribute(\"");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write(a._name);
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write("\", \"");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write(a._version);
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write("\")]\r\npartial class ");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write((string)a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
            Response.Write(" {\r\n");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassPrologue.template"
a._indent=((int)a._indent)+1;
            Response.Flush();
        }
    }
}
