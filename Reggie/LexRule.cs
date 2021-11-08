using System;
using System.Collections.Generic;
using System.Text;
using LC;
namespace Reggie
{
	class LexRule
	{
		public int Id = int.MinValue;
		public string Symbol;
		public string Expression;
		public IList<KeyValuePair<string, object>> Attributes;
		public int Line;
		public int Column;
		public long Position;
		public int ExpressionLine;
		public int ExpressionColumn;
		public long ExpressionPosition;
		public object GetAttribute(string name, object @default = null)
		{
			var attrs = Attributes;
			if (null != attrs)
			{
				for (int ic=attrs.Count,i = 0; i < ic; ++i)
				{
					var attr = attrs[i];
					if (0 == string.Compare(attr.Key, name))
						return attr.Value;
				}
			}
			return @default;
		}
		internal static LexRule Parse(LexContext lc)
		{
			lc.Expecting();
			var result = new LexRule();
			result.Line = lc.Line;
			result.Column = lc.Column;
			result.Position = lc.Position;
			var ll = lc.CaptureBuffer.Length;
			if (!lc.TryReadCIdentifier())
				throw new ExpectingException("Expecting identifier", lc.Line, lc.Column, lc.Position, lc.FileOrUrl, "identifier");
			result.Symbol = lc.GetCapture(ll);
			lc.TrySkipCCommentsAndWhiteSpace();
			lc.Expecting('=','<');
			if ('<' == lc.Current)
			{
				lc.Advance();
				lc.Expecting();
				var attrs = new List<KeyValuePair<string, object>>();
				while (-1 != lc.Current && '>' != lc.Current)
				{
					lc.TrySkipCCommentsAndWhiteSpace();
					lc.ClearCapture();
					var l = lc.Line;
					var c = lc.Column;
					var p = lc.Position;
					if (!lc.TryReadCIdentifier())
						throw new ExpectingException("Identifier expected", l, c, p, "identifier");
					var aname = lc.GetCapture();
					lc.TrySkipCCommentsAndWhiteSpace();
					lc.Expecting('=', '>', ',');
					if ('=' == lc.Current)
					{
						lc.Advance();
						lc.TrySkipCCommentsAndWhiteSpace();
						l = lc.Line;
						c = lc.Column;
						p = lc.Position;
						object value = null;
						if (lc.Current == '\'')
						{
							// this is a regular expression.
							// we store an anonymous LexRule for it in the attribute
							var newRule = new LexRule();
							newRule.ExpressionLine = lc.Line;
							newRule.ExpressionColumn = lc.Column;
							newRule.ExpressionPosition = lc.Position;
							lc.ClearCapture();
							lc.Capture();
							lc.Advance();
							lc.TryReadUntil('\'', '\\', false);
							lc.Expecting('\'');
							lc.Capture();
							newRule.Expression = lc.GetCapture();
							lc.Advance();
							value = newRule;
						}
						else
						{
							value = lc.ParseJsonValue();
						}
						attrs.Add(new KeyValuePair<string, object>(aname, value));
						if (0 == string.Compare("id", aname) && (value is double))
						{
							result.Id = (int)((double)value);
							if (0 > result.Id)
								throw new ExpectingException("Expecting a non-negative integer", l, c, p, "nonNegativeInteger");
						}
					}
					else
					{ // boolean true
						attrs.Add(new KeyValuePair<string, object>(aname, true));
					}
					lc.TrySkipCCommentsAndWhiteSpace();
					lc.Expecting(',', '>');
					if (',' == lc.Current)
						lc.Advance();
				}
				lc.Expecting('>');
				lc.Advance();
				result.Attributes = attrs;
				lc.TrySkipCCommentsAndWhiteSpace();
			}
			lc.Advance();
			lc.TrySkipCCommentsAndWhiteSpace();
			lc.Expecting('\'', '\"');
			result.ExpressionLine = lc.Line;
			result.ExpressionColumn = lc.Column;
			result.ExpressionPosition = lc.Position;
			if ('\'' == lc.Current)
			{
				lc.ClearCapture();
				lc.Capture();
				lc.Advance();
				lc.TryReadUntil('\'', '\\', false);
				lc.Expecting('\'');
				lc.Capture();
				result.Expression = lc.GetCapture();
				lc.Advance();
			}
			else
			{
				lc.ClearCapture();
				lc.Capture();
				lc.Advance();
				lc.TryReadUntil('\"', '\\', false);
				lc.Expecting('\"');
				lc.Capture();
				result.Expression = lc.GetCapture();
			}
			return result;
		}
		public static void FillRuleIds(IList<LexRule> rules, bool forceIds = false)
		{
			var ids = new HashSet<int>();
			for (int ic = rules.Count, i = 0; i < ic; ++i)
			{
				var rule = rules[i];
				if (int.MinValue != rule.Id && !ids.Add(rule.Id))
					throw new InvalidOperationException(string.Format("The input file has a rule with a duplicate id at line {0}, column {1}, position {2}", rule.Line, rule.Column, rule.Position));
				if (forceIds)
					rule.Id = i;
			}
			if (forceIds) return; 
			var lastId = 0;
			for (int ic = rules.Count, i = 0; i < ic; ++i)
			{
				var rule = rules[i];
				if (int.MinValue == rule.Id)
				{
					rule.Id = lastId;
					ids.Add(lastId);
					while (ids.Contains(lastId))
						++lastId;
				}
				else
				{
					lastId = rule.Id;
					while (ids.Contains(lastId))
						++lastId;
				}
			}
		}
	}
}
