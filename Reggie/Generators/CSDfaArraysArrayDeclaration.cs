using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSDfaArraysArrayDeclaration(TextWriter Response, IDictionary<string, object> Arguments, string fieldName, int[][] arrays) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"

dynamic a = Arguments;
a.Comment("DFA state machine tables");

            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write("static readonly int[][] ");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write(fieldName);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write(" = new int[][] {");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
a._indent=((int)a._indent)+1;
for(var i = 0;i < arrays.Length; ++i) {
	var array = arrays[i];
	a._indent=(int)a._indent+1;
	if(null==array) {
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write("\r\nnull");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"

	} else {
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write("\r\nnew int[] {");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"

		
		for(var j = 0;j<array.Length;++j) {
			a._indent=(int)a._indent+1;
			if(0==(j % 20)) {
            #line 16 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write("\r\n");
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"

			}
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write(array[j]);
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write(j<array.Length-1?", ":"");
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"

			a._indent=(int)a._indent-1;
		}
		
		
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write("\r\n}");
            #line 23 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"

	}
            #line 24 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write(i<arrays.Length-1?", ":"");
            #line 24 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"

	a._indent=(int)a._indent-1;
}
            #line 26 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write("\r\n");
            #line 27 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
a._indent=((int)a._indent)-1;
            #line 27 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSDfaArraysArrayDeclaration.template"
            Response.Write("};\r\n");
            Response.Flush();
        }
    }
}
