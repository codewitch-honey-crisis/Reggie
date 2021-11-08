using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledRangeMatchTestPrologue(TextWriter Response, IDictionary<string, object> Arguments, int[] dfa, int prlenIndex) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledRangeMatchTestPrologue.template"

dynamic a = Arguments;
// WriteSqlRangeCharMatchTests is from older code so it doesn't use the template engine
// it could be updated but if it's not broke, don't fix it.

            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledRangeMatchTestPrologue.template"
            Response.Write("IF ");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledRangeMatchTestPrologue.template"
WriteSqlRangeCharMatchTests(dfa, prlenIndex, 2, Response);
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledRangeMatchTestPrologue.template"
            Response.Write("\r\nBEGIN");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledRangeMatchTestPrologue.template"
a._indent = ((int)a._indent) + 1;
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledRangeMatchTestPrologue.template"
            Response.Write("\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledRangeMatchTestPrologue.template"
            Response.Flush();
        }
    }
}
