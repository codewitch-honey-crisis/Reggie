﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Test {
    class Program {
        readonly static string[] Tests = new string[] {
            "/*baz*//*bar*/",
            "...		/* ...a*/.. baz  ... 12343 foo	123.22 bar....", 
            "bar", 
            " \t ", 
            "  /*   */ "
        };
        enum DoDB {
            // don't update the DB
            None = 0,
            // update the DB from the SQL files in the project directory
            Update = 1,
            // only run the DROP commands from the SQL files in the project directory
            Drop = 2
        }
        const DoDB SqlDbAction = DoDB.Update;
        // if /namespace is specified reggie generates "USE [<codenamespace>]" SQL commands. If not, you better put a db name in the string below
        const string SqlDbConnectionString = "Server=(local);Integrated Security=SSPI";
        static void Main(string[] args) {

            SqlConnection sqlconn = new SqlConnection(SqlDbConnectionString);
            sqlconn.Open();
            SqlCommand cmd; 
            try {
#pragma warning disable CS0162
                if (DoDB.None != SqlDbAction) {
                    var lcount = 0;
                    var fcount = 0;
                    var sb = new StringBuilder();
                    var dir = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory)));
                    // the below code isn't robust enough for general purpose use, but will happily execute the SQL
                    // generated by reggie. If you modify those SQL files, this may get confused.
                    foreach (var sqlfile in Directory.GetFiles(dir, "*.sql")) {
                        using (var sr = File.OpenText(sqlfile)) {
                            sb.Clear();
                            string line;
                            int lc = 1;
                            int linNo = 1;
                            while (null != (line = sr.ReadLine())) {
                                if (SqlDbAction == DoDB.Drop) {
                                    var t = line.Trim();

                                    if (t.ToLowerInvariant().Contains("drop table #") || !(t.ToLowerInvariant() == "go" || t.StartsWith("use ", StringComparison.InvariantCultureIgnoreCase) || t.StartsWith("drop ", StringComparison.InvariantCultureIgnoreCase))) {
                                        continue;
                                    } else {
                                        Console.WriteLine(line);
                                    }
                                }
                                if (line.Trim().ToLowerInvariant() == "go") {
                                    if (sb.ToString().Trim().Length > 0) {
                                        cmd = new SqlCommand(sb.ToString(), sqlconn);
                                        sb.Clear();
                                        linNo = lc;
                                        cmd.CommandType = System.Data.CommandType.Text;
                                        try {
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex) {
                                            Console.WriteLine("On line {0} in {1}", linNo, sqlfile);
                                            Console.WriteLine("Error executing query part: {0}, continuing.", ex.Message);
                                        }
                                        cmd.Dispose();

                                    }

                                } else {
                                    sb.AppendLine(line);
                                    ++lcount;
                                }
                                ++lc;
                            }
                            if (sb.Length > 0) {
                                cmd = new SqlCommand(sb.ToString(), sqlconn);
                                cmd.CommandType = System.Data.CommandType.Text;
                                try {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex) {
                                    Console.WriteLine("On line {0} in {1}", linNo, sqlfile);
                                    Console.WriteLine("Error executing query part: {0}, continuing.", ex.Message);
                                }
                                cmd.Dispose();

                            }
                        }
                        ++fcount;

                    }
                    Console.WriteLine("Batched {0} lines of SQL in {1} files.", lcount, fcount);
                    if (SqlDbAction == DoDB.Drop) {

                        Console.WriteLine("All database elements dropped. Exiting.");
                        return;
                    }
                    Console.WriteLine();
                }
#pragma warning restore CS0162
                
                
                var failed =false;
                for (var i = 0; i < Tests.Length; ++i) {

                    if (_RunTests(sqlconn, Tests[i]))
                        failed = true;
                }

                if (!failed) {
                    Console.WriteLine("All tests passed");
                } else {
                    Console.WriteLine("One or more tests failed");
                }
            }
            finally {
                sqlconn.Close();
            }
        }

        static bool _RunTests(SqlConnection sqlconn, string test) {
            var result = false;
            SqlCommand cmd;
            Console.WriteLine("C# Tokenizing (with lines): {0} ", test);
            result = false;
            var cstoklinescompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)>(CSCompiledTokenizerWithLines.Tokenize(test));
            var cstoklinestable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)>(CSTableTokenizerWithLines.Tokenize(test));
            if (!_CompareSets("C# Compiled", cstoklinescompiled, "C# Table", cstoklinestable))
                result = true;

            Console.WriteLine();

            var cstokcompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value)>(CSCompiledTokenizer.Tokenize(test));
            var cstoktable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value)>(CSTableTokenizer.Tokenize(test));

            Console.WriteLine("C# Tokenizing (no lines): {0} ", test);

            if (!_CompareSets("C# Compiled", cstokcompiled, "C# Table", cstoktable))
                result = true;

            Console.WriteLine();

            var csmatchwscompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(CSCompiledMatcher.MatchWhitespace(test));
            var csmatchwstable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(CSTableMatcher.MatchWhitespace(test));

            Console.WriteLine("C# Matching Whitespace: \"{0}\" ", test);

            if (!_CompareSets("C# Compiled", csmatchwscompiled, "C# Table", csmatchwstable))
                result = true;

            Console.WriteLine();

            var csmatchcbcompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(CSCompiledMatcher.MatchCommentBlock(test));
            var csmatchcbtable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(CSTableMatcher.MatchCommentBlock(test));

            Console.WriteLine("C# Matching Comment Block: {0} ", test);

            if (!_CompareSets("C# Compiled", csmatchcbcompiled, "C# Table", csmatchcbtable))
                result = true;

            Console.WriteLine();



            cmd = sqlconn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("value", test));
            cmd.CommandText = "dbo.SqlTableTokenizerWithLines_Tokenize";
            var reader = cmd.ExecuteReader();
            var sqltoklinestable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)>(_EatReaderWithLines(reader));
            cmd.Dispose();

            Console.WriteLine("SQL vs C# Tokenizing Table (with lines): {0}", test);

            if (!_CompareSets("SQL Table", sqltoklinestable, "C# Table", cstoklinestable))
                result = true;

            Console.WriteLine();

            cmd = sqlconn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("value", test));
            cmd.CommandText = "dbo.SqlTableTokenizer_Tokenize";
            reader = cmd.ExecuteReader();
            var sqltoktable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value)>(_EatReader(reader));
            cmd.Dispose();

            Console.WriteLine("SQL Table vs C# Table Tokenizing Table (no lines): {0} ", test);

            if (!_CompareSets("SQL Table", sqltoktable, "C# Table", cstoktable))
                result = true;

            Console.WriteLine();

            try {

                cmd = sqlconn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("value", test));
                cmd.CommandText = "dbo.SqlTableMatcher_MatchWhitespace";
                reader = cmd.ExecuteReader();
                var sqlmatchwstable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(_EatMatchReader(reader));
                cmd.Dispose();

                Console.WriteLine("SQL Table vs C# Table Matching Whitespace: \"{0}\"", test);

                if (!_CompareSets("SQL Table", sqlmatchwstable, "C# Table", csmatchwstable))
                    result = true;

                try {
                    cmd = sqlconn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("value", test));
                    cmd.CommandText = "dbo.SqlCompiledMatcher_MatchWhitespace";
                    reader = cmd.ExecuteReader();
                    var sqlmatchwscompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(_EatMatchReader(reader));
                    cmd.Dispose();
                    Console.WriteLine("SQL Compiled vs SQL Table Matching Whitespace: \"{0}\"", test);

                    if (!_CompareSets("SQL Compiled", sqlmatchwscompiled, "SQL Table", sqlmatchwstable))
                        result = true;

                }
                catch (SqlException) {
                    Console.WriteLine("SQL Compiled vs SQL Table Matching Whitespace: \"{0}\"", "<not implemented or not uploaded>");
                    Console.WriteLine();
                    result = true;
                }

            }
            catch (SqlException) {
                Console.WriteLine("SQL vs C# Table Matcher: \"{0}\"", "<not implemented or not uploaded>");
                Console.WriteLine();
                Console.WriteLine("SQL Compiled vs SQL Table Matching Whitespace: \"{0}\"", "<not implemented or not uploaded>");
                Console.WriteLine();
                result = true;
            }

            try {

                cmd = sqlconn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("value", test));
                cmd.CommandText = "dbo.SqlTableMatcher_MatchCommentBlock";
                reader = cmd.ExecuteReader();
                var sqlmatchcbtable = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(_EatMatchReader(reader));
                cmd.Dispose();

                Console.WriteLine("SQL Table vs C# Table Matching Comment Block: {0}", test);

                if (!_CompareSets("SQL Table", sqlmatchcbtable, "C# Table", csmatchcbtable))
                    result = true;

                try {
                    cmd = sqlconn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("value", test));
                    cmd.CommandText = "dbo.SqlCompiledMatcher_MatchCommentBlock";
                    reader = cmd.ExecuteReader();
                    var sqlmatchcbcompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)>(_EatMatchReader(reader));
                    cmd.Dispose();
                    Console.WriteLine("SQL Compiled vs SQL Table Matching Comment Block: {0}", test);

                    if (!_CompareSets("SQL Compiled", sqlmatchcbcompiled, "SQL Table", sqlmatchcbtable))
                        result = true;

                }
                catch (SqlException) {
                    Console.WriteLine("SQL Compiled vs SQL Table Matching Comment Block: {0}", "<not implemented or not uploaded>");
                    Console.WriteLine();
                    result = true;
                }

            }
            catch (SqlException) {
                Console.WriteLine("SQL vs C# Table Matcher: {0}", "<not implemented or not uploaded>");
                Console.WriteLine();
                Console.WriteLine("SQL Compiled vs SQL Table Matching Comment Block: {0}", "<not implemented or not uploaded>");
                Console.WriteLine();
                result = true;
            }
            cmd = sqlconn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("value", test));
            cmd.CommandText = "dbo.SqlCompiledTokenizerWithLines_Tokenize";
            try {
                reader = cmd.ExecuteReader();
                var sqltoklinescompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)>(_EatReaderWithLines(reader));
                cmd.Dispose();
                Console.WriteLine("SQL Compiled vs SQL Table Tokenizer (with lines): {0}", test);

                if (!_CompareSets("SQL Compiled", sqltoklinescompiled, "SQL Table", sqltoklinestable))
                    result = true;
                try {
                    cmd = sqlconn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("value", test));
                    cmd.CommandText = "dbo.SqlCompiledTokenizer_Tokenize";
                    reader = cmd.ExecuteReader();
                    var sqltokcompiled = new List<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value)>(_EatReader(reader));
                    cmd.Dispose();
                    Console.WriteLine("SQL Compiled vs SQL Table Matching Tokenizer (no lines): {0}", test);

                    if (!_CompareSets("SQL Compiled", sqltoklinescompiled, "SQL Table", sqltoklinestable))
                        result = true;
                    
                }
                catch (SqlException e2) {
                    Console.WriteLine("SQL Compiled vs SQL Table Tokenizer (no lines): {0}", string.Format("<not implemented or not uploaded, or error {0}>",e2.Message));
                    Console.WriteLine();
                    result = true;
                }

            }
            catch (SqlException e) {
                Console.WriteLine("SQL Compiled vs SQL Table Tokenizer (with lines): {0}", string.Format("<not implemented or not uploaded, or error {0}>", e.Message));
                Console.WriteLine();
                Console.WriteLine("SQL Compiled vs SQL Table Tokenizer (no lines): {0}", "<not implemented or not uploaded>");
                Console.WriteLine();
                result = true;
            }
            return result;
        }

        static bool _CompareSets(string nx, IList<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)> x, 
            string ny, IList<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)> y) {
            var failed = false;
            if (x.Count == 0 || y.Count == 0) {
                Console.WriteLine("Warning: One or more sets has a zero count.");
            }
            if (x.Count != y.Count) {
                Console.WriteLine((x.Count < y.Count ? string.Format("{0} version",nx) : string.Format("{0} version",ny)) + " is missing results");
                Console.WriteLine("{0} version:",nx);
                foreach (var tok in x)
                    _WriteFields(tok, Console.Out);
                Console.WriteLine();
                Console.WriteLine("{0} version: ", ny);
                foreach (var tok in y)
                    _WriteFields(tok, Console.Out);
                Console.WriteLine();
                failed = true;
            } else {
                for (var i = 0; i < x.Count; ++i) {
                    if (x[i] != y[i]) {
                        Console.WriteLine("Inconsistent results on result index {0}", i);
                        Console.WriteLine("{0} version: ",nx);
                        _WriteFields(x[i], Console.Out);
                        Console.WriteLine();
                        Console.WriteLine("{0} version: ",ny);
                        _WriteFields(y[i], Console.Out);
                        Console.WriteLine();
                    }
                }
                failed = true;
            }
            Console.WriteLine();
            return failed;
        }
        static bool _CompareSets(string nx, IList<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value)> x, 
            string ny, IList<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value)> y) {
            if (x.Count == 0 || y.Count == 0) {
                Console.WriteLine("Warning: One or more sets has a zero count.");
            }
            var failed = false;
            if (x.Count != y.Count) {
                Console.WriteLine((x.Count < y.Count ? string.Format("{0} version", nx) : string.Format("{0} version", ny)) + " is missing results");
                Console.WriteLine("{0} version:", nx);
                foreach (var tok in x)
                    _WriteFields(tok, Console.Out);
                Console.WriteLine();
                Console.WriteLine("{0} version: ", ny);
                foreach (var tok in y)
                    _WriteFields(tok, Console.Out);
                Console.WriteLine();
                failed = true;
            } else {
                for (var i = 0; i < x.Count; ++i) {
                    if (x[i] != y[i]) {
                        Console.WriteLine("Inconsistent results on result index {0}", i);
                        Console.WriteLine("{0} version: ", nx);
                        _WriteFields(x[i], Console.Out);
                        Console.WriteLine();
                        Console.WriteLine("{0} version: ", ny);
                        _WriteFields(y[i], Console.Out);
                        Console.WriteLine();
                    }
                }
                failed = true;
            }
            Console.WriteLine();
            return failed;
        }
        static bool _CompareSets(string nx, IList<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> x, 
            string ny, IList<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> y) {
            if (x.Count == 0 || y.Count == 0) {
                Console.WriteLine("Warning: One or more sets has a zero count.");
            }
            var failed = false;
            if (x.Count != y.Count) {
                Console.WriteLine((x.Count < y.Count ? string.Format("{0} version", nx) : string.Format("{0} version", ny)) + " is missing results");
                Console.WriteLine("{0} version:", nx);
                foreach (var tok in x)
                    _WriteFields(tok, Console.Out);
                Console.WriteLine();
                Console.WriteLine("{0} version: ", ny);
                foreach (var tok in y)
                    _WriteFields(tok, Console.Out);
                Console.WriteLine();
                failed = true;
            } else {
                for (var i = 0; i < x.Count; ++i) {
                    if (x[i] != y[i]) {
                        Console.WriteLine("Inconsistent results on result index {0}", i);
                        Console.WriteLine("{0} version: ", nx);
                        _WriteFields(x[i], Console.Out);
                        Console.WriteLine();
                        Console.WriteLine("{0} version: ", ny);
                        _WriteFields(y[i], Console.Out);
                        Console.WriteLine();
                    }
                }
                failed = true;
            }
            Console.WriteLine();
            return failed;
        }
        static IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value)> _EatReader(SqlDataReader reader) {
            while (reader.Read())
                yield return (AbsolutePosition: (long)reader[0], AbsoluteLength: (int)reader[1], Position: (long)reader[2], Length: (int)reader[3], SymbolId: (int)reader[4], Value: (string)reader[5]);
            reader.Close();
        }
        static IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> _EatMatchReader(SqlDataReader reader) {
            while (reader.Read())
                yield return (AbsolutePosition: (long)reader[0], AbsoluteLength: (int)reader[1], Position: (long)reader[2], Length: (int)reader[3], Value: (string)reader[4]);
            reader.Close();
        }
        static IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)> _EatReaderWithLines(SqlDataReader reader) {
            while(reader.Read())
                yield return (AbsolutePosition: (long)reader[0], AbsoluteLength: (int)reader[1], Position: (long)reader[2], Length: (int)reader[3], SymbolId: (int)reader[4], Value: (string)reader[5], Line: (int)reader[6], Column: (int)reader[7]);
            reader.Close();
        }
        static void _WriteFields((long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value) value, TextWriter writer) {
            writer.WriteLine($"AbsolutePosition: {value.AbsolutePosition}, AbsoluteLength: {value.AbsoluteLength}, Position: {value.Position}, Length: {value.Length}, SymbolId: {value.SymbolId}, Value: {value.Value}");
        }
        static void _WriteFields((long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value) value, TextWriter writer) {
            writer.WriteLine($"AbsolutePosition: {value.AbsolutePosition}, AbsoluteLength: {value.AbsoluteLength}, Position: {value.Position}, Length: {value.Length}, Value: {value.Value}");
        }
        static void _WriteFields((long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column) value, TextWriter writer) {
            writer.WriteLine($"AbsolutePosition: {value.AbsolutePosition}, AbsoluteLength: {value.AbsoluteLength}, Position: {value.Position}, Length: {value.Length}, SymbolId: {value.SymbolId}, Value: {value.Value}, Line: {value.Line}, Column: {value.Column}");
        }
        static void _WriteFields(object value, TextWriter writer) {
            if (null == value) {
                writer.WriteLine("<null>");
            } else {
                var t = value.GetType();
                var tuple = value as ITuple;
                if (tuple != null) {
                    var attr = t.GetCustomAttribute<TupleElementNamesAttribute>(true);
                    if (attr != null) {
                        for (var i = 0; i < tuple.Length; ++i) {
                            Console.Write(attr.TransformNames[i]);
                            Console.Write(": ");
                            Console.Write(tuple[i]);
                            if (i < tuple.Length - 1)
                                Console.Write(", ");
                        }
                    } else {
                        Console.Write("(");
                        for (var i = 0; i < tuple.Length; ++i) {
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
                for (var i = 0; i < fa.Length; ++i) {
                    var f = fa[i];
                    writer.Write(f.Name);
                    writer.Write(": ");
                    writer.Write(f.GetValue(value));
                    if (i < fa.Length - 1) {
                        writer.Write(", ");
                    }
                    needsComma = true;
                }
                var pa = t.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.GetProperty);
                for (var i = 0; i < pa.Length; ++i) {
                    if (needsComma) {
                        writer.Write(", ");
                        needsComma = false;
                    }
                    var p = pa[i];
                    writer.Write(p.Name);
                    writer.Write(": ");
                    writer.Write(p.GetValue(value));
                    if (i < pa.Length - 1) {
                        writer.Write(", ");
                    }
                }
                writer.WriteLine();
            }
        }
    }
}