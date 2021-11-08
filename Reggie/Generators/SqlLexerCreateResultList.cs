using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlLexerCreateResultList(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerCreateResultList.template"
dynamic a = Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerCreateResultList.template"
            Response.Write("CREATE TABLE #Results (\r\n\t[AbsolutePosition] BIGINT NOT NULL,\r\n\t[AbsoluteLength] INT NOT NULL,\r\n\t[Position] BIGINT NOT NULL,\r\n\t[Length] INT NOT NULL,\r\n    [SymbolId] INT NOT NULL,");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerCreateResultList.template"

    if((bool)a.lines) {
            #line 7 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerCreateResultList.template"
            Response.Write("\r\n    [Value] NVARCHAR(MAX) NOT NULL,\r\n    [Line] INT NOT NULL,\r\n    [Column] INT NOT NULL");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerCreateResultList.template"

    } else {
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerCreateResultList.template"
            Response.Write("\r\n    [Value] NVARCHAR(MAX) NOT NULL");
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerCreateResultList.template"

}
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerCreateResultList.template"
            Response.Write(")\r\n");
            Response.Flush();
        }
    }
}
