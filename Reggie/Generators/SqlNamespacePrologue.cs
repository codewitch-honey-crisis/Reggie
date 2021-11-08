using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlNamespacePrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlNamespacePrologue.template"

dynamic a = Arguments;
a.Comment(string.Format("This file was generated using {0} {1} from the\r\n{2} specification file on {3} UTC",a._name,a._version,Path.GetFileName((string)a.input),DateTime.UtcNow));
if(""!=(string)a.database) {
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlNamespacePrologue.template"
            Response.Write("use [");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlNamespacePrologue.template"
            Response.Write(a.@database);
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlNamespacePrologue.template"
            Response.Write("]\r\nGO\r\n");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlNamespacePrologue.template"
}
            Response.Flush();
        }
    }
}
