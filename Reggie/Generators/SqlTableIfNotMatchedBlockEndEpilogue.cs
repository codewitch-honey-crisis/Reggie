using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableIfNotMatchedBlockEndEpilogue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotMatchedBlockEndEpilogue.template"

dynamic a=Arguments;
a._indent=((int)a._indent)-1;

            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotMatchedBlockEndEpilogue.template"
            Response.Write("END\r\n");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableIfNotMatchedBlockEndEpilogue.template"
            Response.Flush();
        }
    }
}
