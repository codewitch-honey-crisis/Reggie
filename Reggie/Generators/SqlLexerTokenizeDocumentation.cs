using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlLexerTokenizeDocumentation(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerTokenizeDocumentation.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerTokenizeDocumentation.template"
            Response.Write("-- <summary>Lexes tokens off of <paramref name=\"value\"/></summary>\r\n-- <param name=\"value\">The text to tokenize</param>\r\n-- <param name=\"position\">The logical position in codepoints where the tokenizer started. By default assumes the beginning of the stream.</param>\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerTokenizeDocumentation.template"

if((bool)a.lines) {
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerTokenizeDocumentation.template"
            Response.Write("-- <param name=\"line\">The 1 based line where the tokenizer is assumed to have started. Defaults to 1.</param>\r\n-- <param name=\"column\">The 1 based column where the tokenizer is assumed to have started. Defaults to 1.</param>\r\n-- <param name=\"tabWidth\">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>\r\n");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerTokenizeDocumentation.template"

}
            #line 9 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerTokenizeDocumentation.template"
            Response.Write("-- <returns>An result set used to retrieve the tokens.</returns>\r\n-- <remarks>Each row contains both the absolute native character position within <paramref name=\"value\"/> and the logical position in UTF32 codepoints for each token. The former is useful for locating the token within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>\r\n");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlLexerTokenizeDocumentation.template"
            Response.Flush();
        }
    }
}
