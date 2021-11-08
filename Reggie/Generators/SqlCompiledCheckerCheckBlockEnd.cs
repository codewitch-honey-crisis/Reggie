using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledCheckerCheckBlockEnd(TextWriter Response, IDictionary<string, object> Arguments, string symbol) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckBlockEnd.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckBlockEnd.template"
            Response.Write("EXEC @result = ");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckBlockEnd.template"
            Response.Write(a.@class);
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckBlockEnd.template"
            Response.Write("_Is");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckBlockEnd.template"
            Response.Write(symbol);
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckBlockEnd.template"
            Response.Write("BlockEnd @value, @index, @ch\r\nRETURN @result\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerCheckBlockEnd.template"


            Response.Flush();
        }
    }
}
