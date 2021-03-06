using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void TableMatcher(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableMatcher.template"

dynamic a=Arguments;
a.Comment("Matches text based on a DFA table and block end DFA table");
a.MethodPrologue("None",true,"MatcherMatchReturn","TableMatch","TableMatcherMatchImplParams");
	a.MatcherCreateResultList();
	a.TableMatcherMatchDeclarations();
	a.ReadCodePoint(false);
	a.InputLoopPrologue();
		a.MatcherResetMatch();
		a.TableMachineLoopPrologue();
			a.TableMove(false,false,true);
		a.TableMachineLoopEpilogue();
		a.TableAcceptPrologue();
			a.TableCheckerMatcherGetBlockEnd();
			a.TableIfBlockEndPrologue();
				a.TableStateReset();
				a.InputLoopPrologue();
					a.TableMachineLoopPrologue();
						a.TableMove(true,false,true);
					a.TableMachineLoopEpilogue();
					a.TableAcceptPrologue();
						a.MatcherYieldResult();
						a.BreakInputLoop();
					a.TableAcceptEpilogue();
					a.TableRejectPrologue();
						a.UpdateLineAny();
						a.AppendCapture();
						a.ReadCodepoint(false);
						a.AdvanceCursor();
					a.TableRejectEpilogue();
					a.TableStateReset();
				a.InputLoopEpilogue();
				a.TableStateReset();
				a.ContinueInputLoop();
			a.TableIfBlockEndEpilogue();
			a.TableIfNotBlockEndPrologue();
				a.MatcherYieldNonEmptyResult();
			a.TableIfNotBlockEndEpilogue();
		a.TableAcceptEpilogue();
		a.UpdateLineAny();
		a.ReadCodepoint(false);
		a.AdvanceCursor();
		a.TableStateReset();
	a.InputLoopEpilogue();
	a.MatcherReturnResultList();
a.MethodEpilogue();
for(var i = 0;i<((string[])a._symbolTable).Length;++i) {
	var s = ((string[])a._symbolTable)[i];
	if(s!=null) {
		a._symbol = s; // usually needed for the documentation template
		a.MethodPrologue("MatcherMatchDocumentation",false,"MatcherMatchReturn","Match"+s,"MatcherMatchParams");
			a.TableMatcherMatchImplForward(s,i);
		a.MethodEpilogue();
	}
}

            #line 56 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableMatcher.template"
            Response.Flush();
        }
    }
}
