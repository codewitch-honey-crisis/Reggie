using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void CompiledLexer(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledLexer.template"
dynamic a = Arguments;
var symbolTable = (string[])a._symbolTable;
var symbolFlags = (int[])a._symbolFlags;
var dfa = (int[])a._dfa;
int sid, si;
int[] map;
var blockEndDfas = (int[][])a._blockEndDfas;
for(var symId = 0;symId<blockEndDfas.Length;++symId) {
	var bedfa = blockEndDfas[symId];
	if(bedfa!=null) {
		a.Comment("Tokenizes the block end for "+ symbolTable[symId]);
		a.MethodPrologue("None",true,"CompiledLexerTokenizeBlockEndReturn","Tokenize"+symbolTable[symId]+"BlockEnd","CompiledLexerTokenizeBlockEndParams");
			a.CompiledLexerTokenizeBlockEndDeclarations();
			a.InputLoopPrologue();
				a.ClearMatched();
				si = 0;
				sid = 0;
				map = GetDfaStateTransitionMap(bedfa);
				while(si < bedfa.Length) {
					if(sid != 0 || IsQ0Reffed(bedfa)) {
						a.Label("q"+sid.ToString());
					} else {
						a._indent = (int)a._indent - 1;
						a.Comment("q"+sid.ToString());
						a._indent = (int)a._indent + 1;
					}
					var acc = bedfa[si++];
					var tlen = bedfa[si++];
					for(var i = 0; i < tlen; ++i) {
						var tto = map[bedfa[si++]];
						var prlenIndex = si;
						var prlen = bedfa[si++];
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
									a.SetMatched();
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
									a.SetMatched();
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
									a.SetMatched();
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
							a.SetMatched();
							a.CompiledGotoState(tto);
						a.CompiledRangeMatchTestEpilogue();
						si+=prlen*2;
					} // for(i..tlen) ...
					if(acc!=-1) {
						a.CompiledLexerTokenizeBlockEndAccept();
					} else {
						a.CompiledGotoNext();
					}
					++sid;
				} 
				a.Label("next");
				a.IfNotMatchedPrologue();
					a.UpdateLineAny();
					a.AppendCapture();
					a.ReadCodepoint(false);
					a.AdvanceCursor();
				a.IfNotMatchedEpilogue();
			a.InputLoopEpilogue();
			a.CompiledLexerTokenizeBlockEndReject();
		a.MethodEpilogue();
	}
}
a.MethodPrologue("LexerTokenizeDocumentation",false,"LexerTokenizeReturn","Tokenize","LexerTokenizeParams");
	a.CompiledLexerTokenizeDeclarations();
	a.LexerCreateResultList();
	a.ReadCodepoint(false);
	a.InputLoopPrologue();
		a.LexerResetMatch();
		a.LexerClearMatched();
		si = 0;
		sid = 0;
		map = GetDfaStateTransitionMap(dfa);
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
							a.LexerSetMatched();
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
							a.LexerSetMatched();
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
							a.LexerSetMatched();
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
					a.LexerSetMatched();
					a.CompiledGotoState(tto);
				a.CompiledRangeMatchTestEpilogue();
				si+=prlen*2;
			} // for(i..tlen) ...
			if(acc!=-1) { // accepting
				a.LexerYieldPendingErrorResult(false,false);
				var bedfa = blockEndDfas[acc];
				if(bedfa==null) {
					if(0==(symbolFlags[acc] & 1)) {
						a.CompiledLexerYieldNonEmptyResult(symbolTable[acc], acc);
					}
					a.ClearCapture();
					a.ContinueInputLoop();
				} else {
					a.CompiledLexerDoBlockEndPrologue(symbolTable[acc]);
						a.CompiledLexerYieldResult(symbolTable[acc], acc);
						a.ClearCapture();
						a.ContinueInputLoop();
					a.CompiledLexerDoBlockEndEpilogue();
					a.LexerYieldPendingErrorResult(false,true);
					a.ClearCapture();
					a.ContinueInputLoop();
				}
			} else { // not accepting
				a.CompiledGotoError();
			}
			++sid;
		} // while(si < dfa.Length) ...
		a.Label("error");
		a.LexerIfNotMatchedWithErrorPrologue();
			a.UpdateLineAny(); // TODO: Test this because in reference code it comes after cursor advance for some reason
			a.AppendCapture();
			a.ReadCodepoint(false);
			a.AdvanceCursor();
		a.LexerIfNotMatchedWithErrorEpilogue();
		a.LexerHandleError();
	a.InputLoopEpilogue();
	a.LexerYieldPendingErrorResult(true,false);
	a.LexerReturnResultList();
a.MethodEpilogue();


            #line 245 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\CompiledLexer.template"
            Response.Flush();
        }
    }
}
