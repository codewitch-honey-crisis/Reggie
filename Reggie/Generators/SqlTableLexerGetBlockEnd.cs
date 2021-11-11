using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableLexerGetBlockEnd(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
dynamic a = Arguments; 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write("SET @blockId = -1\r\nSELECT TOP 1 @blockId = [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write("TokenizeSymbol].[BlockEndId] FROM [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write("TokenizeState] INNER JOIN [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write("TokenizeSymbol] ON [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write("TokenizeState].[AcceptId] = [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write("TokenizeSymbol].[Id] WHERE [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Write("TokenizeState].[AcceptId] = @acc\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableLexerGetBlockEnd.template"
            Response.Flush();
        }
    }
}
