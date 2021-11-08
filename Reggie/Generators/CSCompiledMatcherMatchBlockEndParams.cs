using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledMatcherMatchBlockEndParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchBlockEndParams.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchBlockEndParams.template"
            Response.Write((bool)a.textreader?"System.IO.TextReader text":"System.Collections.Generic.IEnumerator<char> cursor");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchBlockEndParams.template"
            Response.Write(", System.Text.StringBuilder sb, ref long cursorPos, ref long absi, ref int ch");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledMatcherMatchBlockEndParams.template"
            Response.Write((bool)a.lines?", ref int lc, ref int cc, int tabWidth":"");
            Response.Flush();
        }
    }
}
