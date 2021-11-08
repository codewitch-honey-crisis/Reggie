using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reggie {
    partial class Generator {

        public static void RggTargetGenerator(Stream Response, IDictionary<string, object> Arguments) {
            var input = (string)Arguments["input"];
            var lexer = (bool)Arguments["lexer"];
            int[] dfa = null;
            int[] symbolFlags = null;
            int[][] dfas = null;
            var symbolTable = (string[])Arguments["_symbolTable"];
            var rules = new List<string>();
            var ruleIds = new List<int>();
            for (var i = 0;i<symbolTable.Length;++i) {
                if (!string.IsNullOrEmpty(symbolTable[i])) {
                    rules.Add(symbolTable[i]);
                    ruleIds.Add(i);

                }
            }
            if (lexer) {
                dfa = (int[])Arguments["_dfa"];
                symbolFlags = (int[])Arguments["_symbolFlags"];
            } else {
                dfas = (int[][])Arguments["_dfas"];
            }
            
            
            var blockEndDfas = (int[][])Arguments["_blockEndDfas"];
            var fourcc = Encoding.ASCII.GetBytes(lexer ? "rgl\0" : "rgm\0");
            Response.Write(fourcc, 0, fourcc.Length);
            var w = new BinaryWriter(Response);
            var v = (Version)Arguments["_version"];
            w.Write(LE(v.Major));
            w.Write(LE(v.Minor));
            w.Write(LE(v.Build));
            w.Write(LE(v.Revision));
            w.Write(LE(symbolTable.Length));
            w.Write(LE(rules.Count));
            if (lexer) {
                for (var i = 0; i < rules.Count; ++i) {
                    w.Write(LE(ruleIds[i]));
                    var name = Encoding.UTF8.GetBytes(symbolTable[ruleIds[i]]);
                    w.Write(LE(name.Length));
                    w.Write(name);
                    w.Write(LE(symbolFlags[ruleIds[i]]));
                    var bed = blockEndDfas[ruleIds[i]];
                    if (bed == null) {
                        w.Write(LE(0));
                    } else {
                        w.Write(LE(bed.Length));
                        for (var j = 0; j < bed.Length; ++j)
                            w.Write(LE(bed[j]));
                    }
                }
                w.Write(LE(dfa.Length));
                for (var j = 0; j < dfa.Length; ++j)
                    w.Write(LE(dfa[j]));

            } else {
                for (var i = 0; i < rules.Count; ++i) {
                    var rule = rules[i];
                    var name = Encoding.UTF8.GetBytes(rule);
                    w.Write(LE(name.Length));
                    w.Write(name);
                    dfa = dfas[ruleIds[i]];
                    w.Write(LE(dfa.Length));
                    for (var j = 0; j < dfa.Length; ++j)
                        w.Write(LE(dfa[j]));
                    var bed = blockEndDfas[ruleIds[i]];
                    if (bed == null) {
                        w.Write(LE(0));
                    } else {
                        w.Write(LE(bed.Length));
                        for (var j = 0; j < bed.Length; ++j)
                            w.Write(LE(bed[j]));
                    }
                    
                }

            }
            Response.Flush();
        }
    }
}
