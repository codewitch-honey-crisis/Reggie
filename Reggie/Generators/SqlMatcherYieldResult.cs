using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlMatcherYieldResult(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherYieldResult.template"
dynamic a= Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherYieldResult.template"
            Response.Write("INSERT INTO #Results SELECT @absoluteIndex AS [AbsolutePosition], CAST((@absi - @absoluteIndex) AS INT) AS [AbsoluteLength], @position AS [Position], CAST((@cursorPos - @position) AS INT) AS [Length], @capture AS [Value]");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherYieldResult.template"
            Response.Write(((bool)a.lines)?", @line AS [Line], @column AS [Column]":"");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherYieldResult.template"


            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherYieldResult.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
