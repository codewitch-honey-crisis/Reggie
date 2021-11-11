using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlLexerResetMatch(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"
            Response.Write("SET @position = @cursorPos\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"
if((bool)a.tables) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"
            Response.Write("SET @blockId = -1");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"
}
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"
            Response.Write("\r\nSET @absoluteIndex = @absi");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"

if((bool)a.lines) {
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"
            Response.Write("\r\nSET @line = @lc\r\nSET @column = @cc");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"

}
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"
            Response.Write("\r\n");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerResetMatch.template"
            Response.Flush();
        }
    }
}
