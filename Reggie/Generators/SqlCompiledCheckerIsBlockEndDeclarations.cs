using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledCheckerIsBlockEndDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerIsBlockEndDeclarations.template"
            Response.Write("DECLARE @adv INT\r\nDECLARE @matched INT\r\nDECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\nDECLARE @tch BIGINT\r\nDECLARE @accept INT = -1\r\nDECLARE @result INT = 0\r\n");
            Response.Flush();
        }
    }
}
