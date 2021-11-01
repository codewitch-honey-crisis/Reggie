using LC;
using F;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Reggie
{
    /// <summary>
    /// The main application class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The codebase of the application
        /// </summary>
        public static readonly string CodeBase = _GetCodeBase();
        /// <summary>
        /// The filename of the application
        /// </summary>
        public static readonly string Filename = Path.GetFileName(CodeBase);
        /// <summary>
        /// The display name of the application
        /// </summary>
        public static readonly string Name = _GetName();
        /// <summary>
        /// The version of the application
        /// </summary>
        public static readonly Version Version = _GetVersion();
        /// <summary>
        /// The application description
        /// </summary>
        public static readonly string Description = _GetDescription();
        static int Main(string[] args) => Run(args, Console.In, Console.Out, Console.Error);
        
        /// <summary>
        /// Allows for linking to this application as a library
        /// such that you can call Run to do the actual process
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <param name="stdin">Serves as a substitude for STDIN</param>
        /// <param name="stdout">Serves as a substitude for STDOUT</param>
        /// <param name="stderr">Serves as a substitude for STDERR</param>
        /// <returns>The process exit code</returns>
        /// <remarks>Mainly this is used for things like Visual Studio code generators</remarks>
        public static int Run(string[] args, TextReader stdin, TextWriter stdout, TextWriter stderr)
        {
            // our return code
            var result = 0;
            // our supported targets
            var targets = new Dictionary<string, string>();
            targets.Add("cs", "CSharpMainGenerator");
            targets.Add("sql", "SqlMainGenerator");
            // app parameters
            var arguments = new Dictionary<string, object>();
            bool ifstale = false;
            string inputfile = null;
            string outputfile = null;
            arguments["inputfile"] = null;
            arguments["outputfile"] = null;
            arguments["codeclass"] = null;
            arguments["codenamespace"] = null;
            arguments["codetoken"] = null;
            arguments["ignorecase"] = false;
            arguments["ifstale"] = false;
            arguments["dot"] = false;
            arguments["jpg"] = false;
            arguments["tables"] = false;
            arguments["lexer"] = false;
            arguments["lines"] = false;
            arguments["codetarget"] = "cs";
            // our working variables
            TextReader input = null;
            TextWriter output = null;
            try
            {
                if (0 == args.Length)
                {
                    _PrintUsage(stderr,targets);
                    result = -1;
                }
                else if (args[0].StartsWith("/"))
                {
                    throw new ArgumentException("Missing input file.");
                }
                else
                {
                    string codetarget=null;
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
                            case "/token":
                                if (args.Length - 1 == i) // check if we're at the end
                                    throw new ArgumentException(string.Format("The parameter \"{0}\" is missing an argument", args[i].Substring(1)));
                                ++i; // advance 
                                arguments["codetoken"] = args[i];
                                break;
                            case "/target":
                                if (args.Length - 1 == i) // check if we're at the end
                                    throw new ArgumentException(string.Format("The parameter \"{0}\" is missing an argument", args[i].Substring(1)));
                                ++i; // advance 
                                codetarget = args[i].ToLowerInvariant();
                                if("csharp"==codetarget || "c#" == codetarget)
                                {
                                    codetarget = "cs";
                                }
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
                    if(string.IsNullOrEmpty(codetarget))
                    {
                        if (null != outputfile)
                        {
                            codetarget = Path.GetExtension(outputfile).Substring(1);
                        } 
                    }
                    if (string.IsNullOrEmpty(codetarget))
                    {
                        codetarget = "cs";
                    }
                    arguments["codetarget"] = codetarget;
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
                            
                        }
                        string typename;
                        if(targets.TryGetValue(codetarget,out typename))
                        {
                            TemplateCore.Run(typename, arguments, output);
                        } else
                        {
                            throw new ArgumentException(string.Format("The target \"{0}\" is not supported",codetarget), "target");
                        }
                        
                    }
                }
            }
            // we don't like to catch in debug mode
#if !DEBUG
			catch (Exception ex)
			{
				result = _ReportError(ex, stderr, targets);
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
        static int _ReportError(Exception ex, TextWriter stderr, IDictionary<string,string> targets)
        {
            //_PrintUsage(stderr, targets);
            stderr.WriteLine("Error: {0}", ex.Message);
            return -1;
        }
        static void _PrintUsage(TextWriter w,IDictionary<string,string> targets)
        {
            w.Write("Usage: " + Filename + " ");
            w.WriteLine("<inputfile> [/output <outputfile>] [/class <codeclass>]");
            w.WriteLine("   [/namespace <codenamespace>] [/token <codetoken>] [/ tables] [/lexer]");
            w.WriteLine("   [/target <codetarget>] [/lines] [/ignorecase] [/dot] [/jpg] [/ifstale]");
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
            w.WriteLine("   <codetoken>     The fully qualified name of an external token - defaults to internal");
            w.WriteLine("   <tables>        Generate DFA table code - defaults to compiled");
            w.WriteLine("   <lexer>         Generate a lexer instead of matcher functions");
            w.WriteLine("   <codetarget>    The output target to generate for - default derived from <outputfile>");
            var sb = new StringBuilder();
            var i = 0;
            foreach(var s in targets.Keys)
            {
                sb.Append('\"');
                sb.Append(s);
                sb.Append('\"');
                if (i < targets.Count - 3)
                    sb.Append(", ");
                else if (i == targets.Count - 2)
                    sb.Append(" and ");
                ++i;
            }
            w.Write("                   Supports ");
            w.WriteLine(sb.ToString());
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
