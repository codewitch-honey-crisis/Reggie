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
        public static void GenerateMatchClass(TextWriter writer)
        {
            var w = new IndentedTextWriter(writer);
            w.WriteLine("public struct Match");
            w.WriteLine("{");
            ++w.IndentLevel;
            w.WriteLine("internal Match(string text, long position) {");
            ++w.IndentLevel;
            w.WriteLine("Text = text;");
            w.WriteLine("Position = position;");
            --w.IndentLevel;
            w.WriteLine("}");
            w.WriteLine("public string Text { get; } ");
            w.WriteLine("public long Position { get; }");
            --w.IndentLevel;
            w.WriteLine("}");
        }
        public static void GenerateCodeAttribute(TextWriter writer)
        {
            // [System.CodeDom.Compiler.GeneratedCodeAttribute("Reggie", "*.*.*.*")]
            writer.WriteLine("[System.CodeDom.Compiler.GeneratedCodeAttribute(\"Reggie\", \"{0}\")]", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
        public static void GenerateMatchExpression(LexRule rule,FFA fa,bool reader,TextWriter writer)
        {
            var closure = new List<FFA>();
            fa.FillClosure(closure);
            var isQ0reffed = _IsQ0Reffed(fa, closure);
            var w = new IndentedTextWriter(writer);
            w.WriteLine("/// <summary>Finds occurrances of a string matching the {0} expression.</summary>", rule.Symbol);
            w.WriteLine("/// <param name=\"text\">The text stream to match on.</param>");
            w.WriteLine("/// <returns>A <see cref=\"System.Collections.Generic.IEnumerable{Match}\"/> object that enumerates the match information.</returns>");
            w.WriteLine("/// <remarks>{0} is defined as {1}</remarks>", rule.Symbol, rule.Expression);
            w.Write("public static System.Collections.Generic.IEnumerable<Match> Match{0}({1} text)", rule.Symbol,reader?"System.IO.TextReader": "System.Collections.Generic.IEnumerable<char>");
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
                    w.WriteLine("sb.Append(char.ConvertFromUtf32(ch));");
                    w.WriteLine("ch = _FetchNextInput({0});",reader?"text":"cursor");
                    w.WriteLine("++cursorPos;");
                    w.WriteLine("goto q{0};", closure.IndexOf(it.Key).ToString());
                    --w.IndentLevel;
                    w.WriteLine("}");
                }
                if(fa.IsAccepting)
                {
                    w.WriteLine("if (sb.Length > 0) yield return new Match(sb.ToString(), position);");
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
        public static void GenerateIsExpression(LexRule rule,FFA fa, TextWriter writer)
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
