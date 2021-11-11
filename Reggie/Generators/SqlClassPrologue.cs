using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlClassPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassPrologue.template"
 dynamic a = Arguments;
string s = "matcher and checker";
if((bool)a.lexer) {
	s = "lexer/tokenizer";
} else if((bool)a.matcher!=(bool)a.checker) {
	s = ((bool)a.matcher)?"matcher":"checker";
}

            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassPrologue.template"
            Response.Write("-- <summary>Represents a ");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassPrologue.template"
            Response.Write(s);
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassPrologue.template"
            Response.Write(" for the regular expressions in ");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassPrologue.template"
            Response.Write(Path.GetFileName((string)a.input));
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassPrologue.template"
            Response.Write("</summary>\r\n");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassPrologue.template"
            Response.Flush();
        }
    }
}
