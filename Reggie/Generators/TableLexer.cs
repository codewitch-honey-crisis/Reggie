using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void TableLexer(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableLexer.template"

dynamic a = Arguments;
var symbolTable = (string[])a._symbolTable;
var symbolFlags = (int[])a._symbolFlags;
var dDfa = (int[])a._dfa;
var blockEndDfas = (int[][])a._blockEndDfas;

a.MethodPrologue("LexerTokenizeDocumentation",false,"LexerTokenizeReturn","Tokenize","LexerTokenizeParams");
	a.TableLexerTokenizeDeclarations();
	a.LexerCreateResultList();
	a.ReadCodepoint(false);
	a.InputLoopPrologue();
		a.LexerResetMatch();
		a.TableStateReset();
		a.LexerClearMatched();
		a.TableMachineLoopPrologue();
			a.TableMove(false,false,false);
		a.TableMachineLoopEpilogue();
		a.TableAcceptPrologue();
			a.LexerYieldPendingErrorResult(false,false);
			a.TableLexerGetBlockEnd();
			a.TableIfBlockEndPrologue();
				a.TableLexerStoreAccept();
				a.TableStateReset();
				a.InputLoopPrologue();
					a.ClearMatched();
					a.TableMachineLoopPrologue();
						a.TableMove(true,false,false);
					a.TableMachineLoopEpilogue();
					a.TableAcceptPrologue();
						a.TableLexerYieldResult(true);
						a.ClearCapture();
						a.BreakInputLoop();
					a.TableAcceptEpilogue();
					a.UpdateLineAny();
					a.AppendCapture();
					a.ReadCodepoint(false);
					a.AdvanceCursor();
					a.TableStateReset();
				a.InputLoopEpilogue();
				a.TableIfNotMatchedBlockEndPrologue();
					a.LexerYieldPendingErrorResult(false,true);
					a.ClearCapture();
				a.TableIfNotMatchedBlockEndEpilogue();
				a.ContinueInputLoop();
			a.TableIfBlockEndEpilogue();
			a.TableIfNotBlockEndPrologue();
				a.LexerYieldPendingErrorResult(false,false);
				a.TableLexerYieldNonEmptyResult(false);
				a.ClearCapture();
			a.TableIfNotBlockEndEpilogue();
		a.TableAcceptEpilogue();
		a.TableRejectPrologue();
			a.LexerIfNotMatchedWithErrorPrologue();
				a.UpdateLineAny(); // TODO: Test this because in reference code it comes after cursor advance for some reason
				a.AppendCapture();
				a.ReadCodepoint(false);
				a.AdvanceCursor();
			a.LexerIfNotMatchedWithErrorEpilogue();
			a.LexerHandleError();
		a.TableRejectEpilogue();
	a.InputLoopEpilogue();
	a.LexerYieldPendingErrorResult(true,false);
	a.LexerReturnResultList();
a.MethodEpilogue();

            #line 66 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableLexer.template"
            Response.Flush();
        }
    }
}
