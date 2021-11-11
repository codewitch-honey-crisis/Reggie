using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSLexerTokenizeDocumentation(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
dynamic a=Arguments;
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
            Response.Write("/// <summary>Lexes tokens off of <paramref name=\"text\"/></summary>\r\n/// <param name=\"text\">The text to tokenize</param>\r\n/// <param name=\"position\">The logical position in codepoints where the tokenizer started. By default assumes the beginning of the stream.</param>\r\n");
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"

if((bool)a.lines) {
            #line 5 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
            Response.Write("/// <param name=\"line\">The 1 based line where the tokenizer is assumed to have started. Defaults to 1.</param>\r\n/// <param name=\"column\">The 1 based column where the tokenizer is assumed to have started. Defaults to 1.</param>\r\n/// <param name=\"tabWidth\">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>\r\n");
            #line 8 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"

}
if(""!=(string)a.token){
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
            Response.Write("/// <returns>An instance of <see cref=\"System.Collections.Generic.IEnumerable{");
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
            Response.Write(((string)a.token).Replace("<","{").Replace(">","}"));
            #line 10 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
            Response.Write("}\"/> used to retrieve the tokens.</returns>\r\n");
            #line 11 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"

} else {
            #line 12 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
            Response.Write("/// <returns>An enumeration of tuples used to retrieve the tokens.</returns>\r\n");
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
}
            #line 13 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
            Response.Write("/// <remarks>The token contains both the absolute native character position within <paramref name=\"text\"/> and the logical position in UTF32 codepoints for each token. The former is useful for locating the token within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>\r\n");
            #line 14 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSLexerTokenizeDocumentation.template"
            Response.Flush();
        }
    }
}
