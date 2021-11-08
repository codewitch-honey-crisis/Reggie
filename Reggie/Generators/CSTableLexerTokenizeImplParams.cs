using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSTableLexerTokenizeImplParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeImplParams.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeImplParams.template"
            Response.Write("int[] dfa, int[] blockEnd, ");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeImplParams.template"
            Response.Write((bool)a.textreader?"System.IO.TextReader":"System.Collections.Generic.IEnumerable<char>");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeImplParams.template"
            Response.Write(" text, long position");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSTableLexerTokenizeImplParams.template"
            Response.Write(((bool)a.lines)?", int line, int column, int tabWidth":"");
            Response.Flush();
        }
    }
}
