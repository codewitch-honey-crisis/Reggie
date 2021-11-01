using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class SqlTableTokenizerCreateGenerator {
        public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {

var inputFile = (string)Arguments["inputfile"];
var outputFile = (string)Arguments["outputfile"];
var stderr = (TextWriter)Arguments["stderr"];
var codeclass = (string)Arguments["codeclass"];
var codenamespace = (string)Arguments["codenamespace"];
            Response.Write("\r\nDROP TABLE [dbo].[");
            Response.Write(codeclass);
            Response.Write("State]\r\nGO\r\n\r\nCREATE TABLE [dbo].[");
            Response.Write(codeclass);
            Response.Write("State] (\r\n    [StateId]  INT NOT NULL,\r\n    [AcceptId] INT NOT NULL DEFAULT -1,\r\n    [BlockEndId] INT NOT NULL DEFAULT -1\r\n    CONSTRAINT [PK_");
            Response.Write(codeclass);
            Response.Write("State] PRIMARY KEY ([StateId], [BlockEndId])\r\n)\r\nGO\r\n\r\nDROP TABLE [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition]\r\nGO\r\n\r\nCREATE TABLE [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition]\r\n(\r\n    [StateId] INT NOT NULL , \r\n    [BlockEndId] INT NOT NULL , \r\n\t[ToStateId] INT NOT NULL,\r\n    [Min] BIGINT NOT NULL, \r\n    [Max] BIGINT NOT NULL, \r\n    CONSTRAINT [PK_");
            Response.Write(codeclass);
            Response.Write("StateTransition] PRIMARY KEY ([StateId], [BlockEndId], [Min], [Max]) \r\n)\r\nGO\r\nDROP TABLE [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol]\r\nGO\r\n\r\nCREATE TABLE [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol] (\r\n    [Id] INT NOT NULL,\r\n    [Flags] INT NOT NULL DEFAULT 0,\r\n    [BlockEndId] INT NOT NULL DEFAULT -1,\r\n    [SymbolName] NVARCHAR(MAX) NULL,\r\n    PRIMARY KEY CLUSTERED ([Id] ASC)\r\n)\r\nGO\r\n");
            Response.Flush();
        }
    }
}
