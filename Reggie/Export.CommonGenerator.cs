using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class CommonGenerator {
        public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {
            Response.Write("static int _FetchNextInput(System.Collections.Generic.IEnumerator<char> cursor) {\r\n    if(!cursor.MoveNext()) return -1;\r\n    var chh = cursor.Current;\r\n    int ch = chh;\r\n    if(char.IsHighSurrogate(chh)) {\r\n        if(!cursor.MoveNext()) throw new System.IO.IOException(\"Invalid surrogate found in Unicode stream\");\r\n        var chl = cursor.Current;\r\n        if(!char.IsLowSurrogate(chl)) throw new System.IO.IOException(\"Invalid surrogate found in Unicode stream\");\r\n        ch = char.ConvertToUtf32(chh,chl);\r\n    }\r\n    return ch;\r\n}\r\nstatic int _FetchNextInput(System.IO.TextReader reader) {\r\n    var result = reader.Read();\r\n    if (-1 != result) {\r\n        if (char.IsHighSurrogate((char)result)) {\r\n            var chl = reader.Read();\r\n            if (-1 == chl) throw new System.IO.IOException(\"Invalid surrogate found in Unicode stream\");\r\n            if (!char.IsLowSurrogate((char)chl)) throw new System.IO.IOException(\"Invalid surrogate found in Unicode stream\");\r\n            result = char.ConvertToUtf32((c");
            Response.Write("har)result, (char)chl);\r\n        }\r\n    }\r\n    return result;\r\n}\r\n");
            Response.Flush();
        }
    }
}
