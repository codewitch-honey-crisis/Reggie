using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlCompiledGotoState(TextWriter Response, IDictionary<string, object> Arguments, int stateId) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledGotoState.template"
            Response.Write("GOTO q");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledGotoState.template"
            Response.Write(stateId);
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlCompiledGotoState.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
