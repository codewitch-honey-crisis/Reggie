using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSCompiledLexerDoBlockEndPrologue(TextWriter Response, IDictionary<string, object> Arguments, string symbol) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"
            Response.Write("if(Tokenize");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"
            Response.Write(symbol);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"
            Response.Write("BlockEnd(");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"
            Response.Write((bool)a.textreader?"text":"cursor");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"
            Response.Write(", sb, ref cursorPos, ref absi, ref ch");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"
            Response.Write((bool)a.lines?", ref lc, ref cc, tabWidth":"");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"
            Response.Write(")) {");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"
a._indent=((int)a._indent)+1;
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSCompiledLexerDoBlockEndPrologue.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
