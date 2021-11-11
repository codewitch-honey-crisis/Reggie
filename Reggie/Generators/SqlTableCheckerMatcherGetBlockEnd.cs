using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableCheckerMatcherGetBlockEnd(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
dynamic a = Arguments; 
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Write("SET @blockId = -1\r\nSELECT TOP 1 @blockId = [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Write("State].[BlockEndId] FROM [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Write("State] WHERE [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Write("State].[SymbolId]=@symbolId AND [dbo].[");
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Write(a.@class);
            #line 2 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Write("State].[BlockEndId] <> -1\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCheckerMatcherGetBlockEnd.template"
            Response.Flush();
        }
    }
}
