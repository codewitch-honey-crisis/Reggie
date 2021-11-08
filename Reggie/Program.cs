using LC;
using F;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Diagnostics;

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

            try
            {
                
                var arguments = new Expando();
                arguments.Add("_stdin", stdin);
                arguments.Add("_stdout", stdout);
                arguments.Add("_stderr", stderr);
                arguments.Add("_exe", Filename);
                arguments.Add("_codebase", CodeBase);
                arguments.Add("_name", Name);
                arguments.Add("_version", Version);
                arguments.Add("_description", Description);
                arguments.Add("_args", args);
                arguments.Add("_cwd", Environment.CurrentDirectory);

                TemplateCore.Run("Start", arguments, stderr);


                  
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
        static int _ReportError(Exception ex, TextWriter stderr)
        {
     
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
