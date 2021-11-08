using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableMove(TextWriter Response, IDictionary<string, object> Arguments, bool isBlockEnd, bool isChecker, bool isMatcher) {
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"

dynamic a = Arguments;
string labelName = isBlockEnd?"block_end":"dfa";
a.Label("start_"+labelName);

            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("SET @done = 1\r\nSET @toState = -1\r\n");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
if((bool)a.lexer) {

            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("SELECT @toState = [dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeStateTransition].[ToStateId] FROM [dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeState] INNER JOIN [dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeStateTransition] ON [dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeState].[StateId]=[dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeStateTransition].[StateId] WHERE [dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeStateTransition].[BlockEndId]=@blockId AND [dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeState].[StateId]=@state AND [dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeState].[BlockEndId] = @blockId AND @ch BETWEEN [dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeStateTransition].[Min] AND [dbo].[");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("TokenizeStateTransition].[Max]");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"

} else {

            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("SELECT @toState = [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("StateTransition].[ToStateId] FROM [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("State] INNER JOIN [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("StateTransition] ON [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("State].[StateId]=[dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("StateTransition].[StateId] AND [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("State].[SymbolId]=[dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("StateTransition].[SymbolId] AND [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("StateTransition].[BlockEndId]=[dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("State].[BlockEndId] WHERE [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("State].[SymbolId] = @symbolId AND [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("State].[StateId] = @state AND [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("State].[BlockEndId] = @blockId AND [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("StateTransition].[SymbolId] = @symbolId AND @ch BETWEEN [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("StateTransition].[Min] AND [dbo].[");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(a.@class);
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("StateTransition].[Max]");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"

}
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("\r\nIF @toState <> -1\r\nBEGIN");
            #line 15 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
a._indent = ((int)a._indent) + 1;
            #line 15 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("\r\nSET @state = @toState\r\nSET @done = 0");
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"

if(!isMatcher) {
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("\r\nSET @matched = 1\r\n");
            #line 20 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
}
if(!isChecker) {
a.UpdateLineAny();
a.AppendCapture();
}
a.ReadCodepoint(isChecker);
if(!isChecker) {
a.AdvanceCursor();

            #line 28 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("GOTO start_");
            #line 28 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write(labelName);
            #line 28 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("\r\n");
            #line 29 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
}
a._indent = ((int)a._indent) - 1;
            #line 30 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableMove.template"
            Response.Write("END\r\n");
            Response.Flush();
        }
    }
}
