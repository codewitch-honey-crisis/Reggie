using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableLexerGetBlockEnd(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerGetBlockEnd.template"
            Response.Write("var blockEnd = TokenizeBlockEndDfaStateTables[acc];\r\n");
            Response.Flush();
        }
    }
}
