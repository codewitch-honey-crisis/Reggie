using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledMatcherMatchDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchDeclarations.template"
dynamic a = Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchDeclarations.template"
            Response.Write("DECLARE @adv INT\r\nDECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\nDECLARE @index INT = 0\r\nDECLARE @absi BIGINT = 0\r\nDECLARE @ch BIGINT\r\nDECLARE @ch1 NCHAR\r\nDECLARE @ch2 NCHAR\r\nDECLARE @tch BIGINT\r\nDECLARE @state INT = 0\r\nDECLARE @toState INT = -1\r\nDECLARE @accept INT = -1\r\nDECLARE @capture NVARCHAR(MAX)\r\nDECLARE @blockEndId INT\r\nDECLARE @cursorPos BIGINT = @position\r\nDECLARE @absoluteIndex INT\r\nDECLARE @result INT = 0\r\nDECLARE @len INT = 0\r\nDECLARE @newIndex INT\r\nDECLARE @newCursorPos INT\r\nDECLARE @newCapture NVARCHAR(MAX)\r\nDECLARE @newCh BIGINT\r\nDECLARE @newTch BIGINT\r\nDECLARE @newAbsi BIGINT\r\nDECLARE @newCh1 NCHAR\r\nDECLARE @newCh2 NCHAR");
            #line 26 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchDeclarations.template"

if((bool)a.lines) {
            #line 27 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchDeclarations.template"
            Response.Write("\r\nDECLARE @lc INT = @line\r\nDECLARE @cc INT = @column\r\nDECLARE @newLC INT\r\nDECLARE @newCC INT");
            #line 31 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchDeclarations.template"
}

            #line 32 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchDeclarations.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
