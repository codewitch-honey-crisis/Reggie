using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableMatcherMatchImplParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplParams.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplParams.template"
            Response.Write("@symbolId INT, @value ");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplParams.template"
            Response.Write((bool)a.ntext?"NTEXT":"NVARCHAR(MAX)");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplParams.template"
            Response.Write(", @position BIGINT = 0");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplParams.template"
            Response.Write(((bool)a.lines)?", @line INT = 1, @column INT = 1, @tabWidth INT = 4":"");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMatcherMatchImplParams.template"
            Response.Flush();
        }
    }
}
