using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableCheckerIsDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsDeclarations.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsDeclarations.template"
            Response.Write("DECLARE @adv INT\r\nDECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\nDECLARE @index INT = 0\r\nDECLARE @ch BIGINT\r\nDECLARE @tch BIGINT\r\nDECLARE @state INT = 0\r\nDECLARE @toState INT = -1\r\nDECLARE @acc INT = -1\r\nDECLARE @done INT\r\nDECLARE @blockId INT = -1\r\nDECLARE @matched INT\r\nDECLARE @result INT = 0\r\n");
            Response.Flush();
        }
    }
}
