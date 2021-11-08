using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableCheckerIsImplParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplParams.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplParams.template"
            Response.Write("@symbolId INT, @value ");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerIsImplParams.template"
            Response.Write((bool)a.ntext?"NTEXT":"NVARCHAR(MAX)");
            Response.Flush();
        }
    }
}
