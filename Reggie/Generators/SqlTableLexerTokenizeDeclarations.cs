using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableLexerTokenizeDeclarations(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeDeclarations.template"
dynamic a = Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeDeclarations.template"
            Response.Write("DECLARE @adv INT\r\nDECLARE @capture NVARCHAR(MAX) = N\'\'\r\nDECLARE @newCapture NVARCHAR(MAX)\r\nDECLARE @index INT = 0\r\nDECLARE @newIndex INT\r\nDECLARE @valueEnd INT = DATALENGTH(@value) / 2 + 1\r\nDECLARE @tch INT\r\nDECLARE @newTch INT\r\nDECLARE @ch1 NCHAR\r\nDECLARE @newCh1 NCHAR\r\nDECLARE @ch2 NCHAR\r\nDECLARE @newCh2 NCHAR\r\nDECLARE @absi BIGINT = 0\r\nDECLARE @newAbsi BIGINT\r\nDECLARE @tto INT\r\nDECLARE @hasError INT = 0\r\nDECLARE @matched INT\r\nDECLARE @errorPos BIGINT = @position\r\nDECLARE @absoluteIndex BIGINT = 0\r\nDECLARE @errorIndex BIGINT = @absoluteIndex\r\nDECLARE @cursorPos BIGINT = @position");
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeDeclarations.template"
if((bool)a.lines) {
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeDeclarations.template"
            Response.Write("\r\nDECLARE @lc INT = @line\r\nDECLARE @newLC INT\r\nDECLARE @cc INT = @column\r\nDECLARE @newCC INT\r\nDECLARE @errorLine INT = @line\r\nDECLARE @errorColumn INT = @column");
            #line 28 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeDeclarations.template"

}
            #line 29 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeDeclarations.template"
            Response.Write("\r\nDECLARE @flags INT\r\nDECLARE @toState INT\r\nDECLARE @blockId INT = -1\r\nDECLARE @tmp NVARCHAR(MAX)\r\nDECLARE @ch BIGINT\r\nDECLARE @state INT = 0\r\nDECLARE @done INT = 0\r\nDECLARE @sacc INT\r\nDECLARE @acc INT = -1\r\nDECLARE @ai INT\r\n");
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeDeclarations.template"
            Response.Flush();
        }
    }
}
