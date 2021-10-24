using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace F
{
	partial class FFA
	{
		
		/// <summary>
		/// Represents optional rendering parameters for a dot graph.
		/// </summary>
		public sealed class DotGraphOptions
		{
			/// <summary>
			/// The resolution, in dots-per-inch to render at
			/// </summary>
			public int Dpi { get; set; } = 300;
			/// <summary>
			/// The prefix used for state labels
			/// </summary>
			public string StatePrefix { get; set; } = "q";

		}
		/// <summary>
		/// Writes a Graphviz dot specification of the specified closure to the specified <see cref="TextWriter"/>
		/// </summary>
		/// <param name="closure">The closure of all states</param>
		/// <param name="writer">The writer</param>
		/// <param name="options">A <see cref="DotGraphOptions"/> instance with any options, or null to use the defaults</param>
		static void _WriteDotTo(IList<FFA> closure, TextWriter writer, DotGraphOptions options = null)
		{
			if (null == options) options = new DotGraphOptions();
			string spfx = null == options.StatePrefix ? "q" : options.StatePrefix;
			writer.WriteLine("digraph FFA {");
			writer.WriteLine("rankdir=LR");
			writer.WriteLine("node [shape=circle]");
			var finals = new List<FFA>();

			var accepting = closure[0].FillAcceptingStates();
			foreach (var ffa in closure)
				if (ffa.IsFinal && !ffa.IsAccepting)
					finals.Add(ffa);


			int i = 0;
			foreach (var ffa in closure)
			{
				if (!finals.Contains(ffa))
				{
					if (ffa.IsAccepting)
						accepting.Add(ffa);
					
				}
				var rngGrps = ffa.FillInputTransitionRangesGroupedByState();
				foreach (var rngGrp in rngGrps)
				{
					var di = closure.IndexOf(rngGrp.Key);
					writer.Write(spfx);
					writer.Write(i);
					writer.Write("->");
					writer.Write(spfx);
					writer.Write(di.ToString());
					writer.Write(" [label=\"");
					var sb = new StringBuilder();
					IList<KeyValuePair<int,int>> rngs = _ToPairs(rngGrp.Value);
					var nrngs = new List<KeyValuePair<int,int>>( _NotRanges(rngs));
					var isNot = false;
					if (nrngs.Count < rngs.Count || (nrngs.Count == rngs.Count && 0x10ffff == rngs[rngs.Count - 1].Value))
					{
						isNot = true;
						if (0 != nrngs.Count)
						{
							sb.Append("^");
						}
						else
						{
							sb.Append(".");
						}
						rngs = nrngs;
					}
					var rpairs = _FromPairs(rngs);
					for (var r = 0; r < rpairs.Length;r+=2)
						_AppendRangeTo(sb,rpairs, r);
					if (isNot || sb.Length != 1 || (char.IsWhiteSpace(sb.ToString(), 0)))
					{
						writer.Write('[');
						writer.Write(_EscapeLabel(sb.ToString()));
						writer.Write(']');
					}
					else
						writer.Write(_EscapeLabel(sb.ToString()));
					writer.WriteLine("\"]");
				}
				
				++i;
			}
			
			i = 0;
			foreach (var ffa in closure)
			{
				writer.Write(spfx);
				writer.Write(i);
				writer.Write(" [");

				writer.Write("label=<");
				writer.Write("<TABLE BORDER=\"0\"><TR><TD>");
				writer.Write(spfx);
				writer.Write("<SUB>");
				writer.Write(i);
				writer.Write("</SUB></TD></TR>");


				if (ffa.IsAccepting)
				{
					writer.Write("<TR><TD>");
					writer.Write(Convert.ToString(ffa.AcceptSymbol).Replace("\"", "&quot;"));
					writer.Write("</TD></TR>");

				}
				writer.Write("</TABLE>");
				writer.Write(">");
				bool isfinal = false;
				if (accepting.Contains(ffa) || (isfinal = finals.Contains(ffa)))
					writer.Write(",shape=doublecircle");
				if (isfinal)
				{

					writer.Write(",color=gray");

				}
				writer.WriteLine("]");
				++i;
			}
			string delim = "";
			if (0 < accepting.Count)
			{
				foreach (var ntfa in accepting)
				{
					writer.Write(delim);
					writer.Write(spfx);
					writer.Write(closure.IndexOf(ntfa));
					delim = ",";
				}
				writer.WriteLine(" [shape=doublecircle]");
			}
			
			delim = "";
			if (0 < finals.Count)
			{
				foreach (var ntfa in finals)
				{
					writer.Write(delim);
					writer.Write(spfx);
					writer.Write(closure.IndexOf(ntfa));
					delim = ",";
				}
				writer.WriteLine(" [shape=doublecircle,color=gray]");
			}

			writer.WriteLine("}");
		}
		/// <summary>
		/// Renders Graphviz output for this machine to the specified file
		/// </summary>
		/// <param name="filename">The output filename. The format to render is indicated by the file extension.</param>
		/// <param name="options">A <see cref="DotGraphOptions"/> instance with any options, or null to use the defaults</param>
		public void RenderToFile(string filename, DotGraphOptions options = null)
		{
			if (null == options)
				options = new DotGraphOptions();
			string args = "-T";
			string ext = Path.GetExtension(filename);
			if (0 == string.Compare(".png", ext, StringComparison.InvariantCultureIgnoreCase))
				args += "png";
			else if (0 == string.Compare(".jpg", ext, StringComparison.InvariantCultureIgnoreCase))
				args += "jpg";
			else if (0 == string.Compare(".bmp", ext, StringComparison.InvariantCultureIgnoreCase))
				args += "bmp";
			else if (0 == string.Compare(".svg", ext, StringComparison.InvariantCultureIgnoreCase))
				args += "svg";
			if (0 < options.Dpi)
				args += " -Gdpi=" + options.Dpi.ToString();

			args += " -o\"" + filename + "\"";

			var psi = new ProcessStartInfo("dot", args)
			{
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardInput = true
			};
			using (var proc = Process.Start(psi))
			{
				WriteDotTo(proc.StandardInput, options);
				proc.StandardInput.Close();
				proc.WaitForExit();
			}

		}
		public void WriteDotTo(TextWriter writer, DotGraphOptions options = null)
		{
			_WriteDotTo(FillClosure(), writer, options);
		}
		/// <summary>
		/// Renders Graphviz output for this machine to a stream
		/// </summary>
		/// <param name="format">The output format. The format to render can be any supported dot output format. See dot command line documation for details.</param>
		/// <param name="copy">True to copy the stream, otherwise false</param>
		/// <param name="options">A <see cref="DotGraphOptions"/> instance with any options, or null to use the defaults</param>
		/// <returns>A stream containing the output. The caller is expected to close the stream when finished.</returns>
		public Stream RenderToStream(string format, bool copy = false, DotGraphOptions options = null)
		{
			if (null == options)
				options = new DotGraphOptions();
			string args = "-T";
			args += string.Concat(" ", format);
			if (0 < options.Dpi)
				args += " -Gdpi=" + options.Dpi.ToString();

			var psi = new ProcessStartInfo("dot", args)
			{
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardInput = true,
				RedirectStandardOutput = true
			};
			using (var proc = Process.Start(psi))
			{
				WriteDotTo(proc.StandardInput, options);
				proc.StandardInput.Close();
				if (!copy)
					return proc.StandardOutput.BaseStream;
				else
				{
					var stm = new MemoryStream();
					proc.StandardOutput.BaseStream.CopyTo(stm);
					proc.StandardOutput.BaseStream.Close();
					proc.WaitForExit();
					return stm;
				}
			}
		}
		static void _AppendRangeTo(StringBuilder builder, int[] ranges, int index)
		{
			var first = ranges[index];
			var last = ranges[index + 1];
			_AppendRangeCharTo(builder, first);
			if (0 == last.CompareTo(first)) return;
			if (last == first + 1) // spit out 1 length ranges as two chars
			{
				_AppendRangeCharTo(builder, last);
				return;
			}
			builder.Append('-');
			_AppendRangeCharTo(builder, last);
		}
		static void _AppendRangeCharTo(StringBuilder builder, int rangeChar)
		{
			switch (rangeChar)
			{
				case '.':
				case '[':
				case ']':
				case '^':
				case '-':
				case '\\':
					builder.Append('\\');
					builder.Append(char.ConvertFromUtf32(rangeChar));
					return;
				case '\t':
					builder.Append("\\t");
					return;
				case '\n':
					builder.Append("\\n");
					return;
				case '\r':
					builder.Append("\\r");
					return;
				case '\0':
					builder.Append("\\0");
					return;
				case '\f':
					builder.Append("\\f");
					return;
				case '\v':
					builder.Append("\\v");
					return;
				case '\b':
					builder.Append("\\b");
					return;
				default:
					var s = char.ConvertFromUtf32(rangeChar);
					if (!char.IsLetterOrDigit(s, 0) && !char.IsSeparator(s, 0) && !char.IsPunctuation(s, 0) && !char.IsSymbol(s, 0))
					{
						if (s.Length == 1)
						{
							builder.Append("\\u");
							builder.Append(unchecked((ushort)rangeChar).ToString("x4"));
						}
						else
						{
							builder.Append("\\U");
							builder.Append(rangeChar.ToString("x8"));
						}

					}
					else
						builder.Append(s);
					break;
			}
		}
		static string _EscapeLabel(string label)
		{
			if (string.IsNullOrEmpty(label)) return label;

			string result = label.Replace("\\", @"\\");
			result = result.Replace("\"", "\\\"");
			result = result.Replace("\n", "\\n");
			result = result.Replace("\r", "\\r");
			result = result.Replace("\0", "\\0");
			result = result.Replace("\v", "\\v");
			result = result.Replace("\t", "\\t");
			result = result.Replace("\f", "\\f");
			return result;
		}
	}
}
