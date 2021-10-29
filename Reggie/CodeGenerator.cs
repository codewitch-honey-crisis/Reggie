using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using F;
using LC;
using System.Reflection;

namespace Reggie
{
    static class CodeGenerator
    {
        public static void GenerateTableSupport(TextWriter writer)
        {
            var w = new IndentedTextWriter(writer);
            
            w.WriteLine("static bool _IsTable(int[] entries,int[] blockEnd, System.Collections.Generic.IEnumerable<char> text) {");
            ++w.IndentLevel;
            w.WriteLine("var cursor = text.GetEnumerator();");
            w.WriteLine("var ch = _FetchNextInput(cursor);");
            w.WriteLine("if (ch == -1) return (blockEnd == null || blockEnd.Length == 0) && -1 != entries[0];");
            w.WriteLine("var state = 0;");
            w.WriteLine("var acc = -1;");
            w.WriteLine("while (ch != -1) {");
            ++w.IndentLevel;
            w.WriteLine("acc = entries[state++];");
            w.WriteLine("var tlen = entries[state++];");
            w.WriteLine("var matched = false;");
            w.WriteLine("for (var i = 0; i < tlen; ++i) {");
            ++w.IndentLevel;
            w.WriteLine("var tto = entries[state++];");
            w.WriteLine("var plen = entries[state++];");
            w.WriteLine("for (var j = 0; j < plen; ++j) {");
            ++w.IndentLevel;
            w.WriteLine("if (ch >= entries[state++] && ch <= entries[state++]) {");
            ++w.IndentLevel;
            w.WriteLine("matched = true;");
            w.WriteLine("ch = _FetchNextInput(cursor);");
            w.WriteLine("state = tto;");
            w.WriteLine("i = tlen;");
            w.WriteLine("break;");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("if (!matched) {");
            ++w.IndentLevel;
            w.WriteLine("if (acc != -1) break;");
            w.WriteLine("return false;");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("if (acc != -1) {");
            ++w.IndentLevel;
            w.WriteLine("if (blockEnd != null && blockEnd.Length > 0) {");
            ++w.IndentLevel;
            w.WriteLine("return _IsBlockEndTable(blockEnd, cursor, ch);"); 
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");

            w.WriteLine("return false;");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine();
            w.WriteLine("static bool _IsBlockEndTable(int[] blockEnd, System.Collections.Generic.IEnumerator<char> cursor, int ch) {");
            ++w.IndentLevel;
            w.WriteLine("var state = 0;");
            w.WriteLine("while (ch != -1) {");
            ++w.IndentLevel;
            w.WriteLine("var done = false;");
            w.WriteLine("var acc = -1;");
            w.WriteLine("while (!done) {");
            ++w.IndentLevel;
            w.WriteLine("done = true;");
            w.WriteLine("acc = blockEnd[state++];");
            w.WriteLine("var tlen = blockEnd[state++];");
            w.WriteLine("for (var i = 0; i < tlen; ++i) {");
            ++w.IndentLevel;
            w.WriteLine("var tto = blockEnd[state++];");
            w.WriteLine("var prlen = blockEnd[state++];");
            w.WriteLine("for (var j = 0; j < prlen; ++j) {");
            ++w.IndentLevel;
            w.WriteLine("var pmin = blockEnd[state++];");
            w.WriteLine("var pmax = blockEnd[state++];");
            w.WriteLine("if (ch >= pmin && ch <= pmax) {");
            ++w.IndentLevel;
            w.WriteLine("ch = _FetchNextInput(cursor);");
            w.WriteLine("state = tto;");
            w.WriteLine("i = tlen;");
            w.WriteLine("done = false;");
            w.WriteLine("break;");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("if (-1 != acc)");
            ++w.IndentLevel;
            w.WriteLine("return ch == -1;");
            --w.IndentLevel;
            w.WriteLine("else");
            ++w.IndentLevel;
            w.WriteLine("ch = _FetchNextInput(cursor);");
            --w.IndentLevel;
            w.WriteLine("state = 0;");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("return false;");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine();
            for (var i = 0; i < 2; ++i)
            {
                w.Write("static System.Collections.Generic.KeyValuePair<long, string> _MatchBlockEndTable(int[] blockEnd, {0}, System.Text.StringBuilder sb, long position, ref long cursorPos, ref int ch) ", i == 0 ? "System.Collections.Generic.IEnumerator<char> cursor" : "System.IO.TextReader text");
                w.WriteLine("{");
                ++w.IndentLevel;
                w.WriteLine("var state = 0;");
                w.WriteLine("while (ch != -1) {");
                ++w.IndentLevel;
                w.WriteLine("var done = false;");
                w.WriteLine("var acc = -1;");
                w.WriteLine("while (!done) {");
                ++w.IndentLevel;
                w.WriteLine("done = true;");
                w.WriteLine("acc = blockEnd[state++];");
                w.WriteLine("var tlen = blockEnd[state++];");
                w.WriteLine("for (var i = 0; i < tlen; ++i) {");
                ++w.IndentLevel;
                w.WriteLine("var tto = blockEnd[state++];");
                w.WriteLine("var prlen = blockEnd[state++];");
                w.WriteLine("for (var j = 0; j < prlen; ++j) {");
                ++w.IndentLevel;
                w.WriteLine("var pmin = blockEnd[state++];");
                w.WriteLine("var pmax = blockEnd[state++];");
                w.WriteLine("if (ch >= pmin && ch <= pmax) {");
                ++w.IndentLevel;
                w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
                w.WriteLine("ch = _FetchNextInput({0});", i == 0 ? "cursor" : "text");
                w.WriteLine("++cursorPos;");
                w.WriteLine("state = tto;");
                w.WriteLine("i = tlen;");
                w.WriteLine("done = false;");
                w.WriteLine("break;");
                --w.IndentLevel;
                w.WriteLine("}");
                --w.IndentLevel;
                w.WriteLine("}");
                --w.IndentLevel;
                w.WriteLine("}");
                --w.IndentLevel;
                w.WriteLine("}");
                w.WriteLine("if (-1 != acc)");
                ++w.IndentLevel;
                w.WriteLine("return new System.Collections.Generic.KeyValuePair<long, string>(position, sb.ToString());");
                --w.IndentLevel;
                w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
                w.WriteLine("ch = _FetchNextInput({0});", i == 0 ? "cursor" : "text");
                w.WriteLine("++cursorPos;");
                w.WriteLine("state = 0;");
                --w.IndentLevel;
                w.WriteLine("}");
                w.WriteLine("return new System.Collections.Generic.KeyValuePair<long, string>(-1, null);");
                --w.IndentLevel;
                w.WriteLine("}");
                w.WriteLine();
                w.Write("static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,string>> _MatchTable(int[] entries, int[] blockEnd, {0} text) ",i==0?"System.Collections.Generic.IEnumerable<char>":"System.IO.TextReader");
                w.WriteLine("{");
                ++w.IndentLevel;
                w.WriteLine("var sb = new System.Text.StringBuilder();");
                w.WriteLine("long position;");
                if(0==i) 
                    w.WriteLine("var cursor = text.GetEnumerator();");
                w.WriteLine("var cursorPos = 0L;");
                w.WriteLine("var ch = _FetchNextInput({0});",i==0?"cursor":"text");
                w.WriteLine("var state = 0;");
                w.WriteLine("while (ch != -1) {");
                ++w.IndentLevel;
                w.WriteLine("sb.Clear();");
                w.WriteLine("position = cursorPos;");
                w.WriteLine("var done = false;");
                w.WriteLine("var acc = -1;");
                w.WriteLine("while (!done) {");
                ++w.IndentLevel;
                w.WriteLine("done = true;");
                w.WriteLine("acc = entries[state++];");
                w.WriteLine("var tlen = entries[state++];");
                w.WriteLine("for (var i = 0; i < tlen; ++i) {");
                ++w.IndentLevel;
                w.WriteLine("var tto = entries[state++];");
                w.WriteLine("var prlen = entries[state++];");
                w.WriteLine("for (var j = 0; j < prlen; ++j) {");
                ++w.IndentLevel;
                w.WriteLine("var pmin = entries[state++];");
                w.WriteLine("var pmax = entries[state++];");
                w.WriteLine("if (ch >= pmin && ch <= pmax) {");
                ++w.IndentLevel;
                w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
                w.WriteLine("ch = _FetchNextInput({0});", i == 0 ? "cursor" : "text");
                w.WriteLine("++cursorPos;"); 
                w.WriteLine("state = tto;");
                w.WriteLine("i = tlen;");
                w.WriteLine("done = false;");
                w.WriteLine("break;");
                --w.IndentLevel;
                w.WriteLine("}");
                --w.IndentLevel;
                w.WriteLine("}");
                --w.IndentLevel;
                w.WriteLine("}");
                --w.IndentLevel;
                w.WriteLine("}");
                w.WriteLine("if (-1 != acc) {");
                ++w.IndentLevel;
                w.WriteLine("if (blockEnd != null && blockEnd.Length > 0) {");
                ++w.IndentLevel;
                w.WriteLine("var b = _MatchBlockEndTable(blockEnd, {0}, sb, position, ref cursorPos, ref ch);",i==0?"cursor":"text");
                w.WriteLine("if (null != b.Value) yield return b;");
                w.WriteLine("state = 0;");
                w.WriteLine("continue;");
                --w.IndentLevel;
                w.WriteLine("} else");
                ++w.IndentLevel;
                w.WriteLine("if (sb.Length > 0) yield return new System.Collections.Generic.KeyValuePair<long, string>(position, sb.ToString());");
                --w.IndentLevel;
                --w.IndentLevel;
                w.WriteLine("}");
                w.WriteLine("ch = _FetchNextInput({0});",i==0?"cursor":"text");
                w.WriteLine("++cursorPos;");
                w.WriteLine("state = 0;");
                --w.IndentLevel;
                w.WriteLine("}");
                --w.IndentLevel;
                w.WriteLine("}");
                w.WriteLine();
            }
        }
        
