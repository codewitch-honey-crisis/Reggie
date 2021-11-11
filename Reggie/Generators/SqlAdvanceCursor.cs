using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlAdvanceCursor(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlAdvanceCursor.template"
            Response.Write("SET @absi = @absi + @adv;\r\nSET @cursorPos = @cursorPos + 1\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlAdvanceCursor.template"
            Response.Flush();
        }
    }
}
