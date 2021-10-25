// Portions of this code adopted from Fare, itself adopted from brics
// original copyright notice included.
// This is the only file this applies to.

/*
 * dk.brics.automaton
 * 
 * Copyright (c) 2001-2011 Anders Moeller
 * All rights reserved.
 * http://github.com/moodmosaic/Fare/
 * Original Java code:
 * http://www.brics.dk/automaton/
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Text;
using LC;
namespace F
{
#if FFALIB
	public
#endif
	partial struct FFATransition
	{
		public int Min;
		public int Max;
		public FFA To;
		public FFATransition(int min, int max, FFA to)
		{
			Min = min;
			Max = max;
			To = to;
		}

	}
#if FFALIB
	public
#endif
	partial class FFA
	{
		public bool IsDeterministic = true;
		public bool IsAccepting = false;
		public int AcceptSymbol = -1;
		public int Tag = 0;
		public readonly IList<FFATransition> Transitions = new List<FFATransition>();
		public FFA(bool isAccepting, int acceptSymbol)
		{
			IsAccepting = isAccepting;
			AcceptSymbol = acceptSymbol;
		}
		public FFA() { }
		public bool IsFinal { get { return 0 == Transitions.Count; } }
		public void AddEpsilon(FFA to)
		{
			if (to.IsAccepting && !IsAccepting)
			{
				IsAccepting = true;
				AcceptSymbol = to.AcceptSymbol;
			}
			for (int ic = to.Transitions.Count, i = 0; i < ic; ++i)
			{
				Transitions.Add(to.Transitions[i]);
			}
			IsDeterministic = false;
		}
		public IList<FFA> FillClosure(IList<FFA> result = null)
		{
			if (null == result)
				result = new List<FFA>();
			if (result.Contains(this))
				return result;
			result.Add(this);
			for (int ic = Transitions.Count, i = 0; i < ic; ++i)
			{
				var t = Transitions[i];
				t.To.FillClosure(result);
			}
			return result;
		}



		public IList<FFA> FillAcceptingStates(IList<FFA> result = null)
		{
			return FillAcceptingStates(FillClosure(), result);
		}
		public static IList<FFA> FillAcceptingStates(IList<FFA> closure, IList<FFA> result = null)
		{
			if (null == result)
				result = new List<FFA>();
			for (int ic = closure.Count, i = 0; i < ic; ++i)
			{
				var fa = closure[i];
				if (fa.IsAccepting)
					result.Add(fa);
			}
			return result;
		}
		public IDictionary<FFA, int[]> FillInputTransitionRangesGroupedByState(IDictionary<FFA, int[]> result = null)
		{
			var working = new Dictionary<FFA, List<KeyValuePair<int, int>>>();
			foreach (var trns in Transitions)
			{
				List<KeyValuePair<int, int>> l;
				if (!working.TryGetValue(trns.To, out l))
				{
					l = new List<KeyValuePair<int, int>>();
					working.Add(trns.To, l);
				}
				l.Add(new KeyValuePair<int, int>(trns.Min, trns.Max));
			}
			if (null == result)
				result = new Dictionary<FFA, int[]>();
			foreach (var item in working)
			{
				item.Value.Sort((x, y) => { var c = x.Key.CompareTo(y.Key); if (0 != c) return c; return x.Value.CompareTo(y.Value); });
				_NormalizeSortedRangeList(item.Value);
				result.Add(item.Key, _FromPairs(item.Value));
			}
			return result;
		}
		static void _NormalizeSortedRangeList(IList<KeyValuePair<int, int>> pairs)
		{

			var or = default(KeyValuePair<int, int>);
			for (int i = 1; i < pairs.Count; ++i)
			{
				if (pairs[i - 1].Value + 1 >= pairs[i].Key)
				{
					var nr = new KeyValuePair<int, int>(pairs[i - 1].Key, pairs[i].Value);
					pairs[i - 1] = or = nr;
					pairs.RemoveAt(i);
					--i; // compensated for by ++i in for loop
				}
			}
		}
		public FFA Clone() { return Clone(FillClosure()); }
		public static FFA Clone(IList<FFA> closure)
		{
			var nclosure = new FFA[closure.Count];
			for (var i = 0; i < nclosure.Length; i++)
			{
				var fa = closure[i];
				var nfa = new FFA();
				nfa.IsAccepting = fa.IsAccepting;
				nfa.AcceptSymbol = fa.AcceptSymbol;
				nfa.IsDeterministic = fa.IsDeterministic;
				nclosure[i] = nfa;
			}
			for (var i = 0; i < nclosure.Length; i++)
			{
				var fa = closure[i];
				var nfa = nclosure[i];
				for (int jc = fa.Transitions.Count, j = 0; j < jc; ++j)
				{
					var t = fa.Transitions[j];
					nfa.Transitions.Add(new FFATransition(t.Min, t.Max, nclosure[closure.IndexOf(t.To)]));
				}
			}
			return nclosure[0];
		}
		public static FFA Literal(IEnumerable<int> @string, int accept = -1)
		{
			var result = new FFA();
			var current = result;
			foreach (var ch in @string)
			{
				current.IsAccepting = false;
				var fa = new FFA();
				fa.IsAccepting = true;
				fa.AcceptSymbol = accept;
				current.Transitions.Add(new FFATransition(ch, ch, fa));
				current = fa;
			}
			return result;
		}
		public static FFA Set(IEnumerable<KeyValuePair<int, int>> ranges, int accept = -1)
		{
			var result = new FFA();
			var final = new FFA(true, accept);
			var pairs = new List<KeyValuePair<int, int>>(ranges);
			pairs.Sort((x, y) => { var c = x.Key.CompareTo(y.Key); if (0 != c) return c; return x.Value.CompareTo(y.Value); });
			foreach (var pair in pairs)
				result.Transitions.Add(new FFATransition(pair.Key, pair.Value, final));

			return result;
		}

		public static FFA Concat(IEnumerable<FFA> exprs, int accept = -1)
		{
			FFA result = null, left = null, right = null;
			foreach (var val in exprs)
			{
				if (null == val) continue;
				//Debug.Assert(null != val.FirstAcceptingState);
				var nval = val.Clone();
				//Debug.Assert(null != nval.FirstAcceptingState);
				if (null == left)
				{
					if (null == result)
						result = nval;
					left = nval;
					//Debug.Assert(null != left.FirstAcceptingState);
					continue;
				}
				if (null == right)
				{
					right = nval;
				}

				//Debug.Assert(null != left.FirstAcceptingState);
				nval = right.Clone();
				_Concat(left, nval);
				right = null;
				left = nval;

				//Debug.Assert(null != left.FirstAcceptingState);

			}
			if (null != right)
			{
				var acc = right.FillAcceptingStates();
				for (int ic = acc.Count, i = 0; i < ic; ++i)
					acc[i].AcceptSymbol = accept;
			}
			else
			{
				var acc = result.FillAcceptingStates();
				for (int ic = acc.Count, i = 0; i < ic; ++i)
					acc[i].AcceptSymbol = accept;
			}
			return result;
		}
		static void _Concat(FFA lhs, FFA rhs)
		{
			//Debug.Assert(lhs != rhs);
			var acc = lhs.FillAcceptingStates();
			for (int ic = acc.Count, i = 0; i < ic; ++i)
			{
				var f = acc[i];
				//Debug.Assert(null != rhs.FirstAcceptingState);
				f.IsAccepting = false;
				f.AddEpsilon(rhs);
				//Debug.Assert(null!= lhs.FirstAcceptingState);
			}
		}
		public static FFA Or(IEnumerable<FFA> exprs, int accept = -1)
		{
			var result = new FFA();
			var final = new FFA(true, accept);
			foreach (var fa in exprs)
			{
				if (null != fa)
				{
					var nfa = fa.Clone();
					result.AddEpsilon(nfa);
					var acc = nfa.FillAcceptingStates();
					for (int ic = acc.Count, i = 0; i < ic; ++i)
					{
						var nffa = acc[i];
						nffa.IsAccepting = false;
						nffa.AddEpsilon(final);
					}
				}
				else result.AddEpsilon(final);
			}
			return result;
		}
		public static FFA Optional(FFA expr, int accept = -1)
		{
			var result = expr.Clone();
			var acc = result.FillAcceptingStates();
			for (int ic = acc.Count, i = 0; i < ic; ++i)
			{
				var fa = acc[i];
				fa.IsAccepting = true;
				fa.AcceptSymbol = accept;
				result.AddEpsilon(fa);
			}
			return result;
		}
		public static FFA Repeat(FFA expr, int minOccurs = -1, int maxOccurs = -1, int accept = -1)
		{
			expr = expr.Clone();
			if (minOccurs > 0 && maxOccurs > 0 && minOccurs > maxOccurs)
				throw new ArgumentOutOfRangeException(nameof(maxOccurs));
			FFA result;
			switch (minOccurs)
			{
				case -1:
				case 0:
					switch (maxOccurs)
					{
						case -1:
						case 0:
							result = new FFA(true, accept);
							result.AddEpsilon(expr);
							foreach (var afa in expr.FillAcceptingStates())
							{
								afa.AddEpsilon(result);
							}
							return result;
						case 1:
							result = Optional(expr, accept);
							//Debug.Assert(null != result.FirstAcceptingState);
							return result;
						default:
							var l = new List<FFA>();
							expr = Optional(expr);
							l.Add(expr);
							for (int i = 1; i < maxOccurs; ++i)
							{
								l.Add(expr.Clone());
							}
							result = Concat(l, accept);
							//Debug.Assert(null != result.FirstAcceptingState);
							return result;
					}
				case 1:
					switch (maxOccurs)
					{
						case -1:
						case 0:
							result = Concat(new FFA[] { expr, Repeat(expr, 0, 0, accept) }, accept);
							//Debug.Assert(null != result.FirstAcceptingState);
							return result;
						case 1:
							//Debug.Assert(null != expr.FirstAcceptingState);
							return expr;
						default:
							result = Concat(new FFA[] { expr, Repeat(expr.Clone(), 0, maxOccurs - 1) }, accept);
							//Debug.Assert(null != result.FirstAcceptingState);
							return result;
					}
				default:
					switch (maxOccurs)
					{
						case -1:
						case 0:
							result = Concat(new FFA[] { Repeat(expr, minOccurs, minOccurs, accept), Repeat(expr, 0, 0, accept) }, accept);
							//Debug.Assert(null != result.FirstAcceptingState);
							return result;
						case 1:
							throw new ArgumentOutOfRangeException(nameof(maxOccurs));
						default:
							if (minOccurs == maxOccurs)
							{
								var l = new List<FFA>();
								l.Add(expr);
								//Debug.Assert(null != expr.FirstAcceptingState);
								for (int i = 1; i < minOccurs; ++i)
								{
									var e = expr.Clone();
									//Debug.Assert(null != e.FirstAcceptingState);
									l.Add(e);
								}
								result = Concat(l, accept);
								//Debug.Assert(null != result.FirstAcceptingState);
								return result;
							}
							result = Concat(new FFA[] { Repeat(expr.Clone(), minOccurs, minOccurs, accept), Repeat(Optional(expr.Clone()), maxOccurs - minOccurs, maxOccurs - minOccurs, accept) }, accept);
							//Debug.Assert(null != result.FirstAcceptingState);
							return result;
					}
			}
			// should never get here
			throw new NotImplementedException();
		}
		public static FFA CaseInsensitive(FFA expr, int accept = -1)
		{
			var result = expr.Clone();
			var closure = new List<FFA>();
			result.FillClosure(closure);
			for (int ic = closure.Count, i = 0; i < ic; ++i)
			{
				var fa = closure[i];
				var t = new List<FFATransition>(fa.Transitions);
				fa.Transitions.Clear();
				foreach (var trns in t)
				{
					var f = char.ConvertFromUtf32(trns.Min);
					var l = char.ConvertFromUtf32(trns.Max);
					if (char.IsLower(f, 0))
					{
						if (!char.IsLower(l, 0))
							throw new NotSupportedException("Attempt to make an invalid range case insensitive");
						fa.Transitions.Add(new FFATransition(trns.Min, trns.Max, trns.To));
						f = f.ToUpperInvariant();
						l = l.ToUpperInvariant();
						fa.Transitions.Add(new FFATransition(char.ConvertToUtf32(f, 0), char.ConvertToUtf32(l, 0), trns.To));

					}
					else if (char.IsUpper(f, 0))
					{
						if (!char.IsUpper(l, 0))
							throw new NotSupportedException("Attempt to make an invalid range case insensitive");
						fa.Transitions.Add(new FFATransition(trns.Min, trns.Max, trns.To));
						f = f.ToLowerInvariant();
						l = l.ToLowerInvariant();
						fa.Transitions.Add(new FFATransition(char.ConvertToUtf32(f, 0), char.ConvertToUtf32(l, 0), trns.To));
					}
					else
					{
						fa.Transitions.Add(new FFATransition(trns.Min, trns.Max, trns.To));
					}
				}
			}
			return result;
		}

		public static FFA Parse(IEnumerable<char> input, int accept = -1, int line = 1, int column = 1, long position = 0, string fileOrUrl = null)
		{
			var lc = LexContext.Create(input);
			lc.EnsureStarted();
			lc.SetLocation(line, column, position, fileOrUrl);
			var result = Parse(lc, accept);
			return result;
		}
		internal static FFA Parse(LexContext pc, int accept = -1)
		{

			FFA result = null, next = null;
			int ich;
			pc.EnsureStarted();
			while (true)
			{
				switch (pc.Current)
				{
					case -1:
						result = result.ToMinimized();
						return result;
					case '.':
						var dot = FFA.Set(new KeyValuePair<int, int>[] { new KeyValuePair<int, int>(0, 0x10ffff) }, accept);
						if (null == result)
							result = dot;
						else
						{
							result = FFA.Concat(new FFA[] { result, dot }, accept);
						}
						pc.Advance();
						result = _ParseModifier(result, pc, accept);
						break;
					case '\\':

						pc.Advance();
						pc.Expecting();
						var isNot = false;
						switch (pc.Current)
						{
							case 'P':
								isNot = true;
								goto case 'p';
							case 'p':
								pc.Advance();
								pc.Expecting('{');
								var uc = new StringBuilder();
								int uli = pc.Line;
								int uco = pc.Column;
								long upo = pc.Position;
								while (-1 != pc.Advance() && '}' != pc.Current)
									uc.Append((char)pc.Current);
								pc.Expecting('}');
								pc.Advance();
								int uci = 0;
								switch (uc.ToString())
								{
									case "Pe":
										uci = 21;
										break;
									case "Pc":
										uci = 19;
										break;
									case "Cc":
										uci = 14;
										break;
									case "Sc":
										uci = 26;
										break;
									case "Pd":
										uci = 19;
										break;
									case "Nd":
										uci = 8;
										break;
									case "Me":
										uci = 7;
										break;
									case "Pf":
										uci = 23;
										break;
									case "Cf":
										uci = 15;
										break;
									case "Pi":
										uci = 22;
										break;
									case "Nl":
										uci = 9;
										break;
									case "Zl":
										uci = 12;
										break;
									case "Ll":
										uci = 1;
										break;
									case "Sm":
										uci = 25;
										break;
									case "Lm":
										uci = 3;
										break;
									case "Sk":
										uci = 27;
										break;
									case "Mn":
										uci = 5;
										break;
									case "Ps":
										uci = 20;
										break;
									case "Lo":
										uci = 4;
										break;
									case "Cn":
										uci = 29;
										break;
									case "No":
										uci = 10;
										break;
									case "Po":
										uci = 24;
										break;
									case "So":
										uci = 28;
										break;
									case "Zp":
										uci = 13;
										break;
									case "Co":
										uci = 17;
										break;
									case "Zs":
										uci = 11;
										break;
									case "Mc":
										uci = 6;
										break;
									case "Cs":
										uci = 16;
										break;
									case "Lt":
										uci = 2;
										break;
									case "Lu":
										uci = 0;
										break;
								}
								if (isNot)
								{
									next = FFA.Set(_ToPairs(CharacterClasses.UnicodeCategories[uci]), accept);
								}
								else
									next = FFA.Set(_ToPairs(CharacterClasses.NotUnicodeCategories[uci]), accept);
								break;
							case 'd':
								next = FFA.Set(_ToPairs(CharacterClasses.digit), accept);
								pc.Advance();
								break;
							case 'D':
								next = FFA.Set(_NotRanges(CharacterClasses.digit), accept);
								pc.Advance();
								break;

							case 's':
								next = FFA.Set(_ToPairs(CharacterClasses.space), accept);
								pc.Advance();
								break;
							case 'S':
								next = FFA.Set(_NotRanges(CharacterClasses.space), accept);
								pc.Advance();
								break;
							case 'w':
								next = FFA.Set(_ToPairs(CharacterClasses.word), accept);
								pc.Advance();
								break;
							case 'W':
								next = FFA.Set(_NotRanges(CharacterClasses.word), accept);
								pc.Advance();
								break;
							default:
								if (-1 != (ich = _ParseEscapePart(pc)))
								{
									next = FFA.Literal(new int[] { ich }, accept);

								}
								else
								{
									pc.Expecting(); // throw an error
									return null; // doesn't execute
								}
								break;
						}
						next = _ParseModifier(next, pc, accept);
						if (null != result)
						{
							result = FFA.Concat(new FFA[] { result, next }, accept);
						}
						else
							result = next;
						break;
					case ')':
						result = result.ToMinimized();
						return result;
					case '(':
						pc.Advance();
						pc.Expecting();
						next = Parse(pc, accept);
						pc.Expecting(')');
						pc.Advance();
						next = _ParseModifier(next, pc, accept);
						if (null == result)
							result = next;
						else
						{
							result = FFA.Concat(new FFA[] { result, next }, accept);
						}
						break;
					case '|':
						if (-1 != pc.Advance())
						{
							next = Parse(pc, accept);
							result = FFA.Or(new FFA[] { result, next }, accept);
						}
						else
						{
							result = FFA.Optional(result, accept);
						}
						break;
					case '[':
						var seti = _ParseSet(pc);
						IEnumerable<KeyValuePair<int, int>> set;
						if (seti.Key)
							set = _NotRanges(seti.Value);
						else
							set = _ToPairs(seti.Value);
						next = FFA.Set(set, accept);
						next = _ParseModifier(next, pc, accept);

						if (null == result)
							result = next;
						else
						{
							result = FFA.Concat(new FFA[] { result, next }, accept);

						}
						break;
					default:
						ich = pc.Current;
						if (ich == '\"') System.Diagnostics.Debugger.Break();
						if (char.IsHighSurrogate((char)ich))
						{
							if (-1 == pc.Advance())
								throw new ExpectingException("Expecting low surrogate in Unicode stream", pc.Line, pc.Column, pc.Position, pc.FileOrUrl, "low-surrogate");
							ich = char.ConvertToUtf32((char)ich, (char)pc.Current);
						}
						next = FFA.Literal(new int[] { ich }, accept);
						pc.Advance();
						next = _ParseModifier(next, pc, accept);
						if (null == result)
							result = next;
						else
						{
							result = FFA.Concat(new FFA[] { result, next }, accept);
						}
						break;
				}
			}
		}

		static KeyValuePair<bool, int[]> _ParseSet(LexContext pc)
		{
			var result = new List<int>();
			pc.EnsureStarted();
			pc.Expecting('[');
			pc.Advance();
			pc.Expecting();
			var isNot = false;
			if ('^' == pc.Current)
			{
				isNot = true;
				pc.Advance();
				pc.Expecting();
			}
			var firstRead = true;
			int firstChar = '\0';
			var readFirstChar = false;
			var wantRange = false;
			while (-1 != pc.Current && (firstRead || ']' != pc.Current))
			{
				if (!wantRange)
				{
					// can be a single char,
					// a range
					// or a named character class
					if ('[' == pc.Current) // named char class
					{
						pc.Advance();
						pc.Expecting();
						if (':' != pc.Current)
						{
							firstChar = '[';
							readFirstChar = true;
						}
						else
						{
							pc.Advance();
							pc.Expecting();
							var ll = pc.CaptureBuffer.Length;
							if (!pc.TryReadUntil(':', false))
								throw new ExpectingException("Expecting character class", pc.Line, pc.Column, pc.Position, pc.FileOrUrl);
							pc.Expecting(':');
							pc.Advance();
							pc.Expecting(']');
							pc.Advance();
							var cls = pc.GetCapture(ll);
							int[] ranges;
							if (!CharacterClasses.Known.TryGetValue(cls, out ranges))
								throw new ExpectingException("Unknown character class \"" + cls + "\" specified", pc.Line, pc.Column, pc.Position, pc.FileOrUrl);
							result.AddRange(ranges);
							readFirstChar = false;
							wantRange = false;
							firstRead = false;
							continue;
						}
					}
					if (!readFirstChar)
					{
						if (char.IsHighSurrogate((char)pc.Current))
						{
							var chh = (char)pc.Current;
							pc.Advance();
							pc.Expecting();
							firstChar = char.ConvertToUtf32(chh, (char)pc.Current);
							pc.Advance();
							pc.Expecting();
						}
						else if ('\\' == pc.Current)
						{
							pc.Advance();
							firstChar = _ParseRangeEscapePart(pc);
						}
						else
						{
							firstChar = pc.Current;
							pc.Advance();
							pc.Expecting();
						}
						readFirstChar = true;

					}
					else
					{
						if ('-' == pc.Current)
						{
							pc.Advance();
							pc.Expecting();
							wantRange = true;
						}
						else
						{
							result.Add(firstChar);
							result.Add(firstChar);
							readFirstChar = false;
						}
					}
					firstRead = false;
				}
				else
				{
					if ('\\' != pc.Current)
					{
						var ch = 0;
						if (char.IsHighSurrogate((char)pc.Current))
						{
							var chh = (char)pc.Current;
							pc.Advance();
							pc.Expecting();
							ch = char.ConvertToUtf32(chh, (char)pc.Current);
						}
						else
							ch = (char)pc.Current;
						pc.Advance();
						pc.Expecting();
						result.Add(firstChar);
						result.Add(ch);
					}
					else
					{
						result.Add(firstChar);
						pc.Advance();
						result.Add(_ParseRangeEscapePart(pc));
					}
					wantRange = false;
					readFirstChar = false;
				}

			}
			if (readFirstChar)
			{
				result.Add(firstChar);
				result.Add(firstChar);
				if (wantRange)
				{
					result.Add('-');
					result.Add('-');
				}
			}
			pc.Expecting(']');
			pc.Advance();
			return new KeyValuePair<bool, int[]>(isNot, result.ToArray());
		}
		/*static int[] _ParseRanges(LexContext pc)
		{
			pc.EnsureStarted();
			var result = new List<int>();
			int[] next = null;
			bool readDash = false;
			while (-1 != pc.Current && ']' != pc.Current)
			{
				switch (pc.Current)
				{
					case '[': // char class 
						if (null != next)
						{
							result.Add(next[0]);
							result.Add(next[1]);
							if (readDash)
							{
								result.Add('-');
								result.Add('-');
							}
						}
						pc.Advance();
						pc.Expecting(':');
						pc.Advance();
						var l = pc.CaptureBuffer.Length;
						var lin = pc.Line;
						var col = pc.Column;
						var pos = pc.Position;
						pc.TryReadUntil(':', false);
						var n = pc.GetCapture(l);
						pc.Advance();
						pc.Expecting(']');
						pc.Advance();
						int[] rngs;
						if (!CharacterClasses.Known.TryGetValue(n, out rngs))
						{
							var sa = new string[CharacterClasses.Known.Count];
							CharacterClasses.Known.Keys.CopyTo(sa, 0);
							throw new ExpectingException("Invalid character class " + n, lin, col, pos, pc.FileOrUrl, sa);
						}
						result.AddRange(rngs);
						readDash = false;
						next = null;
						break;
					case '\\':
						pc.Advance();
						pc.Expecting();
						switch (pc.Current)
						{
							case 'h':
								_ParseCharClassEscape(pc, "space", result, ref next, ref readDash);
								break;
							case 'd':
								_ParseCharClassEscape(pc, "digit", result, ref next, ref readDash);
								break;
							case 'D':
								_ParseCharClassEscape(pc, "^digit", result, ref next, ref readDash);
								break;
							case 'l':
								_ParseCharClassEscape(pc, "lower", result, ref next, ref readDash);
								break;
							case 's':
								_ParseCharClassEscape(pc, "space", result, ref next, ref readDash);
								break;
							case 'S':
								_ParseCharClassEscape(pc, "^space", result, ref next, ref readDash);
								break;
							case 'u':
								_ParseCharClassEscape(pc, "upper", result, ref next, ref readDash);
								break;
							case 'w':
								_ParseCharClassEscape(pc, "word", result, ref next, ref readDash);
								break;
							case 'W':
								_ParseCharClassEscape(pc, "^word", result, ref next, ref readDash);
								break;
							default:
								var ch = (char)_ParseRangeEscapePart(pc);
								if (null == next)
									next = new int[] { ch, ch };
								else if (readDash)
								{
									result.Add(next[0]);
									result.Add(ch);
									next = null;
									readDash = false;
								}
								else
								{
									result.AddRange(next);
									next = new int[] { ch, ch };
								}

								break;
						}

						break;
					case '-':
						pc.Advance();
						if (null == next)
						{
							next = new int[] { '-', '-' };
							readDash = false;
						}
						else
						{
							if (readDash)
								result.AddRange(next);

							readDash = true;
						}
						break;
					default:
						if (null == next)
						{
							next = new int[] { pc.Current, pc.Current };
						}
						else
						{
							if (readDash)
							{
								result.Add(next[0]);
								result.Add((char)pc.Current);
								next = null;
								readDash = false;
							}
							else
							{
								result.AddRange(next);
								next = new int[] { pc.Current, pc.Current };
							}
						}
						pc.Advance();
						break;
				}
			}
			if (null != next)
			{
				result.AddRange(next);
				if (readDash)
				{
					result.Add('-');
					result.Add('-');
				}
			}
			return result.ToArray();
		}

		static void _ParseCharClassEscape(LexContext pc, string cls, List<int> result, ref int[] next, ref bool readDash)
		{
			if (null != next)
			{
				result.AddRange(next);
				if (readDash)
				{
					result.Add('-');
					result.Add('-');
				}
				result.Add('-');
				result.Add('-');
			}
			pc.Advance();
			int[] rngs;
			if (!CharacterClasses.Known.TryGetValue(cls, out rngs))
			{
				var sa = new string[CharacterClasses.Known.Count];
				CharacterClasses.Known.Keys.CopyTo(sa, 0);
				throw new ExpectingException("Invalid character class " + cls, pc.Line, pc.Column, pc.Position, pc.FileOrUrl, sa);
			}
			result.AddRange(rngs);
			next = null;
			readDash = false;
		}
		*/
		static FFA _ParseModifier(FFA expr, LexContext pc, int accept)
		{
			var line = pc.Line;
			var column = pc.Column;
			var position = pc.Position;
			switch (pc.Current)
			{
				case '*':
					expr = Repeat(expr, 0, 0, accept);
					pc.Advance();
					break;
				case '+':
					expr = Repeat(expr, 1, 0, accept);
					pc.Advance();
					break;
				case '?':
					expr = Optional(expr, accept);
					pc.Advance();
					break;
				case '{':
					pc.Advance();
					pc.TrySkipWhiteSpace();
					pc.Expecting('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '}');
					var min = -1;
					var max = -1;
					if (',' != pc.Current && '}' != pc.Current)
					{
						var l = pc.CaptureBuffer.Length;
						pc.TryReadDigits();
						min = int.Parse(pc.GetCapture(l));
						pc.TrySkipWhiteSpace();
					}
					if (',' == pc.Current)
					{
						pc.Advance();
						pc.TrySkipWhiteSpace();
						pc.Expecting('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '}');
						if ('}' != pc.Current)
						{
							var l = pc.CaptureBuffer.Length;
							pc.TryReadDigits();
							max = int.Parse(pc.GetCapture(l));
							pc.TrySkipWhiteSpace();
						}
					}
					else { max = min; }
					pc.Expecting('}');
					pc.Advance();
					expr = Repeat(expr, min, max, accept);
					break;
			}
			return expr;
		}
		static byte _FromHexChar(char hex)
		{
			if (':' > hex && '/' < hex)
				return (byte)(hex - '0');
			if ('G' > hex && '@' < hex)
				return (byte)(hex - '7'); // 'A'-10
			if ('g' > hex && '`' < hex)
				return (byte)(hex - 'W'); // 'a'-10
			throw new ArgumentException("The value was not hex.", "hex");
		}
		static bool _IsHexChar(char hex)
		{
			if (':' > hex && '/' < hex)
				return true;
			if ('G' > hex && '@' < hex)
				return true;
			if ('g' > hex && '`' < hex)
				return true;
			return false;
		}
		// return type is either char or ranges. this is kind of a union return type.
		static int _ParseEscapePart(LexContext pc)
		{
			if (-1 == pc.Current) return -1;
			switch (pc.Current)
			{
				case 'f':
					pc.Advance();
					return '\f';
				case 'v':
					pc.Advance();
					return '\v';
				case 't':
					pc.Advance();
					return '\t';
				case 'n':
					pc.Advance();
					return '\n';
				case 'r':
					pc.Advance();
					return '\r';
				case 'x':
					if (-1 == pc.Advance() || !_IsHexChar((char)pc.Current))
						return 'x';
					byte b = _FromHexChar((char)pc.Current);
					if (-1 == pc.Advance() || !_IsHexChar((char)pc.Current))
						return unchecked((char)b);
					b <<= 4;
					b |= _FromHexChar((char)pc.Current);
					if (-1 == pc.Advance() || !_IsHexChar((char)pc.Current))
						return unchecked((char)b);
					b <<= 4;
					b |= _FromHexChar((char)pc.Current);
					if (-1 == pc.Advance() || !_IsHexChar((char)pc.Current))
						return unchecked((char)b);
					b <<= 4;
					b |= _FromHexChar((char)pc.Current);
					return unchecked((char)b);
				case 'u':
					if (-1 == pc.Advance())
						return 'u';
					ushort u = _FromHexChar((char)pc.Current);
					u <<= 4;
					if (-1 == pc.Advance())
						return unchecked((char)u);
					u |= _FromHexChar((char)pc.Current);
					u <<= 4;
					if (-1 == pc.Advance())
						return unchecked((char)u);
					u |= _FromHexChar((char)pc.Current);
					u <<= 4;
					if (-1 == pc.Advance())
						return unchecked((char)u);
					u |= _FromHexChar((char)pc.Current);
					return unchecked((char)u);
				default:
					int i = pc.Current;
					pc.Advance();
					if (char.IsHighSurrogate((char)i))
					{
						i = char.ConvertToUtf32((char)i, (char)pc.Current);
						pc.Advance();
					}
					return (char)i;
			}
		}
		static int _ParseRangeEscapePart(LexContext pc)
		{
			if (-1 == pc.Current)
				return -1;
			switch (pc.Current)
			{
				case 'f':
					pc.Advance();
					return '\f';
				case 'v':
					pc.Advance();
					return '\v';
				case 't':
					pc.Advance();
					return '\t';
				case 'n':
					pc.Advance();
					return '\n';
				case 'r':
					pc.Advance();
					return '\r';
				case 'x':
					if (-1 == pc.Advance() || !_IsHexChar((char)pc.Current))
						return 'x';
					byte b = _FromHexChar((char)pc.Current);
					if (-1 == pc.Advance() || !_IsHexChar((char)pc.Current))
						return unchecked((char)b);
					b <<= 4;
					b |= _FromHexChar((char)pc.Current);
					if (-1 == pc.Advance() || !_IsHexChar((char)pc.Current))
						return unchecked((char)b);
					b <<= 4;
					b |= _FromHexChar((char)pc.Current);
					if (-1 == pc.Advance() || !_IsHexChar((char)pc.Current))
						return unchecked((char)b);
					b <<= 4;
					b |= _FromHexChar((char)pc.Current);
					return unchecked((char)b);
				case 'u':
					if (-1 == pc.Advance())
						return 'u';
					ushort u = _FromHexChar((char)pc.Current);
					u <<= 4;
					if (-1 == pc.Advance())
						return unchecked((char)u);
					u |= _FromHexChar((char)pc.Current);
					u <<= 4;
					if (-1 == pc.Advance())
						return unchecked((char)u);
					u |= _FromHexChar((char)pc.Current);
					u <<= 4;
					if (-1 == pc.Advance())
						return unchecked((char)u);
					u |= _FromHexChar((char)pc.Current);
					return unchecked((char)u);
				default:
					int i = pc.Current;
					pc.Advance();
					if (char.IsHighSurrogate((char)i))
					{
						i = char.ConvertToUtf32((char)i, (char)pc.Current);
						pc.Advance();
					}
					return (char)i;
			}
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
		static int[] _FromPairs(IList<KeyValuePair<int, int>> pairs)
		{
			var result = new int[pairs.Count * 2];
			for (int ic = pairs.Count, i = 0; i < ic; ++i)
			{
				var pair = pairs[i];
				var j = i * 2;
				result[j] = pair.Key;
				result[j + 1] = pair.Value;
			}
			return result;
		}
		static IList<KeyValuePair<int, int>> _NotRanges(int[] ranges)
		{
			return new List<KeyValuePair<int, int>>(_NotRanges(_ToPairs(ranges)));
		}
		static IEnumerable<KeyValuePair<int, int>> _NotRanges(IEnumerable<KeyValuePair<int, int>> ranges)
		{
			// expects ranges to be normalized
			var last = 0x10ffff;
			using (var e = ranges.GetEnumerator())
			{
				if (!e.MoveNext())
				{
					yield return new KeyValuePair<int, int>(0x0, 0x10ffff);
					yield break;
				}
				if (e.Current.Key > 0)
				{
					yield return new KeyValuePair<int, int>(0, unchecked(e.Current.Key - 1));
					last = e.Current.Value;
					if (0x10ffff <= last)
						yield break;
				}
				while (e.MoveNext())
				{
					if (0x10ffff <= last)
						yield break;
					if (unchecked(last + 1) < e.Current.Key)
						yield return new KeyValuePair<int, int>(unchecked(last + 1), unchecked((e.Current.Key - 1)));
					last = e.Current.Value;
				}
				if (0x10ffff > last)
					yield return new KeyValuePair<int, int>(unchecked((last + 1)), 0x10ffff);

			}

		}
		public FFA ToDfa()
		{
			return _Determinize(this);
		}
		public FFA ToMinimized()
		{
			return _Minimize(this);
		}
		public void Totalize()
		{
			Totalize(FillClosure());
		}
		public static void Totalize(IList<FFA> closure)
		{
			var s = new FFA();
			s.Transitions.Add(new FFATransition(0, 0x10ffff, s));
			foreach (FFA p in closure)
			{
				int maxi = 0;
				var sortedTrans = new List<FFATransition>(p.Transitions);
				sortedTrans.Sort((x, y) => { var c = x.Min.CompareTo(y.Min); if (0 != c) return c; return x.Max.CompareTo(y.Max); });
				foreach (var t in sortedTrans)
				{
					if (t.Min > maxi)
					{
						p.Transitions.Add(new FFATransition(maxi, (t.Min - 1), s));
					}

					if (t.Max + 1 > maxi)
					{
						maxi = t.Max + 1;
					}
				}

				if (maxi <= 0x10ffff)
				{
					p.Transitions.Add(new FFATransition(maxi, 0x10ffff, s));
				}
			}
		}

		static FFA _Minimize(FFA a)
		{
			a = a.ToDfa();
			var tr = a.Transitions;
			if (1 == tr.Count)
			{
				FFATransition t = tr[0];
				if (t.To == a && t.Min == 0 && t.Max == 0x10ffff)
				{
					return a;
				}
			}

			a.Totalize();

			// Make arrays for numbered states and effective alphabet.
			var cl = a.FillClosure();
			var states = new FFA[cl.Count];
			int number = 0;
			foreach (var q in cl)
			{
				states[number] = q;
				q.Tag = number;
				++number;
			}

			var pp = new List<int>();
			for (int ic = cl.Count, i = 0; i < ic; ++i)
			{
				var ffa = cl[i];
				pp.Add(0);
				foreach (var t in ffa.Transitions)
				{
					pp.Add(t.Min);
					if (t.Max < 0x10ffff)
					{
						pp.Add((t.Max + 1));
					}
				}
			}

			var sigma = new int[pp.Count];
			pp.CopyTo(sigma, 0);
			Array.Sort(sigma);

			// Initialize data structures.
			var reverse = new List<List<Queue<FFA>>>();
			foreach (var s in states)
			{
				var v = new List<Queue<FFA>>();
				_Init(v, sigma.Length);
				reverse.Add(v);
			}

			var reverseNonempty = new bool[states.Length, sigma.Length];

			var partition = new List<LinkedList<FFA>>();
			_Init(partition, states.Length);

			var block = new int[states.Length];
			var active = new _FList[states.Length, sigma.Length];
			var active2 = new _FListNode[states.Length, sigma.Length];
			var pending = new Queue<_IntPair>();
			var pending2 = new bool[sigma.Length, states.Length];
			var split = new List<FFA>();
			var split2 = new bool[states.Length];
			var refine = new List<int>();
			var refine2 = new bool[states.Length];

			var splitblock = new List<List<FFA>>();
			_Init(splitblock, states.Length);

			for (int q = 0; q < states.Length; q++)
			{
				splitblock[q] = new List<FFA>();
				partition[q] = new LinkedList<FFA>();
				for (int x = 0; x < sigma.Length; x++)
				{
					reverse[q][x] = new Queue<FFA>();
					active[q, x] = new _FList();
				}
			}

			// Find initial partition and reverse edges.
			foreach (var qq in states)
			{
				int j = qq.IsAccepting ? 0 : 1;

				partition[j].AddLast(qq);
				block[qq.Tag] = j;
				for (int x = 0; x < sigma.Length; x++)
				{
					var y = sigma[x];
					var p = qq._Step(y);
					var pn = p.Tag;
					reverse[pn][x].Enqueue(qq);
					reverseNonempty[pn, x] = true;
				}
			}

			// Initialize active sets.
			for (int j = 0; j <= 1; j++)
			{
				for (int x = 0; x < sigma.Length; x++)
				{
					foreach (var qq in partition[j])
					{
						if (reverseNonempty[qq.Tag, x])
						{
							active2[qq.Tag, x] = active[j, x].Add(qq);
						}
					}
				}
			}

			// Initialize pending.
			for (int x = 0; x < sigma.Length; x++)
			{
				int a0 = active[0, x].Count;
				int a1 = active[1, x].Count;
				int j = a0 <= a1 ? 0 : 1;
				pending.Enqueue(new _IntPair(j, x));
				pending2[x, j] = true;
			}

			// Process pending until fixed point.
			int k = 2;
			while (pending.Count > 0)
			{
				_IntPair ip = pending.Dequeue();
				int p = ip.N1;
				int x = ip.N2;
				pending2[x, p] = false;

				// Find states that need to be split off their blocks.
				for (var m = active[p, x].First; m != null; m = m.Next)
				{
					foreach (var s in reverse[m.State.Tag][x])
					{
						if (!split2[s.Tag])
						{
							split2[s.Tag] = true;
							split.Add(s);
							int j = block[s.Tag];
							splitblock[j].Add(s);
							if (!refine2[j])
							{
								refine2[j] = true;
								refine.Add(j);
							}
						}
					}
				}

				// Refine blocks.
				foreach (int j in refine)
				{
					if (splitblock[j].Count < partition[j].Count)
					{
						LinkedList<FFA> b1 = partition[j];
						LinkedList<FFA> b2 = partition[k];
						foreach (var s in splitblock[j])
						{
							b1.Remove(s);
							b2.AddLast(s);
							block[s.Tag] = k;
							for (int c = 0; c < sigma.Length; c++)
							{
								_FListNode sn = active2[s.Tag, c];
								if (sn != null && sn.StateList == active[j, c])
								{
									sn.Remove();
									active2[s.Tag, c] = active[k, c].Add(s);
								}
							}
						}

						// Update pending.
						for (int c = 0; c < sigma.Length; c++)
						{
							int aj = active[j, c].Count;
							int ak = active[k, c].Count;
							if (!pending2[c, j] && 0 < aj && aj <= ak)
							{
								pending2[c, j] = true;
								pending.Enqueue(new _IntPair(j, c));
							}
							else
							{
								pending2[c, k] = true;
								pending.Enqueue(new _IntPair(k, c));
							}
						}

						k++;
					}

					foreach (var s in splitblock[j])
					{
						split2[s.Tag] = false;
					}

					refine2[j] = false;
					splitblock[j].Clear();
				}

				split.Clear();
				refine.Clear();
			}

			// Make a new state for each equivalence class, set initial state.
			var newstates = new FFA[k];
			for (int n = 0; n < newstates.Length; n++)
			{
				var s = new FFA();
				newstates[n] = s;
				foreach (var q in partition[n])
				{
					if (q == a)
					{
						a = s;
					}

					s.IsAccepting = q.IsAccepting;
					s.AcceptSymbol = q.AcceptSymbol;
					s.Tag = q.Tag; // Select representative.
					q.Tag = n;
				}
			}

			// Build transitions and set acceptance.
			foreach (var s in newstates)
			{
				var st = states[s.Tag];
				s.IsAccepting = st.IsAccepting;
				s.AcceptSymbol = st.AcceptSymbol;
				foreach (var t in st.Transitions)
				{
					s.Transitions.Add(new FFATransition(t.Min, t.Max, newstates[t.To.Tag]));
				}
			}
			// remove dead transitions
			foreach (var ffa in a.FillClosure())
			{
				var itrns = new List<FFATransition>(ffa.Transitions);
				foreach (var trns in itrns)
				{
					var acc = trns.To.FillAcceptingStates();
					if (0 == acc.Count)
					{
						ffa.Transitions.Remove(trns);
					}
				}
			}
			return a;
		}
		FFA _Step(int input)
		{
			for (int ic = Transitions.Count, i = 0; i < ic; ++i)
			{
				var t = Transitions[i];
				if (t.Min <= input && input <= t.Max)
					return t.To;

			}
			return null;
		}
		static void _Init<T>(IList<T> list, int count)
		{
			for (int i = 0; i < count; ++i)
			{
				list.Add(default(T));
			}
		}
		private sealed class _IntPair
		{
			private readonly int n1;
			private readonly int n2;

			public _IntPair(int n1, int n2)
			{
				this.n1 = n1;
				this.n2 = n2;
			}

			public int N1 {
				get { return n1; }
			}

			public int N2 {
				get { return n2; }
			}
		}
		private sealed class _FList
		{
			public int Count { get; set; }

			public _FListNode First { get; set; }

			public _FListNode Last { get; set; }

			public _FListNode Add(FFA q)
			{
				return new _FListNode(q, this);
			}
		}



		private sealed class _FListNode
		{
			public _FListNode(FFA q, _FList sl)
			{
				State = q;
				StateList = sl;
				if (sl.Count++ == 0)
				{
					sl.First = sl.Last = this;
				}
				else
				{
					sl.Last.Next = this;
					Prev = sl.Last;
					sl.Last = this;
				}
			}

			public _FListNode Next { get; private set; }

			private _FListNode Prev { get; set; }

			public _FList StateList { get; private set; }

			public FFA State { get; private set; }

			public void Remove()
			{
				StateList.Count--;
				if (StateList.First == this)
				{
					StateList.First = Next;
				}
				else
				{
					Prev.Next = Next;
				}

				if (StateList.Last == this)
				{
					StateList.Last = Prev;
				}
				else
				{
					Next.Prev = Prev;
				}
			}
		}

		static FFA _Determinize(FFA fa)
		{
			var p = new HashSet<int>();
			var closure = new List<FFA>();
			fa.FillClosure(closure);
			for (int ic = closure.Count, i = 0; i < ic; ++i)
			{
				var ffa = closure[i];
				p.Add(0);
				foreach (var t in ffa.Transitions)
				{
					p.Add(t.Min);
					if (t.Max < 0x10ffff)
					{
						p.Add((t.Max + 1));
					}
				}
			}

			var points = new int[p.Count];
			p.CopyTo(points, 0);
			Array.Sort(points);

			var sets = new Dictionary<KeySet<FFA>, KeySet<FFA>>();
			var working = new Queue<KeySet<FFA>>();
			var dfaMap = new Dictionary<KeySet<FFA>, FFA>();
			var initial = new KeySet<FFA>();
			initial.Add(fa);
			sets.Add(initial, initial);
			working.Enqueue(initial);
			var result = new FFA();
			foreach (var afa in initial)
			{
				if (afa.IsAccepting)
				{
					result.IsAccepting = true;
					result.AcceptSymbol = afa.AcceptSymbol;
					break;
				}
			}
			dfaMap.Add(initial, result);
			while (working.Count > 0)
			{
				var s = working.Dequeue();
				FFA dfa;
				dfaMap.TryGetValue(s, out dfa);
				foreach (FFA q in s)
				{
					if (q.IsAccepting)
					{
						dfa.IsAccepting = true;
						dfa.AcceptSymbol = q.AcceptSymbol;
						break;
					}
				}

				for (var i = 0; i < points.Length; i++)
				{
					var pnt = points[i];
					var set = new KeySet<FFA>();
					foreach (FFA c in s)
					{
						foreach (var trns in c.Transitions)
						{
							if (trns.Min <= pnt && pnt <= trns.Max)
							{
								set.Add(trns.To);
							}
						}
					}
					if (!sets.ContainsKey(set))
					{
						sets.Add(set, set);
						working.Enqueue(set);
						dfaMap.Add(set, new FFA());
					}

					FFA dst;
					dfaMap.TryGetValue(set, out dst);
					int first = pnt;
					int last;
					if (i + 1 < points.Length)
						last = (points[i + 1] - 1);
					else
						last = 0x10ffff;
					dfa.Transitions.Add(new FFATransition(first, last, dst));
				}

			}
			// remove dead transitions
			foreach (var ffa in result.FillClosure())
			{
				var itrns = new List<FFATransition>(ffa.Transitions);
				foreach (var trns in itrns)
				{
					var acc = trns.To.FillAcceptingStates();
					if (0 == acc.Count)
					{
						ffa.Transitions.Remove(trns);
					}
				}
			}
			return result;
		}
	}
}
