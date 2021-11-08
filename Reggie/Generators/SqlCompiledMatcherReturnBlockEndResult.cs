using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledMatcherReturnBlockEndResult(TextWriter Response, IDictionary<string, object> Arguments, bool success) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherReturnBlockEndResult.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherReturnBlockEndResult.template"
            Response.Write("SET @newCh = @ch\r\nSET @newCapture = @capture\r\nSET @newTch = @tch\r\nSET @newCh1 = @ch1\r\nSET @newCh2 = @ch2\r\nSET @newIndex = @index\r\nSET @newCursorPos = @cursorPos\r\nSET @newAbsi = @absi\r\n");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherReturnBlockEndResult.template"
if((bool)a.lines) {

            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherReturnBlockEndResult.template"
            Response.Write("SET @newLC = @lc\r\nSET @newCC = @cc");
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherReturnBlockEndResult.template"
}
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherReturnBlockEndResult.template"
            Response.Write("\r\nRETURN ");
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherReturnBlockEndResult.template"
            Response.Write(success?"1":"0");
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherReturnBlockEndResult.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
