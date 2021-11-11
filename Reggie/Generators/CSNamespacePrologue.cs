using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSNamespacePrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespacePrologue.template"

dynamic a = Arguments;
a.Comment(string.Format("This file was generated using {0} {1} from the\r\n{2} specification file on {3} UTC",a._name,a._version,Path.GetFileName((string)a.input),DateTime.UtcNow));
if(""!=a.@namespace) {
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespacePrologue.template"
            Response.Write("namespace ");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespacePrologue.template"
            Response.Write(a.@namespace);
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespacePrologue.template"
            Response.Write(" {\r\n");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespacePrologue.template"

a._indent=((int)a._indent)+1;
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespacePrologue.template"
            Response.Write("\r\n");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespacePrologue.template"

}
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSNamespacePrologue.template"
            Response.Flush();
        }
    }
}
