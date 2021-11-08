using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableMatcherMatchImplForward(TextWriter Response, IDictionary<string, object> Arguments, string symbol, int symbolId) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplForward.template"

dynamic a = Arguments;

            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplForward.template"
            Response.Write("EXEC ");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplForward.template"
            Response.Write(a.@class);
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplForward.template"
            Response.Write("_TableMatch @symbolId = ");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplForward.template"
            Response.Write(symbolId);
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplForward.template"
            Response.Write(", @value = @value, @position = @position");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplForward.template"
            Response.Write(((bool)a.lines)?", @line = @line, @column = @column, @tabWidth = @tabWidth":"");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplForward.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
