using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlMatcherReturnResultList(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherReturnResultList.template"
            Response.Write("SELECT * FROM #Results\r\nDROP TABLE #Results\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherReturnResultList.template"
            Response.Flush();
        }
    }
}
