using System;
using System.Collections.Generic;

namespace F
{
	partial class CharacterClasses
	{
		static Lazy<IDictionary<string, int[]>> _Known = new Lazy<IDictionary<string, int[]>>(_GetKnown);
		static IDictionary<string,int[]> _GetKnown()
		{
			var result = new Dictionary<string, int[]>();
			var fa = typeof(CharacterClasses).GetFields();
			for (var i = 0; i < fa.Length; i++)
			{
				var f = fa[i];
				if (f.FieldType == typeof(int[]))
				{
					result.Add(f.Name, (int[])f.GetValue(null));
				}
				
			}
			return result;
		}
		public static IDictionary<string,int[]> Known {  get { return _Known.Value; } }
	}
}
