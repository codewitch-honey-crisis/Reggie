using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CompiledCommonLexer(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledCommonLexer.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledCommonLexer.template"
a.LexerSymbolFields();
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledCommonLexer.template"
            Response.Write("\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledCommonLexer.template"
            Response.Flush();
        }
    }
}
