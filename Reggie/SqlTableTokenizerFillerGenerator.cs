using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Reggie {
    internal partial class SqlTableTokenizerFillerGenerator {
        public static void Run(TextWriter Response, IDictionary<string, object> Arguments) {

var rules = (IList<LexRule>)Arguments["rules"];
var ignoreCase = (bool)Arguments["ignorecase"];
var inputFile = (string)Arguments["inputfile"];
var outputFile = (string)Arguments["outputfile"];
var stderr = (TextWriter)Arguments["stderr"];
var codeclass = (string)Arguments["codeclass"];
var dot = (bool)Arguments["dot"];
var jpg = (bool)Arguments["jpg"];
var cwd = Path.GetDirectoryName(outputFile!=null?outputFile:inputFile);
var blockEnds = (F.FFA[])Arguments["blockEnds"];
var symbolFlags = (int[])Arguments["symbolFlags"];
var symbolTable = (string[])Arguments["symbolTable"];
var lexer = (F.FFA)Arguments["lexer"];
var dfa = ToDfaTable(lexer);
var dfaMap = GetDfaStateTransitionMap(dfa);
var beMap = new Dictionary<int,int>();
var bei = 0;
for(var i = 0;i<blockEnds.Length;++i) {
    var be = blockEnds[i];
    if(null!=be) {
        beMap.Add(i,bei);
        ++bei;
    } else
        beMap.Add(i,-1);
}

            Response.Write("\r\nTRUNCATE TABLE [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol]\r\nTRUNCATE TABLE [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition]\r\nTRUNCATE TABLE [dbo].[");
            Response.Write(codeclass);
            Response.Write("State]\r\nGO\r\nBEGIN TRANSACTION");

var si = 0;
var sid = 0;
while(si<dfa.Length) {
        
    var acc = dfa[si++];
    var tlen = dfa[si++];
    
            Response.Write("\r\nINSERT INTO [dbo].[");
            Response.Write(codeclass);
            Response.Write("State] VALUES(");
            Response.Write(sid);
            Response.Write(", ");
            Response.Write(acc);
            Response.Write(", -1)");

    for(var i = 0;i<tlen;++i) {
        var tto = dfa[si++];
        var prlen = dfa[si++];
        for(var j = 0;j<prlen;++j) {
            // MSSQL doesn't have unsigned support, so all 
            // transitions are stored as an unsigned bigint
            var pmin = (long)dfa[si++];
            var pmax = (long)dfa[si++];
            if(pmin<0) pmin += 2147483648;
            if(pmax<0) pmax += 2147483648;
            Response.Write("\r\nINSERT INTO [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition] VALUES(");
            Response.Write(sid);
            Response.Write(",-1,");
            Response.Write(dfaMap[tto]);
            Response.Write(",");
            Response.Write(pmin);
            Response.Write(",");
            Response.Write(pmax);
            Response.Write(")");

        }
    }
    ++sid;
}
foreach(var rule in rules) {
            Response.Write("\r\nINSERT INTO [dbo].[");
            Response.Write(codeclass);
            Response.Write("Symbol] VALUES(");
            Response.Write(rule.Id);
            Response.Write(", ");
            Response.Write(symbolFlags[rule.Id]);
            Response.Write(", ");
            Response.Write(beMap[rule.Id]);
            Response.Write(", N\'");
            Response.Write(rule.Symbol);
            Response.Write("\')");

}
var bid = 0;

for(var sacc = 0;sacc<blockEnds.Length;++sacc) {
    var be = blockEnds[sacc];
    if(be!=null) {
  	    dfa = ToDfaTable(be);
        dfaMap = GetDfaStateTransitionMap(dfa);
    
        si = 0;
        sid = 0;
        while(si<dfa.Length) {
            var acc = dfa[si++];
            var tlen = dfa[si++];
            Response.Write("\r\nINSERT INTO [dbo].[");
            Response.Write(codeclass);
            Response.Write("State] VALUES(");
            Response.Write(sid);
            Response.Write(", ");
            Response.Write(acc==-1?-1:sacc);
            Response.Write(",");
            Response.Write(bid);
            Response.Write(")");

            for(var i = 0;i<tlen;++i) {
                var tto = dfa[si++];
                var prlen = dfa[si++];
                for(var j = 0;j<prlen;++j) {
                    // MSSQL doesn't have unsigned support, so all 
                    // transitions are stored as an unsigned bigint
                    var pmin = (long)dfa[si++];
                    var pmax = (long)dfa[si++];
                    if(pmin<0) pmin += 2147483648;
                    if(pmax<0) pmax += 2147483648;
            Response.Write("\r\nINSERT INTO [dbo].[");
            Response.Write(codeclass);
            Response.Write("StateTransition] VALUES(");
            Response.Write(sid);
            Response.Write(",");
            Response.Write(bid);
            Response.Write(",");
            Response.Write(dfaMap[tto]);
            Response.Write(",");
            Response.Write(pmin);
            Response.Write(",");
            Response.Write(pmax);
            Response.Write(")");

                } // for(var j = 0;j<prlen;++j) ...
            } // for(var i = 0;i<tlen;++i) ...
            ++sid;
        } // while(si<dfa.Length) ...
        ++bid;
    } // if(be!=null) ...
} // for(var sacc = 0;sacc<blockEnds.Length;++sacc) ...

            Response.Write("\r\nCOMMIT\r\nGO\r\n");
            Response.Flush();
        }
    }
}
