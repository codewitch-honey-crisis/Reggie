using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void TableCommonLexer(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableCommonLexer.template"
dynamic a = Arguments;
a.LexerSymbolFields();
a.DfaArrayDeclaration("TokenizeDfaStateTable",a._dfa);
a.DfaArraysArrayDeclaration("TokenizeBlockEndDfaStateTables",a._blockEndDfas);
a.DfaArrayDeclaration("TokenizeSymbolFlags",a._symbolFlags);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableCommonLexer.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
