using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledCheckerIsDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledCheckerIsDeclarations.template"
            Response.Write("DECLARE @adv INT\r\nDECLARE @ch BIGINT\r\nDECLARE @tch BIGINT\r\nDECLARE @index INT = 0\r\nDECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\nDECLARE @result INT\r\n");
            Response.Flush();
        }
    }
}
