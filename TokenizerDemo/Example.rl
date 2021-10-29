VerbatimStringLiteral= '@"([^"]|"")*"'
StringLiteral='"([^"]|\\.)*"'
CharacterLiteral= '[\']([^\']|\\.)[\']'
IntegerLiteral= '(0x[0-9A-Fa-f]{1,16}|([0-9]+))([Uu][Ll]?|[Ll][Uu]?)?'
FloatLiteral= '(([0-9]+)(\.[0-9]+)?([Ee][+-]?[0-9]+)?[DdMmFf]?)|((\.[0-9]+)([Ee][+-]?[0-9]+)?[DdMmFf]?)'
// the following takes a long time to generate
//Keyword = 'abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|volatile|while'
Whitespace<hidden>='[\t\r\n\v\f ]+'
Identifier='[_[:IsLetter:]][_[:IsLetterOrDigit:]]*'
CommentBlock<id=40,blockEnd="*/">="/*"
//Bar="bar"
