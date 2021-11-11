using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledLexerTokenizeBlockEndReject(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeBlockEndReject.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeBlockEndReject.template"
            Response.Write("SET @newCh = @ch\r\nSET @newCapture = @capture\r\nSET @newTch = @tch\r\nSET @newCh1 = @ch1\r\nSET @newCh2 = @ch2\r\nSET @newIndex = @index\r\nSET @newCursorPos = @cursorPos\r\nSET @newAbsi = @absi\r\n");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeBlockEndReject.template"
if((bool)a.lines) {

            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeBlockEndReject.template"
            Response.Write("SET @newLC = @lc\r\nSET @newCC = @cc");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeBlockEndReject.template"

}
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeBlockEndReject.template"
            Response.Write("\r\nRETURN 0\r\n");
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledLexerTokenizeBlockEndReject.template"
            Response.Flush();
        }
    }
}
