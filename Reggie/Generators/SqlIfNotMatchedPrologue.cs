using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlIfNotMatchedPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlIfNotMatchedPrologue.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlIfNotMatchedPrologue.template"
            Response.Write("IF @matched = 0\r\nBEGIN");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlIfNotMatchedPrologue.template"
a._indent = ((int)a._indent) + 1;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlIfNotMatchedPrologue.template"
            Response.Flush();
        }
    }
}
