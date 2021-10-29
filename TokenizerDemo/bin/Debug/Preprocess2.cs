using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
public class Preprocessor {
    public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {
        Response.WriteLine("using System;");
        Response.WriteLine("using System.Collections.Generic;");
        Response.WriteLine("using System.Diagnostics;");
        Response.WriteLine("using System.IO;");
        Response.WriteLine("using System.Linq;");
        Response.WriteLine("using System.Text;");
        Response.WriteLine("using System.Threading.Tasks;");
        Response.WriteLine("using System.Runtime.InteropServices;");
        Response.WriteLine("");
        Response.WriteLine("namespace TokenizerDemo");
        Response.WriteLine("{");
        Response.WriteLine("    class Program");
        Response.WriteLine("    {");
        Response.WriteLine("        static void Main(string[] args)");
        Response.WriteLine("        {");
        Response.WriteLine("            foreach (var token in Example.Tokenize(File.OpenText(@\"..\\..\\Program.cs\")))");
        Response.WriteLine("            {");
        Response.WriteLine("                Console.WriteLine(\"id: {0}, pos: {1}, val: {3}\", token.Id, token.Position, token.Value.Length, token.Value);");
        Response.WriteLine("            }");
        Response.WriteLine("            /*");
        Response.WriteLine("            // barbarbar bar bar bar");
        Response.WriteLine("            foreach (var token in Example.Tokenize(File.OpenText(@\"..\\..\\Program.cs\")))");
        Response.WriteLine("            {");
        Response.WriteLine("                Console.WriteLine(\"id: {0}, pos: {1}, val: {3}\", token.Id, token.Position, token.Value.Length, token.Value);");
        Response.WriteLine("            }");
        Response.WriteLine("            *************/var i = 1213551L;");
        Response.WriteLine("        }");
        Response.WriteLine("    }");
        Response.WriteLine("}");
        Response.Flush();
    }
}
