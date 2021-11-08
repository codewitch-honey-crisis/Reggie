using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableMove(TextWriter Response, IDictionary<string, object> Arguments, bool isBlockEnd, bool isChecker, bool isMatcher) {
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"

dynamic a = Arguments;
string array;
string labelName = isBlockEnd?"block_end":"dfa";
if(!(bool)a.lexer) {
    array = isBlockEnd?"blockEnd":"dfa";
} else {
    array = isBlockEnd?"blockEnd":"TokenizeDfaStateTable";
}
a.Label("start_"+labelName);

            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("done = true;\r\nacc = ");
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write(array);
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("[state++];\r\ntlen = ");
            #line 15 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write(array);
            #line 15 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("[state++];\r\nfor (i = 0; i < tlen; ++i) {\r\n    tto = ");
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write(array);
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("[state++];\r\n    prlen = ");
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write(array);
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("[state++];\r\n    for (j = 0; j < prlen; ++j) {\r\n        pmin = ");
            #line 20 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write(array);
            #line 20 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("[state++];\r\n        pmax = ");
            #line 21 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write(array);
            #line 21 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("[state++];\r\n        if(ch < pmin) break;\r\n        if (ch <= pmax) {\r\n");
            #line 24 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
a._indent=(int)a._indent+3;
if(!isChecker) {
    a.UpdateLineAny();
    a.AppendCapture();
}
a.ReadCodepoint(isChecker);
if(!isChecker) {
    a.AdvanceCursor();
}

            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("state = tto;\r\ndone = false;\r\n");
            #line 35 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"

if(!isMatcher ) {
    a.SetMatched();
}

            #line 39 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("\r\n            goto start_");
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write(labelName);
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write(";");
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
a._indent=(int)a._indent-3;
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableMove.template"
            Response.Write("\r\n        }\r\n    }\r\n}\r\n");
            Response.Flush();
        }
    }
}
