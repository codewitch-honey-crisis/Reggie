using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TokenizerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var token in Example.Tokenize(File.OpenText(@"..\..\Program.cs")))
            {
                Console.WriteLine("id: {0}, pos: {1}, val: {3}", token.Id, token.Position, token.Value.Length, token.Value);
            }
            /*
            // barbarbar bar bar bar
            foreach (var token in Example.Tokenize(File.OpenText(@"..\..\Program.cs")))
            {
                Console.WriteLine("id: {0}, pos: {1}, val: {3}", token.Id, token.Position, token.Value.Length, token.Value);
            }
            *************/var i = 1213551L;
        }
    }
}
