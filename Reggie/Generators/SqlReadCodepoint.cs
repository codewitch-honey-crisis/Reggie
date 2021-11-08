using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlReadCodepoint(TextWriter Response, IDictionary<string, object> Arguments, bool isChecker) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
            Response.Write("\r\nSET @index = @index + 1\r\nSET @adv = 1\r\nIF @index < @valueEnd\r\nBEGIN\r\n");
            #line 6 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"

if(isChecker) {

            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
            Response.Write("\tSET @ch = UNICODE(SUBSTRING(@value, @index, 1))");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"

} else {

            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
            Response.Write("\tSET @ch1 = SUBSTRING(@value, @index, 1)\r\n\tSET @ch = UNICODE(@ch1)");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"

}
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
            Response.Write("\r\n\tSET @tch = @ch - 0xd800\r\n\tIF @tch < 0 SET @tch = @tch + 2147483648\r\n\tIF @tch < 2048\r\n\tBEGIN\r\n\t\tSET @ch = @ch * 1024\r\n\t\tSET @index = @index + 1\r\n\t\tSET @adv = 2\r\n\t\tIF @index >= @valueEnd RETURN -1\r\n");
            #line 21 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
if(isChecker) {

            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
            Response.Write("\t\tSET @ch = @ch + UNICODE(SUBSTRING(@value, @index, 1)) - 0x35fdc00");
            #line 22 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"

} else {

            #line 24 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
            Response.Write("\t\tSET @ch2 = SUBSTRING(@value, @index, 1)\r\n\t\tSET @ch = @ch + UNICODE(@ch2) - 0x35fdc00");
            #line 25 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"

}
            #line 26 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
            Response.Write("\r\n\tEND\r\nEND\r\nELSE\r\nBEGIN\r\n\tSET @ch = -1\r\nEND\r\n");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlReadCodepoint.template"
            Response.Flush();
        }
    }
}
