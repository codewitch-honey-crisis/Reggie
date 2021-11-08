using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSMethodPrologue(TextWriter Response, IDictionary<string, object> Arguments, string docTemplate, bool @private, string returnTemplate, string methodName, string parametersTemplate) {
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"

dynamic a = Arguments;
Generate(docTemplate,Arguments,Response);
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"
            Response.Write(!@private?"public ":"");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"
            Response.Write("static ");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"
Generate(returnTemplate,Arguments,Response);
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"
            Response.Write(" ");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"
            Response.Write(methodName);
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"
            Response.Write("(");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"
Generate(parametersTemplate,Arguments,Response);
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"
            Response.Write(") {\r\n");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMethodPrologue.template"
a._indent=(int)a._indent+1;
            Response.Flush();
        }
    }
}
