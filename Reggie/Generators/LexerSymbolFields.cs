using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void LexerSymbolFields(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\LexerSymbolFields.template"

dynamic a = Arguments;
a.LexerSymbolField("ERROR",-1);
var symbolTable = (string[])a._symbolTable;
for(var i = 0;i<symbolTable.Length;++i) {
	var sym = symbolTable[i];
	if(sym!=null) {
		a.LexerSymbolField(sym,i);
	}
}

            Response.Flush();
        }
    }
}
