using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlMatcherMatchDocumentation(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchDocumentation.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchDocumentation.template"
            Response.Write("-- <summary>Returns all occurances of the expression indicated by ");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchDocumentation.template"
            Response.Write(a._symbol);
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchDocumentation.template"
            Response.Write(" within <paramref name=\"value\"/></summary>\r\n-- <param name=\"value\">The text to search</param>\r\n-- <param name=\"position\">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchDocumentation.template"

if((bool)a.lines) {
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchDocumentation.template"
            Response.Write("-- <param name=\"line\">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>\r\n-- <param name=\"column\">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>\r\n-- <param name=\"tabWidth\">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>\r\n");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchDocumentation.template"
}

            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchDocumentation.template"
            Response.Write("-- <remarks>The matches contain both the absolute native character position within <paramref name=\"value\"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>\r\n");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlMatcherMatchDocumentation.template"
            Response.Flush();
        }
    }
}