        public static void GenerateFetchNextInputEnum(TextWriter writer)
        {
            writer.WriteLine("static int _FetchNextInput(System.Collections.Generic.IEnumerator<char> cursor) {");
            writer.WriteLine("    if(!cursor.MoveNext()) return -1;");
            writer.WriteLine("    var chh = cursor.Current;");
            writer.WriteLine("    int ch = chh;");
            writer.WriteLine("    if(char.IsHighSurrogate(chh)) {");
            writer.WriteLine("        if(!cursor.MoveNext()) throw new System.IO.IOException(\"Invalid surrogate found in Unicode stream\");");
            writer.WriteLine("        var chl = cursor.Current;");
            writer.WriteLine("        if(!char.IsLowSurrogate(chl)) throw new System.IO.IOException(\"Invalid surrogate found in Unicode stream\");");
            writer.WriteLine("        ch = char.ConvertToUtf32(chh,chl);");
            writer.WriteLine("    }");
            writer.WriteLine("    return ch;");
            writer.WriteLine("}");
        }
        public static void GenerateFetchNextInputReader(TextWriter writer)
        {
            writer.WriteLine("static int _FetchNextInput(System.IO.TextReader reader) {");
            writer.WriteLine("    var result = reader.Read();");
            writer.WriteLine("    if (-1 != result) {");
            writer.WriteLine("        if (char.IsHighSurrogate((char)result)) {");
            writer.WriteLine("            var chl = reader.Read();");
            writer.WriteLine("            if (-1 == chl) throw new System.IO.IOException(\"Invalid surrogate found in Unicode stream\");");
            writer.WriteLine("            if (!char.IsLowSurrogate((char)chl)) throw new System.IO.IOException(\"Invalid surrogate found in Unicode stream\");");
            writer.WriteLine("            result = char.ConvertToUtf32((char)result, (char)chl);");
            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("    return result;");
            writer.WriteLine("}");
        }
        public static void GenerateCommon(bool lineCounted, TextWriter writer)
        {
            //var memstm = new MemoryStream(Properties.Resources.Common_cs, false);
            var args = new Dictionary<string, object>();
            args.Add("lines", lineCounted);
            //Preprocessor.Run(new StreamReader(memstm), writer, args);
            CommonGenerator.Run(writer, args);
        }
        public static string GetSafeSymbolName(string symbol)
        {
            // TODO: make the identifier safe for C# such that if you use a keyword or invalid characters it doesn't break
            return symbol; 
        }
        
