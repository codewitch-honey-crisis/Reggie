using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledMatcherMatchBlockEndParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchBlockEndParams.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchBlockEndParams.template"
            Response.Write("@value ");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchBlockEndParams.template"
            Response.Write((bool)a.ntext?"NTEXT":"NVARCHAR(MAX)");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchBlockEndParams.template"
            Response.Write(", @ch BIGINT, @tch BIGINT, @ch1 NCHAR, @ch2 NCHAR, @cursorPos BIGINT, @index INT, @absi BIGINT, @valueEnd INT, @capture NVARCHAR(MAX)");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchBlockEndParams.template"
            Response.Write((bool)a.lines?", @lc INT, @cc INT, @tabWidth INT":"");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchBlockEndParams.template"
            Response.Write(", @newIndex INT OUTPUT, @newCursorPos BIGINT OUTPUT, @newAbsi BIGINT OUTPUT, @newCapture NVARCHAR(MAX) OUTPUT, @newCh BIGINT OUTPUT, @newTch BIGINT OUTPUT, @newCh1 NCHAR OUTPUT, @newCh2 NCHAR OUTPUT");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchBlockEndParams.template"
            Response.Write((bool)a.lines?", @newLC INT OUTPUT, @newCC INT OUTPUT":"");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledMatcherMatchBlockEndParams.template"
            Response.Flush();
        }
    }
}
