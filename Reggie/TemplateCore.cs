using F;
using LC;
using System;
using System.Collections.Generic;
using System.IO;

namespace Reggie
{
    class TemplateCore
    {
        public static void Run(string template,IDictionary<string,object> arguments,TextWriter writer, int indentLevel = 0)
        {
            if (0 < indentLevel)
            {
                var iw = new IndentedTextWriter(writer);
                iw.IndentLevel = indentLevel;
                writer = iw;
            }
            var s = "Reggie." + template;
            var ta = typeof(Reggie.Program).Assembly.GetTypes();
            foreach (var t in ta)
            {
               
                if(0==string.Compare(t.FullName,s,StringComparison.InvariantCultureIgnoreCase))
                {
                    var meth = t.GetMethod("Run", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly);
                    if (null == meth)
                        break;
                    try
                    {
                        meth.Invoke(null, new object[] { writer, arguments });
                        return;
                    }
                    catch (System.Reflection.TargetInvocationException tie)
                    {
                        throw tie.InnerException;
                    }
                    catch { throw; }
                }
            }
            
            throw new InvalidProgramException(string.Format("Template {0} not found!", template));
        }
        public static int[] ToDfaTable(F.FFA fa)
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
        public static T GetArgument<T>(IDictionary<string, object> arguments, string name, T @default = default)
        {
            object v;
            if (arguments.TryGetValue(name, out v) && v is T)
                return (T)v;
            return @default;
        }
        public static void WriteCSArray(int[] array, TextWriter writer, bool endLine = true)
        {
            if (null == array) { writer.Write("null"); return; }
            writer.Write("new int[] { ");
            var arrayCount = 4;
            for (var i = 0; i < array.Length; ++i)
            {
                writer.Write(array[i]);
                if (i < array.Length - 1) writer.Write(", ");
                if (0 == ((arrayCount++) % 50)) { writer.WriteLine(); writer.Write("    "); }
            }
            if (0 != ((arrayCount) % 50))
                writer.WriteLine();
            if (endLine)
                writer.WriteLine("};");
            else
                writer.Write("}");
        }
        public static void WriteCSArray(int[][] array, TextWriter writer, bool endLine = true)
        {
            if (null == array) { writer.Write("null"); return; }
            var w = new IndentedTextWriter(writer);
            w.WriteLine("new int[][] { ");
            ++w.IndentLevel;
            for (var i = 0; i < array.Length; ++i)
            {
                WriteCSArray(array[i], w, false);
                if (i < array.Length - 1) w.WriteLine(","); else w.WriteLine();
            }
            --w.IndentLevel;
            if (endLine)
                writer.WriteLine("};");
            else
                writer.WriteLine("}");
        }
        public static FFA ParseToFA(LexRule rule, string inputFile, bool ignoreCase)
        {
            FFA fa;
            if (rule.Expression.StartsWith("\""))
            {
                var pc = LexContext.Create(rule.Expression);
                fa = FFA.Literal(UnicodeUtility.ToUtf32(pc.ParseJsonString()), rule.Id);
            }
            else
                fa = FFA.Parse(rule.Expression.Substring(1, rule.Expression.Length - 2), rule.Id, rule.ExpressionLine, rule.ExpressionColumn, rule.ExpressionPosition, inputFile);
            if (!ignoreCase)
            {
                var ic = (bool)rule.GetAttribute("ignoreCase", false);
                if (ic)
                    fa = FFA.CaseInsensitive(fa, rule.Id);
            }
            else
            {
                var ic = (bool)rule.GetAttribute("ignoreCase", true);
                if (ic)
                    fa = FFA.CaseInsensitive(fa, rule.Id);
            }
            return fa.ToDfa();
        }
        public static int[] ToDfaTable(LexRule rule, string inputFile, bool ignoreCase)
        {
            return ToDfaTable(ParseToFA(rule, inputFile, ignoreCase));
        }
        public static FFA BuildLexer(IList<LexRule> rules, string inputFile, bool ignoreCase)
        {
            var exprs = new FFA[rules.Count];
            var result = new FFA();
            for (var i = 0; i < exprs.Length; ++i)
            {
                var rule = rules[i];
                FFA fa;
                if (rule.Expression.StartsWith("\""))
                {
                    var pc = LexContext.Create(rule.Expression);
                    fa = FFA.Literal(UnicodeUtility.ToUtf32(pc.ParseJsonString()), rule.Id);
                }
                else
                    fa = FFA.Parse(rule.Expression.Substring(1, rule.Expression.Length - 2), rule.Id, rule.ExpressionLine, rule.ExpressionColumn, rule.ExpressionPosition, inputFile);
                if (0 > rule.Id)
                    throw new Exception("Invalid rule id in lexer file"); ;
                if (!ignoreCase)
                {
                    var ic = (bool)rule.GetAttribute("ignoreCase", false);
                    if (ic)
                        fa = FFA.CaseInsensitive(fa, rule.Id);
                }
                else
                {
                    var ic = (bool)rule.GetAttribute("ignoreCase", true);
                    if (ic)
                        fa = FFA.CaseInsensitive(fa, rule.Id);
                }


                result.AddEpsilon(fa);
            }
            return result.ToDfa();
        }

