using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableAcceptPrologue(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
dynamic a = Arguments; 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("SET @acc = -1\r\n");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
if((bool)a.lexer) { 

            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("SELECT @acc = [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("TokenizeState].[AcceptId], @flags = [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("TokenizeSymbol].[Flags] FROM [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("TokenizeState] INNER JOIN [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("TokenizeSymbol] ON [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("TokenizeState].[AcceptId] = [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("TokenizeSymbol].[Id] WHERE [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("TokenizeState].[StateId] = @state AND [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("TokenizeState].[BlockEndId] = @blockId AND [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("TokenizeState].[AcceptId] <> -1\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
} else {

            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("SELECT @acc = [dbo].[");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("State].[SymbolId] FROM [dbo].[");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("State] WHERE [dbo].[");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("State].[SymbolId] = @symbolId AND [dbo].[");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("State].[StateId] = @state AND [dbo].[");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("State].[BlockEndId] = @blockId AND [dbo].[");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write(a.@class);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("State].[Accepts] = 1\r\n");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
}
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("IF @acc <> -1\r\nBEGIN");
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
a._indent=((int)a._indent) +1;
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Write("\r\n");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableAcceptPrologue.template"
            Response.Flush();
        }
    }
}
