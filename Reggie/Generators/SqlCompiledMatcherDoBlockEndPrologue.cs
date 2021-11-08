using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledMatcherDoBlockEndPrologue(TextWriter Response, IDictionary<string, object> Arguments, string symbol) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write("EXEC @len = [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write("_Match");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write(symbol);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write("BlockEnd] @value = @value, @ch = @ch,  @tch = @tch, @ch1 = @ch1, @ch2 = @ch2, @cursorPos = @cursorPos, @index = @index, @absi = @absi, @valueEnd = @valueEnd, @capture = @capture");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write((bool)a.lines?", @lc = @lc, @cc = @cc, @tabwidth = @tabWidth":"");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write(", @newIndex = @newIndex OUTPUT, @newAbsi = @newAbsi OUTPUT, @newCursorPos = @newCursorPos OUTPUT, @newCapture = @newCapture OUTPUT, @newCh = @newCh OUTPUT, @newTch = @newTch OUTPUT, @newCh1 = @newCh1 OUTPUT, @newCh2 = @newCh2 OUTPUT");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write((bool)a.lines?", @newLC = @newLC OUTPUT, @newCC = @newCC OUTPUT":"");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write("\r\nSET @index = @newIndex\r\nSET @absi = @newAbsi\r\nSET @cursorPos = @newCursorPos\r\nSET @absi = @newAbsi\r\nSET @capture = @newCapture\r\nSET @ch = @newCh\r\nSET @tch = @newTch\r\nSET @ch1 = @newCh1\r\nSET @ch2 = @newCh2");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"

if((bool)a.lines) {
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write("\r\nSET @lc = @newLC\r\nSET @cc = @newCC");
            #line 15 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
}
            #line 15 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write("\r\nIF @len = 1\r\nBEGIN");
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
a._indent=((int)a._indent)+1;
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherDoBlockEndPrologue.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
