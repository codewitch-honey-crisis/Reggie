using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
namespace LexTableGen
{
	using CU = CD.CodeDomUtility;
	class Program
	{
		static void Main(string[] args)
		{
			
			var fn = (args.Length>0)?args[0]:null;
			using (var sw =null==fn?Console.Out:new StreamWriter(File.OpenWrite(fn)))
			{
				var ccu = new CodeCompileUnit();
				var ns = new CodeNamespace("F");
				ccu.Namespaces.Add(ns);
				var td = CU.Class("CharacterClasses");
				ns.Types.Add(td);
				td.TypeAttributes = System.Reflection.TypeAttributes.Public;
				td.IsPartial = true;
				var uc = new List<int>[30];
				var nuc = new List<int>[30];
				var isLetter = new List<int>();
				var isLetterOrDigit = new List<int>();
				var isDigit = new List<int>();
				var isWhiteSpace = new List<int>();
				var graph = new List<int>();
				for (var i = 0; i < uc.Length; i++)
				{
					uc[i] = new List<int>();
					nuc[i] = new List<int>();
				}
				for (var i = 0; i < 0x110000; i++)
				{
					if (i >= 0x00d800 && i <= 0x00dfff)
						continue;
					var ch = char.ConvertFromUtf32(i);
					var uci = (int)char.GetUnicodeCategory(ch, 0);
					uc[uci].Add(i);
					for(var j=0;j<uc.Length;++j)
					{
						if(j!=uci)
						{
							nuc[j].Add(i);
						}
					}

					if (char.IsLetter(ch,0))
						isLetter.Add(i);
					if (char.IsDigit(ch,0))
						isDigit.Add(i);
					if (char.IsLetterOrDigit(ch,0))
						isLetterOrDigit.Add(i);
					if (char.IsWhiteSpace(ch, 0))
						isWhiteSpace.Add(i);
					if (!char.IsWhiteSpace(ch, 0) && !char.IsControl(ch, 0))
						graph.Add(i);
				}
				var uca = new int[30][];
				for(var i = 0;i<uca.Length;i++)
					uca[i] = _GetRanges(uc[i]);
				var nuca = new int[30][];
				for (var i = 0; i < nuca.Length; i++)
					nuca[i] = _GetRanges(nuc[i]);
				var alnum = new List<int>();
				alnum.AddRange(uc[(int)UnicodeCategory.LetterNumber]);
				alnum.AddRange(isLetter);
				alnum.AddRange(uc[(int)UnicodeCategory.DecimalDigitNumber]);
				alnum.Sort();

				var alpha = new List<int>();
				alpha.AddRange(uc[(int)UnicodeCategory.LetterNumber]);
				alpha.AddRange(isLetter);
				alpha.Sort();

				var blank = new List<int>();
				blank.AddRange(uc[(int)UnicodeCategory.SpaceSeparator]);
				blank.Add('\t');
				blank.Sort();
				
				var asciiRanges = new int[] { 0, 0x7F };

				var punct = new List<int>();
				punct.AddRange(uca[(int)UnicodeCategory.ClosePunctuation]);
				punct.AddRange(uca[(int)UnicodeCategory.ConnectorPunctuation]);
				punct.AddRange(uca[(int)UnicodeCategory.DashPunctuation]);
				punct.AddRange(uca[(int)UnicodeCategory.FinalQuotePunctuation]);
				punct.AddRange(uca[(int)UnicodeCategory.InitialQuotePunctuation]);
				punct.AddRange(uca[(int)UnicodeCategory.OpenPunctuation]);
				punct.AddRange(uca[(int)UnicodeCategory.OtherPunctuation]);

				var word = new List<int>();
				word.AddRange(isLetter);
				word.AddRange(uca[(int)UnicodeCategory.LetterNumber]);
				word.AddRange(uca[(int)UnicodeCategory.ConnectorPunctuation]);
				word.AddRange(uca[(int)UnicodeCategory.DecimalDigitNumber]);

				var xdigit = new List<int>();
				xdigit.Add('0');
				xdigit.Add('9');
				xdigit.Add('A');
				xdigit.Add('F');
				xdigit.Add('a');
				xdigit.Add('f');

				td.Members.Add(CU.Field(uca.GetType(), "UnicodeCategories", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(uca)));
				td.Members.Add(CU.Field(nuca.GetType(), "NotUnicodeCategories", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(nuca)));
				td.Members.Add(CU.Field(typeof(int[]), "IsLetter", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(isLetter))));
				td.Members.Add(CU.Field(typeof(int[]), "IsDigit", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(isDigit))));
				td.Members.Add(CU.Field(typeof(int[]), "IsLetterOrDigit", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(isLetterOrDigit))));
				td.Members.Add(CU.Field(typeof(int[]), "IsWhiteSpace", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(isWhiteSpace))));
				td.Members.Add(CU.Field(typeof(int[]), "alnum", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(alnum))));
				td.Members.Add(CU.Field(typeof(int[]), "alpha", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(alpha))));
				td.Members.Add(CU.Field(typeof(int[]), "cntrl", MemberAttributes.Public | MemberAttributes.Static, CU.ArrIndexer(CU.FieldRef(CU.TypeRef(td.Name), "UnicodeCategories"), CU.Literal((int)UnicodeCategory.Control))));
				td.Members.Add(CU.Field(typeof(int[]), "digit", MemberAttributes.Public | MemberAttributes.Static, CU.ArrIndexer(CU.FieldRef(CU.TypeRef(td.Name), "UnicodeCategories"), CU.Literal((int)UnicodeCategory.DecimalDigitNumber))));
				td.Members.Add(CU.Field(typeof(int[]), "graph", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(graph))));
				td.Members.Add(CU.Field(typeof(int[]), "ascii", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(asciiRanges)));
				td.Members.Add(CU.Field(typeof(int[]), "blank", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(blank))));
				td.Members.Add(CU.Field(typeof(int[]), "lower", MemberAttributes.Public | MemberAttributes.Static, CU.ArrIndexer(CU.FieldRef(CU.TypeRef(td.Name), "UnicodeCategories"), CU.Literal((int)UnicodeCategory.LowercaseLetter))));
				td.Members.Add(CU.Field(typeof(int[]), "print", MemberAttributes.Public | MemberAttributes.Static, CU.ArrIndexer(CU.FieldRef(CU.TypeRef(td.Name), "NotUnicodeCategories"), CU.Literal((int)UnicodeCategory.Control))));
				td.Members.Add(CU.Field(typeof(int[]), "punct", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(punct))));
				td.Members.Add(CU.Field(typeof(int[]), "space", MemberAttributes.Public | MemberAttributes.Static, CU.FieldRef(CU.TypeRef(td.Name), "IsWhiteSpace")));
				td.Members.Add(CU.Field(typeof(int[]), "upper", MemberAttributes.Public | MemberAttributes.Static, CU.ArrIndexer(CU.FieldRef(CU.TypeRef(td.Name), "UnicodeCategories"), CU.Literal((int)UnicodeCategory.UppercaseLetter))));
				td.Members.Add(CU.Field(typeof(int[]), "word", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(word))));
				td.Members.Add(CU.Field(typeof(int[]), "xdigit", MemberAttributes.Public | MemberAttributes.Static, CU.Literal(_GetRanges(xdigit))));
				sw.Write(CU.ToString(ccu));	
			}
		}
		static int[] _GetRanges(IEnumerable<int> chars)
		{
			var result = new List<int>();
			int first = '\0';
			int last = '\0';
			using (IEnumerator<int> e = chars.GetEnumerator())
			{
				bool moved = e.MoveNext();
				while (moved)
				{
					first = last = e.Current;
					while ((moved = e.MoveNext()) && (e.Current == last || e.Current == last + 1))
					{
						last = e.Current;
					}
					result.Add(first);
					result.Add(last);
				}
			}
			return result.ToArray();
		}
	}
}
