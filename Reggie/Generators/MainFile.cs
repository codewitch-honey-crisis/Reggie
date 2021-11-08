using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void MainFile(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\MainFile.template"

// this creates the class and namespace headers and footers, and then 
// routes to the appropriate master templates based on the command line switches
dynamic a = Arguments;

a.NamespacePrologue();

a.ClassPrologue();
a.ClassCommon();
if((bool)a.lexer) {
	if((bool)a.tables) {
		a.TableLexer();
		a.TableCommonLexer();
	} else {
		a.CompiledLexer();
		a.CompiledCommonLexer();
	}
} else {
	if((bool)a.matcher) {
		if((bool)a.tables) {
			a.TableMatcher();
		} else {
			a.CompiledMatcher();
		}
	} 
	if((bool)a.checker) {
		if((bool)a.tables) {
			a.TableChecker();
		} else {
			a.CompiledChecker();
		}
	}
	if((bool)a.tables) {
		a.TableCommonCheckerMatcher();
	}
}

a.ClassEpilogue();

a.NamespaceEpilogue();

            #line 41 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\MainFile.template"
            Response.Write("\r\n");
            Response.Flush();
        }
    }
}
