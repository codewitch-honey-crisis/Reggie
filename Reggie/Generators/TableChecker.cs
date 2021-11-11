using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void TableChecker(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableChecker.template"

dynamic a=Arguments;
for(var i = 0;i<((string[])a._symbolTable).Length;++i) {
	var s = ((string[])a._symbolTable)[i];
	if(s!=null) {
		a._symbol = s; // usually needed for the documentation template
		a.MethodPrologue("CheckerIsDocumentation",false,"CheckerIsReturn","Is"+s,"CheckerIsParams");
		a.TableCheckerIsImplForward(s,i);
		a.MethodEpilogue();
	}
}
a.Comment("Validates text based on a DFA table and block end DFA table");
a.MethodPrologue("None",true,"CheckerIsReturn","TableIs","TableCheckerIsImplParams");
	a.TableCheckerIsDeclarations();
	a.TableCheckerSetInitialAccept();
	a.ReadCodePoint(true);
	a.InputLoopPrologue();
		a.ClearMatched();
		a.TableMachineLoopPrologue();
			a.TableMove(false,true,false);
		a.TableMachineLoopEpilogue();
		a.TableAcceptPrologue();
			a.TableCheckerMatcherGetBlockEnd();
			a.TableIfBlockEndPrologue();
				a.TableStateReset();
				a.TableResetAccept();
				a.InputLoopPrologue();
					a.ClearMatched();
					a.TableMachineLoopPrologue();
						a.TableMove(true,true,false);
					a.TableMachineLoopEpilogue();
					a.TableAcceptPrologue();
						a.CheckerAccept();
					a.TableAcceptEpilogue();
					a.TableRejectPrologue();
						a.IfNotMatchedPrologue();
							a.ReadCodepoint(true);
						a.IfNotMatchedEpilogue();
					a.TableRejectEpilogue();
					a.TableStateReset();
				a.InputLoopEpilogue();
			a.TableIfBlockEndEpilogue();
			a.TableIfNotBlockEndPrologue();
				a.CheckerAccept();
			a.TableIfNotBlockEndEpilogue();
		a.TableAcceptEpilogue();
		a.TableRejectPrologue();
			a.CheckerReject();
		a.TableRejectEpilogue();
	a.InputLoopEpilogue();
	a.TableAcceptPrologue();
		a.CheckerAccept();
	a.TableAcceptEpilogue();
	a.CheckerReject();
a.MethodEpilogue();

            #line 56 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\TableChecker.template"
            Response.Flush();
        }
    }
}
