using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CompiledMatcher(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledMatcher.template"

dynamic a=Arguments;
for(var symId = 0;symId<((string[])a._symbolTable).Length;++symId) {
	var sym = ((string[])a._symbolTable)[symId];
	if(sym!=null) {
		var dfa = ((int[][])a._dfas)[symId];
		var bedfa = ((int[][])a._blockEndDfas)[symId];
		a._symbol = sym; // usually needed for the documentation template
		if(bedfa!=null) { // need the block end helper method
			a.Comment("Matches the block end for "+sym);
			a.MethodPrologue("None",true,"CompiledMatcherMatchBlockEndReturn","Match"+sym+"BlockEnd","CompiledMatcherMatchBlockEndParams");
				a.CompiledMatcherMatchBlockEndDeclarations();
				a.InputLoopPrologue();
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
							var rstart = prlenIndex;
							var ranges = bedfa;
							if((bool)a.lines) {
								var lclist = new List<int>(10);
								if (DfaRangesContains('\n',ranges,rstart)) {
									lclist.Add('\n');
									ranges = DfaExcludeFromRanges('\n',ranges,rstart);
									rstart = 0;        
								}
								if (DfaRangesContains('\r',ranges, rstart)) {
									lclist.Add('\r');
									ranges = DfaExcludeFromRanges('\r',ranges,rstart);
									rstart = 0; 
								}
								if (DfaRangesContains('\t',ranges, rstart)) {
									lclist.Add('\t');
									ranges = DfaExcludeFromRanges('\t',ranges,rstart);
									rstart = 0;
								}
								if(lclist.Contains('\t')) {
									var temprange = new int[] {1,'\t','\t'};
									a.CompiledRangeMatchTestPrologue(temprange,0);
										a.CompiledAppendCapture(true);
										a.UpdateTab();
										a.ReadCodepoint(false);
										a.AdvanceCursor();
										a.CompiledGotoState(tto);
									a.CompiledRangeMatchTestEpilogue();
								}
								if(lclist.Contains('\n')) {
									var temprange = new int[] {1,'\n','\n'};
									a.CompiledRangeMatchTestPrologue(temprange,0);
										a.CompiledAppendCapture(true);
										a.UpdateLineFeed();
										a.ReadCodepoint(false);
										a.AdvanceCursor();
										a.CompiledGotoState(tto);
									a.CompiledRangeMatchTestEpilogue();
								}
								if(lclist.Contains('\r')) {
									var temprange = new int[] {1,'\r','\r'};
									a.CompiledRangeMatchTestPrologue(temprange,0);
										a.CompiledAppendCapture(true);
										a.UpdateCarriageReturn();
										a.ReadCodepoint(false);
										a.AdvanceCursor();
										a.CompiledGotoState(tto);
									a.CompiledRangeMatchTestEpilogue();
								}
							} // if(lines) ...
							var exts = GetTransitionExtents(ranges,rstart);
							a.CompiledRangeMatchTestPrologue(ranges,rstart);
								a.CompiledAppendCapture(exts.Value<128);
								if(exts.Value>31) {
									a.UpdateNonControl(exts.Key<32);
								}
								a.ReadCodepoint(false);
								a.AdvanceCursor();
								a.CompiledGotoState(tto);
							a.CompiledRangeMatchTestEpilogue();
							besi+=prlen*2;
						} // for(i..tlen) ...
						if(acc!=-1) { // accepting
							a.CompiledMatcherReturnBlockEndResult(true);
						} else { // not accepting
							if(besi<bedfa.Length) {
								a.CompiledGotoNext();
							}
						}
						++besid;
					} // while(besi < bedfa.Length) ...
					a.Label("next");
					a.UpdateLineAny();
					a.CompiledAppendCapture(false);
					a.ReadCodepoint(false);
					a.AdvanceCursor();				
				a.InputLoopEpilogue();
				a.CompiledMatcherReturnBlockEndResult(false);
			a.MethodEpilogue();
		}
		a.MethodPrologue("MatcherMatchDocumentation",false,"MatcherMatchReturn","Match"+sym,"MatcherMatchParams");
			a.CompiledMatcherMatchDeclarations();
			a.MatcherCreateResultList();
			a.ReadCodepoint(false);
			a.InputLoopPrologue();
				a.MatcherResetMatch();
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
						var rstart = prlenIndex;
						var ranges = dfa;
						if((bool)a.lines) {
							var lclist = new List<int>(10);
							if (DfaRangesContains('\n',ranges,rstart)) {
								lclist.Add('\n');
								ranges = DfaExcludeFromRanges('\n',ranges,rstart);
								rstart = 0;        
							}
							if (DfaRangesContains('\r',ranges, rstart)) {
								lclist.Add('\r');
								ranges = DfaExcludeFromRanges('\r',ranges,rstart);
								rstart = 0; 
							}
							if (DfaRangesContains('\t',ranges, rstart)) {
								lclist.Add('\t');
								ranges = DfaExcludeFromRanges('\t',ranges,rstart);
								rstart = 0;
							}
							if(lclist.Contains('\t')) {
								var temprange = new int[] {1,'\t','\t'};
								a.CompiledRangeMatchTestPrologue(temprange,0);
									a.CompiledAppendCapture(true);
									a.UpdateTab();
									a.ReadCodepoint(false);
									a.AdvanceCursor();
									a.CompiledGotoState(tto);
								a.CompiledRangeMatchTestEpilogue();
							}
							if(lclist.Contains('\n')) {
								var temprange = new int[] {1,'\n','\n'};
								a.CompiledRangeMatchTestPrologue(temprange,0);
									a.CompiledAppendCapture(true);
									a.UpdateLineFeed();
									a.ReadCodepoint(false);
									a.AdvanceCursor();
									a.CompiledGotoState(tto);
								a.CompiledRangeMatchTestEpilogue();
							}
							if(lclist.Contains('\r')) {
								var temprange = new int[] {1,'\r','\r'};
								a.CompiledRangeMatchTestPrologue(temprange,0);
									a.CompiledAppendCapture(true);
									a.UpdateCarriageReturn();
									a.ReadCodepoint(false);
									a.AdvanceCursor();
									a.CompiledGotoState(tto);
								a.CompiledRangeMatchTestEpilogue();
							}
						} // if(lines) ...
						var exts = GetTransitionExtents(ranges,rstart);
						a.CompiledRangeMatchTestPrologue(ranges,rstart);
							a.CompiledAppendCapture(exts.Value<128);
							if(exts.Value>31) {
								a.UpdateNonControl(exts.Key<32);
							}
							a.ReadCodepoint(false);
							a.AdvanceCursor();
							a.CompiledGotoState(tto);
						a.CompiledRangeMatchTestEpilogue();
						si+=prlen*2;
					} // for(i..tlen) ...
					if(acc!=-1) { // accepting
						if(bedfa==null) {
							a.MatcherYieldNonEmptyResult();
							if(si<dfa.Length) {
								a.CompiledGotoNext();
							}
						} else {
							a.CompiledMatcherDoBlockEndPrologue(sym);
								a.MatcherYieldResult();
							a.CompiledMatcherDoBlockEndEpilogue();
							a.ContinueInputLoop();
						}
					} else { // not accepting
						if(si<dfa.Length) {
							a.CompiledGotoNext();
						}
					}
					++sid;
				} // while(si < dfa.Length) ...
				a.Label("next");
				a.UpdateLineAny();
				a.ReadCodepoint(false);
				a.AdvanceCursor();
			a.InputLoopEpilogue();
			a.MatcherReturnResultList();
		a.MethodEpilogue();
	} // if(sym != null) ...
} // for(var symId...) ...

            #line 224 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledMatcher.template"
            Response.Flush();
        }
    }
}
