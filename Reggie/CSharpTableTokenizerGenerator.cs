using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class CSharpTableTokenizerGenerator {
        public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {

var rules = (IList<LexRule>)Arguments["rules"];
var ignoreCase = (bool)Arguments["ignorecase"];
var lineCounted = (bool)Arguments["lines"];
var inputFile = (string)Arguments["inputfile"];
var outputFile = (string)Arguments["outputfile"];
var stderr = (TextWriter)Arguments["stderr"];
var codeclass = (string)Arguments["codeclass"];
var codetoken = (string)Arguments["codetoken"];
var dot = (bool)Arguments["dot"];
var jpg = (bool)Arguments["jpg"];
var cwd = Path.GetDirectoryName(outputFile!=null?outputFile:inputFile);
var blockEnds = BuildBlockEnds(rules,inputFile,ignoreCase);
var symbolTable = BuildSymbolTable(rules);
var symbolFlags = BuildSymbolFlags(rules);
var lexer = BuildLexer(rules,inputFile,ignoreCase);
if(dot) {
    var opts = new F.FFA.DotGraphOptions();
    var fn = Path.Combine(cwd, codeclass + ".dot");
    stderr.WriteLine("Writing {0}...",fn);
    using(var sw=new StreamWriter(fn)) {
        lexer.WriteDotTo(sw,opts);
    }        
}
if(jpg) {
    var opts = new F.FFA.DotGraphOptions();
    var fn = Path.Combine(cwd, codeclass + ".jpg");
    stderr.WriteLine("Writing {0}...",fn);
    lexer.RenderToFile(fn,opts);
}
            Response.Write("\r\n/// <summary>Indicates the id of the #ERROR symbol</summary>\r\npublic const int ERROR = -1;\r\n");

for(var i = 0;i<symbolTable.Length;++i) {
if(!string.IsNullOrEmpty(symbolTable[i])) {

            Response.Write("\r\n/// <summary>Indicates the id of the ");
            Response.Write(symbolTable[i]);
            Response.Write(" symbol</summary>");

if(0!=(symbolFlags[i]&1)){
            Response.Write("\r\n/// <remarks>This symbol is hidden and will not be reported by the tokenizer</remarks>");

}
            Response.Write("\r\npublic const int ");
            Response.Write(symbolTable[i]);
            Response.Write(" = ");
            Response.Write(i);
            Response.Write(";\r\n");
}}
            Response.Write("\r\n\r\n");
 for(var k = 0;k<2;++k) { 
bool reader = k==1;
string curtype = reader ? "System.IO.TextReader":"System.Collections.Generic.IEnumerator<char>";
string curname = reader ? "text":"cursor";
string texttype = reader ? "System.IO.TextReader":"System.Collections.Generic.IEnumerable<char>";


            Response.Write("\r\npublic static System.Collections.Generic.IEnumerable< ");
            Response.Write(string.IsNullOrEmpty(codetoken)?"(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value"+(lineCounted?", int Line, int Column":"")+")":codetoken);
            Response.Write(" > Tokenize(");
            Response.Write(texttype);
            Response.Write(" text, long position = 0");
            Response.Write(lineCounted?", int line = 1, int column = 1, int tabWidth = 4":"");
            Response.Write(") {\r\n    var sb = new System.Text.StringBuilder();\r\n    var adv = 0;\r\n    var absi = 0;\r\n    var absoluteIndex = 0;\r\n    var hasError = false;\r\n    var errorIndex = 0L;\r\n    var errorPos = 0L;");

if(!reader) {
            Response.Write("\r\n    var cursor = text.GetEnumerator();");

}
if(lineCounted) {
            Response.Write("\r\n    var errorLine = 0;\r\n    var errorColumn = 0;");

}
            Response.Write("\r\n    var cursorPos = position;\r\n    var ch = _ReadUtf32(");
            Response.Write(curname);
            Response.Write(", out adv);");

if(lineCounted) {
            Response.Write("\r\n    var lc = line;\r\n    var cc = column;");

}
            Response.Write("        \r\n    while (ch != -1) {\r\n        position = cursorPos;\r\n        absoluteIndex = absi;");

if(lineCounted) {
            Response.Write("\r\n        line = lc;\r\n        column = cc;");

}
            Response.Write("     \r\n        var done = false;\r\n        var acc = -1;\r\n        var state = 0;\r\n        while (!done) {\r\n            done = true;\r\n            // state starts with accept \r\n            acc = _TokenizerDfa[state++];\r\n            // next is the number of transitions\r\n            var tlen = _TokenizerDfa[state++];\r\n            for (var i = 0; i < tlen; ++i) {\r\n                // each transition starts with the destination index\r\n                var tto = _TokenizerDfa[state++];\r\n                // next with a packed range length\r\n                var prlen = _TokenizerDfa[state++];\r\n                for (var j = 0; j < prlen; ++j) {\r\n                    // then the packed ranges\r\n                    var pmin = _TokenizerDfa[state++];\r\n                    var pmax = _TokenizerDfa[state++];\r\n                    if(ch < pmin) break;\r\n                    if (ch <= pmax) {");

if(lineCounted) {
            Response.Write("\r\n                        switch(ch) {\r\n                            case \'\\t\':\r\n                                cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n                                break;\r\n                            case \'\\r\':\r\n                                cc = 1;\r\n                                break;\r\n                            case \'\\n\':\r\n                                ++lc;\r\n                                cc = 1;\r\n                                break;\r\n                            default:\r\n                                if (ch > 31) ++cc;\r\n                                break;\r\n                        }");

}
            Response.Write("     \r\n                        sb.Append(char.ConvertFromUtf32(ch));\r\n                        ch = _ReadUtf32(");
            Response.Write(curname);
            Response.Write(", out adv);\r\n                        absi += adv;\r\n                        ++cursorPos;\r\n                        state = tto;\r\n                        i = tlen;\r\n                        done = false;\r\n                        break;\r\n                    }\r\n                }\r\n            }\r\n        }\r\n        if (-1 != acc) {\r\n            // HACK:\r\n            if (ch == -1) ++absi;\r\n            if(hasError)\r\n            {\r\n                var ai = (int)(absoluteIndex - errorIndex);");

if(string.IsNullOrEmpty(codetoken)) {
            Response.Write("\r\n                yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: -1, Value: sb.ToString(0, ai)");
            Response.Write(lineCounted?", Line: errorLine, Column: errorColumn":"");
            Response.Write(");");

} else {
            Response.Write("\r\n                yield return new ");
            Response.Write(codetoken);
            Response.Write("(errorIndex, ai, errorPos, (int)(position - errorPos), -1, sb.ToString(0, ai)");
            Response.Write(lineCounted?", errorLine, errorColumn":"");
            Response.Write(");");

}
            Response.Write("\r\n                sb.Remove(0, ai);\r\n                hasError = false;\r\n            }\r\n            var sacc = acc;\r\n            // process block ends\r\n            var blockEnd = _TokenizerBlockEndDfas[acc];\r\n            if (blockEnd != null)\r\n            {\r\n                state = 0;\r\n                while (ch != -1)\r\n                {\r\n                    done = false;\r\n                    acc = -1;\r\n                    while (!done)\r\n                    {\r\n                        done = true;\r\n                        // state starts with accept \r\n                        acc = blockEnd[state++];\r\n                        // next is the number of transitions\r\n                        var tlen = blockEnd[state++];\r\n                        for (var i = 0; i < tlen; ++i)\r\n                        {\r\n                            // each transition starts with the destination index\r\n                            var tto = blockEnd[state++];\r\n                            // next with a packed range length\r\n               ");
            Response.Write("             var prlen = blockEnd[state++];\r\n                            for (var j = 0; j < prlen; ++j)\r\n                            {\r\n                                // then the packed ranges\r\n                                var pmin = blockEnd[state++];\r\n                                var pmax = blockEnd[state++];\r\n                                if(ch<pmin) break;\r\n                                if (ch <= pmax) \r\n                                {");

if(lineCounted) {
            Response.Write("\r\n                                    switch (ch)\r\n                                    {\r\n                                        case \'\\t\':\r\n                                            cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n                                            break;\r\n                                        case \'\\r\':\r\n                                            cc = 1;\r\n                                            break;\r\n                                        case \'\\n\':\r\n                                            ++lc;\r\n                                            cc = 1;\r\n                                            break;\r\n                                        default:\r\n                                            if (ch > 31) ++cc;\r\n                                            break;\r\n                                    }");

}
            Response.Write("\r\n                                    sb.Append(char.ConvertFromUtf32(ch));\r\n                                    ch = _ReadUtf32(");
            Response.Write(curname);
            Response.Write(", out adv);\r\n                                    absi += adv;\r\n                                    ++cursorPos;\r\n                                    state = tto;\r\n                                    i = tlen;\r\n                                    done = false;\r\n                                    break;\r\n                                }\r\n                            }\r\n                        }\r\n                    }\r\n                    if (-1 != acc)\r\n                    {\r\n                        // HACK:\r\n                        if (ch == -1) ++absi;\r\n\r\n                        if (0 == (_SymbolFlags[sacc] & 1))\r\n                        {");

if(string.IsNullOrEmpty(codetoken)) {
            Response.Write("\r\n                            yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: sb.Length, Position: position, Length: (int)(cursorPos - position), SymbolId: sacc, Value: sb.ToString()");
            Response.Write(lineCounted?", Line: line, Column: column":"");
            Response.Write(");");

} else {
            Response.Write("\r\n                            yield return new ");
            Response.Write(codetoken);
            Response.Write("(absoluteIndex, sb.Length, position, (int)(cursorPos - position), sacc, sb.ToString()");
            Response.Write(lineCounted?", line, column":"");
            Response.Write(");");

}
            Response.Write("\r\n                        }\r\n                        sb.Clear();\r\n                        break;\r\n                    }");

if(lineCounted) {
            Response.Write("\r\n                    switch (ch)\r\n                    {\r\n                        case \'\\t\':\r\n                            cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n                            break;\r\n                        case \'\\r\':\r\n                            cc = 1;\r\n                            break;\r\n                        case \'\\n\':\r\n                            ++lc;\r\n                            cc = 1;\r\n                            break;\r\n                        default:\r\n                            if (ch > 31) ++cc;\r\n                            break;\r\n                    }");

}
            Response.Write("\r\n                    sb.Append(char.ConvertFromUtf32(ch));\r\n                    ch = _ReadUtf32(");
            Response.Write(curname);
            Response.Write(", out adv);\r\n                    absi += adv;\r\n                    ++cursorPos;\r\n                    state = 0;\r\n                } // while(ch != -1)\r\n                continue;\r\n            } // if (blockEnd != null) ...\r\n            else\r\n            {\r\n                // HACK:\r\n                if (ch == -1) ++absi;\r\n\r\n                if (hasError)\r\n                {    \r\n                    var ai = (int)(absoluteIndex - errorIndex);");

if(string.IsNullOrEmpty(codetoken)) {
            Response.Write("\r\n                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: -1, Value: sb.ToString(0, ai)");
            Response.Write(lineCounted?", Line: errorLine, Column: errorColumn":"");
            Response.Write(");");

} else {
            Response.Write("\r\n                    yield return new ");
            Response.Write(codetoken);
            Response.Write("(errorIndex, ai, errorPos, (int)(position - errorPos), -1, sb.ToString(0, ai)");
            Response.Write(lineCounted?", errorLine, errorColumn":"");
            Response.Write(");");

}
            Response.Write("\r\n                    sb.Remove(0, ai);\r\n                }\r\n                if (sb.Length > 0 && 0 == (_SymbolFlags[acc] & 1))\r\n                {");

if(string.IsNullOrEmpty(codetoken)) {
            Response.Write("\r\n                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: sb.Length, Position: position, Length: (int)(cursorPos - position), SymbolId: acc, Value: sb.ToString()");
            Response.Write(lineCounted?", Line: line, Column: column":"");
            Response.Write(");");

} else {
            Response.Write("\r\n                    yield return new ");
            Response.Write(codetoken);
            Response.Write("(absoluteIndex, sb.Length, position, (int)(cursorPos - position), acc, sb.ToString()");
            Response.Write(lineCounted?", line, column":"");
            Response.Write(");");

}
            Response.Write("\r\n                }\r\n                sb.Clear();\r\n                hasError = false;\r\n            }\r\n        } \r\n        else {\r\n            if(!hasError)\r\n            {\r\n                errorPos = position;\r\n                errorIndex = absoluteIndex;");

if(lineCounted) {
            Response.Write("\r\n                errorColumn = column;\r\n                errorLine = line;");

}
            Response.Write("\r\n            }\r\n            hasError = true;                 \r\n        }    \r\n    }\r\n    if(hasError && sb.Length>0)\r\n    {");

if(string.IsNullOrEmpty(codetoken)) {
            Response.Write("\r\n        yield return (AbsolutePosition: errorIndex, AbsoluteLength: sb.Length, Position: errorPos, Length: (int)(position- errorPos +1 ), SymbolId: -1, Value: sb.ToString()");
            Response.Write(lineCounted?", Line: errorLine, Column: errorColumn":"");
            Response.Write(");");

} else {
            Response.Write("\r\n        yield return new ");
            Response.Write(codetoken);
            Response.Write("(errorIndex, sb.Length, errorPos, (int)(position- errorPos+1), -1, sb.ToString()");
            Response.Write(lineCounted?", errorLine, errorColumn":"");
            Response.Write(");");

}
            Response.Write("\r\n    }\r\n}\r\n");
}
            Response.Write("\r\n\r\n");

var tokenizerBlockEndDfas = new int[blockEnds.Length][];
for(var i = 0;i<blockEnds.Length;++i) {
    tokenizerBlockEndDfas[i] = (blockEnds[i]!=null)?ToDfaTable(blockEnds[i]):null;
}


            Response.Write("\r\nstatic readonly int[] _TokenizerDfa = ");
WriteCSArray(ToDfaTable(lexer),Response);
            Response.Write("\r\n\r\nstatic readonly int[][] _TokenizerBlockEndDfas = ");
WriteCSArray(tokenizerBlockEndDfas,Response);
            Response.Write("\r\n\r\nstatic readonly int[] _SymbolFlags = ");
WriteCSArray(symbolFlags,Response);
            Response.Flush();
        }
    }
}
