using F;
using LC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Reggie
{
    class TemplateCore
    {
        public static bool IsStale(string inputfile, string outputfile) {
            
            if (string.IsNullOrEmpty(outputfile) || string.IsNullOrEmpty(inputfile))
                return true;
            var result = true;
            // File.Exists doesn't always work right
            try {
                if (File.GetLastWriteTimeUtc(outputfile) >= File.GetLastWriteTimeUtc(inputfile))
                    result = false;
            }
            catch { }
            return result;
        }

        public static void CrackArguments(string defaultname, string[] args, ICollection<string> required, IDictionary<string,object> arguments) {

            var argi = 0; 
            if (!string.IsNullOrEmpty(defaultname)) {
                if(args.Length==0 || args[0][0]=='/') {
                    if (required.Contains(defaultname))
                        throw new ArgumentException(string.Format("<{0}> must be specified.",defaultname));
                } else {
                    var o = arguments[defaultname];
                    var isarr = o is string[];
                    var iscol = o is ICollection<string>;
                    if (!isarr && !iscol && !(o is string))
                        throw new InvalidProgramException(string.Format("Type for {0} must be string or a string collection or array", defaultname));
                    
                    for (; argi < args.Length; ++argi) {
                        var arg = args[argi];
                        if (arg[0] == '/') break;
                        if (isarr) {
                            var sa = new string[((string[])o).Length + 1];
                            Array.Copy((string[])o, sa, sa.Length - 1);
                            sa[sa.Length - 1] = arg;
                            arguments[defaultname] = sa;
                        } else if (iscol) {
                            ((ICollection<string>)o).Add(arg);
                        } else if ("" == (string)o) {
                            arguments[defaultname] = arg;
                        } else
                            throw new ArgumentException(string.Format("Only one <{0}> value may be specified.",defaultname));
                    }
                }
            }
            for(; argi<args.Length;++argi) {
                var arg = args[argi];
                if(string.IsNullOrWhiteSpace(arg) || arg[0]!='/') {
                    throw new ArgumentException(string.Format("Expected switch instead of {0}", arg));
                }
                arg = arg.Substring(1);
                if (!char.IsLetterOrDigit(arg, 0))
                    throw new ArgumentException("Invalid switch /{0}", arg);
                object o;
                if(!arguments.TryGetValue(arg,out o)) {
                    throw new InvalidProgramException(string.Format("Unknown switch /{0}", arg));
                }
                var isarr = o is string[];
                var iscol = o is ICollection<string>;
                var isbool = o is bool;
                var isstr = o is string;
                
                if (isarr || iscol) {
                    while (++argi < args.Length) {
                        var sarg = args[argi];
                        if (sarg[0] == '/')
                            break;
                        if (isarr) {
                            var sa = new string[((string[])o).Length + 1];
                            Array.Copy((string[])o, sa, sa.Length - 1);
                            sa[sa.Length - 1] = sarg;
                            arguments[arg] = sa;
                        } else if (iscol) {
                            ((ICollection<string>)o).Add(sarg);
                        }
                    }
                } else if (isstr) {
                    if (argi == args.Length - 1)
                        throw new ArgumentException(string.Format("Missing value for /{0}", arg));
                    var sarg = args[++argi];
                    if ("" == (string)o) {
                        arguments[arg] = sarg;
                    } else
                        throw new ArgumentException(string.Format("Only one <{0}> value may be specified.",arg));
                } else if(isbool) {
                    if((bool)o) {
                        throw new ArgumentException(string.Format("Only one /{0} switch may be specified.", arg));
                    }
                    arguments[arg] = true; 
                } else
                    throw new InvalidProgramException(string.Format("Type for {0} must be a boolean, a string, a string collection or a string array", arg));
            }
            foreach(var arg in required) {
                if(!arguments.ContainsKey(arg)) {
                    throw new ArgumentException(string.Format("Missing required switch /{0}", arg));
                }
                var o = arguments[arg];
                if(null==o || ((o is string) && ((string)o)=="") || ((o is System.Collections.ICollection) && ((System.Collections.ICollection)o).Count==0) /*|| ((o is bool) && (!(bool)o))*/)
                    throw new ArgumentException(string.Format("Missing required switch /{0}", arg));
            }
        }
        public static bool IsBinaryInputFile(IDictionary<string, object> arguments) {
            var result = false;
            string s;
            if (arguments.ContainsKey("_fourcc")) {
                s = (string)arguments["_fourcc"];
                if (s.StartsWith("rgl")) {
                    return true;
                } else if (s.StartsWith("rgm")) {
                    return true;
                }
            }
            using (var fstm = File.OpenRead(Path.GetFullPath((string)arguments["input"]))) {
                var ba = new byte[4];
                if (ba.Length == fstm.Read(ba, 0, ba.Length)) {
                    if (ba[ba.Length - 1] == 0) {
                        s = Encoding.ASCII.GetString(ba);
                        arguments["_fourcc"] = s.Substring(0, 3);
                        if (s.StartsWith("rgl")) {
                            result = true;
                        } else if (s.StartsWith("rgm")) {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
        public static void LoadInputFile(IDictionary<string, object> arguments) {
            var isbinary = false;
            object ob;

            if (!arguments.TryGetValue("_isbinary", out ob) || !(ob is bool)) {
                isbinary = IsBinaryInputFile(arguments);
                arguments["_isbinary"] = isbinary;
            }
            var lexer = (bool)arguments["lexer"];
            var _stderr = (TextWriter)arguments["_stderr"];
            if (!isbinary) {
                var rules = new List<Reggie.LexRule>();
                using (var input = File.OpenText((string)arguments["input"])) {
                    string line;
                    while (null != (line = input.ReadLine())) {
                        var lc = LC.LexContext.Create(line);
                        lc.TrySkipCCommentsAndWhiteSpace();
                        if (-1 != lc.Current)
                            rules.Add(Reggie.LexRule.Parse(lc));
                    }
                }
                if (rules.Count == 0) throw new ArgumentException("<input> file contains no rules.");
                Reggie.LexRule.FillRuleIds(rules, !lexer);
                arguments["_symbolTable"] = (string[])BuildSymbolTable(rules);
                if (lexer) {
                    arguments["_dfa"] = (int[])BuildLexerDfa(rules, (string)arguments["input"], (bool)arguments["ignorecase"]);
                } else {
                    arguments["_dfas"] = (int[][])BuildMatcherDfas(rules, (string)arguments["input"], (bool)arguments["ignorecase"]);
                }
                arguments["_blockEndDfas"] = (int[][])BuildBlockEndDfas(rules, (string)arguments["input"], (bool)arguments["ignorecase"]);
                if (lexer) {
                    arguments["_symbolFlags"] = (int[])BuildSymbolFlags(rules);
                }
            } else {
                var target = (string)arguments["target"];
                if (target == "rgg") {
                    throw new ArgumentException("You cannot /target rgg with a reggie binary input file");
                }
                if (lexer && "rgl" != (string)arguments["_fourcc"]) {
                    throw new InvalidOperationException("The input file is a binary matcher, but /lexer was specified.");
                }
                int[] dfa = null;
                int[][] blockEndDfas = null;
                string[] symbolTable = null;
                int[] symbolFlags = null;
                using (var stm = File.OpenRead((string)arguments["input"])) {
                    var br = new BinaryReader(stm);
                    // TODO: Version check the file
                    stm.Position += 20; // skip the fourcc and the version
                    var afterMaxId = LE(br.ReadInt32());
                    var ruleCount = LE(br.ReadInt32());
                    var dfas = new int[afterMaxId][];
                    blockEndDfas = new int[afterMaxId][];
                    symbolTable = new string[afterMaxId];
                    symbolFlags = new int[afterMaxId];
                    arguments["_dfas"] = dfas;
                    arguments["_blockEndDfas"] = blockEndDfas;
                    arguments["_symbolTable"] = symbolTable;
                    arguments["_symbolFlags"] = symbolFlags;
                    if (!lexer) {
                        for (var i = 0; i < ruleCount; ++i) {
                            var len = LE(br.ReadInt32());
                            symbolTable[i] = Encoding.UTF8.GetString(br.ReadBytes(len));
                            var dfalen = LE(br.ReadInt32());
                            dfa = new int[dfalen];
                            dfas[i] = dfa;
                            for (var j = 0; j < dfa.Length; ++j) {
                                dfa[j] = LE(br.ReadInt32());
                            }
                            var bedfacount = LE(br.ReadInt32());
                            if (bedfacount > 0) {
                                var bedfa = new int[bedfacount];
                                blockEndDfas[i] = bedfa;
                                for (var j = 0; j < bedfa.Length; ++j) {
                                    bedfa[j] = LE(br.ReadInt32());
                                }
                            }
                        }
                    } else { // if(!lexer) ...
                        for (var i = 0; i < ruleCount; ++i) {
                            var id = LE(br.ReadInt32());
                            var len = LE(br.ReadInt32());
                            symbolTable[id] = Encoding.UTF8.GetString(br.ReadBytes(len));
                            symbolFlags[id] = LE(br.ReadInt32());
                            var bedfacount = LE(br.ReadInt32());
                            if (bedfacount > 0) {
                                var bedfa = new int[bedfacount];
                                blockEndDfas[id] = bedfa;
                                for (var j = 0; j < bedfa.Length; ++j) {
                                    bedfa[j] = LE(br.ReadInt32());
                                }
                            }
                        }
                        var dfalen = LE(br.ReadInt32());
                        dfa = new int[dfalen];
                        for (var j = 0; j < dfa.Length; ++j) {
                            dfa[j] = LE(br.ReadInt32());
                        }
                        arguments["_dfa"] = dfa;
                    }
                }

            } // if(!isbinary) ...

        }
        [System.Diagnostics.DebuggerHidden()]
        public static bool Run(string template, IDictionary<string, object> arguments, TextWriter writer, bool throwIfNotFound=true,params object[] extraArguments) {
            var ma = typeof(Generator).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var m in ma) {
                
                if (0 == string.Compare(m.Name, template, StringComparison.InvariantCultureIgnoreCase)) {
                    try {
                        var pa = m.GetParameters();
                        if(pa.Length==2+extraArguments.Length && pa[1].ParameterType.IsAssignableFrom((typeof(IDictionary<string,object>)))) {
                            var pt = pa[0].ParameterType;
                            if (pt.IsAssignableFrom(typeof(TextWriter))) {
                                
                                if (extraArguments == null || extraArguments.Length == 0) {
                                    arguments["$Response"] = writer;
                                    m.Invoke(null, new object[] { writer, arguments });
                                } else {
                                    var args = new List<object>(extraArguments.Length + 2);
                                    args.Add(writer);
                                    args.Add(arguments);
                                    args.AddRange(extraArguments);
                                    arguments["$Response"] = writer;
                                    m.Invoke(null, args.ToArray());
                                }
                               
                                return true;
                            } else if (pt.IsAssignableFrom(typeof(Stream))) {
                                // unwind any nested indented text writers
                                var itw = writer as IndentedTextWriter;
                                while(itw!=null) {
                                    writer = itw.BaseWriter;
                                    itw = writer as IndentedTextWriter;
                                }
                                var w = writer as StreamWriter;
                                if (null != w) {
                                    var stream = w.BaseStream;
                                    if (null != stream) {

                                        if (extraArguments == null || extraArguments.Length == 0) {
                                            m.Invoke(null, new object[] { stream, arguments });
                                        } else {
                                            var args = new List<object>(extraArguments.Length + 2);
                                            args.Add(stream);
                                            args.Add(arguments);
                                            args.AddRange(extraArguments);
                                            m.Invoke(null, args.ToArray());
                                        }
                                        return true;
                                    }
                                }
                            } else
                                throw new InvalidOperationException("Cannot establish a binary connection to the output stream.");
                        }
                    }
                    catch (System.Reflection.TargetInvocationException tie) {
                        throw tie.InnerException;
                    }
                    catch { throw; }
                }
            }
            if(throwIfNotFound)
                throw new InvalidProgramException(string.Format("Template {0} not found!", template));
            return false;
        }
        public static bool IsValidTarget(IDictionary<string,object> arguments) {
            if (!arguments.ContainsKey("target")) return false;
            var t = ((string)arguments["target"]).ToLowerInvariant();
            foreach(var s in SupportedTargets) {
                if (t == s) return true;
            }
            return false;
        }
        public static IEnumerable<string> SupportedTargets {
            get {
                const string targetSuffix = "TargetGenerator";
                var ma = typeof(Generator).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly);
                foreach (var m in ma) {
                    if (m.Name.EndsWith(targetSuffix, StringComparison.InvariantCulture)) {
                        var s = m.Name.Substring(0, m.Name.Length - targetSuffix.Length);
                        var pa = m.GetParameters();
                        if (pa.Length == 2 && pa[1].ParameterType.IsAssignableFrom((typeof(IDictionary<string, object>)))) {
                            var pt = pa[0].ParameterType;
                            if (pt.IsAssignableFrom(typeof(TextWriter))) {
                                yield return s.ToLowerInvariant();
                            } else if (pt.IsAssignableFrom(typeof(Stream))) {
                                yield return s.ToLowerInvariant();
                            }
                        }
                    }
                }
            }
        }
        public static void Generate(IDictionary<string, object> arguments, Stream stream) {
            Run((string)arguments["target"] + "TargetGenerator", arguments, stream);
        }
        public static void Generate(IDictionary<string,object> arguments,TextWriter writer) {
            Run((string)arguments["target"]+ "TargetGenerator", arguments, writer);
        }
        public static bool Generate(string subfunction, IDictionary<string,object> arguments,TextWriter writer,bool throwIfNotFound=true, params object[] extraArguments) {
            if (null == subfunction) throw new ArgumentNullException("subfunction");
            if (string.IsNullOrWhiteSpace(subfunction)) throw new ArgumentException("Subfunction must not be empty", "subfunction");
            if (subfunction.EndsWith("TargetGenerator", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("Illegal attempt to use subfunction to call reserved methodsets. No.","subfunction");
            if(Run((string)arguments["target"] + subfunction, arguments, writer,false,extraArguments)) {
                return true;
            }
                
            return Run(subfunction , arguments, writer,throwIfNotFound,extraArguments);
        }
        public static bool Generate(string subfunction, IDictionary<string, object> arguments, TextWriter writer, params object[] extraArguments)
            => Generate(subfunction, arguments, writer, true, extraArguments);
        [System.Diagnostics.DebuggerHidden()]
        public static bool Run(string template, IDictionary<string, object> arguments, Stream stream,bool throwIfNotFound = true,params object[] extraArguments) {
            var ma = typeof(Generator).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly);
            foreach (var m in ma) {

                if (0 == string.Compare(m.Name, template, StringComparison.InvariantCultureIgnoreCase)) {
                    try {
                        if(extraArguments==null || extraArguments.Length==0)
                            m.Invoke(null, new object[] { stream, arguments });
                        else {
                            var args = new List<object>(extraArguments.Length+2);
                            args.Add(stream);
                            args.Add(arguments);
                            args.AddRange(extraArguments);
                            arguments["$Response"] = stream;
                            m.Invoke(null, args.ToArray());
                        }
                        return true;
                    }
                    catch (System.Reflection.TargetInvocationException tie) {
                        throw tie.InnerException;
                    }
                    catch { throw; }
                }
            }
            if (throwIfNotFound) {
                throw new InvalidProgramException(string.Format("Template {0} not found!", template));
            }
            return false;
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
        public static int[][] BuildMatcherDfas(IList<LexRule> rules, string inputFile, bool ignoreCase) {
            int max = int.MinValue;
            for (int ic = rules.Count, i = 0; i < ic; ++i) {
                var rule = rules[i];
                if (rule.Id > max)
                    max = rule.Id;
            }
            var result = new int[max+1][];
            foreach(var rule in rules) {
                result[rule.Id] = ToDfaTable(ParseToFA(rule, inputFile, ignoreCase));
            }
            return result;
        }
        public static int[] BuildLexerDfa(IList<LexRule> rules, string inputFile, bool ignoreCase) => ToDfaTable(BuildLexer(rules, inputFile, ignoreCase));
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
        public static int[][] BuildBlockEndDfas(FFA[] blockEnds) {
            var result = new int[blockEnds.Length][];
            for(var i = 0;i<blockEnds.Length;++i) {
                var be = blockEnds[i];
                if(null!=be) {
                    result[i] = ToDfaTable(be);
                }
            }
            return result;
        }
        public static int[][] BuildBlockEndDfas(IList<LexRule> rules, string inputFile, bool ignoreCase) {
            return BuildBlockEndDfas(BuildBlockEnds(rules, inputFile, ignoreCase));
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
            var prlen = dfa[prlenIndex];
            if (prlen * 2 > dfa.Length - prlenIndex) throw new ArgumentException("The dfa was too small", "dfa");
            prlenIndex++;
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
                        w.IndentLevel += indentLevel;
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
            int ic = 1;
            var prlen = dfa[prlenIndex++];
            var indented = false;
            for (var i = 0; i < prlen; ++i) {
                _WriteSqlRangeCharMatchTest(dfa, prlenIndex + (2 * i), w, prlen == 1);
                if (ic < prlen) {
                    w.Write(" OR ");
                }
                if (1 != ic && (0 == (ic - 1) % 10)) {
                    if (!indented) {
                        indented = true;
                        w.IndentLevel += indentLevel;
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
        public static int LE(int value) {
            // there are faster ways to do this but don't care
            if (BitConverter.IsLittleEndian) return value;
            var ba = BitConverter.GetBytes(value);
            Array.Reverse(ba);
            return BitConverter.ToInt32(ba,0);
        }
        
    }
   
    partial class Generator : TemplateCore { }
}


