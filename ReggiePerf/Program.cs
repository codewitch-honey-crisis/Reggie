using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ReggiePerf
{
    class Program
    {
        static readonly Regex _regexCmp = new Regex(@"[\t\r\n\v\f ]+", RegexOptions.Compiled);
        
        static void Main(string[] args)
        {
            for (var k = 0; k < 3; ++k)
            {
                Console.WriteLine("Pass {0}", k + 1);
                Console.WriteLine();
                var sw = new Stopwatch();
                Console.WriteLine("Table - Matching whitespace in test.txt");
                ConsoleUtility.WriteProgressBar(0);
                sw.Start();
                for (var i = 0; i < 100; ++i)
                {
                    using (var sr = new StreamReader(@"..\..\test.txt"))
                    {
                        foreach (var match in ExampleTable.MatchWhitespace(sr)) ;
                    }
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar(i + 1, true);
                    sw.Start();
                }
                Console.WriteLine(" matched all 100 times in {0}ms", sw.ElapsedMilliseconds);

                sw.Reset();
                Console.WriteLine("Compiled - Matching whitespace in test.txt");
                ConsoleUtility.WriteProgressBar(0);
                sw.Start();
                for (var i = 0; i < 100; ++i)
                {
                    using (var sr = new StreamReader(@"..\..\test.txt"))
                    {
                        foreach (var match in ExampleCompiled.MatchWhitespace(sr)) ;
                    }
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar(i + 1, true);
                    sw.Start();
                }
                Console.WriteLine(" matched all 100 times in {0}ms", sw.ElapsedMilliseconds);

                sw.Reset();
                Console.WriteLine(".NET Compiled - Matching whitespace in test.txt");
                ConsoleUtility.WriteProgressBar(0);
                sw.Start();
                for (var i = 0; i < 100; ++i)
                {
                    using (var sr = new StreamReader(@"..\..\test.txt"))
                    {
                        foreach (Match match in _regexCmp.Matches(sr.ReadToEnd())) ;
                    }
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar(i + 1, true);
                    sw.Start();
                }
                Console.WriteLine(" matched all 100 times in {0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine();
                sw.Reset();
            }
            for (var k = 0; k < 3; ++k)
            {
                Console.WriteLine("Pass {0}", k + 1);
                Console.WriteLine();
                var sw = new Stopwatch();
                Console.WriteLine("Table - Matching 50 whitespace runs at codeproject.com");
                ConsoleUtility.WriteProgressBar(0);
                sw.Start();
                for (var i = 0; i < 10; ++i)
                {
                    var wr = WebRequest.Create("https://www.codeproject.com");
                    using (var wrs = wr.GetResponse())
                    {
                        using (var sr = new StreamReader(wrs.GetResponseStream()))
                        {
                            var j = 0;
                            foreach (var match in ExampleTable.MatchWhitespace(sr.ReadToEnd())) if (++j == 10) break;
                        }
                    }
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar((i + 1)*10, true);
                    sw.Start();
                }
                Console.WriteLine(" matched all 10 times in {0}ms", sw.ElapsedMilliseconds);

                sw.Reset();
                Console.WriteLine("Compiled - Matching 50 whitespace runs at codeproject.com");
                ConsoleUtility.WriteProgressBar(0);
                sw.Start();
                for (var i = 0; i < 10; ++i)
                {
                    var wr = WebRequest.Create("https://www.codeproject.com");
                    using (var wrs = wr.GetResponse())
                    {
                        using (var sr = new StreamReader(wrs.GetResponseStream()))
                        {
                            var j = 0;
                            foreach (var match in ExampleTable.MatchWhitespace(sr.ReadToEnd())) if (++j == 10) break;
                        }
                    }
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar((i + 1)*10, true);
                    sw.Start();
                }
                Console.WriteLine(" matched all 10 times in {0}ms", sw.ElapsedMilliseconds);

                sw.Reset();
                Console.WriteLine(".NET Compiled - Matching 50 whitespace runs at codeproject.com");
                ConsoleUtility.WriteProgressBar(0);
                sw.Start();
                for (var i = 0; i < 10; ++i)
                {
                    var wr = WebRequest.Create("https://www.codeproject.com");
                    using (var wrs = wr.GetResponse())
                    {
                        using (var sr = new StreamReader(wrs.GetResponseStream()))
                        {
                            var j = 0;
                            foreach (Match match in _regexCmp.Matches(sr.ReadToEnd())) if(++j==10) break;
                        }
                    }
                    
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar((i + 1)*10, true);
                    sw.Start();
                }
                Console.WriteLine(" matched all 10 times in {0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine();
                sw.Reset();
            }
        }
    }
}
