using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlLabel(TextWriter Response, IDictionary<string, object> Arguments, string name) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLabel.template"

dynamic a = Arguments;
a._indent = (int)a._indent - 1;
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLabel.template"
            Response.Write(name);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLabel.template"
            Response.Write(":");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLabel.template"
a._indent = (int)a._indent + 1;
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLabel.template"
            Response.Write("\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLabel.template"
            Response.Flush();
        }
    }
}
