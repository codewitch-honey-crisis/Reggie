using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledGotoState(TextWriter Response, IDictionary<string, object> Arguments, int stateId) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledGotoState.template"
            Response.Write("goto q");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledGotoState.template"
            Response.Write(stateId);
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledGotoState.template"
            Response.Write(";\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledGotoState.template"
            Response.Flush();
        }
    }
}
