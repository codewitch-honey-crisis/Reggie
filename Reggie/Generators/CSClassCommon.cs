using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CSClassCommon(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassCommon.template"
            Response.Write("// The error message to report when an invalid UTF32 stream is encountered\r\nconst string UnicodeSurrogateError = \"Invalid surrogate found in Unicode stream\";\r\n");
            #line 3 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassCommon.template"
dynamic a = Arguments;

            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassCommon.template"
if(!(bool)a.textreader || (bool)a.checker) {
            #line 4 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassCommon.template"
            Response.Write("// Reads the next UTF32 codepoint off an enumerator\r\nstatic int ReadUtf32(System.Collections.Generic.IEnumerator<char> cursor, out int adv) {\r\n    adv = 1;\r\n    if(!cursor.MoveNext()) return -1;\r\n    var chh = cursor.Current;\r\n    int result = chh;\r\n    if(char.IsHighSurrogate(chh)) {\r\n        ++adv;\r\n        if(!cursor.MoveNext()) throw new System.IO.IOException(UnicodeSurrogateError);\r\n        var chl = cursor.Current;\r\n        if(!char.IsLowSurrogate(chl)) throw new System.IO.IOException(UnicodeSurrogateError);\r\n        result = char.ConvertToUtf32(chh,chl);\r\n    }\r\n    return result;\r\n}");
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassCommon.template"
}
if((bool)a.textreader) { 
            #line 19 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassCommon.template"
            Response.Write("// Reads the next UTF32 codepoint off a text reader\r\nstatic int ReadUtf32(System.IO.TextReader reader, out int adv) {\r\n    adv=1;\r\n    var result = reader.Read();\r\n    if (-1 != result) {\r\n        if (char.IsHighSurrogate(unchecked((char)result))) {\r\n            ++adv;\r\n            var chl = reader.Read();\r\n            if (-1 == chl) throw new System.IO.IOException(UnicodeSurrogateError);\r\n            if (!char.IsLowSurrogate(unchecked((char)chl))) throw new System.IO.IOException(UnicodeSurrogateError);\r\n            result = char.ConvertToUtf32(unchecked((char)result), unchecked((char)chl));\r\n        }\r\n    }\r\n    return result;\r\n}");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassCommon.template"
}
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CS\CSClassCommon.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
