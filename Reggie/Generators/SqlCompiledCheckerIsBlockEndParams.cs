using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledCheckerIsBlockEndParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerIsBlockEndParams.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerIsBlockEndParams.template"
            Response.Write("@value ");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerIsBlockEndParams.template"
            Response.Write((bool)a.ntext?"NTEXT":"NVARCHAR(MAX)");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerIsBlockEndParams.template"
            Response.Write(", @index INT, @ch INT");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerIsBlockEndParams.template"
            Response.Flush();
        }
    }
}
