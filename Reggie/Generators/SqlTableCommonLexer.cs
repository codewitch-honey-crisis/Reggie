using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableCommonLexer(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"

dynamic a = Arguments;
var blockEndDfas = (int[][])a._blockEndDfas;
var symbolFlags = (int[])a._symbolFlags;
var symbolTable = (string[])a._symbolTable;
var dfa = (int[])a._dfa;
var map = GetDfaStateTransitionMap(dfa);
var bemap = new Dictionary<int,int>();
var bei = 0;
for(var i = 0;i<blockEndDfas.Length;++i) {
    var be = blockEndDfas[i];
    if(null!=be) {
        bemap.Add(i,bei);
        ++bei;
    } else
        bemap.Add(i,-1);
}

            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("\r\nTRUNCATE TABLE [dbo].[");
            #line 19 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(a.@class);
            #line 19 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("TokenizeSymbol]\r\nTRUNCATE TABLE [dbo].[");
            #line 20 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(a.@class);
            #line 20 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("TokenizeStateTransition]\r\nTRUNCATE TABLE [dbo].[");
            #line 21 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(a.@class);
            #line 21 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("TokenizeState]\r\nGO\r\nBEGIN TRANSACTION");
            #line 23 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"

var si = 0;
var sid = 0;
while(si<dfa.Length) {
        
    var acc = dfa[si++];
    var tlen = dfa[si++];
    
            #line 30 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 31 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(a.@class);
            #line 31 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("TokenizeState] VALUES(");
            #line 31 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(sid);
            #line 31 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 31 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(acc);
            #line 31 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", -1)");
            #line 31 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"

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
            #line 41 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(a.@class);
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("TokenizeStateTransition] VALUES(");
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(sid);
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", -1, ");
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(map[tto]);
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(pmin);
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(pmax);
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(")");
            #line 42 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"

        }
    }
    ++sid;
}
for(var symId = 0; symId < symbolTable.Length; ++symId) {
    var sym = symbolTable[symId];
    if(sym != null) {

            #line 50 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(a.@class);
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("TokenizeSymbol] VALUES(");
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(symId);
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(symbolFlags[symId]);
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(bemap[symId]);
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", N\'");
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(sym);
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("\')");
            #line 51 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"

    }
}
var bid = 0;

for(var sacc = 0;sacc<blockEndDfas.Length;++sacc) {
    var be = blockEndDfas[sacc];
    if(be!=null) {
  	    dfa = be;
        map = GetDfaStateTransitionMap(dfa);
        si = 0;
        sid = 0;
        while(si<dfa.Length) {
            var acc = dfa[si++];
            var tlen = dfa[si++];
            #line 65 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(a.@class);
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("TokenizeState] VALUES(");
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(sid);
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(acc==-1?-1:sacc);
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(bid);
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(")");
            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"

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
            #line 76 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(a.@class);
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("TokenizeStateTransition] VALUES(");
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(sid);
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(bid);
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(map[tto]);
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(pmin);
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(", ");
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(pmax);
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write(")");
            #line 77 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"

                } // for(var j = 0;j<prlen;++j) ...
            } // for(var i = 0;i<tlen;++i) ...
            ++sid;
        } // while(si<dfa.Length) ...
        ++bid;
    } // if(be!=null) ...
} // for(var sacc = 0;sacc<blockEnds.Length;++sacc) ...

            #line 85 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Write("\r\nCOMMIT\r\nGO\r\n");
            #line 88 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonLexer.template"
            Response.Flush();
        }
    }
}
