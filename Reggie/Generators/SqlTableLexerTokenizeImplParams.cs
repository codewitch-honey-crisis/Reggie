using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableLexerTokenizeImplParams(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeImplParams.template"

dynamic a = Arguments;

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeImplParams.template"
            Response.Write("@symbolId INT, @value ");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeImplParams.template"
            Response.Write((bool)a.ntext?"NTEXT":"NVARCHAR(MAX)");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeImplParams.template"
            Response.Write(", @position BIGINT = 0");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeImplParams.template"
            Response.Write(((bool)a.lines)?", @line INT = 0, @column INT = 0, @tabWidth INT = 4":"");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerTokenizeImplParams.template"
            Response.Flush();
        }
    }
}
