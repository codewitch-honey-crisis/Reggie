using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void TableCommonCheckerMatcher(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableCommonCheckerMatcher.template"
dynamic a = Arguments;
for(var i = 0;i<((string[])a._symbolTable).Length;++i) {
	var s = ((string[])a._symbolTable)[i];
	if(s!=null) {
		a.DfaArrayDeclaration(s+"DfaStateTable",((int[][])a._dfas)[i]);
	}
	var bedfa = ((int[][])a._blockEndDfas)[i];
	if(null!=bedfa) {
		a.DfaArrayDeclaration(s+"BlockEndDfaStateTable",bedfa);
	}
}
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableCommonCheckerMatcher.template"
            Response.Write("\r\n");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableCommonCheckerMatcher.template"
            Response.Flush();
        }
    }
}
