using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableCheckerSetInitialAccept(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
dynamic a = Arguments;

            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write("SELECT @acc = [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write("State].[SymbolId] FROM [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write("State] WHERE [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write("State].[SymbolId] = @symbolId AND [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write("State].[StateId] = 0 AND [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write("State].[BlockEndId] = -1 AND [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Write("State].[Accepts] = 1\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerSetInitialAccept.template"
            Response.Flush();
        }
    }
}
