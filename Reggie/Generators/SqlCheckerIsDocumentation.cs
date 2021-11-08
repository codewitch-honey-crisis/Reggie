using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCheckerIsDocumentation(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCheckerIsDocumentation.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCheckerIsDocumentation.template"
            Response.Write("-- <summary>Indicates whether the <paramref name=\"text\"/> matches the expression indicated by ");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCheckerIsDocumentation.template"
            Response.Write(a._symbol);
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCheckerIsDocumentation.template"
            Response.Write("</summary>\r\n-- <param name=\"text\">The text to validate</param>\r\n-- <returns>True if the entire contents match the expression, otherwise false</returns>\r\n");
            Response.Flush();
        }
    }
}
