using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSLexerSymbolField(TextWriter Response, IDictionary<string, object> Arguments, string symbol, int symbolId) {
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerSymbolField.template"
dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerSymbolField.template"
            Response.Write("/// <summary>Indicates the symbol id for the ");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerSymbolField.template"
            Response.Write(symbol);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerSymbolField.template"
            Response.Write(" symbol</summary>\r\npublic const int ");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerSymbolField.template"
            Response.Write(symbol);
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerSymbolField.template"
            Response.Write(" = ");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerSymbolField.template"
            Response.Write(symbolId);
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerSymbolField.template"
            Response.Write(";\r\n");
            Response.Flush();
        }
    }
}