        public static string[] BuildSymbolTable(IList<LexRule> rules)
        {
            int max = int.MinValue;
            for (int ic = rules.Count, i = 0; i < ic; ++i)
            {
                var rule = rules[i];
                if (rule.Id > max)
                    max = rule.Id;
            }
            var result = new string[max + 1];
            for (int ic = rules.Count, i = 0; i < ic; ++i)
            {
                var rule = rules[i];
                result[rule.Id] = rule.Symbol;
            }
            return result;
        }
        public static FFA[] BuildBlockEnds(IList<LexRule> rules, string inputFile, bool ignoreCase)
        {
            int max = int.MinValue;
            for (int ic = rules.Count, i = 0; i < ic; ++i)
            {
                var rule = rules[i];
                if (rule.Id > max)
                    max = rule.Id;
            }
            var result = new FFA[max + 1];
            for (int ic = rules.Count, i = 0; i < ic; ++i)
            {
                var ci = ignoreCase;

                var rule = rules[i];
                var ica = rule.GetAttribute("ignoreCase");
                if (null != ica && ica is bool)
                {
                    ci = (bool)ica;
                }
                var v = rule.GetAttribute("blockEnd");
                var be = v as string;
                if (!string.IsNullOrEmpty(be))
                {
                    var cfa = FFA.Literal(UnicodeUtility.ToUtf32(be), 0);
                    if (ci)
                        cfa = FFA.CaseInsensitive(cfa, 0);
                    result[rule.Id] = cfa.ToDfa();
                }
                else
                {
                    var lr = v as LexRule;
                    if (null != lr)
                    {
                        result[rule.Id] = ParseToFA(lr, inputFile, ci);
                    }
                }
            }
            return result;
        }
        public static bool DfaRangesContains(int ch,int[] dfa,int prlenIndex = 0)
        {
            var prlen = dfa[prlenIndex++];
            for(var i = 0;i<prlen;++i)
            {
                var first = dfa[prlenIndex++];
                var last = dfa[prlenIndex++];
                if (ch >= first && ch <= last) return true;
            }
            return false;
        }
        public static int[] DfaExcludeFromRanges(int ch,int[] dfa,int prlenIndex=0)
        {
            var result = new List<int>(dfa.Length);
            result.Add(0); // placeholder
            var prlen = dfa[prlenIndex++];
            for(var i = 0;i<prlen;++i)
            {
                var first = dfa[prlenIndex++];
                var last = dfa[prlenIndex++];
                if (first == ch)
                {
                    if (last == ch) continue;
                    result.Add(first + 1);
                    result.Add(last);
                }
                else if (last == ch)
                {
                    result.Add(first);
                    result.Add(last - 1);
                }
                else if (ch > first && ch < last)
                {
                    result.Add(first);
                    result.Add(ch - 1);
                    result.Add(ch + 1);
                    result.Add(last);
                }
                else
                {
                    result.Add(first);
                    result.Add(last);
                }
            }
            result[0] = (result.Count - 1)/2;
            return result.ToArray();
        }
        public static int[] BuildSymbolFlags(IList<LexRule> rules)
        {
            int max = int.MinValue;
            for (int ic = rules.Count, i = 0; i < ic; ++i)
            {
                var rule = rules[i];
                if (rule.Id > max)
                    max = rule.Id;
            }
            var result = new int[max + 1];
            for (int ic = rules.Count, i = 0; i < ic; ++i)
            {
                var rule = rules[i];
                var hidden = rule.GetAttribute("hidden");
                if ((hidden is bool) && (bool)hidden)
                    result[rule.Id] = 1;
            }
            return result;
        }
        // gets the min and max values for a set of transition ranges
        public static KeyValuePair<int, int > GetTransitionExtents(int[] dfa, int prlenIndex) {
            var prlen = dfa[prlenIndex++];
            var first = int.MaxValue;
            var last = int.MinValue;
            for(var i = 0;i<prlen;++i)
            {
                var pmin = dfa[prlenIndex++];
                var pmax = dfa[prlenIndex++];
                if (first > pmin) first = pmin;
                if (last < pmax) last = pmax;
            }
            if(first>last) return new KeyValuePair<int, int>(last, first);
            return new KeyValuePair<int, int>(first, last);
        }
        // returns an array that maps each index to its associated stateid. use like stateId = map[dfaIndex];
        public static int[] GetDfaStateTransitionMap(int[] dfa)
        {
            var result = new int[dfa.Length];
            var sid = 0;
            var si = 0;
            while(si<dfa.Length)
            {
                result[si] = sid;
                ++si; // accept
                result[si] = sid;
                var tlen = dfa[si++];
                for (var i = 0;i<tlen;++i)
                {
                    result[si] = sid;
                    ++si; // tto
                    result[si] = sid;
                    var prlen = dfa[si++];
                    for(var j = 0;j<prlen;++j)
                    {
                        result[si++] = sid;
                        result[si++] = sid;
                    }
                }
                ++sid;
            }
            return result; 
        }
        public static bool IsQ0Reffed(int[] dfa)
        {
            // search the machine to see if any other state references the first state
            // if we never have to do a goto q0: then we don't need the label q0: either
            // and if can eliminate it, then we can stop the C# compiler from generating
            // a warning:
            var si = 0;
            while(si<dfa.Length)
            {
                ++si; // accept
                var tlen = dfa[si++]; // # trans
                for(var i = 0;i<tlen;++i)
                {
                    if (dfa[si++] == 0) return true;
                    var prlen = dfa[si++];
                    si += prlen * 2;
                }
            }
            return false;
        }
        // create a series of test expressions for a series of character ranges
        public static void WriteCSRangeCharMatchTests(int[] dfa, int prlenIndex, int indentLevel, TextWriter writer)
        {
            var w = new IndentedTextWriter(writer);
            w.IndentLevel = indentLevel;
            int ic = 1;
            var prlen = dfa[prlenIndex++];
            var indented = false;
            for (var i = 0; i < prlen; ++i)
            {
                _WriteCSRangeCharMatchTest(dfa, prlenIndex+(2*i), w, prlen == 1);
                if (ic < prlen)
                {
                    w.Write(" || ");
                }
                if (1 != ic && (0 == (ic - 1) % 10))
                {
                    if (!indented)
                    {
                        indented = true;
                        w.IndentLevel += 2;
                    }
                    w.WriteLine();
                }
                ++ic;
            }
        }
        // generates a test expression for a single character range
        static void _WriteCSRangeCharMatchTest(int[] dfa, int rangeIndex, TextWriter writer, bool noparens = false)
        {
            var first = dfa[rangeIndex++];
            var last = dfa[rangeIndex++];
            if (first == last)
            {
                writer.Write("ch == ");
                _WriteCSRangeChar(first, writer);
            }
            else if (first + 1 == last)
            {
                writer.Write("ch == ");
                _WriteCSRangeChar(first, writer);
                writer.Write(" || ch == ");
                _WriteCSRangeChar(last, writer);
            }
            else
            {
                if (!noparens)
                    writer.Write("(");
                writer.Write("ch >= ");
                _WriteCSRangeChar(first, writer);
                writer.Write(" && ch <= ");
                _WriteCSRangeChar(last, writer);
                if (!noparens)
                    writer.Write(")");

            }
        }
        // create escape sequences or valid C# char literals or integer literals as required
        static void _WriteCSRangeChar(int ch, TextWriter writer)
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
                    if (ch >= 0 && ch < 128)
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
        // create a series of test expressions for a series of character ranges
        public static void WriteSqlRangeCharMatchTests(int[] dfa, int prlenIndex, int indentLevel, TextWriter writer)
        {
            var w = new IndentedTextWriter(writer);
            w.IndentLevel = indentLevel;
            int ic = 1;
            var prlen = dfa[prlenIndex++];
            var indented = false;
            for (var i = 0; i < prlen; ++i)
            {
                _WriteSqlRangeCharMatchTest(dfa, prlenIndex + (2 * i), w, prlen == 1);
                if (ic < prlen)
                {
                    w.Write(" OR ");
                }
                if (1 != ic && (0 == (ic - 1) % 10))
                {
                    if (!indented)
                    {
                        indented = true;
                        w.IndentLevel += 2;
                    }
                    w.WriteLine();
                }
                ++ic;
            }
        }
        // generates a test expression for a single character range
        static void _WriteSqlRangeCharMatchTest(int[] dfa, int rangeIndex, TextWriter writer, bool noparens = false)
        {
            var first = dfa[rangeIndex++];
            var last = dfa[rangeIndex++];
            if (first == last)
            {
                writer.Write("@ch = ");
                _WriteSqlRangeChar(first, writer);
            }
            else if (first + 1 == last)
            {
                writer.Write("@ch = ");
                _WriteSqlRangeChar(first, writer);
                writer.Write(" OR @ch = ");
                _WriteSqlRangeChar(last, writer);
            }
            else
            {
                if (!noparens)
                    writer.Write("(");
                writer.Write("@ch >= ");
                _WriteSqlRangeChar(first, writer);
                writer.Write(" AND @ch <= ");
                _WriteSqlRangeChar(last, writer);
                if (!noparens)
                    writer.Write(")");

            }
        }
        // writes a single SQL literal value in a range
        static void _WriteSqlRangeChar(int ch, TextWriter writer)
        {
            // just write it out as an integer - harder to read
            // but avoids a DB conversion.
            writer.Write(ch.ToString());
        }
    }
    partial class CSharpCommonGenerator : TemplateCore {}
    partial class CSharpTableMatcherGenerator : TemplateCore { }
    partial class CSharpTableTokenizerGenerator : TemplateCore { }
    partial class CSharpCompiledMatcherGenerator : TemplateCore { }
    partial class CSharpCompiledTokenizerGenerator : TemplateCore { }
    partial class CSharpMainGenerator : TemplateCore { }

    partial class SqlTableMatcherCreateGenerator : TemplateCore { }
    partial class SqlTableTokenizerCreateGenerator : TemplateCore { }
    partial class SqlTableMatcherGenerator : TemplateCore { }
    partial class SqlTableTokenizerGenerator : TemplateCore { }
    partial class SqlCompiledMatcherGenerator : TemplateCore { }
    partial class SqlCompiledTokenizerGenerator : TemplateCore { }
    partial class SqlMainGenerator : TemplateCore { }
    partial class SqlTableMatcherFillerGenerator : TemplateCore { }
    partial class SqlTableTokenizerFillerGenerator : TemplateCore { }
}


