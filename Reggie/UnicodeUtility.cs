using System.Collections.Generic;
using System.IO;

namespace F
{
#if FALIB
	public
#endif
	static class UnicodeUtility
	{
		public static IEnumerable<int> ToUtf32(IEnumerable<char> @string)
		{
			int chh=-1;
			foreach(var ch in @string)
			{
				if (char.IsHighSurrogate(ch))
				{
					chh = ch;
					continue;
				}
				else
					chh = -1;
				if(-1!=chh)
				{
					if (!char.IsLowSurrogate(ch))
						throw new IOException("Unterminated Unicode surrogate pair found in string.");
					yield return char.ConvertToUtf32(unchecked((char)chh), ch);
					chh = -1;
					continue;
				}
				yield return ch;
			}
		}
	}
}
