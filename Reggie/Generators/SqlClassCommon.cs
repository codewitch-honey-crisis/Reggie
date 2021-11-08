using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlClassCommon(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
dynamic a = Arguments;
if((bool)a.tables) {
if((bool)a.lexer) {
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("DROP TABLE [dbo].[");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("TokenizeState]\r\nGO\r\nCREATE TABLE [dbo].[");
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("TokenizeState] (\r\n    [StateId]  INT NOT NULL,\r\n    [AcceptId] INT NOT NULL DEFAULT -1,\r\n    [BlockEndId] INT NOT NULL DEFAULT -1\r\n    CONSTRAINT [PK_");
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("TokenizeState] PRIMARY KEY ([StateId], [BlockEndId])\r\n)\r\nGO\r\n\r\nDROP TABLE [dbo].[");
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("TokenizeStateTransition]\r\nGO\r\n\r\nCREATE TABLE [dbo].[");
            #line 16 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 16 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("TokenizeStateTransition]\r\n(\r\n    [StateId] INT NOT NULL , \r\n    [BlockEndId] INT NOT NULL , \r\n\t[ToStateId] INT NOT NULL,\r\n    [Min] BIGINT NOT NULL, \r\n    [Max] BIGINT NOT NULL, \r\n    CONSTRAINT [PK_");
            #line 23 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 23 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("TokenizeStateTransition] PRIMARY KEY ([StateId], [BlockEndId], [Min], [Max]) \r\n)\r\nGO\r\nDROP TABLE [dbo].[");
            #line 26 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 26 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("TokenizeSymbol]\r\nGO\r\n\r\nCREATE TABLE [dbo].[");
            #line 29 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 29 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("TokenizeSymbol] (\r\n    [Id] INT NOT NULL,\r\n    [Flags] INT NOT NULL DEFAULT 0,\r\n    [BlockEndId] INT NOT NULL DEFAULT -1,\r\n    [SymbolName] NVARCHAR(MAX) NULL,\r\n    PRIMARY KEY CLUSTERED ([Id] ASC)\r\n)\r\nGO\r\n");
            #line 37 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
 } else { 
            #line 37 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("DROP TABLE [dbo].[");
            #line 37 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 37 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("State]\r\nGO\r\n\r\nCREATE TABLE [dbo].[");
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 40 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("State] (\r\n    [SymbolId]  INT NOT NULL,\r\n    [StateId]  INT NOT NULL,\r\n    [Accepts] INT NOT NULL DEFAULT 0,\r\n    [BlockEndId] INT NOT NULL DEFAULT -1\r\n    CONSTRAINT [PK_");
            #line 45 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 45 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("State] PRIMARY KEY ([SymbolId], [StateId], [BlockEndId])\r\n)\r\nGO\r\n\r\nDROP TABLE [dbo].[");
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("StateTransition]\r\nGO\r\n\r\nCREATE TABLE [dbo].[");
            #line 52 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 52 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("StateTransition]\r\n(\r\n    [SymbolId]  INT NOT NULL,\r\n\t[StateId] INT NOT NULL , \r\n    [BlockEndId] INT NOT NULL , \r\n\t[ToStateId] INT NOT NULL,\r\n    [Min] BIGINT NOT NULL, \r\n    [Max] BIGINT NOT NULL, \r\n    CONSTRAINT [PK_");
            #line 60 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 60 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("StateTransition] PRIMARY KEY ([SymbolId], [StateId], [BlockEndId], [Min], [Max]) \r\n)\r\nGO\r\nDROP TABLE [dbo].[");
            #line 63 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 63 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("Symbol]\r\nGO\r\n\r\nCREATE TABLE [dbo].[");
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write(a.@class);
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
            Response.Write("Symbol] (\r\n    [Id] INT NOT NULL,\r\n    [Flags] INT NOT NULL DEFAULT 0,\r\n    [SymbolName] NVARCHAR(MAX) NULL,\r\n    PRIMARY KEY CLUSTERED ([Id] ASC)\r\n)\r\nGO\r\n");
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlClassCommon.template"
}}
            Response.Flush();
        }
    }
}
