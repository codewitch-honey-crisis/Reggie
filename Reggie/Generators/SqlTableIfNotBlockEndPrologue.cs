using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableIfNotBlockEndPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotBlockEndPrologue.template"
            Response.Write("ELSE -- IF not block end\r\nBEGIN");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotBlockEndPrologue.template"
dynamic a = Arguments; a._indent=((int)a._indent) +1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotBlockEndPrologue.template"
            Response.Write("\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotBlockEndPrologue.template"
            Response.Flush();
        }
    }
}
