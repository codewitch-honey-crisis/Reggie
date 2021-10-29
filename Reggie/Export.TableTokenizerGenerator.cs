using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class TableTokenizerGenerator {
        public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {

var rules = (IList<LexRule>)Arguments["rules"];
var ignoreCase = (bool)Arguments["ignorecase"];
var lineCounted = (bool)Arguments["lines"];
var inputFile = (string)Arguments["inputfile"];
var outputFile = (string)Arguments["outputfile"];
var stderr = (TextWriter)Arguments["stderr"];
var codeclass = (string)Arguments["codeclass"];
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

            Response.Write("\r\n/// <summary>Represents a single token in a tokenized input stream</summary>\r\npublic struct Token\r\n{\r\n    /// <summary>Indicates the accepted symbol id</summary>\r\n    public int Id { get; }\r\n    /// <summary>Indicates the captured value</summary>\r\n    public string Value { get; }\r\n    /// <summary>Indicates the position within the stream</summary>\r\n    public long Position { get; }\r\n");
  if (lineCounted) {
            Response.Write("\r\n    /// <summary>Indicates the line number within the stream</summary>\r\n    public int Line { get; }\r\n    /// <summary>Indicates the column number within the stream</summary>\r\n    public int Column { get; }\r\n");
 } 
            Response.Write("\r\n    /// <summary>Constructs a new instance of a token</summary>\r\n    /// <param name=\"id\">The symbol id</param>\r\n    /// <param name=\"value\">The captured value</param>\r\n    /// <param name=\"position\">The position</param>\r\n");
  if (lineCounted) {
            Response.Write("\r\n    /// <param name=\"line\">The line</param>\r\n    /// <param name=\"column\">The column</param>\r\n    public Token(int id, string value, long position, int line, int column) {\r\n");
 } else { 
            Response.Write("\r\n    public Token(int id, string value, long position) {\r\n");
 } 
            Response.Write("\r\n        Id = id;\r\n        Value = value;\r\n        Position = position;\r\n");
  if (lineCounted) {
            Response.Write("\r\n        Line = line;\r\n        Column = column;\r\n");
 } 
            Response.Write("\r\n    }\r\n}\r\n/// <summary>Indicates the id of the #ERROR symbol</summary>\r\npublic const int ERROR = -1;\r\n");

for(var i = 0;i<symbolTable.Length;++i) {
if(!string.IsNullOrEmpty(symbolTable[i])) {

            Response.Write("\r\n/// <summary>Indicates the id of the ");
            Response.Write(symbolTable[i]);
            Response.Write(" symbol</summary>\r\npublic const int ");
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


            Response.Write("\r\n/// <summary>Lexes tokens off of an input stream</summary>\r\n/// <param name=\"text\">The text to tokenize</param>\r\n/// <param name=\"position\">The starting position</param>\r\n");
 if(lineCounted) {
            Response.Write("\r\n/// <param name=\"line\">The starting line</param>\r\n/// <param name=\"column\">The starting column</param>\r\n/// <param name=\"tabWidth\">The tab width</param>\r\npublic static System.Collections.Generic.IEnumerable<Token> Tokenize(");
            Response.Write(texttype);
            Response.Write(" text, long position = 0, int line = 1, int column = 1, int tabWidth = 4) {\r\n");
 } else {
            Response.Write("\r\npublic static System.Collections.Generic.IEnumerable<Token> Tokenize(");
            Response.Write(texttype);
            Response.Write(" text, long position = 0) {\r\n");
}
            Response.Write("\r\n    var sb = new System.Text.StringBuilder();\r\n");
if(!reader) {
            Response.Write("\r\n    var cursor = text.GetEnumerator();\r\n");
}
            Response.Write("\r\n    var cursorPos = position;\r\n    var ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n");
 if(lineCounted) {
            Response.Write("\r\n    var lc = line;\r\n    var cc = column;\r\n");
}
            Response.Write("\r\n    while (ch != -1) {\r\n        sb.Clear();\r\n        position = cursorPos;\r\n");
 if(lineCounted) {
            Response.Write("\r\n        line = lc;\r\n        column = cc;\r\n");
}
            Response.Write("\r\n        var done = false;\r\n        var acc = -1;\r\n        var state = 0;\r\n        while (!done) {\r\n            done = true;\r\n            // state starts with accept \r\n            acc = _TokenizerDfa[state++];\r\n            // next is the number of transitions\r\n            var tlen = _TokenizerDfa[state++];\r\n            for (var i = 0; i < tlen; ++i) {\r\n                // each transition starts with the destination index\r\n                var tto = _TokenizerDfa[state++];\r\n                // next with a packed range length\r\n                var prlen = _TokenizerDfa[state++];\r\n                for (var j = 0; j < prlen; ++j) {\r\n                    // then the packed ranges\r\n                    var pmin = _TokenizerDfa[state++];\r\n                    var pmax = _TokenizerDfa[state++];\r\n                    if (ch >= pmin && ch <= pmax) {\r\n");
 if(lineCounted) {
            Response.Write("\r\n                        switch(ch) {\r\n                            case \'\\t\':\r\n                                cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n                                break;\r\n                            case \'\\r\':\r\n                                cc = 1;\r\n                                break;\r\n                            case \'\\n\':\r\n                                ++lc;\r\n                                cc = 1;\r\n                                break;\r\n                            default:\r\n                                if (ch > 31) ++cc;\r\n                                break;\r\n                        }\r\n");
}
            Response.Write("\r\n                        sb.Append(char.ConvertFromUtf32(ch));\r\n                        ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                        ++cursorPos;\r\n                        state = tto;\r\n                        i = tlen;\r\n                        done = false;\r\n                        break;\r\n                    }\r\n                }\r\n            }\r\n        }\r\n        if (-1 != acc) {\r\n            var sacc = acc;\r\n            // process block ends\r\n            var blockEnd = _TokenizerBlockEndDfas[acc];\r\n            if (blockEnd != null) {\r\n                state = 0;\r\n                while (ch != -1) {\r\n                    done = false;\r\n                    acc = -1;\r\n                    while (!done) {\r\n                        done = true;\r\n                        // state starts with accept \r\n                        acc = blockEnd[state++];\r\n                        // next is the number of transitions\r\n                        var tlen = blockEnd[state++];\r\n                        for (var i = 0; i < tlen; ++i) {\r\n                            // each transition starts with the destination index\r\n                    ");
            Response.Write("        var tto = blockEnd[state++];\r\n                            // next with a packed range length\r\n                            var prlen = blockEnd[state++];\r\n                            for (var j = 0; j < prlen; ++j) {\r\n                                // then the packed ranges\r\n                                var pmin = blockEnd[state++];\r\n                                var pmax = blockEnd[state++];\r\n                                if (ch >= pmin && ch <= pmax) {\r\n");
 if(lineCounted) {
            Response.Write("\r\n                                    switch (ch) {\r\n                                        case \'\\t\':\r\n                                            cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n                                            break;\r\n                                        case \'\\r\':\r\n                                            cc = 1;\r\n                                            break;\r\n                                        case \'\\n\':\r\n                                            ++lc;\r\n                                            cc = 1;\r\n                                            break;\r\n                                        default:\r\n                                            if (ch > 31) ++cc;\r\n                                            break;\r\n                                    }\r\n");
}
            Response.Write("\r\n                                    sb.Append(char.ConvertFromUtf32(ch));\r\n                                    ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                                    ++cursorPos;\r\n                                    state = tto;\r\n                                    i = tlen;\r\n                                    done = false;\r\n                                    break;\r\n                                }\r\n                            }\r\n                        }\r\n                    }\r\n                    if (-1 != acc && 0==(_SymbolFlags[sacc]&1)) {\r\n");
 if(lineCounted) {
            Response.Write("\r\n                        yield return new Token(sacc, sb.ToString(), position, line, column);\r\n");
 } else { 
            Response.Write("\r\n                        yield return new Token(sacc, sb.ToString(), position);\r\n");
}
            Response.Write("\r\n                    }\r\n");
 if(lineCounted) {
            Response.Write("\r\n                    switch (ch) {\r\n                        case \'\\t\':\r\n                            cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n                            break;\r\n                        case \'\\r\':\r\n                            cc = 1;\r\n                            break;\r\n                        case \'\\n\':\r\n                            ++lc;\r\n                            cc = 1;\r\n                            break;\r\n                        default:\r\n                            if (ch > 31) ++cc;\r\n                            break;\r\n                    }\r\n");
}
            Response.Write("\r\n                    sb.Append(char.ConvertFromUtf32(ch));\r\n                    ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                    ++cursorPos;\r\n                    state = 0;\r\n                }\r\n                continue;\r\n            } // if (blockEnd != null) ...\r\n            else if (sb.Length > 0 && 0==(_SymbolFlags[acc]&1)) {\r\n");
 if(lineCounted) {
            Response.Write("\r\n                yield return new Token(acc, sb.ToString(), position, line, column);            \r\n");
 } else { 
            Response.Write("\r\n                yield return new Token(acc, sb.ToString(), position);            \r\n");
}
            Response.Write("\r\n            }\r\n        } \r\n        else {\r\n            // handle the errors\r\n            done = false;\r\n            while (!done) {\r\n                done = true;\r\n                // state starts with accept \r\n                state = 1;\r\n                // next is the number of transitions\r\n                var tlen = _TokenizerDfa[state++];\r\n                var matched = false;\r\n                for (var i = 0; i < tlen; ++i) {\r\n                    // each transition starts with the destination index\r\n                    state++; // skip it\r\n                    // next with a packed range length\r\n                    var prlen = _TokenizerDfa[state++];\r\n                    for (var j = 0; j < prlen; ++j) {\r\n                        // then the packed ranges\r\n                        var pmin = _TokenizerDfa[state++];\r\n                        var pmax = _TokenizerDfa[state++];\r\n                        if (ch >= pmin && ch <= pmax) {\r\n                            state = 0;\r\n                            i = tlen;\r\n ");
            Response.Write("                           matched = true;\r\n                            break;\r\n                        }\r\n                    }\r\n                }\r\n                if(!matched && ch!=-1) {\r\n");
 if(lineCounted) {
            Response.Write("\r\n                    switch (ch) {\r\n                        case \'\\t\':\r\n                            cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n                            break;\r\n                        case \'\\r\':\r\n                            cc = 1;\r\n                            break;\r\n                        case \'\\n\':\r\n                            ++lc;\r\n                            cc = 1;\r\n                            break;\r\n                        default:\r\n                            if (ch > 31) ++cc;\r\n                            break;\r\n                    }\r\n");
}
            Response.Write("\r\n                    sb.Append(char.ConvertFromUtf32(ch));\r\n                    ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                    ++cursorPos;\r\n                    done = false;\r\n                } \r\n                else {\r\n");
 if(lineCounted) {
            Response.Write("\r\n                    yield return new Token(ERROR, sb.ToString(), position, line, column);\r\n");
 } else { 
            Response.Write("\r\n                    yield return new Token(ERROR, sb.ToString(), position);\r\n");
}
            Response.Write("\r\n                    done = true;\r\n                }\r\n            }\r\n        }    \r\n    }\r\n    yield break;\r\n}\r\n");
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
