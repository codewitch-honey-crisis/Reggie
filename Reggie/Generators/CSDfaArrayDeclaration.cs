using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSDfaArrayDeclaration(TextWriter Response, IDictionary<string, object> Arguments, string fieldName, int[] array) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"

dynamic a = Arguments;
a.Comment("DFA state machine table");

            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"
            Response.Write("static readonly int[] ");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"
            Response.Write(fieldName);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"
            Response.Write(" = new int[] {");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"

a._indent=(int)a._indent+1;
for(var i = 0;i<array.Length;++i) {
	if(0==(i % 20)) {
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"
            Response.Write("\r\n");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"

	}
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"
            Response.Write(array[i]);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"
            Response.Write(i<array.Length-1?", ":"");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"

}
a._indent=(int)a._indent-1;

            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArrayDeclaration.template"
            Response.Write("\r\n};\r\n");
            Response.Flush();
        }
    }
}
