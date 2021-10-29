using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class TableMatcherGenerator {
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

            Response.Write("\r\n");
 for(var k = 0;k<2;++k) { 
bool reader = k==1;
string curtype = reader ? "System.IO.TextReader":"System.Collections.Generic.IEnumerator<char>";
string curname = reader ? "text":"cursor";
string texttype = reader ? "System.IO.TextReader":"System.Collections.Generic.IEnumerable<char>";

            Response.Write("\r\nstatic System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long, string>> _MatchTable(int[] entries,int[] blockEnd, ");
            Response.Write(texttype);
            Response.Write(" text, long position)\r\n{\r\n    var sb = new System.Text.StringBuilder();\r\n");
if(!reader) {
            Response.Write("    var cursor = text.GetEnumerator();");
}
            Response.Write("\r\n    var cursorPos = position;\r\n    var ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n    var state = 0;\r\n    while (ch != -1)\r\n    {\r\n        sb.Clear();\r\n        position = cursorPos;\r\n        var done = false;\r\n        var acc = -1;\r\n        while (!done)\r\n        {\r\n            done = true;\r\n            // state starts with accept \r\n            acc = entries[state++];\r\n            // next is the number of transitions\r\n            var tlen = entries[state++];\r\n            for (var i = 0; i < tlen; ++i)\r\n            {\r\n                // each transition starts with the destination index\r\n                var tto = entries[state++];\r\n                // next with a packed range length\r\n                var prlen = entries[state++];\r\n                for (var j = 0; j < prlen; ++j)\r\n                {\r\n                    // then the packed ranges\r\n                    var tmin = entries[state++];\r\n                    var tmax = entries[state++];\r\n                    if (ch >= tmin && ch <= tmax)\r\n                    {\r\n                        sb.Append(char.ConvertFromUtf32(ch));\r\n             ");
            Response.Write("           ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                        ++cursorPos;\r\n                        state = tto;\r\n                        i = tlen;\r\n                        done = false;\r\n                        break;\r\n                    }\r\n                }\r\n            }\r\n        }\r\n        if (-1 != acc)\r\n        {\r\n            if (blockEnd != null && blockEnd.Length > 0)\r\n            {\r\n                state = 0;\r\n                while (ch != -1)\r\n                {\r\n\r\n                    done = false;\r\n                    acc = -1;\r\n                    while (!done)\r\n                    {\r\n                        done = true;\r\n                        // state starts with accept \r\n                        acc = blockEnd[state++];\r\n                        // next is the number of transitions\r\n                        var tlen = blockEnd[state++];\r\n                        for (var i = 0; i < tlen; ++i)\r\n                        {\r\n                            // each transition starts with the destination index\r\n                            var");
            Response.Write(" tto = blockEnd[state++];\r\n                            // next with a packed range length\r\n                            var prlen = blockEnd[state++];\r\n                            for (var j = 0; j < prlen; ++j)\r\n                            {\r\n                                // then the packed ranges\r\n                                var tmin = blockEnd[state++];\r\n                                var tmax = blockEnd[state++];\r\n                                if (ch >= tmin && ch <= tmax)\r\n                                {\r\n                                    sb.Append(char.ConvertFromUtf32(ch));\r\n                                    ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                                    ++cursorPos;\r\n                                    state = tto;\r\n                                    i = tlen;\r\n                                    done = false;\r\n                                    break;\r\n                                }\r\n                            }\r\n                        }\r\n                    }\r\n                    if (-1 != acc)\r\n                    {\r\n                        yield return new System.Collections.Generic.KeyValuePair<long, string>(position, sb.ToString());\r\n                    }\r\n                    sb.Append(char.ConvertFromUtf32(ch));\r\n                    ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                    ++cursorPos;\r\n                    state = 0;\r\n                }\r\n                state = 0;\r\n                continue;\r\n            } \r\n            else\r\n                if (sb.Length > 0) yield return new System.Collections.Generic.KeyValuePair<long, string>(position, sb.ToString());\r\n        }\r\n        ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n        ++cursorPos;\r\n        state = 0;\r\n    }\r\n    yield break;\r\n}\r\n\r\nstatic bool _IsTable(int[] entries, int[] blockEnd, ");
            Response.Write(texttype);
            Response.Write(" text)\r\n{\r\n");
if(!reader) {
            Response.Write("    var cursor = text.GetEnumerator();");
}
            Response.Write("\r\n    var ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n    if (ch == -1) return (blockEnd==null||blockEnd.Length==0) && -1!=entries[0];\r\n    var state = 0;\r\n    var acc = -1;\r\n    while (ch != -1)\r\n    {\r\n        // state starts with accept symbol\r\n        acc = entries[state++];\r\n        // next is the num of transitions\r\n        var tlen = entries[state++];\r\n        var matched = false;\r\n        for (var i = 0; i < tlen; ++i)\r\n        {\r\n            // each transition starts with the destination index\r\n            var tto = entries[state++];\r\n            // next with a packed range length\r\n            var prlen = entries[state++];\r\n            for (var j = 0; j < prlen; ++j)\r\n            {\r\n                // next each packed range\r\n                var tmin = entries[state++];\r\n                var tmax = entries[state++];\r\n                if (ch >= tmin && ch <= tmax)\r\n                {\r\n                    matched = true;\r\n                    ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                    state = tto;\r\n                    i = tlen;\r\n                    break;\r\n                }\r\n            }\r\n        }\r\n        if (!matched)\r\n        {\r\n            if (acc != -1)\r\n                break;\r\n            return false;\r\n        }\r\n    }\r\n    if(-1!=acc)\r\n    {\r\n        if (blockEnd != null && blockEnd.Length > 0)\r\n        {\r\n            state = 0;\r\n            while (ch != -1)\r\n            {\r\n                var done = false;\r\n                acc = -1;\r\n                while (!done)\r\n                {\r\n                    done = true;\r\n                    // state starts with accept \r\n                    acc = blockEnd[state++];\r\n                    // next is the number of transitions\r\n                    var tlen = blockEnd[state++];\r\n                    for (var i = 0; i < tlen; ++i)\r\n                    {\r\n                        // each transition starts with the destination index\r\n                        var tto = blockEnd[state++];\r\n                        // next wit");
            Response.Write("h a packed range length\r\n                        var prlen = blockEnd[state++];\r\n                        for (var j = 0; j < prlen; ++j)\r\n                        {\r\n                            // then the packed ranges\r\n                            var tmin = blockEnd[state++];\r\n                            var tmax = blockEnd[state++];\r\n                            if (ch >= tmin && ch <= tmax)\r\n                            {\r\n                                ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                                state = tto;\r\n                                i = tlen;\r\n                                done = false;\r\n                                break;\r\n                            }\r\n                        }\r\n                    }\r\n                }\r\n                if (-1 != acc)\r\n                {\r\n                    return ch == -1;\r\n                }\r\n                else\r\n                {\r\n                    ch = _FetchNextInput(");
            Response.Write(curname);
            Response.Write(");\r\n                }\r\n                state = 0;\r\n            }\r\n            return false;\r\n        }\r\n        return true;\r\n    }\r\n    return false;\r\n}\r\n");
}
            Response.Write("\r\n\r\n");

foreach(var rule in rules) {
            Response.Write("static int[] _");
            Response.Write(rule.Symbol);
            Response.Write("Dfa = ");

    var fa = ParseToFA(rule,inputFile,ignoreCase);
    var dt = ToDfaTable(fa);
    WriteCSArray(dt,Response);
    
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
}

foreach(var rule in rules) {
    var be = blockEnds[rule.Id];
    if(be!=null) {

            Response.Write("static int[] _");
            Response.Write(rule.Symbol);
            Response.Write("BlockEndDfa = ");
WriteCSArray(ToDfaTable(be),Response);
            Response.Write("\r\n");
}}

foreach(var rule in rules) {
    var s = blockEnds[rule.Id]!=null?("_"+rule.Symbol+"BlockEndDfa"):"null";

            Response.Write("/// <summary>Validates that input character stream contains content that matches the ");
            Response.Write(rule.Symbol);
            Response.Write(" expression.</summary>\r\n/// <param name=\"text\">The text stream to validate. The entire stream must match the expression.</param>\r\n/// <returns>True if <paramref name=\"text\"/> matches the expression indicated by ");
            Response.Write(rule.Symbol);
            Response.Write(", otherwise false.</returns>\r\npublic static bool Is");
            Response.Write(rule.Symbol);
            Response.Write("(System.Collections.Generic.IEnumerable<char> text) => _IsTable(_");
            Response.Write(rule.Symbol);
            Response.Write("Dfa, ");
            Response.Write(s);
            Response.Write(", text);\r\n");

}
foreach(var rule in rules) {
    var s = blockEnds[rule.Id]!=null?("_"+rule.Symbol+"BlockEndDfa"):"null";

            Response.Write("/// <summary>Finds occurrances of a string matching the ");
            Response.Write(rule.Symbol);
            Response.Write(" expression.</summary>\r\n/// <param name=\"text\">The text stream to match on.</param>\r\n/// <param name=\"position\">The logical cursor position where the counting starts.</param>\r\n/// <returns>A <see cref=\"System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.Int64,System.String}}\"/> object that enumerates the match information.</returns>\r\npublic static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long, string>> Match");
            Response.Write(rule.Symbol);
            Response.Write("(System.Collections.Generic.IEnumerable<char> text, long position = 0) => _MatchTable(_");
            Response.Write(rule.Symbol);
            Response.Write("Dfa, ");
            Response.Write(s);
            Response.Write(", text, position);\r\n/// <summary>Finds occurrances of a string matching the ");
            Response.Write(rule.Symbol);
            Response.Write(" expression.</summary>\r\n/// <param name=\"text\">The text stream to match on.</param>\r\n/// <param name=\"position\">The logical cursor position where the counting starts.</param>\r\n/// <returns>A <see cref=\"System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.Int64,System.String}}\"/> object that enumerates the match information.</returns>\r\npublic static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long, string>> Match");
            Response.Write(rule.Symbol);
            Response.Write("(System.IO.TextReader text, long position = 0) => _MatchTable(_");
            Response.Write(rule.Symbol);
            Response.Write("Dfa, ");
            Response.Write(s);
            Response.Write(", text, position);\r\n");
}
            Response.Write("\r\n\r\n\r\n");
            Response.Flush();
        }
    }
}
