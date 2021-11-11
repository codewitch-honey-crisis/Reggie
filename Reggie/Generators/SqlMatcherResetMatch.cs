using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlMatcherResetMatch(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"
            Response.Write("SET @capture = N\'\'\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"
if((bool)a.tables) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"
            Response.Write("SET @blockId = -1");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"
}
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"
            Response.Write("\r\nSET @position = @cursorPos\r\nSET @absoluteIndex = @absi");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"

if((bool)a.lines) {
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"
            Response.Write("\r\nSET @line = @lc\r\nSET @column = @cc");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"

}
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"
            Response.Write("\r\n");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherResetMatch.template"
            Response.Flush();
        }
    }
}
