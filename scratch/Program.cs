using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = "...		/* ...a*/.. baz  ... 12343 foo	123.22 bar....";
            
            Console.WriteLine("Tokenizing: {0} ", str);
            var failed = false;
            var cstokcompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)>(CSCompiledTokenizerWithLines.Tokenize(str));
            var cstoktable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)>(CSCompiledTokenizerWithLines.Tokenize(str));
            Console.WriteLine("\tfound {0} compiled tokens and {1} table tokens (should match)",cstokcompiled.Count,cstoktable.Count);
            Console.WriteLine();
            if (cstokcompiled.Count != cstoktable.Count) {
                Console.WriteLine((cstokcompiled.Count < cstoktable.Count ? "C# Compiled version" : "C# Table version") + " is missing tokens");
                Console.WriteLine("C# Table version:");
                foreach (var tok in cstoktable)
                    _WriteFields(tok, Console.Out);
                Console.WriteLine();
                Console.WriteLine("C# Compiled version:");
                foreach (var tok in cstokcompiled)
                    _WriteFields(tok, Console.Out);
                Console.WriteLine();
                Console.WriteLine();
                failed = true;
            } else {
                for(var i = 0;i<cstokcompiled.Count;++i) {
                    if (cstokcompiled[i] != cstoktable[i]) {
                        Console.WriteLine("Inconsistent results on token index {i}", i);
                        failed = true;
                    }
                }
            }
            Console.WriteLine("Matching Whitespace: {0} ", str);
            var csmatchtable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(CSTableMatcher.MatchWhitespace(str));
            var csmatchcompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(CSCompiledMatcher.MatchWhitespace(str));
            Console.WriteLine("\tfound {0} compiled matches and {1} table matches (should match)", csmatchcompiled.Count, csmatchtable.Count);
            Console.WriteLine();
            if (csmatchcompiled.Count != csmatchtable.Count) {
                Console.WriteLine((csmatchcompiled.Count < csmatchtable.Count ? "C# Compiled version" : "C# Table version") + " is missing matches");
                failed = true;
            } else {
                for (var i = 0; i < csmatchtable.Count; ++i) {
                    if (csmatchcompiled[i] != csmatchtable[i]) {
                        Console.WriteLine("Inconsistent results on match index {i}", i);
                        failed = true;
                    }
                }
            }
            var sqltok = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)>();
            Console.Write("SQL tokenizing: {0}", str);
            SqlConnection sqlconn = new SqlConnection("Server=(local);DataBase=Test;Integrated Security=SSPI");
            sqlconn.Open();
            var cmd = sqlconn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("value", str));
            cmd.CommandText = "dbo.Example_Tokenize";
            var reader = cmd.ExecuteReader();
            while(reader.Read()) {
                sqltok.Add((AbsolutePosition:(long)reader[0], AbsoluteLength: (int)reader[1], Position: (long)reader[2], Length: (int)reader[3], SymbolId: (int)reader[4], Value: (string)reader[5], Line: (int)reader[6], Column: (int)reader[7]));
            }
            sqlconn.Close();
            Console.WriteLine("\tfound {0} sql tokens (should match above)", sqltok.Count, cstoktable.Count);
            Console.WriteLine();
            if (sqltok.Count != cstoktable.Count) {
                Console.WriteLine((sqltok.Count < cstoktable.Count ? "SQL version" : "C# Table version") + " is missing tokens");
                Console.WriteLine("C# Table version:");
                foreach (var tok in cstoktable)
                    _WriteFields(tok,Console.Out);
                Console.WriteLine();
                Console.WriteLine("SQL version:");
                foreach (var tok in sqltok)
                    _WriteFields(tok, Console.Out);
                Console.WriteLine();
                Console.WriteLine();
                failed = true;
            } else {
                for (var i = 0; i < sqltok.Count; ++i) {
                    if (sqltok[i] != cstoktable[i]) {
                        Console.WriteLine("Inconsistent results on token index {i}", i);
                        failed = true;
                    }
                }
            }
            if (!failed) {
                Console.WriteLine("All tests passed");
            }
        }
        static void _WriteFields((long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value) value, TextWriter writer)
        {
            writer.WriteLine($"AbsolutePosition: {value.AbsolutePosition}, AbsoluteLength: {value.AbsoluteLength}, Position: {value.Position}, Length: {value.Length}, SymbolId: {value.SymbolId}, Value: {value.Value}");
        }
        static void _WriteFields((long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value) value, TextWriter writer) {
            writer.WriteLine($"AbsolutePosition: {value.AbsolutePosition}, AbsoluteLength: {value.AbsoluteLength}, Position: {value.Position}, Length: {value.Length}, Value: {value.Value}");
        }
        static void _WriteFields((long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column) value,TextWriter writer)
        {
            writer.WriteLine($"AbsolutePosition: {value.AbsolutePosition}, AbsoluteLength: {value.AbsoluteLength}, Position: {value.Position}, Length: {value.Length}, SymbolId: {value.SymbolId}, Value: {value.Value}, Line: {value.Line}, Column: {value.Column}");
        }
        static void _WriteFields(object value,TextWriter writer)
        {
            if(null==value)
            {
                writer.WriteLine("<null>");
            } else
            {
                var t = value.GetType();
                var tuple = value as ITuple;
                if (tuple!=null)
                {
                    var attr = t.GetCustomAttribute<TupleElementNamesAttribute>(true);
                    if (attr != null)
                    {
                        for (var i = 0; i < tuple.Length; ++i)
                        {
                            Console.Write(attr.TransformNames[i]);
                            Console.Write(": ");
                            Console.Write(tuple[i]);
                            if (i < tuple.Length - 1)
                                Console.Write(", ");
                        }
                    }
                    else
                    {
                        Console.Write("(");
                        for (var i = 0; i < tuple.Length; ++i)
                        {
                            Console.Write(tuple[i]);
                            if (i < tuple.Length - 1)
                                Console.Write(", ");
                        }
                        Console.WriteLine(")");
                    }
                    return;
                }
                var needsComma = false;
                var fa = t.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.GetField);
                for(var i = 0;i<fa.Length;++i)
                {
                    var f = fa[i];
                    writer.Write(f.Name);
                    writer.Write(": ");
                    writer.Write(f.GetValue(value));
                    if(i<fa.Length-1)
                    {
                        writer.Write(", ");
                    }
                    needsComma = true;
                }
                var pa = t.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.GetProperty);
                for (var i = 0; i < pa.Length; ++i)
                {
                    if(needsComma)
                    {
                        writer.Write(", ");
                        needsComma = false;
                    }
                    var p = pa[i];
                    writer.Write(p.Name);
                    writer.Write(": ");
                    writer.Write(p.GetValue(value));
                    if (i < pa.Length - 1)
                    {
                        writer.Write(", ");
                    }
                }
                writer.WriteLine();
            }
        }
    }
}
