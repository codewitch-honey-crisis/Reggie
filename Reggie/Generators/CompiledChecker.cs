using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CompiledChecker(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledChecker.template"

dynamic a=Arguments;
for(var symId = 0;symId<((string[])a._symbolTable).Length;++symId) {
	var dfa = ((int[][])a._dfas)[symId];
	var bedfa = ((int[][])a._blockEndDfas)[symId];
	var sym = ((string[])a._symbolTable)[symId];
	if(sym!=null) {
		if(bedfa!=null) {
			a.Comment("Matches the block end for "+sym);
			a.MethodPrologue("None",true,"CheckerIsReturn","Is"+sym+"BlockEnd","CompiledCheckerIsBlockEndParams");
				a.CompiledCheckerIsBlockEndDeclarations();
				a.InputLoopPrologue();
					a.ClearMatched();
					var besi = 0;
					var besid = 0;
					var bemap = GetDfaStateTransitionMap(bedfa);
					while(besi < bedfa.Length) {
						if(besid != 0 || IsQ0Reffed(bedfa)) {
							a.Label("q"+besid.ToString());
						} else {
							a._indent = (int)a._indent - 1;
							a.Comment("q"+besid.ToString());
							a._indent = (int)a._indent + 1;
						}
						var acc = bedfa[besi++];
						var tlen = bedfa[besi++];
						for(var i = 0; i < tlen; ++i) {
							var tto = bemap[bedfa[besi++]];
							var prlenIndex = besi;
							var prlen = bedfa[besi++];
							var exts = GetTransitionExtents(bedfa,prlenIndex);
							a.CompiledRangeMatchTestPrologue(bedfa,prlenIndex);
								a.ReadCodepoint(true);
								a.SetMatched();
								a.CompiledGotoState(tto);
							a.CompiledRangeMatchTestEpilogue();
							besi+=prlen*2;
						} // for(i..tlen) ...
						if(acc!=-1) { // accepting
							a.CheckerAccept();
						} else { // not accepting
							a.CompiledGotoNext();
						}
						++besid;
					} // while(besi < bedfa.Length) ...
					a.Label("next");
					a.IfNotMatchedPrologue();
						a.ReadCodepoint(true);
					a.IfNotMatchedEpilogue();
				a.InputLoopEpilogue();
				a.CheckerReject();
			a.MethodEpilogue();
		}
		a._symbol = sym; // usually needed for the documentation template
		a.MethodPrologue("CheckerIsDocumentation",false,"CheckerIsReturn","Is"+sym,"CheckerIsParams");
			a.CompiledCheckerIsDeclarations();
			a.ReadCodepoint(true);
			if(dfa[0]!=-1) {
				a.CompiledCheckerCheckEmptyString();
			}
			a.InputLoopPrologue();
				var si = 0;
				var sid = 0;
				var map = GetDfaStateTransitionMap(dfa);
				while(si < dfa.Length) {
					if(sid != 0 || IsQ0Reffed(dfa)) {
						a.Label("q"+sid.ToString());
					} else {
						a._indent = (int)a._indent - 1;
						a.Comment("q"+sid.ToString());
						a._indent = (int)a._indent + 1;
					}
					var acc = dfa[si++];
					var tlen = dfa[si++];
					for(var i = 0; i < tlen; ++i) {
						var tto = map[dfa[si++]];
						var prlenIndex = si;
						var prlen = dfa[si++];
						var exts = GetTransitionExtents(dfa,prlenIndex);
						a.CompiledRangeMatchTestPrologue(dfa,prlenIndex);
							a.ReadCodepoint(true);
							a.CompiledGotoState(tto);
						a.CompiledRangeMatchTestEpilogue();
						si+=prlen*2;
					} // for(i..tlen) ...
					if(acc!=-1) { // accepting
						if(bedfa==null) {
							a.CheckerAccept();
						} else {
							a.CompiledCheckerCheckBlockEnd(sym);
						}
					} else { // not accepting
						a.CheckerReject();
					}
					++sid;
				} // while(si < dfa.Length) ...
			a.InputLoopEpilogue();
			a.CheckerReject();
		a.MethodEpilogue();
	}
}

            #line 102 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledChecker.template"
            Response.Flush();
        }
    }
}
