using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void Comment(TextWriter Response, IDictionary<string, object> Arguments, string text) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Comment.template"

dynamic a = Arguments;
using(var sr = new StringReader(text)) {
	string line;
	while(null!=(line=sr.ReadLine())) {
		a.CommentLine(line);
	}
}

            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Comment.template"
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Comment.template"
            Response.Flush();
        }
    }
}
