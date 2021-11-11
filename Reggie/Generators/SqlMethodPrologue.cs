using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlMethodPrologue(TextWriter Response, IDictionary<string, object> Arguments, string docTemplate, bool @private, string returnTemplate, string methodName, string parametersTemplate) {
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
dynamic a = Arguments;
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write("DROP PROCEDURE [dbo].[");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write(a.@class);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write("_");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write(methodName);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write("]\r\nGO\r\n");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"

Generate(docTemplate,Arguments,Response);
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write("CREATE PROCEDURE [dbo].[");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write(a.@class);
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write("_");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write(methodName);
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write("] ");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
Generate(parametersTemplate,Arguments,Response);
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write("\r\nAS\r\nBEGIN");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
a._indent=(int)a._indent+1;
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Write("\r\n");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMethodPrologue.template"
            Response.Flush();
        }
    }
}
