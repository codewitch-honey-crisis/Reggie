using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableIfNotMatchedBlockEndPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotMatchedBlockEndPrologue.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotMatchedBlockEndPrologue.template"
            Response.Write("IF @ch = -1 AND DATALENGTH(@capture) > 0\r\nBEGIN");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotMatchedBlockEndPrologue.template"
a._indent = ((int)a._indent) + 1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotMatchedBlockEndPrologue.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
