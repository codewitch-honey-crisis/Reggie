using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlLexerIfNotMatchedWithErrorPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerIfNotMatchedWithErrorPrologue.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerIfNotMatchedWithErrorPrologue.template"
            Response.Write("IF @matched = 0 AND @hasError = 1\r\nBEGIN");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerIfNotMatchedWithErrorPrologue.template"
a._indent = ((int)a._indent) + 1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerIfNotMatchedWithErrorPrologue.template"
            Response.Write("\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerIfNotMatchedWithErrorPrologue.template"
            Response.Flush();
        }
    }
}
