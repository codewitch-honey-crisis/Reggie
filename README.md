# Reggie

Reggie is a DFA (non-backtracking) regular expression code generator for C#.

Rather than a regular expression *engine*, Reggie generates C# code to match text directly, based on inputted regular expressions.

It is essentially a tool to transform non-backtracking regular expressions into C# code.

The advantage of using it over .NET's intrinsic regular expression engine is that this engine will work over arbitrary forward only streams of text, exposed via `IEnumerable<char>` or `TextReader` without needing the input to be loaded into memory.

Furthermore, since it doesn't backtrack, it also performs better generally in many cases, although in certain situations Microsoft's can overtake this one. It all depends on the scenario.

Since the generated output is pure C#, these do affect application load times.

There are several binaries in the build directory. These are necessary to support the build process, and are not used at runtime. They are safe, and may be decompiled using .NETpeek or similar if you don't trust them.

Each has a codeproject.com article over at that site. They are *safe*.
