using LC;
using F;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Reggie
{
    public class Program
    {
        public static readonly string CodeBase = _GetCodeBase();
        public static readonly string Filename = Path.GetFileName(CodeBase);
        public static readonly string Name = _GetName();
        public static readonly Version Version = _GetVersion();
        public static readonly string Description = _GetDescription();
        static int Main(string[] args)
        {
            /*using (var sr = File.OpenText(@"..\..\Program.cs"))
            {
                foreach (var t in Example.Tokenize(sr))
                {
                    Console.WriteLine("{3}@{0}/{1}: {2}", t.Line, t.Column, t.Value, t.Id);
                }
            }
            return 0;*/
            // we do this to allow for linking to Reggie as a library
            // such that you can call Run to do the actual process
            //
            // mainly this is used for things like visual studio
            // code generators
            return Run(args, Console.In, Console.Out, Console.Error);
        }
        public static int Run(string[] args, TextReader stdin, TextWriter stdout, TextWriter stderr)
        {
            // our return code
            var result = 0;
            // app parameters
            var arguments = new Dictionary<string, object>();
            bool ifstale = false;
            string inputfile = null;
            string outputfile = null;
            arguments["inputfile"] = null;
            arguments["outputfile"] = null;
            arguments["codeclass"] = null;
            arguments["codenamespace"] = null;
            arguments["ignorecase"] = false;
            arguments["ifstale"] = false;
            arguments["dot"] = false;
            arguments["jpg"] = false;
            arguments["tables"] = false;
            arguments["lexer"] = false;
            arguments["lines"] = false;
            // our working variables
            TextReader input = null;
            TextWriter output = null;
            try
            {
                if (0 == args.Length)
                {
                    _PrintUsage(stderr);
                    result = -1;
                }
                else if (args[0].StartsWith("/"))
                {
                    throw new ArgumentException("Missing input file.");
                }
                else
                {
                    // process the command line args
                    inputfile = args[0];
                    arguments["inputfile"] = inputfile;
                    for (var i = 1; i < args.Length; ++i)
                    {
                        switch (args[i].ToLowerInvariant())
                        {
                            case "/output":
                                if (args.Length - 1 == i) // check if we're at the end
                                    throw new ArgumentException(string.Format("The parameter \"{0}\" is missing an argument", args[i].Substring(1)));
                                ++i; // advance 
                                outputfile = args[i];
                                arguments["outputfile"] = outputfile;
                                break;
                            case "/class":
                                if (args.Length - 1 == i) // check if we're at the end
                                    throw new ArgumentException(string.Format("The parameter \"{0}\" is missing an argument", args[i].Substring(1)));
                                ++i; // advance 
                                arguments["codeclass"] = args[i];
                                break;
                            case "/namespace":
                                if (args.Length - 1 == i) // check if we're at the end
                                    throw new ArgumentException(string.Format("The parameter \"{0}\" is missing an argument", args[i].Substring(1)));
                                ++i; // advance 
                                arguments["codenamespace"] = args[i];
                                break;
                            case "/lexer":
                                arguments["lexer"] = true;
                                break;
                            case "/lines":
                                arguments["lines"] = true;
                                break;
                            case "/tables":
                                arguments["tables"] = true;
                                break;
                            case "/ignorecase":
                                arguments["ignorecase"] = true;
                                break;
                            case "/ifstale":
                                ifstale = true;
                                arguments["ifstale"] = ifstale;
                                break;
                            case "/dot":
                                arguments["dot"] = true;
                                break;
                            case "/jpg":
                                arguments["jpg"] = true;
                                break;
                            default:
                                throw new ArgumentException(string.Format("Unknown switch {0}", args[i]));
                        }
                    }
                    // now build it
                    var stale = true;
                    if (ifstale && null != outputfile)
                    {
                        stale = _IsStale(inputfile, outputfile);
                        if (!stale)
                            stale = _IsStale(CodeBase, outputfile);
                    }
                    if (!stale)
                    {
                        stderr.WriteLine("{0} skipped building {1} because it was not stale.", Name, outputfile);
                    }
                    else
                    {
                        var cwd = Environment.CurrentDirectory;
                        if (null != outputfile)
                        {
                            stderr.Write("{0} is building file: {1}", Name, outputfile);
                            cwd = Path.GetDirectoryName(outputfile);
                        }
                        else
                        {
                            stderr.Write("{0} is building output.", Name);
                        }
                        input = new StreamReader(inputfile);
                        arguments["input"] = input;
                        arguments["stderr"] = stderr;
                        stderr.WriteLine();
                        if (null == outputfile)
                            output = stdout;
                        else
                        {
                            // open the file and truncate it if necessary
                            var stm = File.Open(outputfile, FileMode.Create);
                            stm.SetLength(0);
                            output = new StreamWriter(stm);

                            TemplateCore.Run("MainGenerator", arguments, output);
                        }
                    }
                }
            }
            // we don't like to catch in debug mode
#if !DEBUG
			catch (Exception ex)
			{
				result = _ReportError(ex, stderr);
			}
#endif
            finally
            {
                // close the input file if necessary
                if (null != input)
                    input.Close();
                // close the output file if necessary
                if (null != outputfile && null != output)
                    output.Close();
            }
            return result;
        }
        static string[] _BuildSymbolTable(IList<LexRule> rules)
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
        static FFA[] _BuildBlockEnds(IList<LexRule> rules, string inputFile, bool ignoreCase)
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
                        result[rule.Id] = _ParseFA(lr, inputFile, ci);
                    }
                }
            }
            return result;
        }
        static int[] _BuildSymbolFlags(IList<LexRule> rules)
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
        static FFA _ParseFA(LexRule rule, string inputFile, bool ignoreCase)
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
        static FFA _BuildLexer(IList<LexRule> rules, bool ignoreCase, string inputFile)
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
                    System.Diagnostics.Debugger.Break();
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
        static bool _IsStale(string inputfile, string outputfile)
        {
            if (string.IsNullOrEmpty(outputfile) || string.IsNullOrEmpty(inputfile))
                return true;
            var result = true;
            // File.Exists doesn't always work right
            try
            {
                if (File.GetLastWriteTimeUtc(outputfile) >= File.GetLastWriteTimeUtc(inputfile))
                    result = false;
            }
            catch { }
            return result;
        }


        // do our error handling here (release builds)
        static int _ReportError(Exception ex, TextWriter stderr)
        {
            //_PrintUsage(stderr);
            stderr.WriteLine("Error: {0}", ex.Message);
            return -1;
        }
        static void _PrintUsage(TextWriter w)
        {
            w.Write("Usage: " + Filename + " ");
            w.WriteLine("<inputfile> [/output <outputfile>] [/class <codeclass>]");
            w.WriteLine("   [/namespace <codenamespace>] [/tables] [/lexer] [/lines]");
            w.WriteLine("   [/ignorecase] [/dot] [/jpg] [/ifstale]");
            w.WriteLine();

            w.Write(Name);
            w.Write(" ");
            w.Write(Version.ToString());
            if (!string.IsNullOrWhiteSpace(Description))
            {
                w.Write(" - ");
                w.WriteLine(Description);
            }
            else
            {
                w.WriteLine("<No description>");
            }
            w.WriteLine();
            w.WriteLine("   <inputfile>     The input lexer specification");
            w.WriteLine("   <outputfile>    The output source file - defaults to STDOUT");
            w.WriteLine("   <codeclass>     The name of the main class to generate - default derived from <outputfile>");
            w.WriteLine("   <codenamespace> The namespace to generate the code under - defaults to none");
            w.WriteLine("   <tables>        Generate DFA table code - defaults to compiled");
            w.WriteLine("   <lexer>         Generate a lexer instead of matcher functions");
            w.WriteLine("   <lines>         Generate line and column tracking code - lexer only");
            w.WriteLine("   <ignorecase>    Create case insensitive matchers - defaults to case sensitive");
            w.WriteLine("   <dot>           Creates .dot files for the state graph(s)");
            w.WriteLine("   <jpg>           Creates .jpg files for the state graph(s) (requires GraphViz)");
            w.WriteLine("   <ifstale>       Only generate if the input is newer than the output");
            w.WriteLine();
        }
        static string _GetCodeBase()
        {
            try { return Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName; }
            catch { return Path.Combine(Environment.CurrentDirectory, "reggie.exe"); }
        }
        static string _GetName()
        {
            try
            {
                foreach (var attr in Assembly.GetExecutingAssembly().CustomAttributes)
                {
                    if (typeof(AssemblyTitleAttribute) == attr.AttributeType)
                    {
                        return attr.ConstructorArguments[0].Value as string;
                    }
                }
            }
            catch { }
            return Path.GetFileNameWithoutExtension(Filename);
        }
        static Version _GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }
        static string _GetDescription()
        {
            string result = null;
            foreach (Attribute ca in Assembly.GetExecutingAssembly().GetCustomAttributes())
            {
                var ada = ca as AssemblyDescriptionAttribute;
                if (null != ada && !string.IsNullOrWhiteSpace(ada.Description))
                {
                    result = ada.Description;
                    break;
                }
            }
            return result;
        }

    }
}
