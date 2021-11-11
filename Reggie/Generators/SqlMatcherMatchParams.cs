using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlMatcherMatchParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchParams.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchParams.template"
            Response.Write("@value ");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchParams.template"
            Response.Write((bool)a.ntext?"NTXT":"NVARCHAR(MAX)");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchParams.template"
            Response.Write(", @position BIGINT = 0");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchParams.template"
            Response.Write(((bool)a.lines)?", @line INT = 1, @column INT = 1, @tabWidth INT = 4":"");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchParams.template"
            Response.Flush();
        }
    }
}
