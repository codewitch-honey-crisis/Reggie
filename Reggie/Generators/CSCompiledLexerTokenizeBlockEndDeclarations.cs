using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledLexerTokenizeBlockEndDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeBlockEndDeclarations.template"
            Response.Write("int adv = 0;\r\nbool matched;\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerTokenizeBlockEndDeclarations.template"
            Response.Flush();
        }
    }
}
