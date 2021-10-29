using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class CompiledMatcherGenerator {
        public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {

var rules = (IList<LexRule>)Arguments["rules"];
var ignoreCase = (bool)Arguments["ignorecase"];
var inputFile = (string)Arguments["inputfile"];
var outputFile = (string)Arguments["outputfile"];
var stderr = (TextWriter)Arguments["stderr"];
var dot = (bool)Arguments["dot"];
var jpg = (bool)Arguments["jpg"];
var cwd = Path.GetDirectoryName(outputFile!=null?outputFile:inputFile);
var blockEnds = BuildBlockEnds(rules,inputFile,ignoreCase);
int[][] blockEndDfas = new int[blockEnds.Length][];
//var hasBlockEnds = false;
for(var i = 0;i<blockEnds.Length;++i) {
    var be = blockEnds[i];
    if(be!=null) {
        blockEndDfas[i] = ToDfaTable(be);
       // hasBlockEnds = true;
    }
}

foreach(var rule in rules) {
var fa = ParseToFA(rule,inputFile,ignoreCase);
var dfa = ToDfaTable(fa);
if(dot) {
    var opts = new F.FFA.DotGraphOptions();
    opts.HideAcceptSymbolIds = true;
    var fn = Path.Combine(cwd, rule.Symbol + ".dot");
    stderr.WriteLine("Writing {0}...",fn);
    using(var sw=new StreamWriter(fn)) {
        fa.WriteDotTo(sw,opts);
    }        
}
if(jpg) {
    var opts = new F.FFA.DotGraphOptions();
    opts.HideAcceptSymbolIds = true;
    var fn = Path.Combine(cwd, rule.Symbol + ".jpg");
    stderr.WriteLine("Writing {0}...",fn);
    try {
        fa.RenderToFile(fn,opts);
    }  
    catch {}
}

var dfaMap = GetDfaStateTransitionMap(dfa);
var bedfa = blockEndDfas[rule.Id];
var bedfaMap = bedfa!=null?GetDfaStateTransitionMap(bedfa):(int[])null;
var isQ0reffed = IsQ0Reffed(dfa);
var isBEQ0reffed = bedfa!=null && IsQ0Reffed(bedfa);
int si, sid;
for(var k = 0;k<2;++k) { 

bool reader = k==1;

string curtype = reader ? "System.IO.TextReader":"System.Collections.Generic.IEnumerator<char>";
string curname = reader ? "text":"cursor";
string texttype = reader ? "System.IO.TextReader":"System.Collections.Generic.IEnumerable<char>";

            Response.Write("/// <summary>Finds occurrances of a string matching the ");
            Response.Write(rule.Symbol);
            Response.Write(" expression.</summary>\r\n/// <param name=\"text\">The text stream to match on.</param>\r\n/// <param name=\"position\">The logical cursor position where the counting starts.</param>\r\n/// <returns>A <see cref=\"System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.Int64,System.String}}\"/> object that enumerates the match information.</returns>\r\npublic static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long, string>> Match");
            Response.Write(rule.Symbol);
            Response.Write("(");
            Response.Write(texttype);
            Response.Write(" text, long position = 0) {\r\n    var sb = new System.Text.StringBuilder();");

if(!reader) {
            Response.Write("\r\n    var cursor = text.GetEnumerator();");

}
            Response.Write("\r\n    var cursorPos = position;\r\n    var ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n    while (ch != -1) {\r\n        sb.Clear();\r\n        position = cursorPos;");
 
si = 0;
sid = 0;
while(si<dfa.Length) {
    var sacc = dfa[si++];
    if(sid==0) {
        if(isQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    // q0:");

        }
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = dfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = dfa[si++];
        var exts = GetTransitionExtents(dfa,si);
            Response.Write("\r\n        if(");
WriteCSRangeCharMatchTests(dfa, si, 2, Response);
            Response.Write(") {");

        if(exts.Value<128) {
            Response.Write("\r\n            sb.Append(unchecked((char)ch));");
} else {
            Response.Write("\r\n            sb.Append(char.ConvertFromUtf32(ch));");

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
            Response.Write("\r\n        if(sb.Length>0) yield return new System.Collections.Generic.KeyValuePair<long,string>(position,sb.ToString());");

        if(si<dfa.Length) { // are there more states? 
            Response.Write("\r\n        goto next;");

            }
        } else {
            Response.Write("\r\n        if(_Match");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd(");
            Response.Write(curname);
            Response.Write(",sb, ref cursorPos, ref ch)) {\r\n            yield return new System.Collections.Generic.KeyValuePair<long,string>(position,sb.ToString());\r\n        }\r\n        continue;");

        }
    } else {// not accepting
        if(si<dfa.Length) { // are there more states? 
            Response.Write("\r\n        goto next;");

        }
    }
    ++sid; // we're on the next state now
} // while(si<dfa.Length) 

            Response.Write("\r\n    next:\r\n        ch=_FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n        ++cursorPos;\r\n    }\r\n}");

if(blockEnds[rule.Id]!=null) {
            Response.Write("\r\nstatic bool _Match");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd(");
            Response.Write(curtype);
            Response.Write(" ");
            Response.Write(curname);
            Response.Write(", System.Text.StringBuilder sb, ref long cursorPos, ref int ch) {\r\n    while (ch != -1) {");
 
si = 0;
sid = 0;
while(si<bedfa.Length) {
    var sacc = bedfa[si++];
    if(sid==0) {
        if(isBEQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    // q0:");

        }
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = bedfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = bedfa[si++];
        var exts = GetTransitionExtents(bedfa,si);
            Response.Write("\r\n        if(");
WriteCSRangeCharMatchTests(bedfa, si, 2, Response);
            Response.Write(") {");

        if(exts.Value<128) {
            Response.Write("\r\n            sb.Append(unchecked((char)ch));");
} else {
            Response.Write("\r\n            sb.Append(char.ConvertFromUtf32(ch));");

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
            Response.Write("\r\n        goto next;");

    }
    ++sid; // we're on the next state now
} // while(si<bedfa.Length) 

            Response.Write("\r\n    next:\r\n        sb.Append(char.ConvertFromUtf32(ch));\r\n        ch=_FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n        ++cursorPos;\r\n    }\r\n    return false;\r\n}");

}}

if(blockEnds[rule.Id]!=null) {
            Response.Write("\r\nstatic bool _Is");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd(System.Collections.Generic.IEnumerator<char> cursor, int ch) {\r\n    while (ch != -1) {");
 
si = 0;
sid = 0;
while(si<bedfa.Length) {
    var sacc = bedfa[si++];
    if(sid==0) {
        if(isBEQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    // q0:");

        }
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = bedfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = bedfa[si++];
            Response.Write("\r\n        if(");
WriteCSRangeCharMatchTests(bedfa, si, 2, Response);
            Response.Write(") {\r\n            ch=_FetchNextInput(cursor);\r\n            goto q");
            Response.Write(bedfaMap[tto]);
            Response.Write(";\r\n        }");

        // skip the packed ranges
        var prlen = bedfa[si++];
        si+=prlen*2;
    } // for .. tlen
    if(sacc!=-1) { // accepting
            Response.Write("\r\n        return ch==-1;");

    } else {// not accepting
            Response.Write("\r\n        goto next;");

    }
    ++sid; // we're on the next state now
} // while(si<bedfa.Length) 

            Response.Write("\r\n    next:\r\n        ch=_FetchNextInput(cursor);\r\n    }\r\n    return false;\r\n}\r\n");
}
            Response.Write("\r\n/// <summary>Validates that input character stream contains content that matches the ");
            Response.Write(rule.Symbol);
            Response.Write(" expression.</summary>\r\n/// <param name=\"text\">The text stream to validate. The entire stream must match the expression.</param>\r\n/// <returns>True if <paramref name=\"text\"/> matches the expression indicated by ");
            Response.Write(rule.Symbol);
            Response.Write(", otherwise false.</returns>\r\npublic static bool Is");
            Response.Write(rule.Symbol);
            Response.Write("(System.Collections.Generic.IEnumerable<char> text) {\r\n    var cursor = text.GetEnumerator();\r\n    var ch = _FetchNextInput(cursor);\r\n    while (ch != -1) {");
 
si = 0;
sid = 0;
while(si<dfa.Length) {
    var sacc = dfa[si++];
    if(sid==0) {
        if(isQ0reffed) {
            Response.Write("\r\n    q0:");
} else {
            Response.Write("\r\n    // q0:");

        }
    } else {
            Response.Write("\r\n    q");
            Response.Write(sid);
            Response.Write(":");

    } // if(sid==0)
    var tlen = dfa[si++];
    for(var j=0;j<tlen;++j) {
        var tto = dfa[si++];
        var exts = GetTransitionExtents(dfa,si);
            Response.Write("\r\n        if(");
WriteCSRangeCharMatchTests(dfa, si, 2, Response);
            Response.Write(") {\r\n            ch=_FetchNextInput(cursor);\r\n            goto q");
            Response.Write(dfaMap[tto]);
            Response.Write(";\r\n        }");

        // skip the packed ranges
        var prlen = dfa[si++];
        si+=prlen*2;
    } // for .. tlen
    if(sacc!=-1) { // accepting
        if(blockEnds[sacc]==null) {
            Response.Write("\r\n        return ch==-1;");

        } else {
            Response.Write("\r\n        return _Is");
            Response.Write(rule.Symbol);
            Response.Write("BlockEnd(cursor, ch);");

        }
    } else {// not accepting
            Response.Write("\r\n        return false;");

    }
    ++sid; // we're on the next state now
} // while(si<dfa.Length) 

            Response.Write("\r\n}\r\n    return false;\r\n}\r\n");
}
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
