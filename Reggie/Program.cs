using LC;
using F;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Net;

namespace Reggie
{
	public class Program
	{
		static readonly string CodeBase = _GetCodeBase();
		static readonly string Filename = Path.GetFileName(CodeBase);
		static readonly string Name = _GetName();
		static int Main(string[] args)
		{
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
			string inputfile = null;
			string outputfile = null;
			string codeclass = null;
			string codenamespace = null;
			bool ignorecase = false;
			bool ifstale = false;
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
					for (var i = 1; i < args.Length; ++i)
					{
						switch (args[i].ToLowerInvariant())
						{
							case "/output":
								if (args.Length - 1 == i) // check if we're at the end
									throw new ArgumentException(string.Format("The parameter \"{0}\" is missing an argument", args[i].Substring(1)));
								++i; // advance 
								outputfile = args[i];
								break;
							case "/class":
								if (args.Length - 1 == i) // check if we're at the end
									throw new ArgumentException(string.Format("The parameter \"{0}\" is missing an argument", args[i].Substring(1)));
								++i; // advance 
								codeclass = args[i];
								break;
							case "/namespace":
								if (args.Length - 1 == i) // check if we're at the end
									throw new ArgumentException(string.Format("The parameter \"{0}\" is missing an argument", args[i].Substring(1)));
								++i; // advance 
								codenamespace = args[i];
								break;
							case "/ignorecase":
								ignorecase = true;
								break;
							case "/ifstale":
								ifstale = true;
								break;


							default:
								throw new ArgumentException(string.Format("Unknown switch {0}", args[i]));
						}
					}
					// now build it
					if (string.IsNullOrEmpty(codeclass))
					{
						// default we want it to be named after the code file
						// otherwise we'll use inputfile
						if (null != outputfile)
							codeclass = Path.GetFileNameWithoutExtension(outputfile);
						else
							codeclass = Path.GetFileNameWithoutExtension(inputfile);
					}

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
						if (null != outputfile)
							stderr.Write("{0} is building file: {1}", Name, outputfile);
						else
							stderr.Write("{0} is building expressions.", Name);
						input = new StreamReader(inputfile);
						var rules = new List<LexRule>();
						string line;
						while (null != (line = input.ReadLine()))
						{
							var lc = LexContext.Create(line);
							lc.TrySkipCCommentsAndWhiteSpace();
							if (-1 != lc.Current)
								rules.Add(LexRule.Parse(lc));
						}
						input.Close();
						input = null;
						LexRule.FillRuleIds(rules);

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

						foreach (var rule in rules)
						{

						}

						var tw = new IndentedTextWriter(output);
						if (!string.IsNullOrEmpty(codenamespace))
						{
							tw.WriteLine("namespace {0}", codenamespace);
							tw.WriteLine("{");
							++tw.IndentLevel;
						}
						CodeGenerator.GenerateCodeAttribute(tw);
						tw.WriteLine("partial class {0}", codeclass);
						tw.WriteLine("{");
						++tw.IndentLevel;
						CodeGenerator.GenerateMatchClass(tw);
						CodeGenerator.GenerateFetchNextInputEnum(tw);
						CodeGenerator.GenerateFetchNextInputReader(tw);
						foreach (var rule in rules)
						{
							var fa = _ParseFA(rule, inputfile, ignorecase);
							//fa.RenderToFile(@"..\..\" + rule.Symbol + ".jpg");
							CodeGenerator.GenerateIsExpression(rule, fa, tw);
							CodeGenerator.GenerateMatchExpression(rule, fa, false, tw);
							CodeGenerator.GenerateMatchExpression(rule, fa, true, tw);
						}
						--tw.IndentLevel;
						tw.WriteLine("}");
						if (!string.IsNullOrEmpty(codenamespace))
						{
							--tw.IndentLevel;
							tw.WriteLine("}");
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
		static bool _IsStale(string inputfile, string outputfile)
		{
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
			w.WriteLine("<inputfile> [/output <outputfile>] [/class <codeclass>] [/namespace <codenamespace>]");
			w.WriteLine("   [/ignorecase] [/ifstale]");
			w.WriteLine();
			w.WriteLine(Name + " generates DFA regular expression matching code in C#");
			w.WriteLine();
			w.WriteLine("   <inputfile>     The input lexer specification");
			w.WriteLine("   <outputfile>    The output source file - defaults to STDOUT");
			w.WriteLine("   <codeclass>     The name of the main class to generate - default derived from <outputfile>");
			w.WriteLine("   <codenamespace> The namespace to generate the code under - defaults to none");
			w.WriteLine("   <ignorecase>    Create case insensitive matchers - defaults to case sensitive");
			w.WriteLine("   <ifstale>       Only generate if the input is newer than the output");
			w.WriteLine();
		}
		static string _GetCodeBase()
		{
			try
			{
				return Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
			}
			catch
			{
				return Path.Combine(Environment.CurrentDirectory, "rolex.exe");
			}
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
	}
}
