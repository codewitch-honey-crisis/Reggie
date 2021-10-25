﻿using System;
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
        static int _FetchNextInput(IEnumerator<char> text) { return -1; }
        public static void GenerateDfaTableSupport(TextWriter writer)
        {
            var w = new IndentedTextWriter(writer);
            w.WriteLine("private struct DfaEntry");
            w.WriteLine("{");
            ++w.IndentLevel;
            w.WriteLine("public bool Accept;");
            w.WriteLine("public DfaTransitionEntry[] Transitions;");
            w.WriteLine("public DfaEntry(bool accept, DfaTransitionEntry[] transitions) {");
            ++w.IndentLevel;
            w.Write("Accept = accept;");
            w.Write("Transitions = transitions;");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine();

            w.WriteLine("private struct DfaTransitionEntry");
            w.WriteLine("{");
            ++w.IndentLevel;
            w.WriteLine("public int[] PackedRanges;");
            w.WriteLine("public int Destination;");
            w.WriteLine("public DfaTransitionEntry(int[] packedRanges, int destination) {");
            ++w.IndentLevel;
            w.Write("PackedRanges = packedRanges;");
            w.Write("Destination = destination;");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine();
            w.WriteLine("static bool _IsTable(DfaEntry[] entries, System.Collections.Generic.IEnumerable<char> text) {");
            ++w.IndentLevel;
            w.WriteLine("var cursor = text.GetEnumerator();");
            w.WriteLine("var ch = _FetchNextInput(cursor);");
            w.WriteLine("var state = 0;");
            w.WriteLine("if (ch == -1) return entries[0].Accept;");
            w.WriteLine("while (ch != -1) {");
            ++w.IndentLevel;
            w.WriteLine("var e = entries[state];");
            w.WriteLine("var m = false;");
            w.WriteLine("for (var i = 0; i < e.Transitions.Length; ++i) {");
            ++w.IndentLevel;
            w.WriteLine("var t = e.Transitions[i];");
            w.WriteLine("for (var j = 0; j < t.PackedRanges.Length; j += 2) {");
            ++w.IndentLevel;
            w.WriteLine("if (ch >= t.PackedRanges[j] && ch <= t.PackedRanges[j + 1]) {");
            ++w.IndentLevel;
            w.WriteLine("ch = _FetchNextInput(cursor);");
            w.WriteLine("if (ch == -1) return e.Accept;");
            w.WriteLine("state = t.Destination;");
            w.WriteLine("i = e.Transitions.Length;");
            w.WriteLine("e = entries[state];");
            w.WriteLine("m = true;");
            w.WriteLine("break;");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("if (!m) return false;");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("return false;");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine();
            for (var i = 0; i < 2; ++i)
            {
                w.Write("static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,string>> _MatchTable(DfaEntry[] entries, {0} text) ",i==0?"System.Collections.Generic.IEnumerable<char>":"System.IO.TextReader");
                w.WriteLine("{");
                ++w.IndentLevel;
                w.WriteLine("var sb = new System.Text.StringBuilder();");
                w.WriteLine("var position = 0L;");
                if(0==i)
                    w.WriteLine("var cursor = text.GetEnumerator();");
                w.WriteLine("var cursorPos = 0L;");
                w.WriteLine("var state = 0;");
                w.WriteLine("var ch = _FetchNextInput({0});",i==0?"cursor":"text");
                w.WriteLine("while (ch != -1) {");
                ++w.IndentLevel;
                w.WriteLine("sb.Clear();");
                w.WriteLine("position = cursorPos;");
                w.WriteLine("var e = entries[state];");
                w.WriteLine("for (var i = 0; i < e.Transitions.Length; ++i) {");
                ++w.IndentLevel;
                w.WriteLine("var t = e.Transitions[i];");
                w.WriteLine("for (var j = 0; j < t.PackedRanges.Length; j += 2) {");
                ++w.IndentLevel;
                w.WriteLine("if (ch >= t.PackedRanges[j] && ch <= t.PackedRanges[j + 1]) {");
                ++w.IndentLevel;
                w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
                w.WriteLine("ch = _FetchNextInput({0});", i == 0 ? "cursor" : "text");
                w.WriteLine("++cursorPos;");
                w.WriteLine("state = t.Destination;");
                w.WriteLine("i = e.Transitions.Length;");
                w.WriteLine("e = entries[state];");
                w.WriteLine("break;");
                --w.IndentLevel;
                w.WriteLine("}");
                --w.IndentLevel;
                w.WriteLine("}");
                --w.IndentLevel;
                w.WriteLine("}");
                w.WriteLine("if (e.Accept && sb.Length > 0) yield return new System.Collections.Generic.KeyValuePair<long, string>(position, sb.ToString());");
                w.WriteLine("ch = _FetchNextInput({0});", i == 0 ? "cursor" : "text");
                w.WriteLine("++cursorPos;");
                --w.IndentLevel;
                w.WriteLine("}");
                w.WriteLine("yield break;");
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
        public static void GenerateCodeAttribute(TextWriter writer)
        {
            // [System.CodeDom.Compiler.GeneratedCodeAttribute("Reggie", "*.*.*.*")]
            writer.WriteLine("[System.CodeDom.Compiler.GeneratedCodeAttribute(\"Reggie\", \"{0}\")]", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
        public static void GenerateTableExpressionDfa(LexRule rule, FFA fa, TextWriter writer)
        {
            var closure = new List<FFA>();
            fa.FillClosure(closure);
            var w = new IndentedTextWriter(writer);
            w.Write("static readonly DfaEntry[] _{0}Dfa = new DfaEntry[] ", rule.Symbol);
            w.WriteLine("{");
            ++w.IndentLevel;
            for (var i = 0; i < closure.Count; ++i)
            {
                var cfa = closure[i];
                w.Write("new DfaEntry({0}, new DfaTransitionEntry[] ", cfa.IsAccepting ? "true" : "false");
                w.WriteLine("{");
                ++w.IndentLevel;
                var itrgbs = cfa.FillInputTransitionRangesGroupedByState();
                int k = 0;
                foreach (var itr in itrgbs)
                {
                    w.WriteLine("new DfaTransitionEntry(new int[] {");
                    ++w.IndentLevel;
                    var m = 1;
                    for (var j = 0; j < itr.Value.Length; ++j)
                    {
                        w.Write(itr.Value[j].ToString());
                        if (j < itr.Value.Length - 1)
                        {
                            w.Write(", ");
                        }
                        if (0 == (m % 50))
                        {
                            w.WriteLine();
                        }
                        ++m;
                    }
                    w.Write("}");
                    w.Write(", {0})", closure.IndexOf(itr.Key));
                    if (k < itrgbs.Count - 1)
                    {
                        w.WriteLine(", ");
                    }
                    --w.IndentLevel;
                    ++k;
                }

                --w.IndentLevel;
                w.Write("})");
                if (i != closure.Count - 1)
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
            writer.WriteLine("/// <remarks>{0} is defined as {1}</remarks>", rule.Symbol, rule.Expression);
            writer.WriteLine("public static bool Is{0}(System.Collections.Generic.IEnumerable<char> text) => _IsTable(_{0}Dfa, text);", rule.Symbol);
            writer.WriteLine();
        }
        public static void GenerateTableMatchExpression(LexRule rule, bool reader,TextWriter writer)
        {
            writer.WriteLine("/// <summary>Finds occurrances of a string matching the {0} expression.</summary>", rule.Symbol);
            writer.WriteLine("/// <param name=\"text\">The text stream to match on.</param>");
            writer.WriteLine("/// <returns>A <see cref=\"System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.Int64,System.String}}\"/> object that enumerates the match information.</returns>");
            writer.WriteLine("/// <remarks>{0} is defined as {1}</remarks>", rule.Symbol, rule.Expression);
            writer.WriteLine("public static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long, string>> Match{0}({1} text) => _MatchTable(_{0}Dfa, text);", rule.Symbol, reader?"System.IO.TextReader": "System.Collections.Generic.IEnumerable<char>");
            writer.WriteLine();
        }
        public static void GenerateCompiledMatchExpression(LexRule rule,FFA fa,bool reader,TextWriter writer)
        {
            var closure = new List<FFA>();
            fa.FillClosure(closure);
            var isQ0reffed = _IsQ0Reffed(fa, closure);
            var w = new IndentedTextWriter(writer);
            w.WriteLine("/// <summary>Finds occurrances of a string matching the {0} expression.</summary>", rule.Symbol);
            w.WriteLine("/// <param name=\"text\">The text stream to match on.</param>");
            w.WriteLine("/// <returns>A <see cref=\"System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.Int64,System.String}}\"/> object that enumerates the match information.</returns>");
            w.WriteLine("/// <remarks>{0} is defined as {1}</remarks>", rule.Symbol, rule.Expression);
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
                    w.WriteLine("if (sb.Length > 0) yield return new System.Collections.Generic.KeyValuePair<long,string>(position,sb.ToString());");
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
        public static void GenerateCompiledIsExpression(LexRule rule,FFA fa, TextWriter writer)
        {
            var closure = new List<FFA>();
            fa.FillClosure(closure);
            var isQ0reffed = _IsQ0Reffed(fa, closure);
            writer.WriteLine("/// <summary>Validates that input character stream contains content that matches the {0} expression.</summary>", rule.Symbol);
            writer.WriteLine("/// <param name=\"text\">The text stream to validate. The entire stream must match the expression.</param>");
            writer.WriteLine("/// <returns>True if <paramref name=\"text\"/> matches the expression indicated by {0}, otherwise false.</returns>",rule.Symbol);
            writer.WriteLine("/// <remarks>{0} is defined as {1}</remarks>", rule.Symbol, rule.Expression);
            writer.Write("public static bool Is{0}(System.Collections.Generic.IEnumerable<char> text)", rule.Symbol);
            writer.WriteLine(" {");
            writer.WriteLine("    var cursor = text.GetEnumerator();");
            writer.WriteLine("    var ch = _FetchNextInput(cursor);");
            writer.WriteLine("    if(ch == -1) return {0};", fa.IsAccepting ? "true" : "false");
            for (var i = 0; i < closure.Count; ++i)
            {
                fa = closure[i];
                if (0 != i || isQ0reffed)
                {
                    writer.WriteLine("q{0}:", i.ToString());
                }
                else
                {
                    // this is the first state, and does not need a label
                    // because it is not referenced
                    writer.WriteLine("// q{0}", i.ToString());
                }
                // for each input transition set grouped by destination state:
                var itgbs = fa.FillInputTransitionRangesGroupedByState();
                foreach (var it in itgbs)
                {
                    
                    // get the inputs as a set of ranges
                    var ranges = _ToPairs(it.Value);
                    // write an if statement with a test generated from our ranges
                    writer.Write("    if(");
                    _GenerateCharRangeMatchTests(ranges, writer);
                    writer.WriteLine(") {");
                    // if the test succeeds, we advance the cursor. If we can't, then
                    // if the machine accepts on the next state, we return true
                    // otherwise false if it's not accepting.
                    // However, if we *can* move then we update ch, and goto the next state
                    writer.WriteLine("        ch = _FetchNextInput(cursor);");
                    writer.WriteLine("        if(ch == -1)");
                    writer.WriteLine("            return {0};", it.Key.IsAccepting ? "true" : "false");
                    writer.WriteLine("        goto q{0};", closure.IndexOf(it.Key).ToString());
                    writer.WriteLine("    }");
                }
                // didn't match any of the conditions so always returns false
                writer.WriteLine("    return false;");
            }

            writer.WriteLine("}");
        }
        static ICollection<KeyValuePair<int, int>> _GetRanges(ICollection<int> chars)
        {
            var result = new List<KeyValuePair<int, int>>();
            var sorted = new List<int>(chars);
            sorted.Sort();
            var first = 0;
            var last = 0;
            using (IEnumerator<int> e = sorted.GetEnumerator())
            {
                var moved = e.MoveNext();
                while (moved)
                {
                    first = last = e.Current;
                    while ((moved = e.MoveNext()) && (e.Current == last || e.Current == last + 1))
                    {
                        last = e.Current;
                    }
                    result.Add(new KeyValuePair<int, int>(first, last));

                }
            }
            return result;
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

    }
}
