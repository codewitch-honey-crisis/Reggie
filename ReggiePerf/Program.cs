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
                        foreach (var match in ExampleTableMatcher.MatchWhitespace(sr)) ;
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
                        foreach (var match in ExampleCompiledMatcher.MatchWhitespace(sr)) ;
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
                Console.WriteLine("Table - Tokenizing test.txt");
                ConsoleUtility.WriteProgressBar(0);
                sw.Start();
                for (var i = 0; i < 100; ++i)
                {
                    using (var sr = new StreamReader(@"..\..\test.txt"))
                    {
                        foreach (var token in ExampleTableTokenizer.Tokenize(sr)) ;
                    }
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar(i + 1, true);
                    sw.Start();
                }
                Console.WriteLine(" tokenized all 100 times in {0}ms", sw.ElapsedMilliseconds);

                sw.Reset();
                Console.WriteLine("Compiled - Tokenizing test.txt");
                ConsoleUtility.WriteProgressBar(0);
                sw.Start();
                for (var i = 0; i < 100; ++i)
                {
                    using (var sr = new StreamReader(@"..\..\test.txt"))
                    {
                        foreach (var match in ExampleCompiledTokenizer.Tokenize(sr)) ;
                    }
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar(i + 1, true);
                    sw.Start();
                }
                Console.WriteLine(" tokenized all 100 times in {0}ms", sw.ElapsedMilliseconds);

                Console.WriteLine("Table - Tokenizing test.txt with line counting");
                ConsoleUtility.WriteProgressBar(0);

                sw.Reset();                
                for (var i = 0; i < 100; ++i)
                {
                    using (var sr = new StreamReader(@"..\..\test.txt"))
                    {
                        foreach (var token in ExampleTableTokenizerWithLines.Tokenize(sr)) ;
                    }
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar(i + 1, true);
                    sw.Start();
                }
                Console.WriteLine(" tokenized all 100 times in {0}ms", sw.ElapsedMilliseconds);

                sw.Reset();
                Console.WriteLine("Compiled - Tokenizing test.txt with line counting");
                ConsoleUtility.WriteProgressBar(0);
                sw.Start();
                for (var i = 0; i < 100; ++i)
                {
                    using (var sr = new StreamReader(@"..\..\test.txt"))
                    {
                        foreach (var match in ExampleCompiledTokenizerWithLines.Tokenize(sr)) ;
                    }
                    sw.Stop();
                    ConsoleUtility.WriteProgressBar(i + 1, true);
                    sw.Start();
                }
                Console.WriteLine(" tokenized all 100 times in {0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine();
                sw.Reset();
            }
        }
    }
}
