using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class CompiledTokenizerGenerator {
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
var dfa = ToDfaTable(lexer);
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
var dfaMap = GetDfaStateTransitionMap(dfa);
var isQ0reffed = IsQ0Reffed(dfa);
int si, sid;
int[][] blockEndDfas = new int[blockEnds.Length][];
//var hasBlockEnds = false;
for(var i = 0;i<blockEnds.Length;++i) {
    var be = blockEnds[i];
    if(be!=null) {
        blockEndDfas[i] = ToDfaTable(be);
       // hasBlockEnds = true;
    }
}

            Response.Write("/// <summary>Represents a single token in a tokenized input stream</summary>\r\npublic struct Token\r\n{\r\n    /// <summary>Indicates the accepted symbol id</summary>\r\n    public int Id { get; }\r\n    /// <summary>Indicates the captured value</summary>\r\n    public string Value { get; }\r\n    /// <summary>Indicates the position within the stream</summary>\r\n    public long Position { get; }\r\n");
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
            Response.Write("\r\n    }\r\n}\r\n/// <summary>Indicates the #ERROR symbol id</summary>\r\npublic const int ERROR = -1;");

foreach(var rule in rules) {
            Response.Write("\r\n/// <summary>Indicates the ");
            Response.Write(rule.Symbol);
            Response.Write(" symbol id</summary>\r\npublic const int ");
            Response.Write(rule.Symbol);
            Response.Write(" = ");
            Response.Write(rule.Id);
            Response.Write(";");

}
for(var k = 0;k<2;++k) { 
    bool reader = k==1;

    string curtype = reader ? "System.IO.TextReader":"System.Collections.Generic.IEnumerator<char>";
    string curname = reader ? "text":"cursor";
    string texttype = reader ? "System.IO.TextReader":"System.Collections.Generic.IEnumerable<char>";
            Response.Write("\r\n/// <summary>Lexes tokens off of an input stream</summary>\r\n/// <param name=\"text\">The text to tokenize</param>\r\n/// <param name=\"position\">The starting position</param>");
 
if(lineCounted) { 
            Response.Write("\r\n/// <param name=\"line\">The starting line</param>\r\n/// <param name=\"column\">The starting column</param>\r\n/// <param name=\"tabWidth\">The tab width</param>\r\npublic static System.Collections.Generic.IEnumerable<Token> Tokenize(");
            Response.Write(texttype);
            Response.Write(" text, long position = 0, int line = 1, int column = 1, int tabWidth = 4) {");
 
} else {
            Response.Write("\r\npublic static System.Collections.Generic.IEnumerable<Token> Tokenize(");
            Response.Write(texttype);
            Response.Write(" text, long position = 0) {");

}
            Response.Write("\r\n    var sb = new System.Text.StringBuilder();");

if(!reader) {
            Response.Write("\r\n    var cursor = text.GetEnumerator();");

} // if(!reader) ...
            Response.Write("\r\n    var cursorPos = position;\r\n    var ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");");
 
if(lineCounted) {
            Response.Write("\r\n    var lc = line;\r\n    var cc = column;");

} // if(lineCounted)... 
            Response.Write("\r\n    while (ch != -1) {\r\n        sb.Clear();\r\n        position = cursorPos;");
 
if(lineCounted) {
            Response.Write("\r\n        line = lc;\r\n        column = cc;");

} // if(lineCounted) ...
si = 0;
sid = 0;
while(si<dfa.Length) {
    var sacc = dfa[si++];
    if(sid==0) {
        if(isQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    // q0:");

        } // if(isQ0reffed)...
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = dfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = dfa[si++];
        var rstart = si;
        var ranges = dfa;
        if(lineCounted) {
            var lclist = new List<int>(10);
            if (DfaRangesContains('\n',ranges,rstart)) {
                lclist.Add('\n');
                ranges = DfaExcludeFromRanges('\n',ranges,rstart);
                rstart = 0;        
            }
            if (DfaRangesContains('\r',ranges, rstart)) {
                lclist.Add('\r');
                ranges = DfaExcludeFromRanges('\r',ranges,rstart);
                rstart = 0; 
            }
            if (DfaRangesContains('\t',ranges, rstart)) {
                lclist.Add('\t');
                ranges = DfaExcludeFromRanges('\t',ranges,rstart);
                rstart = 0;
            }
            if(lclist.Contains('\t')) {
            Response.Write("\r\n        if(ch == \'\\t\') {\r\n            sb.Append(unchecked((char)ch));\r\n            cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n            ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n            ++cursorPos;\r\n            goto q");
            Response.Write(dfaMap[tto]);
            Response.Write(";\r\n        }");

            }
            if (lclist.Contains('\r')) {
            Response.Write("\r\n        if(ch == \'\\r\') {\r\n            sb.Append(unchecked((char)ch));\r\n            cc = 1;\r\n            ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n            ++cursorPos;\r\n            goto q");
            Response.Write(dfaMap[tto]);
            Response.Write(";\r\n        }");

            }
            if (lclist.Contains('\n')) {
            Response.Write("\r\n        if(ch == \'\\n\') {\r\n            sb.Append(unchecked((char)ch));\r\n            ++lc;\r\n            cc = 1;\r\n            ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n            ++cursorPos;\r\n            goto q");
            Response.Write(dfaMap[tto]);
            Response.Write(";\r\n        }");

            }
        }
        var exts = GetTransitionExtents(ranges,rstart);
            Response.Write("\r\n        if(");
WriteCSRangeCharMatchTests(ranges, rstart, 2, Response);
            Response.Write(") {");

        if(exts.Value<128) {
            Response.Write("\r\n            sb.Append(unchecked((char)ch));");
} else {
            Response.Write("\r\n            sb.Append(char.ConvertFromUtf32(ch));");

        }
         if(lineCounted) {
            if(exts.Key>31) {
            Response.Write("\r\n            ++cc;");

            } else if(exts.Value>31) {
            Response.Write("\r\n            if(ch > 31) ++cc;");

            }
        }
            Response.Write("\r\n            ch=_FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n            ++cursorPos;\r\n            goto q");
            Response.Write(dfaMap[tto]);
            Response.Write(";\r\n        }");

        // skip the packed ranges
        var prlen = dfa[si++];
        si+=prlen*2;
    } // for .. tlen
    if(sacc!=-1) { // accepting
        if(blockEnds[sacc]==null) {
            if(0==(symbolFlags[sacc]&1)) {
                if(lineCounted) {
            Response.Write("\r\n        yield return new Token(");
            Response.Write(symbolTable[sacc]);
            Response.Write(", sb.ToString(), position, line, column);");
 
                } else {
            Response.Write("\r\n        yield return new Token(");
            Response.Write(symbolTable[sacc]);
            Response.Write(", sb.ToString(), position);");

                } // if(lineCounted)...
            } // if(0==(symbolFlags[sacc]&1))... 
            Response.Write("\r\n            continue;");

        } else // if(blockEnds[sacc]...
        {
            if(lineCounted) {
            Response.Write("\r\n            if(_Tokenize");
            Response.Write(symbolTable[sacc]);
            Response.Write("BlockEnd(");
            Response.Write(curname);
            Response.Write(",sb, ref cursorPos, ref lc, ref cc, tabWidth, ref ch)) {\r\n                yield return new Token(");
            Response.Write(symbolTable[sacc]);
            Response.Write(", sb.ToString(), position, line, column);");
 
            } else { // if(lineCounted)...
            Response.Write("\r\n            if(_Tokenize");
            Response.Write(symbolTable[sacc]);
            Response.Write("BlockEnd(");
            Response.Write(curname);
            Response.Write(",sb, ref cursorPos, ref ch)) {    \r\n                yield return new Token(");
            Response.Write(symbolTable[sacc]);
            Response.Write(", sb.ToString(), position);");

            }
            Response.Write("\r\n            continue;\r\n            }");
 // if(_Tokenize...
            if(!lineCounted) {
            Response.Write("\r\n            yield return new Token(ERROR, sb.ToString(), position);");

            } else {
            Response.Write("\r\n            yield return new Token(ERROR, sb.ToString(), position, line, column);");

            }
            Response.Write("\r\n            continue;");

        } // if(blockEnds[sacc]...
    } else {// not accepting
            Response.Write("\r\n        goto error;");

    }
    ++sid; // we're on the next state now
} // while(si<dfa.Length) 

            Response.Write("\r\n    error:\r\n        while(ch!=-1 && !(");

var esi = 1;
var etlen = dfa[esi++];
for(var i = 0;i<etlen;++i) {
    ++esi;
    WriteCSRangeCharMatchTests(dfa, esi, 2, Response);
    if(i<etlen-1) {
            Response.Write(" || \r\n        ");

    } // if(i<etlen-1) ...
    var plen = dfa[esi++];
    esi+=plen*2;
} // for(var i = 0;i<etlen;++i) ...
        
            Response.Write(")) {\r\n            sb.Append(char.ConvertFromUtf32(ch));");

if(lineCounted)
{
            Response.Write("\r\n            switch(ch) {\r\n            case \'\\t\':\r\n                cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n                break;\r\n            case \'\\r\':\r\n                cc = 1;\r\n                break;\r\n            case \'\\n\':\r\n                cc = 1;\r\n                ++lc;\r\n                break;\r\n            default:\r\n                if(ch>31) ++cc;\r\n                break;\r\n            }");
     
} // if(lineCounted)...
            Response.Write("\r\n            ch=_FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n            ++cursorPos;\r\n        } ");
 // while(!..
if (!lineCounted) {
            Response.Write("\r\n        if(sb.Length>0) yield return new Token(ERROR, sb.ToString(), position);");

            } else {
            Response.Write("\r\n        if(sb.Length>0) yield return new Token(ERROR, sb.ToString(), position, line, column);");

} // if (!lineCounted) ... 
            Response.Write("\r\n    } ");
// while(ch!=-1)...
            Response.Write("\r\n} ");
 // Tokenize(...
foreach(var rule in rules) {
    var be = blockEnds[rule.Id];
    if(be!=null) {
        if(!lineCounted) {
            Response.Write("\r\nstatic bool _Tokenize");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd(");
            Response.Write(curtype);
            Response.Write(" ");
            Response.Write(curname);
            Response.Write(", System.Text.StringBuilder sb, ref long cursorPos, ref int ch) {");

        } else { // if(!lineCounted) ...
            Response.Write("\r\nstatic bool _Tokenize");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd(");
            Response.Write(curtype);
            Response.Write(" ");
            Response.Write(curname);
            Response.Write(", System.Text.StringBuilder sb, ref long cursorPos, ref int lc, ref int cc, int tabWidth, ref int ch) {");

        } // if(!lineCounted) ...
            Response.Write("\r\n    ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n    while (ch != -1) {\r\n        sb.Clear();");
 
si = 0;
sid = 0;
var bedfa = blockEndDfas[rule.Id];
var bedfaMap = GetDfaStateTransitionMap(bedfa);
var beQ0reffed = IsQ0Reffed(bedfa);
while(si<bedfa.Length) {
    var sacc = bedfa[si++];
    if(sid==0) {
        if(beQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    // q0:");

        } // if(beQ0reffed)...
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = bedfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = bedfa[si++];
        var rstart = si;
        var ranges = bedfa;
        if(lineCounted) {
            var lclist = new List<int>(10);
            if (DfaRangesContains('\n',ranges,rstart)) {
                lclist.Add('\n');
                ranges = DfaExcludeFromRanges('\n',ranges,rstart);
                rstart = 0;        
            }
            if (DfaRangesContains('\r',ranges, rstart)) {
                lclist.Add('\r');
                ranges = DfaExcludeFromRanges('\r',ranges,rstart);
                rstart = 0; 
            }
            if (DfaRangesContains('\t',ranges, rstart)) {
                lclist.Add('\t');
                ranges = DfaExcludeFromRanges('\t',ranges,rstart);
                rstart = 0;
            }
            if(lclist.Contains('\t')) {
            Response.Write("\r\n        if(ch == \'\\t\') {\r\n            sb.Append(unchecked((char)ch));\r\n            cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n            ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n            ++cursorPos;\r\n            goto q");
            Response.Write(bedfaMap[tto]);
            Response.Write(";\r\n        }");

            }
            if (lclist.Contains('\r')) {
            Response.Write("\r\n        if(ch == \'\\r\') {\r\n            sb.Append(unchecked((char)ch));\r\n            cc = 1;\r\n            ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n            ++cursorPos;\r\n            goto q");
            Response.Write(bedfaMap[tto]);
            Response.Write(";\r\n        }");

            }
            if (lclist.Contains('\n')) {
            Response.Write("\r\n        if(ch == \'\\n\') {\r\n            sb.Append(unchecked((char)ch));\r\n            ++lc;\r\n            cc = 1;\r\n            ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n            ++cursorPos;\r\n            goto q");
            Response.Write(bedfaMap[tto]);
            Response.Write(";\r\n        }");

            }
        }
        var exts = GetTransitionExtents(ranges,rstart);
            Response.Write("\r\n        if(");
WriteCSRangeCharMatchTests(ranges, rstart, 2, Response);
            Response.Write(") {");

        if(exts.Value<128) {
            Response.Write("\r\n            sb.Append(unchecked((char)ch));");

        } else {
            Response.Write("\r\n            sb.Append(char.ConvertFromUtf32(ch));");

        }
        if(lineCounted) {
            if(exts.Key>31) {
            Response.Write("\r\n            ++cc;");

            } else if(exts.Value>31) {
            Response.Write("\r\n            if(ch > 31) ++cc;");

            }
        }
            Response.Write("\r\n            ch=_FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n            ++cursorPos;\r\n            goto q");
            Response.Write(bedfaMap[tto]);
            Response.Write(";\r\n        }");

        // skip the packed ranges
        var prlen = bedfa[si++];
        si+=prlen*2;
    } // for .. tlen
    if(sacc!=-1) { // accepting
            Response.Write("\r\n        return true;");

    } else {// not accepting
            Response.Write("\r\n        sb.Append(char.ConvertFromUtf32(ch));");

        if(lineCounted)
{
            Response.Write("\r\n        switch(ch) {\r\n        case \'\\t\':\r\n            cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;\r\n            break;\r\n        case \'\\r\':\r\n            cc = 1;\r\n            break;\r\n        case \'\\n\':\r\n            cc = 1;\r\n            ++lc;\r\n            break;\r\n        default:\r\n            if(ch>31) ++cc;\r\n            break;\r\n        }");
     
} // if(lineCounted)...
            Response.Write("\r\n        ch=_FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n        ++cursorPos;");

        if(si<bedfa.Length) {
            Response.Write("\r\n            continue;");

        }
    }
    ++sid; // we're on the next state now
} // while(si<bedfa.Length) 
            Response.Write("\r\n    }\r\n    return false;\r\n}");

    } // if(be!=null) ...
} // foreach(var rule in rules) ...
            Response.Write("\r\n");
} // for(var k = 0;k<2;++k) ... 
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
