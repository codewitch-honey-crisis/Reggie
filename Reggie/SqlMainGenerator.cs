using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class SqlMainGenerator {
        public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {

var codenamespace = (string)Arguments["codenamespace"];
var tables = (bool)Arguments["tables"];
var lexer = (bool)Arguments["lexer"];
var outputfile = (string)Arguments["outputfile"];
var inputfile = (string)Arguments["inputfile"];
var input = (TextReader)Arguments["input"];
var codeclass = (string)Arguments["codeclass"];
if(string.IsNullOrEmpty(codeclass)) {
    if(!string.IsNullOrEmpty(outputfile)) {
        codeclass = Path.GetFileNameWithoutExtension(outputfile);
    } else {
        codeclass = Path.GetFileNameWithoutExtension(inputfile);
    }
}
Arguments["codeclass"]=codeclass;
var rules = new List<Reggie.LexRule>();
string line;
while (null != (line = input.ReadLine()))
{
    var lc = LC.LexContext.Create(line);
    lc.TrySkipCCommentsAndWhiteSpace();
    if (-1 != lc.Current)
        rules.Add(Reggie.LexRule.Parse(lc));
}
Reggie.LexRule.FillRuleIds(rules);
Arguments["rules"]=rules;
if(!string.IsNullOrEmpty(codenamespace)) {
            Response.Write("\r\nUSE [");
            Response.Write(codenamespace);
            Response.Write("]\r\nGO");

}
if(tables) {
    if(lexer) {
        Run("SqlTableTokenizerGenerator",Arguments,Response,0);
    } else { // if(lexer) ...
        Run("SqlTableMatcherGenerator",Arguments,Response,0);
    } // if(lexer) ...
} else { // if(tables) ...
    if(lexer) {
        Run("SqlCompiledTokenizerGenerator",Arguments,Response,0);
    } else { // if(lexer) ...
        Run("SqlCompiledMatcherGenerator",Arguments,Response,0);
    } // if(lexer) ...
} // if(tables) ...
            Response.Flush();
        }
    }
}
