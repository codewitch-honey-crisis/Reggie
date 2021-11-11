using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#line hidden
namespace Reggie {
    internal partial class Generator {
        public static void Start(TextWriter Response, IDictionary<string, object> Arguments) {
            #line 1 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

// for easy access
dynamic a = Arguments;

// set our value types (used for parsing)
a.input = "";
a.output = "";
a.@class = "";
a.@namespace = "";
a.token = "";
a.ignorecase = false;
a.dot = false;
a.jpg = false;
a.tables = false;
a.lexer = false;
a.lines = false;
a.checker = false;
a.matcher = false;
a.target = "";
a.ifstale = false;
a.textreader = false;
a.ntext = false;
a.database = "";
var requiredArgs = new HashSet<string>();
requiredArgs.Add("input");
var failed = false;
a._exception = null;
a._indent = 0;
try {
    // parse input arguments
	CrackArguments("input",(string[])a._args,requiredArgs,Arguments);

    // defaults and validation
    if((string)a.output==(string)a.input) throw new ArgumentException("<input> and <output> indicated the same file.");
    if(""==(string)a.output && (bool)a.ifstale) {
            #line 35 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Warning: /ifstale will be ignored because /output was not specified\r\n");
            #line 36 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

    }
    a.input = Path.GetFullPath((string)a.input);
    if(""!=(string)a.output) {
        a.output = Path.GetFullPath((string)a.output);
    }
    var target = ((string)a.target).ToLowerInvariant();
    if(target == "csharp" || target == "c#")
        target = "cs";
    if(string.IsNullOrEmpty(target)) {
        if(!string.IsNullOrEmpty((string)a.output)) {
            target = Path.GetExtension((string)a.output);
            if(!string.IsNullOrEmpty(target) && target[0]=='.')
                target=target.Substring(1);
        } 
        if(string.IsNullOrEmpty(target)) {
            target = "cs";
        }
        a.target = target;
    }
    if(!IsValidTarget(Arguments))
        throw new NotSupportedException(string.Format("Unsupported target {0} was indicated",a.target));

    if(target=="rgg" && (bool)a.ignorecase) {
            #line 59 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Warning: /ignorecase will be ignored since a Reggie binary is being used.\r\n");
            #line 60 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

    }
    if(""==(string)a.@class) {
        if(""!=(string)a.output) {
            a.@class = Path.GetFileNameWithoutExtension((string)a.output);
        } else {
            a.@class = Path.GetFileNameWithoutExtension((string)a.input);
        }
    }
    if(""!=(string)a.output) {
        a._cwd=Path.GetDirectoryName((string)a.output);
    }
    if((bool)a.lexer) {
        if((bool)a.matcher || (bool)a.checker)
            throw new NotSupportedException("You cannot use /lexer with either /matcher or /checker. It is not possible to generate lexing and matching/checking code from the same state tables.");
    }

    if(!((bool)a.lexer || (bool)a.matcher || (bool)a.checker)) {
        a.matcher = true;
        a.checker = true;
    }
    if((bool)a.textreader && (bool)a.checker && !((bool)a.matcher) && !((bool)a.lexer)) {
            #line 81 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Warning: /textreader will be ignored because only /checker was indicated, which does not use a TextReader\r\n");
            #line 82 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

    }
}
catch(Exception ex) {
    failed = true;
    a._exception = ex;
    goto print_usage;
}
if(!failed) {
            #line 90 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(a._name);
            #line 90 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(" ");
            #line 90 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(a._version);
            #line 90 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(" ");
            #line 90 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

if(!(bool)a.ifstale || IsStale((string)a.input,(string)a.output)) {
            #line 91 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("preparing to generate ");
            #line 91 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(a.output);
            #line 91 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("\r\n");
            #line 92 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

    var isBinary  = IsBinaryInputFile(Arguments);
    var isLexer = false;
    if(isBinary && "rgl"==(string)a._fourcc)
        isLexer = true;
    if(!(bool)a.lexer && isBinary && isLexer) {
            #line 97 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Warning: The input file is a binary lexer. A lexer will be generated.\r\n");
            #line 98 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

        a.lexer = true;
        a.checker = false;
        a.matcher = false;
    }
    if(isBinary && !isLexer) {
        if((bool)a.lexer) {
            #line 104 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Warning: The input file is a binary matcher/checker. Matching/checking code will be generated.\r\n");
            #line 105 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

            a.lexer = false;
            if(!((bool)a.matcher || (bool)a.checker)) {
                a.matcher = true;
                a.checker = true;
            }
        }
    }
    if(!(bool)a.lexer && !(bool)a.matcher && (bool)a.lines) {
            #line 113 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Warning: /lines will be ignored because only /checker was specified\r\n");
            #line 114 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

    }
    if(!isBinary) {
            #line 116 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Parsing input and computing tables...\r\n");
            #line 117 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

    }
    LoadInputFile(Arguments);
    if(!isBinary) {
            #line 120 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Finished crunching input. Building output...\r\n");
            #line 121 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

    } else {
            #line 122 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Loaded state tables. Building output...\r\n");
            #line 123 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

    }

    if((bool)a.dot || (bool)a.jpg) {
        if((bool)a.lexer) {
            var fa = F.FFA.FromDfaTable((int[])a._dfa);
            if((bool)a.dot) {
                var opts = new F.FFA.DotGraphOptions();
                var fn = Path.Combine((string)a._cwd, ((string)a.@class) + ".dot");
            #line 131 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Writing ");
            #line 131 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(fn);
            #line 131 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("...\r\n");
            #line 132 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

                using(var sw=new StreamWriter(fn)) {
                    fa.WriteDotTo(sw,opts);
                }        
                for(var i = 0;i<((int[][])a._blockEndDfas).Length;++i) {
                    var be = ((int[][])a._blockEndDfas)[i];
                    if(be != null) {
                        fn = Path.Combine((string)a._cwd, ((string[])a._symbolTable)[i] + "BlockEnd.dot");
            #line 139 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Writing ");
            #line 139 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(fn);
            #line 139 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("...\r\n");
            #line 140 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

                        var befa = F.FFA.FromDfaTable(be);
                        using(var sw=new StreamWriter(fn)) {
                            befa.WriteDotTo(sw,opts);
                        }        
                    }
                }
            }
            if((bool)a.jpg) {
                var opts = new F.FFA.DotGraphOptions();
                var fn = Path.Combine((string)a._cwd, ((string)a.@class) + ".jpg");
            #line 150 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Writing ");
            #line 150 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(fn);
            #line 150 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("...\r\n");
            #line 151 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

                fa.RenderToFile(fn,opts);
                for(var i = 0;i<((int[][])a._blockEndDfas).Length;++i) {
                    var be = ((int[][])a._blockEndDfas)[i];
                    if(be != null) {
                        fn = Path.Combine((string)a._cwd, ((string[])a._symbolTable)[i] + "BlockEnd.jpg");
            #line 156 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Writing ");
            #line 156 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(fn);
            #line 156 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("...\r\n");
            #line 157 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

                        var befa = F.FFA.FromDfaTable(be);
                        befa.RenderToFile(fn,opts);
                                
                    }
                }
            }
        } else { // if((bool)a.lexer) ...
            var st = (string[])a._symbolTable;
            for(var i = 0;i<st.Length;++i) {
                var sti = st[i];
                if(sti != null && sti.Length > 0) {
                    var fa = F.FFA.FromDfaTable(((int[][])a._dfas)[i]);
                    var s = st[i];
                    if((bool)a.dot) {
                        var opts = new F.FFA.DotGraphOptions();
                        var fn = Path.Combine((string)a._cwd, s + ".dot");
            #line 173 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Writing ");
            #line 173 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(fn);
            #line 173 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("...\r\n");
            #line 174 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

                        using(var sw=new StreamWriter(fn)) {
                            fa.WriteDotTo(sw,opts);
                        }        
                        var be = ((int[][])a._blockEndDfas)[i];
                        if(be != null) {
                            fn = Path.Combine((string)a._cwd, ((string[])a._symbolTable)[i] + "BlockEnd.dot");
            #line 180 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Writing ");
            #line 180 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(fn);
            #line 180 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("...\r\n");
            #line 181 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

                            var befa = F.FFA.FromDfaTable(be);
                            using(var sw=new StreamWriter(fn)) {
                                befa.WriteDotTo(sw,opts);
                                
                            }
                        }       
                    } // if((bool)a.dot) ...
                    if((bool)a.jpg) {
                        var opts = new F.FFA.DotGraphOptions();
                        var fn = Path.Combine((string)a._cwd, s + ".jpg");
            #line 191 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Writing ");
            #line 191 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(fn);
            #line 191 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("...\r\n");
            #line 192 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

                        fa.RenderToFile(fn,opts);
                        var be = ((int[][])a._blockEndDfas)[i];
                        if(be != null) {
                            var befa = F.FFA.FromDfaTable(be);
                            fn = Path.Combine((string)a._cwd, ((string[])a._symbolTable)[i] + "BlockEnd.jpg");
            #line 197 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Writing ");
            #line 197 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(fn);
            #line 197 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("...\r\n");
            #line 198 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

                            befa.RenderToFile(fn,opts);
                                
                            
                        }
                    } // if((bool)a.jpg) ...
                } // if(sti != null) ...
            } // for(var i ...
        } // if((bool)a.lexer) ...
    } // if((bool)a.dot || (bool)a.jpg)
    TextWriter ost = null;
    try {
        bool isfile;
        ost=(isfile=(""!=(string)a.output))?new StreamWriter(Path.Combine((string)a._cwd,(string)a.output)):(TextWriter)a._stdout;
        var torun = ((string)a.target)+"TargetGenerator";
        if(isfile) {
            // truncate it because reasons
            ((StreamWriter)ost).BaseStream.SetLength(0L);
        }
        var itw = new Reggie.IndentedTextWriter(ost);
        Generate(Arguments,itw);
            #line 218 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Generation of ");
            #line 218 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(""!=(string)a.output?(string)a.output:"output");
            #line 218 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(" complete.\r\n");
            #line 219 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

    }
    finally {
        if(null!=ost && ((TextWriter)a._stdout)!=ost) {
            ost.Close();
        }
    }
} else {
            #line 226 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("skipping ");
            #line 226 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(a.output);
            #line 226 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(" because it is not stale.");
            #line 226 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"

}
} //if(!failed) ... 
            #line 228 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("\r\n");
            #line 229 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
return;
print_usage:

            #line 231 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("Usage: ");
            #line 231 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(a._exe);
            #line 231 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(" <input> [/output <output>] [/class <class>]\r\n   [/namespace <namespace>] [/database <database>]\r\n   [/token <token>] [/textreader] [/tables] [/lexer] \r\n   [/target <target>] [/lines] [/ignorecase] [/dot] [/jpg] \r\n   [/ifstale]\r\n            \r\n");
            #line 237 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(a._name);
            #line 237 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(" ");
            #line 237 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(a._version);
            #line 237 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(" - ");
            #line 237 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(a._description);
            #line 237 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("\r\n\r\n   <input>      The input specification file\r\n   <output>     The output source file - defaults to STDOUT\r\n   <class>      The name of the main class to generate \r\n       - default derived from <output>\r\n   <namespace>  The name of the namespace to use \r\n       - defaults to nothing\r\n   <database>   The name of the database to use (w/ SQL)\r\n       - defaults to nothing\r\n   <token>      The fully qualified name of an external token \r\n        - defaults to a tuple\r\n   <textreader> Generate TextReader instead of IEnumerable<char>\r\n                - C#/cs target only\r\n   <tables>     Generate DFA table code - defaults to compiled\r\n   <lexer>      Generate a lexer instead of matcher functions\r\n   <lines>      Generate line counting code\r\n        - defaults to non-line counted, only used with /lexer\r\n   <ignorecase> Generate case insensitive matches by default\r\n        - defaults to case sensitive\r\n   <target>     The output target to generate for\r\n       - default derived from <output> or \"cs\"\r\n       Supporte");
            Response.Write("d targets: ");
            #line 259 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(string.Join(", ",SupportedTargets));
            #line 259 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("\r\n   <dot>        Generate .dot files for the state machine(s)\r\n   <jpg>        Generate .jpg files for the state machine(s)\r\n       - requires GraphViz\r\n   <ifstale>    Skip unless <output> is newer than <input>\r\n      \r\n");
            #line 265 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
if(a._exception!=null) {
            #line 265 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write(((Exception)a._exception).Message );
            #line 265 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
}

            #line 266 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Write("\r\n");
            #line 267 "C:\Users\gazto\source\repos\Reggie\Reggie\Templates\Start.template"
            Response.Flush();
        }
    }
}
