using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSMatcherMatchDocumentation(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
            Response.Write("/// <summary>Returns all occurances of the expression indicated by ");
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
            Response.Write(a._symbol);
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
            Response.Write(" within <paramref name=\"text\"/></summary>\r\n/// <param name=\"text\">The text to search</param>\r\n/// <param name=\"position\">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"

if((bool)a.lines) {
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
            Response.Write("/// <param name=\"line\">The 1 based line where the matching is assumed to have started. Defaults to 1.</param>\r\n/// <param name=\"column\">The 1 based column where the matching is assumed to have started. Defaults to 1.</param>\r\n/// <param name=\"tabWidth\">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>\r\n");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"

}
if(""!=(string)a.token){
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
            Response.Write("/// <returns>An instance of <see cref=\"System.Collections.Generic.IEnumerable{");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
            Response.Write(((string)a.token).Replace("<","{").Replace(">","}"));
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
            Response.Write("}\"/> used to retrieve the match values.</returns>\r\n");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"

} else {
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
            Response.Write("/// <returns>An enumeration of tuples used to retrieve the match values.</returns>\r\n");
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
}
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSMatcherMatchDocumentation.template"
            Response.Write("/// <remarks>The matches contain both the absolute native character position within <paramref name=\"text\"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>\r\n");
            Response.Flush();
        }
    }
}
