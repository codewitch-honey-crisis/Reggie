using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlLexerIfNotMatchedWithErrorEpilogue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerIfNotMatchedWithErrorEpilogue.template"
 dynamic a = Arguments;
a._indent=((int)a._indent)-1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerIfNotMatchedWithErrorEpilogue.template"
            Response.Write("END -- IF not matched with error\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerIfNotMatchedWithErrorEpilogue.template"
            Response.Flush();
        }
    }
}