        public static void GenerateCodeAttribute(TextWriter writer)
        {
            // [System.CodeDom.Compiler.GeneratedCodeAttribute("Reggie", "*.*.*.*")]
            writer.WriteLine("[System.CodeDom.Compiler.GeneratedCodeAttribute(\"Reggie\", \"{0}\")]", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
        public static void GenerateTableExpressionDfa(LexRule rule, FFA fa, FFA blockEnd, TextWriter writer)
        {
            var w = new IndentedTextWriter(writer);
            w.Write("static readonly int[] _{0}Dfa = new int[] ", rule.Symbol);
            w.WriteLine("{");
            ++w.IndentLevel;
            var table = _ToDfaTable(fa);
            for (var i = 0; i < table.Length; ++i)
            {
                w.Write(table[i]);
                if(i<table.Length-1)
                {
                    w.Write(", "); 
                }
                if(((i+1)%50)==0)
                {
                    w.WriteLine();
                }
            }
            w.WriteLine();
            --w.IndentLevel;
            w.WriteLine("};");
            w.WriteLine();
            if (blockEnd != null)
            {
                w.Write("static readonly int[] _{0}BlockEndDfa = new int[] ", rule.Symbol);
                w.WriteLine("{");
                ++w.IndentLevel;
                table = _ToDfaTable(blockEnd);
                for (var i = 0; i < table.Length; ++i)
                {
                    w.Write(table[i]);
                    if (i < table.Length - 1)
                    {
                        w.Write(", ");
                    }
                    if (((i + 1) % 50) == 0)
                    {
                        w.WriteLine();
                    }
                }
                w.WriteLine();
                --w.IndentLevel;
                w.WriteLine("};");
                w.WriteLine();
            }

        }
        public static void GenerateTableTokenizerDfa(FFA fa, TextWriter writer)
        {
            var w = new IndentedTextWriter(writer);
            w.WriteLine("static readonly int[] _TokenizerDfa = new int[] {");
            ++w.IndentLevel;
            var table = _ToDfaTable(fa);
            for (var i = 0; i < table.Length; ++i)
            {
                w.Write(table[i]);
                if (i < table.Length - 1)
                {
                    w.Write(", ");
                }
                if (((i + 1) % 50) == 0)
                {
                    w.WriteLine();
                }
            }
            w.WriteLine();
            --w.IndentLevel;
            w.WriteLine("};");
        }
        public static void GenerateTableTokenizerBlockEndDfas(FFA[] blockEnds, TextWriter writer)
        {
            var w = new IndentedTextWriter(writer);
            w.WriteLine("static readonly int[][] _TokenizerBlockEndDfas = new int[][] {");
            ++w.IndentLevel;
            for(var j = 0;j<blockEnds.Length;++j)
            {
                var be = blockEnds[j];
                if(null!=be)
                {
                    w.WriteLine("new int[] {");
                    ++w.IndentLevel;
                    var table = _ToDfaTable(be);
                    for (var i = 0; i < table.Length; ++i)
                    {
                        w.Write(table[i]);
                        if (i < table.Length - 1)
                        {
                            w.Write(", ");
                        }
                        if (((i + 1) % 50) == 0)
                        {
                            w.WriteLine();
                        }
                    }
                    --w.IndentLevel;
                    w.Write("}");
                } else
                {
                    w.Write("null");
                }
                if (j < blockEnds.Length - 1)
                    w.WriteLine(",");
                else
                    w.WriteLine();
            }
            --w.IndentLevel;
            w.WriteLine("};");
        }
        public static void GenerateTableIsExpression(LexRule rule, TextWriter writer)
        {
            writer.WriteLine("/// <summary>Validates that input character stream contains content that matches the {0} expression.</summary>", rule.Symbol);
            writer.WriteLine("/// <param name=\"text\">The text stream to validate. The entire stream must match the expression.</param>");
            writer.WriteLine("/// <returns>True if <paramref name=\"text\"/> matches the expression indicated by {0}, otherwise false.</returns>", rule.Symbol);
            if (null == rule.GetAttribute("blockEnd"))
                writer.WriteLine("public static bool Is{0}(System.Collections.Generic.IEnumerable<char> text) => _IsTable(_{0}Dfa, null, text);", rule.Symbol);
            else
                writer.WriteLine("public static bool Is{0}(System.Collections.Generic.IEnumerable<char> text) => _IsTable(_{0}Dfa,_{0}BlockEndDfa, text);", rule.Symbol);
            writer.WriteLine();
        }
        public static void GenerateTableMatchExpression(LexRule rule, bool reader,TextWriter writer)
        {
            writer.WriteLine("/// <summary>Finds occurrances of a string matching the {0} expression.</summary>", rule.Symbol);
            writer.WriteLine("/// <param name=\"text\">The text stream to match on.</param>");
            writer.WriteLine("/// <returns>A <see cref=\"System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.Int64,System.String}}\"/> object that enumerates the match information.</returns>");
            var t = reader?"System.IO.TextReader": "System.Collections.Generic.IEnumerable<char>";
            var be = "null";
            if (null != rule.GetAttribute("blockEnd"))
                be = string.Format("_{0}BlockEndDfa", rule.Symbol);
            writer.WriteLine("public static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long, string>> Match{0}({1} text) => _MatchTable(_{0}Dfa, {2}, text);", rule.Symbol,t,be);
            writer.WriteLine();
        }
        public static void GenerateCompiledFetchToBlockEnd(LexRule rule,FFA blockEnd,bool lineCounted,bool reader,TextWriter writer)
        {
            var closure = new List<FFA>();
            blockEnd.FillClosure(closure);
            var isQ0reffed = _IsQ0Reffed(blockEnd, closure);
            var q0 = blockEnd;
            var w = new IndentedTextWriter(writer);
            w.Write("static Token _FetchTo{0}BlockEnd({1}, System.Text.StringBuilder sb, long position, ref long cursorPos, ref int ch{2}) ", rule.Symbol,reader?"System.IO.TextReader text":"System.Collections.Generic.IEnumerator<char> cursor",lineCounted?", int line, int column, ref int lc, ref int cc":"");
            w.WriteLine("{");
            ++w.IndentLevel;

            w.WriteLine("while (ch != -1) {");
            ++w.IndentLevel;
            for (var i = 0; i < closure.Count; ++i)
            {
                var fa = closure[i];
                // write out the state label
                --w.IndentLevel;
                if (i != 0 || isQ0reffed)
                    w.WriteLine("q{0}:", i.ToString());
                else
                    w.WriteLine("// q0");
                ++w.IndentLevel;
                // for each input transition set grouped by destination state:
                var itgbs = fa.FillInputTransitionRangesGroupedByState();
                foreach (var it in itgbs)
                {
                    // get the inputs as a set of ranges
                    var ranges = new List<KeyValuePair<int, int>>(_ToPairs(it.Value));
                    if (lineCounted)
                    {
                        var lclist = new List<int>(10);
                        if (_RangesContains(ranges, '\n'))
                        {
                            lclist.Add('\n');
                            ranges = new List<KeyValuePair<int, int>>(_ExcludeFromRanges(ranges, '\n'));
                        }
                        if (_RangesContains(ranges, '\r'))
                        {
                            lclist.Add('\r');
                            ranges = new List<KeyValuePair<int, int>>(_ExcludeFromRanges(ranges, '\r'));
                        }
                        if (_RangesContains(ranges, '\t'))
                        {
                            lclist.Add('\t');
                            ranges = new List<KeyValuePair<int, int>>(_ExcludeFromRanges(ranges, '\t'));
                        }
                        if (lclist.Contains('\t'))
                        {
                            w.Write("if(ch == \'\\t\') {");
                            ++w.IndentLevel;
                            w.WriteLine("sb.Append(unchecked((char)ch));");
                            w.WriteLine("cc=(((cc-1)/tabWidth)+1)*tabWidth+1;");
                            w.WriteLine("ch = _FetchNextInput({0});", reader ? "text" : "cursor");
                            w.WriteLine("++cursorPos;");
                            w.WriteLine("goto q{0};", closure.IndexOf(it.Key).ToString());
                            --w.IndentLevel;
                            w.WriteLine("}");
                        }
                        if (lclist.Contains('\r'))
                        {
                            w.Write("if(ch == \'\\r\') {");
                            ++w.IndentLevel;
                            w.WriteLine("sb.Append(unchecked((char)ch));");
                            w.WriteLine("cc=1;");
                            w.WriteLine("ch = _FetchNextInput({0});", reader ? "text" : "cursor");
                            w.WriteLine("++cursorPos;");
                            w.WriteLine("goto q{0};", closure.IndexOf(it.Key).ToString());
                            --w.IndentLevel;
                            w.WriteLine("}");
                        }
                        if (lclist.Contains('\n'))
                        {
                            w.Write("if(ch == \'\\n\') {");
                            ++w.IndentLevel;
                            w.WriteLine("sb.Append(unchecked((char)ch));");
                            w.WriteLine("++lc;");
                            w.WriteLine("cc=1;");
                            w.WriteLine("ch = _FetchNextInput({0});", reader ? "text" : "cursor");
                            w.WriteLine("++cursorPos;");
                            w.WriteLine("goto q{0};", closure.IndexOf(it.Key).ToString());
                            --w.IndentLevel;
                            w.WriteLine("}");
                        }
                    }
                    // write an if statement with a test generated from our ranges
                    w.Write("if(");
                    _GenerateCharRangeMatchTests(ranges, writer);
                    w.WriteLine(") {");
                    ++w.IndentLevel;
                    uint m = 0;
                    foreach (var ii in ranges)
                    {
                        if (m < unchecked((uint)ii.Value))
                        {
                            m = unchecked((uint)ii.Value);
                        }
                    }
                    if (m < 128)
                        w.WriteLine("sb.Append(unchecked((char)ch));");
                    else
                        w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
                    if (lineCounted)
                        w.WriteLine("if(ch>31) ++cc;");
                    w.WriteLine("ch = _FetchNextInput({0});", reader ? "text" : "cursor");
                    w.WriteLine("++cursorPos;");
                    w.WriteLine("goto q{0};", closure.IndexOf(it.Key).ToString());
                    --w.IndentLevel;
                    w.WriteLine("}");
                }
                if (fa.IsAccepting)
                {
                    if (!lineCounted)
                        w.WriteLine("return new Token({0}, sb.ToString(), position);", GetSafeSymbolName( rule.Symbol));
                    else
                        w.WriteLine("return new Token({0}, sb.ToString(), position, line, column);",GetSafeSymbolName(rule.Symbol));
                }
                if (i != closure.Count - 1)
                {
                    w.WriteLine("goto next;");
                }
            }
            --w.IndentLevel;
            w.WriteLine("next:");
            ++w.IndentLevel;
            w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
            if (lineCounted)
                w.WriteLine("if(ch>31) ++cc;");
            w.WriteLine("ch = _FetchNextInput({0});", reader ? "text" : "cursor");
            w.WriteLine("++cursorPos;");
            --w.IndentLevel;
            w.WriteLine("}");
            if (!lineCounted)
                w.WriteLine("return new Token(ERROR, sb.ToString(), position);");
            else
                w.WriteLine("return new Token(ERROR, sb.ToString(), position, line, column);");
            --w.IndentLevel;
            w.WriteLine("}");
        }
        public static void GenerateTableMatcher(string inputFile,IList<LexRule> rules, bool ignoreCase, TextWriter writer)
        {
            var args = new Dictionary<string, object>();
            args.Add("rules", rules);
            args.Add("inputfile", inputFile);
            args.Add("lines", false);
            args.Add("ignorecase", ignoreCase);
            // late bind so if we break the build it doesn't blow up all over the place
            // when we have to delete the output
            var tp = Type.GetType("Reggie.TableMatcherGenerator");
            tp.GetMethod("Run", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { writer, args });
            //TableMatcherGenerator.Run(writer, args);
        }
        public static void GenerateTableTokenize(string inputFile,bool ignoreCase,bool lineCounted, IList<LexRule> rules, TextWriter writer)
        {

            var args = new Dictionary<string, object>();
            args.Add("rules", rules);
            args.Add("inputfile", inputFile);
            args.Add("ignorecase", ignoreCase);
            args.Add("lines", lineCounted);
            // late bind so if we break the build it doesn't blow up all over the place
            // when we have to delete the output
            var tp = Type.GetType("Reggie.TableTokenizerGenerator");
            tp.GetMethod("Run", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { writer, args });
        }
        public static void GenerateCompiledMatcher(string inputFile, IList<LexRule> rules, bool ignoreCase, TextWriter writer)
        {
            var args = new Dictionary<string, object>();
            args.Add("rules", rules);
            args.Add("inputfile", inputFile);
            args.Add("lines", false);
            args.Add("ignorecase", ignoreCase);
            // late bind so if we break the build it doesn't blow up all over the place
            // when we have to delete the output
            var tp = Type.GetType("Reggie.CompiledMatcherGenerator");
            tp.GetMethod("Run", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { writer, args });
        }
        public static void GenerateCompiledTokenize(string inputFile, bool ignoreCase, bool lineCounted, IList<LexRule> rules, TextWriter writer)
        {
            var args = new Dictionary<string, object>();
            args.Add("rules", rules);
            args.Add("inputfile", inputFile);
            args.Add("lines", lineCounted);
            args.Add("ignorecase", ignoreCase);
            // late bind so if we break the build it doesn't blow up all over the place
            // when we have to delete the output
            var tp = Type.GetType("Reggie.CompiledTokenizerGenerator");
            tp.GetMethod("Run", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { writer, args });

        }
        public static void GenerateCompiledMatchBlockEndExpression(LexRule rule, FFA blockEnd,bool reader,TextWriter writer)
        {
            var closure = new List<FFA>();
            blockEnd.FillClosure(closure);
            var isQ0reffed = _IsQ0Reffed(blockEnd,closure);
            var w = new IndentedTextWriter(writer);
            w.Write("static System.Collections.Generic.KeyValuePair<long, string> _Match{0}BlockEnd({1}, System.Text.StringBuilder sb,long position, ref long cursorPos, ref int ch) ",rule.Symbol,reader?"System.IO.TextReader text": "System.Collections.Generic.IEnumerator<char> cursor");
            w.WriteLine("{");
            ++w.IndentLevel;
            w.WriteLine("while (ch != -1) {");
            ++w.IndentLevel;
            for (var i = 0; i < closure.Count; ++i)
            {
                var fa = closure[i];
                // write out the state label
                --w.IndentLevel;
                if (i != 0 || isQ0reffed)
                    w.WriteLine("q{0}:", i.ToString());
                else
                    w.WriteLine("// q0");
                ++w.IndentLevel;
                // for each input transition set grouped by destination state:
                var itgbs = fa.FillInputTransitionRangesGroupedByState();
                foreach (var it in itgbs)
                {
                    // get the inputs as a set of ranges
                    var ranges = _ToPairs(it.Value);
                    // write an if statement with a test generated from our ranges
                    w.Write("if(");
                    _GenerateCharRangeMatchTests(ranges, writer);
                    w.WriteLine(") {");
                    ++w.IndentLevel;
                    uint m = 0;
                    foreach (var ii in ranges)
                    {
                        if (m < unchecked((uint)ii.Value))
                        {
                            m = unchecked((uint)ii.Value);
                        }
                    }
                    if (m < 128)
                        w.WriteLine("sb.Append(unchecked((char)ch));");
                    else
                        w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
                    w.WriteLine("ch = _FetchNextInput({0});", reader ? "text" : "cursor");
                    w.WriteLine("++cursorPos;");
                    w.WriteLine("goto q{0};", closure.IndexOf(it.Key).ToString());
                    --w.IndentLevel;
                    w.WriteLine("}");
                }
                if (fa.IsAccepting)
                {
                    w.WriteLine("return new System.Collections.Generic.KeyValuePair<long, string>(position, sb.ToString());");
                }
                if (i != closure.Count - 1)
                {
                    w.WriteLine("goto next;");
                }
            }
            --w.IndentLevel;
            w.WriteLine("next:");
            ++w.IndentLevel;
            w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
            w.WriteLine("ch = _FetchNextInput({0});", reader ? "text" : "cursor");
            w.WriteLine("++cursorPos;");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("return new System.Collections.Generic.KeyValuePair<long, string>(-1, null);");
            --w.IndentLevel;
            w.WriteLine("}");
        }
        
        public static void GenerateCompiledMatchExpression(LexRule rule,FFA fa,FFA blockEnd,bool reader,TextWriter writer)
        {
            var closure = new List<FFA>();
            fa.FillClosure(closure);
            var isQ0reffed = _IsQ0Reffed(fa, closure);
            var w = new IndentedTextWriter(writer);
            w.WriteLine("/// <summary>Finds occurrances of a string matching the {0} expression.</summary>", rule.Symbol);
            w.WriteLine("/// <param name=\"text\">The text stream to match on.</param>");
            w.WriteLine("/// <returns>A <see cref=\"System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.Int64,System.String}}\"/> object that enumerates the match information.</returns>");
            w.Write("public static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,string>> Match{0}({1} text)", rule.Symbol,reader?"System.IO.TextReader": "System.Collections.Generic.IEnumerable<char>");
            w.WriteLine(" {");
            ++w.IndentLevel;
            w.WriteLine("var sb = new System.Text.StringBuilder();");
            w.WriteLine("var position = 0L;");
            if(!reader) 
                w.WriteLine("var cursor = text.GetEnumerator();");
            w.WriteLine("var cursorPos = 0L;");
            w.WriteLine("var ch = _FetchNextInput({0});",reader?"text":"cursor");
            w.WriteLine("while (ch != -1) {");
            ++w.IndentLevel;
            w.WriteLine("sb.Clear();");
            w.WriteLine("position = cursorPos;");
            for(var i = 0;i<closure.Count;++i)
            {
                fa = closure[i];
                // write out the state label
                --w.IndentLevel;
                if (i!=0 || isQ0reffed)
                    w.WriteLine("q{0}:",i.ToString());
                else 
                    w.WriteLine("// q0");
                ++w.IndentLevel;
                // for each input transition set grouped by destination state:
                var itgbs = fa.FillInputTransitionRangesGroupedByState();
                foreach (var it in itgbs)
                {
                    // get the inputs as a set of ranges
                    var ranges = _ToPairs(it.Value);
                    
                    // write an if statement with a test generated from our ranges
                    w.Write("if(");
                    _GenerateCharRangeMatchTests(ranges, writer);
                    w.WriteLine(") {");
                    ++w.IndentLevel;
                    uint m = 0;
                    foreach(var ii in ranges)
                    {
                        if(m<unchecked((uint)ii.Value))
                        {
                            m = unchecked((uint)ii.Value);
                        }
                    }
                    if(m<128)
                        w.WriteLine("sb.Append(unchecked((char)ch));");
                    else
                        w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
                    w.WriteLine("ch = _FetchNextInput({0});",reader?"text":"cursor");
                    w.WriteLine("++cursorPos;");
                    w.WriteLine("goto q{0};", closure.IndexOf(it.Key).ToString());
                    --w.IndentLevel;
                    w.WriteLine("}");
                }
                if(fa.IsAccepting)
                {
                    if(blockEnd!=null)
                    {
                        w.WriteLine("var b = _Match{0}BlockEnd({1}, sb, position, ref cursorPos, ref ch);",rule.Symbol,reader?"text":"cursor");
                        w.WriteLine("if (b.Value != null) yield return b;");
                        w.WriteLine("continue;"); 
                    } else
                        w.WriteLine("if (sb.Length > 0) yield return new System.Collections.Generic.KeyValuePair<long, string>(position, sb.ToString());");
                }
                if(i!=closure.Count-1)
                {
                    w.WriteLine("goto next;");
                }
            }
            --w.IndentLevel;
            w.WriteLine("next:");
            ++w.IndentLevel;
            w.WriteLine("ch = _FetchNextInput({0});",reader?"text":"cursor");
            w.WriteLine("++cursorPos;");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("yield break;");
            --w.IndentLevel;
            w.WriteLine("}");
        }
        public static void GenerateCompiledIsBlockEndExpression(LexRule rule, FFA blockEnd,TextWriter writer)
        {
            var closure = new List<FFA>();
            blockEnd.FillClosure(closure);
            var isQ0reffed = _IsQ0Reffed(blockEnd, closure);
            var w = new IndentedTextWriter(writer);

            w.Write("static bool _Is{0}BlockEnd(System.Collections.Generic.IEnumerator<char> cursor, int ch) ", rule.Symbol);
            w.WriteLine("{");
            ++w.IndentLevel;
            w.WriteLine("while(ch != -1) {");
            ++w.IndentLevel;
            for (var i = 0; i < closure.Count; ++i)
            {
                var fa = closure[i];
                if (0 != i || isQ0reffed)
                {
                    --w.IndentLevel;
                    w.WriteLine("q{0}:", i.ToString());
                    ++w.IndentLevel;
                }
                else
                {
                    --w.IndentLevel;
                    // this is the first state, and does not need a label
                    // because it is not referenced
                    w.WriteLine("// q{0}", i.ToString());
                    ++w.IndentLevel;
                }
                // for each input transition set grouped by destination state:
                var itgbs = fa.FillInputTransitionRangesGroupedByState();
                foreach (var it in itgbs)
                {

                    // get the inputs as a set of ranges
                    var ranges = _ToPairs(it.Value);
                    // write an if statement with a test generated from our ranges
                    w.Write("if(");
                    _GenerateCharRangeMatchTests(ranges, writer);
                    w.WriteLine(") {");
                    ++w.IndentLevel;
                    // if the test succeeds, we advance the cursor. If we can't, then
                    // if the machine accepts on the next state, we return true
                    // otherwise false if it's not accepting.
                    // However, if we *can* move then we update ch, and goto the next state
                    w.WriteLine("ch = _FetchNextInput(cursor);");
                    w.WriteLine("goto q{0};", closure.IndexOf(it.Key).ToString());
                    --w.IndentLevel;
                    w.WriteLine("}");
                }
                // didn't match any of the transitions
                if (fa.IsAccepting)
                    w.WriteLine("return ch == -1;");
                else
                {
                    w.WriteLine("ch = _FetchNextInput(cursor);");
                    w.WriteLine("continue;");
                }

            }
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("return false;");
            --w.IndentLevel;
            w.WriteLine("}");

        }
        public static void GenerateCompiledIsExpression(LexRule rule,FFA fa,FFA blockEnd, TextWriter writer)
        {
            var closure = new List<FFA>();
            fa.FillClosure(closure);
            var isQ0reffed = _IsQ0Reffed(fa, closure);
            var w = new IndentedTextWriter(writer);
            w.WriteLine("/// <summary>Validates that input character stream contains content that matches the {0} expression.</summary>", rule.Symbol);
            w.WriteLine("/// <param name=\"text\">The text stream to validate. The entire stream must match the expression.</param>");
            w.WriteLine("/// <returns>True if <paramref name=\"text\"/> matches the expression indicated by {0}, otherwise false.</returns>",rule.Symbol);
            w.Write("public static bool Is{0}(System.Collections.Generic.IEnumerable<char> text) ", rule.Symbol);
            w.WriteLine("{");
            ++w.IndentLevel;
            w.WriteLine("var cursor = text.GetEnumerator();");
            w.WriteLine("var ch = _FetchNextInput(cursor);");
            w.WriteLine("if(ch == -1) return {0};", (fa.IsAccepting && null==blockEnd)? "true" : "false");
            for (var i = 0; i < closure.Count; ++i)
            {
                fa = closure[i];
                if (0 != i || isQ0reffed)
                {
                    --w.IndentLevel;
                    w.WriteLine("q{0}:", i.ToString());
                    ++w.IndentLevel;
                }
                else
                {
                    --w.IndentLevel;
                    // this is the first state, and does not need a label
                    // because it is not referenced
                    w.WriteLine("// q{0}", i.ToString());
                    ++w.IndentLevel;
                }
                // for each input transition set grouped by destination state:
                var itgbs = fa.FillInputTransitionRangesGroupedByState();
                foreach (var it in itgbs)
                {
                    
                    // get the inputs as a set of ranges
                    var ranges = _ToPairs(it.Value);
                    // write an if statement with a test generated from our ranges
                    w.Write("if(");
                    _GenerateCharRangeMatchTests(ranges, writer);
                    w.WriteLine(") {");
                    ++w.IndentLevel;
                    // if the test succeeds, we advance the cursor. If we can't, then
                    // if the machine accepts on the next state, we return true
                    // otherwise false if it's not accepting.
                    // However, if we *can* move then we update ch, and goto the next state
                    w.WriteLine("ch = _FetchNextInput(cursor);");                    
                    w.WriteLine("goto q{0};", closure.IndexOf(it.Key).ToString());
                    --w.IndentLevel;
                    w.WriteLine("}");
                }
                // didn't match any of the transitions
                if (fa.IsAccepting)
                {
                    if (null == blockEnd)
                        w.WriteLine("return ch == -1;");
                    else
                        w.WriteLine("return _Is{0}BlockEnd(cursor, ch);", rule.Symbol);
                }
                else
                    w.WriteLine("return false;");
                
            }
            --w.IndentLevel;
            w.WriteLine("}");
        }
        
        static KeyValuePair<int, int>[] _ToPairs(int[] packedRanges)
        {
            var result = new KeyValuePair<int, int>[packedRanges.Length / 2];
            for (var i = 0; i < result.Length; ++i)
            {
                var j = i * 2;
                result[i] = new KeyValuePair<int, int>(packedRanges[j], packedRanges[j + 1]);
            }
            return result;
        }
        
        // create escape sequences or valid C# char literals or integer literals as required
        static void _GenerateRangeChar(int ch, TextWriter writer)
        {
            switch (ch)
            {
                case '\0':
                    writer.Write("\'\\0\'");
                    break;
                case '\a':
                    writer.Write("\'\\a\'");
                    break;
                case '\b':
                    writer.Write("\'\\b\'");
                    break;
                case '\f':
                    writer.Write("\'\\f\'");
                    break;
                case '\n':
                    writer.Write("\'\\n\'");
                    break;
                case '\r':
                    writer.Write("\'\\r\'");
                    break;
                case '\t':
                    writer.Write("\'\\t\'");
                    break;
                case '\v':
                    writer.Write("\'\\v\'");
                    break;
                case '\'':
                    writer.Write("\'\\\'\'");
                    break;
                case '\"':
                    writer.Write("\'\\\"\'");
                    break;
                case '\\':
                    writer.Write("\'\\\\\'");
                    break;
                default:
                    if(ch>=0 && ch<128)
                    {
                        var c = (char)ch;
                        if (char.IsWhiteSpace(c) || char.IsLetterOrDigit(c) || char.IsPunctuation(c))
                        {
                            writer.Write("\'{0}\'", c.ToString());
                        }
                        else
                        {
                            writer.Write(ch.ToString());
                        }
                        break;
                    }
                    writer.Write(ch.ToString());
                    
                    break;
            }
        }
        // generates a test expression for a single character range
        static void _GenerateCharRangeMatchTest(KeyValuePair<int, int> range, TextWriter writer, bool noparens = false)
        {
            if (range.Key == range.Value)
            {
                writer.Write("ch == ");
                _GenerateRangeChar(range.Key, writer);
            }
            else if (range.Key + 1 == range.Value)
            {
                writer.Write("ch == ");
                _GenerateRangeChar(range.Key, writer);
                writer.Write(" || ch == ");
                _GenerateRangeChar(range.Value, writer);
            }
            else
            {
                if (!noparens)
                    writer.Write("(");
                writer.Write("ch >= ");
                _GenerateRangeChar(range.Key, writer);
                writer.Write(" && ch <= ");
                _GenerateRangeChar(range.Value, writer);
                if (!noparens)
                    writer.Write(")");

            }
        }
        // create a series of test expressions for a series of character ranges
        static void _GenerateCharRangeMatchTests(ICollection<KeyValuePair<int, int>> ranges, TextWriter writer)
        {
            if (ranges.Count == 0)
                throw new InvalidOperationException("Attempt to generate char range match tests without any ranges");
            int ic = 1;
            foreach (var range in ranges)
            {
                _GenerateCharRangeMatchTest(range, writer, ranges.Count == 1);
                if (ic < ranges.Count)
                {
                    writer.Write(" || ");
                }
                if(1!=ic && (0==(ic-1)%10))
                {
                    writer.WriteLine();
                    writer.Write("        ");
                }
                ++ic;
            }
        }
        static bool _IsQ0Reffed(FFA fa, IEnumerable<FFA> closure)
        {
            // search the machine to see if any other state references the first state
            // if we never have to do a goto q0: then we don't need the label q0: either
            // and if can eliminate it, then we can stop the C# compiler from generating
            // a warning:
            foreach (var cfa in closure)
            {
                foreach (var it in cfa.Transitions)
                {
                    if (it.To == fa)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        static IEnumerable<KeyValuePair<int, int>> _ExcludeFromRanges(IEnumerable<KeyValuePair<int, int>> ranges,int ch)
        {
            foreach(var range in ranges)
            {
                if (range.Key == ch)
                {
                    if (range.Value == ch) continue;
                    yield return new KeyValuePair<int, int>(range.Key + 1, range.Value);
                }
                else if (range.Value == ch)
                {
                    yield return new KeyValuePair<int, int>(range.Key, range.Value - 1);
                }
                else if (ch > range.Key && ch < range.Value)
                {
                    yield return new KeyValuePair<int, int>(range.Key, ch - 1);
                    yield return new KeyValuePair<int, int>(ch+1, range.Value);
                }
                else
                    yield return range;
            }
        }
        
        static bool _RangesContains(IEnumerable<KeyValuePair<int,int>> ranges, int value)
        {
            foreach(var range in ranges)
            {
                if (value >= range.Key && value <= range.Value) return true;
            }
            return false;
        }
        static int[] _ToDfaTable(F.FFA fa)
        {
            var working = new List<int>();
            var closure = new List<F.FFA>();
            fa.FillClosure(closure);
            var stateIndices = new int[closure.Count];
            for (var i = 0; i < closure.Count; ++i)
            {
                var cfa = closure[i];
                stateIndices[i] = working.Count;
                // add the accept
                working.Add(cfa.IsAccepting ? cfa.AcceptSymbol : -1);
                var itrgp = cfa.FillInputTransitionRangesGroupedByState();
                // add the number of transitions
                working.Add(itrgp.Count);
                foreach (var itr in itrgp)
                {
                    // We have to fill in the following after the fact
                    // We don't have enough info here
                    // for now just drop the state index as a placeholder
                    working.Add(closure.IndexOf(itr.Key));
                    // add the number of packed ranges
                    working.Add(itr.Value.Length / 2);
                    // add the packed ranges
                    working.AddRange(itr.Value);

                }
            }
            var result = working.ToArray();
            var state = 0;
            while (state < result.Length)
            {
                state++;
                var tlen = result[state++];
                for (var i = 0; i < tlen; ++i)
                {
                    // patch the destination
                    result[state] = stateIndices[result[state]];
                    ++state;
                    var prlen = result[state++];
                    state += prlen * 2;
                }
            }
            return result;
        }
    }
}
