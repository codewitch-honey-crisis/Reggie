using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledCheckerCheckEmptyString(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckEmptyString.template"
dynamic a = Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckEmptyString.template"
            Response.Write("IF @ch = -1 RETURN 1\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckEmptyString.template"
            Response.Flush();
        }
    }
}
