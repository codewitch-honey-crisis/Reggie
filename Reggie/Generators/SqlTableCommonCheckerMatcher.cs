using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void SqlTableCommonCheckerMatcher(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
dynamic a = Arguments;
var symbolTable = (string[])a._symbolTable;
var symbolFlags = (int[])a._symbolFlags;
var dfas = (int[][])a._dfas;
var blockEndDfas = (int[][])a._blockEndDfas;
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


            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("TRUNCATE TABLE [dbo].[");
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(a.@class);
            #line 17 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("Symbol]\r\nTRUNCATE TABLE [dbo].[");
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(a.@class);
            #line 18 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("StateTransition]\r\nTRUNCATE TABLE [dbo].[");
            #line 19 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(a.@class);
            #line 19 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("State]\r\nGO\r\nBEGIN TRANSACTION");
            #line 21 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"

for(var symId = 0; symId<symbolTable.Length;++symId) {
    var sym = symbolTable[symId];
    if(sym != null) {
        var flags = symbolFlags[symId];
        var dfa = dfas[symId];
        var map = GetDfaStateTransitionMap(dfa);
        var si = 0;
        var sid = 0;
        while(si<dfa.Length) {    
            var acc = dfa[si++];
            var tlen = dfa[si++];
            #line 32 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(a.@class);
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("State] VALUES(");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(symId);
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(sid);
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(acc==-1?0:1);
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", -1)");
            #line 33 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"

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
            #line 43 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(a.@class);
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("StateTransition] VALUES(");
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(symId);
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(sid);
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", -1, ");
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(map[tto]);
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(pmin);
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(pmax);
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(")");
            #line 44 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"

                }
            }
            ++sid;
        }
            #line 48 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(a.@class);
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("Symbol] VALUES(");
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(symId);
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(flags);
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(",N\'");
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(sym);
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("\')");
            #line 49 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"

    }
}
var bid = 0;    
for(var symId = 0; symId<symbolTable.Length;++symId) {
    var bedfa = blockEndDfas[symId];
    if(bedfa!=null) {
        var map = GetDfaStateTransitionMap(bedfa);
        var si = 0;
        var sid = 0;
        while(si<bedfa.Length) {
            var acc = bedfa[si++];
            var tlen = bedfa[si++];
            #line 61 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(a.@class);
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("State] VALUES(");
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(symId);
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(sid);
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(acc==-1?0:1);
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(bemap[symId]);
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(")");
            #line 62 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"

            for(var i = 0;i<tlen;++i) {
                var tto = bedfa[si++];
                var prlen = bedfa[si++];
                for(var j = 0;j<prlen;++j) {
                    // MSSQL doesn't have unsigned support, so all 
                    // transitions are stored as an unsigned bigint
                    var pmin = (long)bedfa[si++];
                    var pmax = (long)bedfa[si++];
                    if(pmin<0) pmin += 2147483648;
                    if(pmax<0) pmax += 2147483648;
            #line 72 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("\r\nINSERT INTO [dbo].[");
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(a.@class);
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("StateTransition] VALUES(");
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(symId);
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(sid);
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(bemap[symId]);
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(map[tto]);
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(pmin);
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(", ");
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(pmax);
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write(")");
            #line 73 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"

               
                }
                ++sid;
            }   
            ++bid;
        }
    }
}
            #line 81 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\SQL\SqlTableCommonCheckerMatcher.template"
            Response.Write("\r\nCOMMIT\r\nGO\r\n");
            Response.Flush();
        }
    }
}
