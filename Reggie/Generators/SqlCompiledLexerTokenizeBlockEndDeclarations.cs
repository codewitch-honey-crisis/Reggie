using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledLexerTokenizeBlockEndDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeBlockEndDeclarations.template"
            Response.Write("DECLARE @adv INT = 0\r\nDECLARE @matched INT\r\n");
            Response.Flush();
        }
    }
}
