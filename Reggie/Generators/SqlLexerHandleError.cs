using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlLexerHandleError(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerHandleError.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerHandleError.template"
            Response.Write("if @hasError = 0\r\nBEGIN");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerHandleError.template"
a._indent=((int)a._indent)+1;
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerHandleError.template"
            Response.Write("\r\nSET @errorPos = @position\r\nSET @errorIndex = @absoluteIndex");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerHandleError.template"

if((bool)a.lines) {
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerHandleError.template"
            Response.Write("\r\nSET @errorColumn = @column\r\nSET @errorLine = @line");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerHandleError.template"
}
a._indent=((int)a._indent)-1;

            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerHandleError.template"
            Response.Write("\r\nEND\r\nSET @hasError = 1\r\n");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerHandleError.template"
            Response.Flush();
        }
    }
}
