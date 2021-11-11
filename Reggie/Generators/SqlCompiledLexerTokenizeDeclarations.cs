using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledLexerTokenizeDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeDeclarations.template"
dynamic a = Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeDeclarations.template"
            Response.Write("DECLARE @adv INT\r\nDECLARE @valueEnd INT = DATALENGTH(@value)/2+1\r\nDECLARE @index INT = 0\r\nDECLARE @absoluteIndex BIGINT = 0\r\nDECLARE @absi BIGINT = 0\r\nDECLARE @ch BIGINT\r\nDECLARE @ch1 NCHAR\r\nDECLARE @ch2 NCHAR\r\nDECLARE @tch BIGINT\r\nDECLARE @state INT = 0\r\nDECLARE @toState INT = -1\r\nDECLARE @accept INT = -1\r\nDECLARE @cursorPos BIGINT = @position\r\nDECLARE @capture NVARCHAR(MAX) = N\'\'\r\nDECLARE @tmp NVARCHAR(MAX) = N\'\'\r\nDECLARE @blockId INT\r\nDECLARE @result INT = 0\r\nDECLARE @len INT = 0\r\nDECLARE @flags INT = 0\r\nDECLARE @matched INT = 0\r\nDECLARE @errorIndex BIGINT = 0\r\nDECLARE @errorPos BIGINT = 0\r\nDECLARE @hasError INT = 0\r\nDECLARE @ai INT\r\nDECLARE @newIndex INT\r\nDECLARE @newCursorPos INT\r\nDECLARE @newAbsi BIGINT\r\nDECLARE @newCapture NVARCHAR(MAX)\r\nDECLARE @newCh BIGINT\r\nDECLARE @newTch BIGINT\r\nDECLARE @newCh1 NCHAR\r\nDECLARE @newCh2 NCHAR");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeDeclarations.template"
 
if((bool)a.lines) {
            #line 34 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeDeclarations.template"
            Response.Write("\r\nDECLARE @lc INT = @line\r\nDECLARE @cc INT = @column\r\nDECLARE @newLC INT\r\nDECLARE @newCC INT\r\nDECLARE @errorLine INT\r\nDECLARE @errorColumn INT");
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeDeclarations.template"

}
            #line 41 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeDeclarations.template"
            Response.Write("\r\n");
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeDeclarations.template"
            Response.Flush();
        }
    }
}
