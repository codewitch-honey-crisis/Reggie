using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace LC{partial class LexContext{/// <summary>
/// Attempts to read whitespace from the current input, capturing it
/// </summary>
/// <returns>True if whitespace was read, otherwise false</returns>
public bool TryReadWhiteSpace(){EnsureStarted();if(-1==Current||!char.IsWhiteSpace((char)Current))return false;Capture();while(-1!=Advance()&&char.IsWhiteSpace((char)Current))
Capture();return true;}/// <summary>
/// Attempts to skip whitespace in the current input without capturing it
/// </summary>
/// <returns>True if whitespace was skipped, otherwise false</returns>
public bool TrySkipWhiteSpace(){EnsureStarted();if(-1==Current||!char.IsWhiteSpace((char)Current))return false;while(-1!=Advance()&&char.IsWhiteSpace((char)Current))
;return true;}/// <summary>
/// Attempts to read up until the specified character, optionally consuming it
/// </summary>
/// <param name="character">The character to halt at</param>
/// <param name="readCharacter">True if the character should be consumed, otherwise false</param>
/// <returns>True if the character was found, otherwise false</returns>
public bool TryReadUntil(int character,bool readCharacter=true){EnsureStarted();if(0>character)character=-1;Capture();if(Current==character){return true;
}while(-1!=Advance()&&Current!=character)Capture(); if(Current==character){if(readCharacter){Capture();Advance();}return true;}return false;}/// <summary>
/// Attempts to skip up until the specified character, optionally consuming it
/// </summary>
/// <param name="character">The character to halt at</param>
/// <param name="skipCharacter">True if the character should be consumed, otherwise false</param>
/// <returns>True if the character was found, otherwise false</returns>
public bool TrySkipUntil(int character,bool skipCharacter=true){EnsureStarted();if(0>character)character=-1;if(Current==character)return true;while(-1
!=Advance()&&Current!=character);if(Current==character){if(skipCharacter)Advance();return true;}return false;}/// <summary>
/// Attempts to read up until the specified character, using the specified escape, optionally consuming it
/// </summary>
/// <param name="character">The character to halt at</param>
/// <param name="escapeChar">The escape indicator character to use</param>
/// <param name="readCharacter">True if the character should be consumed, otherwise false</param>
/// <returns>True if the character was found, otherwise false</returns>
public bool TryReadUntil(int character,int escapeChar,bool readCharacter=true){EnsureStarted();if(0>character)character=-1;if(-1==Current)return false;
if(Current==character){if(readCharacter){Capture();Advance();}return true;}do{if(escapeChar==Current){Capture();if(-1==Advance())return false;Capture();
}else{if(character==Current){if(readCharacter){Capture();Advance();}return true;}else Capture();}}while(-1!=Advance());return false;}/// <summary>
/// Attempts to skip up until the specified character, using the specified escape, optionally consuming it
/// </summary>
/// <param name="character">The character to halt at</param>
/// <param name="escapeChar">The escape indicator character to use</param>
/// <param name="skipCharacter">True if the character should be consumed, otherwise false</param>
/// <returns>True if the character was found, otherwise false</returns>
public bool TrySkipUntil(int character,int escapeChar,bool skipCharacter=true){EnsureStarted();if(0>character)character=-1;if(Current==character)return
 true;while(-1!=Advance()&&Current!=character){if(character==escapeChar)if(-1==Advance())break;}if(Current==character){if(skipCharacter)Advance();return
 true;}return false;}private static bool _ContainsChar(char[]chars,char ch){foreach(char cmp in chars)if(cmp==ch)return true;return false;}/// <summary>
/// Attempts to read until any of the specified characters, optionally consuming it
/// </summary>
/// <param name="readCharacter">True if the character should be consumed, otherwise false</param>
/// <param name="anyOf">A list of characters that signal the end of the scan</param>
/// <returns>True if one of the characters was found, otherwise false</returns>
public bool TryReadUntil(bool readCharacter=true,params char[]anyOf){EnsureStarted();if(null==anyOf)anyOf=Array.Empty<char>();Capture();if(-1!=Current
&&_ContainsChar(anyOf,(char)Current)){if(readCharacter){Capture();Advance();}return true;}while(-1!=Advance()&&!_ContainsChar(anyOf,(char)Current))Capture();
if(-1!=Current&&_ContainsChar(anyOf,(char)Current)){if(readCharacter){Capture();Advance();}return true;}return false;}/// <summary>
/// Attempts to skip until any of the specified characters, optionally consuming it
/// </summary>
/// <param name="skipCharacter">True if the character should be consumed, otherwise false</param>
/// <param name="anyOf">A list of characters that signal the end of the scan</param>
/// <returns>True if one of the characters was found, otherwise false</returns>
public bool TrySkipUntil(bool skipCharacter=true,params char[]anyOf){EnsureStarted();if(null==anyOf)anyOf=Array.Empty<char>();if(-1!=Current&&_ContainsChar(anyOf,
(char)Current)){if(skipCharacter)Advance();return true;}while(-1!=Advance()&&!_ContainsChar(anyOf,(char)Current));if(-1!=Current&&_ContainsChar(anyOf,
(char)Current)){if(skipCharacter)Advance();return true;}return false;}/// <summary>
/// Reads up to the specified text string, consuming it
/// </summary>
/// <param name="text">The text to read until</param>
/// <returns>True if the text was found, otherwise false</returns>
public bool TryReadUntil(string text){EnsureStarted();if(string.IsNullOrEmpty(text))return false;while(-1!=Current&&TryReadUntil(text[0],false)){bool found
=true;for(int i=1;i<text.Length;++i){if(Advance()!=text[i]){found=false;break;}Capture();}if(found){Advance();return true;}}return false;}/// <summary>
/// Skips up to the specified text string, consuming it
/// </summary>
/// <param name="text">The text to skip until</param>
/// <returns>True if the text was found, otherwise false</returns>
public bool TrySkipUntil(string text){EnsureStarted();if(string.IsNullOrEmpty(text))return false;while(-1!=Current&&TrySkipUntil(text[0],false)){bool found
=true;for(int i=1;i<text.Length;++i){if(Advance()!=text[i]){found=false;break;}}if(found){Advance();return true;}}return false;}/// <summary>
/// Attempts to read a series of digits, consuming them
/// </summary>
/// <returns>True if digits were consumed, otherwise false</returns>
public bool TryReadDigits(){EnsureStarted();if(-1==Current||!char.IsDigit((char)Current))return false;Capture();while(-1!=Advance()&&char.IsDigit((char)Current))
Capture();return true;}/// <summary>
/// Attempts to skip a series of digits, consuming them
/// </summary>
/// <returns>True if digits were consumed, otherwise false</returns>
public bool TrySkipDigits(){EnsureStarted();if(-1==Current||!char.IsDigit((char)Current))return false;while(-1!=Advance()&&char.IsDigit((char)Current))
;return true;}/// <summary>
/// Attempts to read a series of letters, consuming them
/// </summary>
/// <returns>True if letters were consumed, otherwise false</returns>
public bool TryReadLetters(){EnsureStarted();if(-1==Current||!char.IsLetter((char)Current))return false;Capture();while(-1!=Advance()&&char.IsLetter((char)Current))
Capture();return true;}/// <summary>
/// Attempts to skip a series of letters, consuming them
/// </summary>
/// <returns>True if letters were consumed, otherwise false</returns>
public bool TrySkipLetters(){EnsureStarted();if(-1==Current||!char.IsLetter((char)Current))return false;while(-1!=Advance()&&char.IsLetter((char)Current))
;return true;}/// <summary>
/// Attempts to read a series of letters or digits, consuming them
/// </summary>
/// <returns>True if letters or digits were consumed, otherwise false</returns>
public bool TryReadLettersOrDigits(){EnsureStarted();if(-1==Current||!char.IsLetterOrDigit((char)Current))return false;Capture();while(-1!=Advance()&&
char.IsLetterOrDigit((char)Current))Capture();return true;}/// <summary>
/// Attempts to skip a series of letters or digits, consuming them
/// </summary>
/// <returns>True if letters or digits were consumed, otherwise false</returns>
public bool TrySkipLettersOrDigits(){EnsureStarted();if(-1==Current||!char.IsLetterOrDigit((char)Current))return false;while(-1!=Advance()&&char.IsLetterOrDigit((char)Current))
;return true;}}}namespace LC{partial class LexContext{/// <summary>
/// Indicates if the character is hex
/// </summary>
/// <param name="hex">The character to examine</param>
/// <returns>True if the character is a valid hex character, otherwise false</returns>
internal static bool IsHexChar(char hex){return(':'>hex&&'/'<hex)||('G'>hex&&'@'<hex)||('g'>hex&&'`'<hex);}/// <summary>
/// Converts a hex character to its byte representation
/// </summary>
/// <param name="hex">The character to convert</param>
/// <returns>The byte that the character represents</returns>
internal static byte FromHexChar(char hex){if(':'>hex&&'/'<hex)return(byte)(hex-'0');if('G'>hex&&'@'<hex)return(byte)(hex-'7'); if('g'>hex&&'`'<hex)return
(byte)(hex-'W'); throw new ArgumentException("The value was not hex.","hex");}/// <summary>
/// Attempts to read a generic integer into the capture buffer
/// </summary>
/// <returns>True if a valid integer was read, otherwise false</returns>
public bool TryReadInteger(){EnsureStarted();bool neg=false;if('-'==Current){neg=true;Capture();Advance();}else if('0'==Current){Capture();Advance();if
(-1==Current)return true;return!char.IsDigit((char)Current);}if(-1==Current||(neg&&'0'==Current)||!char.IsDigit((char)Current))return false;if(!TryReadDigits())
return false;return true;}/// <summary>
/// Attempts to skip a generic integer without capturing
/// </summary>
/// <returns>True if an integer was found and skipped, otherwise false</returns>
public bool TrySkipInteger(){EnsureStarted();bool neg=false;if('-'==Current){neg=true;Advance();}else if('0'==Current){Advance();if(-1==Current)return
 true;return!char.IsDigit((char)Current);}if(-1==Current||(neg&&'0'==Current)||!char.IsDigit((char)Current))return false;if(!TrySkipDigits())return false;
return true;}/// <summary>
/// Attempts to read a C# integer into the capture buffer while parsing it
/// </summary>
/// <param name="result">The value the literal represents</param>
/// <returns>True if the value was a valid literal, otherwise false</returns>
public bool TryParseInteger(out object result){result=null;EnsureStarted();if(-1==Current)return false;bool neg=false;if('-'==Current){Capture();Advance();
neg=true;}int l=CaptureBuffer.Length;if(TryReadDigits()){string num=CaptureBuffer.ToString(l,CaptureBuffer.Length-l);if(neg)num='-'+num;int r;if(int.TryParse(num,
out r)){result=r;return true;}long ll;if(long.TryParse(num,out ll)){result=ll;return true;}System.Numerics.BigInteger b;if(System.Numerics.BigInteger.TryParse(num,
out b)){result=b;return true;}}return false;}/// <summary>
/// Reads a C# integer literal into the capture buffer while parsing it
/// </summary>
/// <returns>The value the literal represents</returns>
/// <exception cref="ExpectingException">The input was not valid</exception>
public object ParseInteger(){EnsureStarted();Expecting('-','0','1','2','3','4','5','6','7','8','9');bool neg=('-'==Current);if(neg){Advance();Expecting('0',
'1','2','3','4','5','6','7','8','9');}System.Numerics.BigInteger i=0;if(!neg){i+=((char)Current)-'0';while(-1!=Advance()&&char.IsDigit((char)Current))
{i*=10;i+=((char)Current)-'0';}}else{i-=((char)Current)-'0';while(-1!=Advance()&&char.IsDigit((char)Current)){i*=10;i-=((char)Current)-'0';}}if(i<=int.MaxValue
&&i>=int.MinValue)return(int)i;else if(i<=long.MaxValue&&i>=long.MinValue)return(long)i;return i;}/// <summary>
/// Attempts to read a generic floating point number into the capture buffer
/// </summary>
/// <returns>True if a valid floating point number was read, otherwise false</returns>
public bool TryReadReal(){EnsureStarted();bool readAny=false;if('-'==Current){Capture();Advance();}if(char.IsDigit((char)Current)){if(!TryReadDigits())
return false;readAny=true;}if('.'==Current){Capture();Advance();if(!TryReadDigits())return false;readAny=true;}if('E'==Current||'e'==Current){Capture();
Advance();if('-'==Current||'+'==Current){Capture();Advance();}return TryReadDigits();}return readAny;}/// <summary>
/// Attempts to skip a generic floating point literal without capturing
/// </summary>
/// <returns>True if a literal was found and skipped, otherwise false</returns>
public bool TrySkipReal(){bool readAny=false;EnsureStarted();if('-'==Current)Advance();if(char.IsDigit((char)Current)){if(!TrySkipDigits())return false;
readAny=true;}if('.'==Current){Advance();if(!TrySkipDigits())return false;readAny=true;}if('E'==Current||'e'==Current){Advance();if('-'==Current||'+'==
Current)Advance();return TrySkipDigits();}return readAny;}/// <summary>
/// Attempts to read a floating point literal into the capture buffer while parsing it
/// </summary>
/// <param name="result">The value the literal represents</param>
/// <returns>True if the value was a valid literal, otherwise false</returns>
public bool TryParseReal(out double result){result=default(double);int l=CaptureBuffer.Length;if(!TryReadReal())return false;return double.TryParse(CaptureBuffer.ToString(l,
CaptureBuffer.Length-l),out result);}/// <summary>
/// Reads a floating point literal into the capture buffer while parsing it
/// </summary>
/// <returns>The value the literal represents</returns>
/// <exception cref="ExpectingException">The input was not valid</exception>
public double ParseReal(){EnsureStarted();var sb=new StringBuilder();Expecting('-','.','0','1','2','3','4','5','6','7','8','9');bool neg=('-'==Current);
if(neg){sb.Append((char)Current);Advance();Expecting('.','0','1','2','3','4','5','6','7','8','9');}while(-1!=Current&&char.IsDigit((char)Current)){sb.Append((char)Current);
Advance();}if('.'==Current){sb.Append((char)Current);Advance();Expecting('0','1','2','3','4','5','6','7','8','9');sb.Append((char)Current);while(-1!=Advance()
&&char.IsDigit((char)Current)){sb.Append((char)Current);}}if('E'==Current||'e'==Current){sb.Append((char)Current);Advance();Expecting('+','-','0','1',
'2','3','4','5','6','7','8','9');switch(Current){case'+':case'-':sb.Append((char)Current);Advance();break;}Expecting('0','1','2','3','4','5','6','7','8',
'9');sb.Append((char)Current);while(-1!=Advance()){Expecting('0','1','2','3','4','5','6','7','8','9');sb.Append((char)Current);}}return double.Parse(sb.ToString());
}/// <summary>
/// Attempts to read the specified literal from the input, optionally checking if it is terminated
/// </summary>
/// <param name="literal">The literal to attempt to read</param>
/// <param name="checkTerminated">If true, will check the end to make sure it's not a letter or digit</param>
/// <returns></returns>
public bool TryReadLiteral(string literal,bool checkTerminated=true){foreach(char ch in literal){if(Current==ch){Capture();if(-1==Advance())break;}}if
(checkTerminated)return-1==Current||!char.IsLetterOrDigit((char)Current);return true;}/// <summary>
/// Attempts to skip the specified literal without capturing, optionally checking for termination
/// </summary>
/// <param name="literal">The literal to skip</param>
/// <param name="checkTerminated">True if the literal should be checked for termination by a charcter other than a letter or digit, otherwise false</param>
/// <returns>True if the literal was found and skipped, otherwise false</returns>
public bool TrySkipLiteral(string literal,bool checkTerminated=true){foreach(char ch in literal){if(Current==ch){if(-1==Advance())break;}}if(checkTerminated)
return-1==Current||!char.IsLetterOrDigit((char)Current);return true;}/// <summary>
/// Attempts to read a C style line comment into the capture buffer
/// </summary>
/// <returns>True if a valid comment was read, otherwise false</returns>
public bool TryReadCLineComment(){EnsureStarted();if('/'!=Current)return false;Capture();if('/'!=Advance())return false;Capture();while(-1!=Advance()&&
'\r'!=Current&&'\n'!=Current)Capture();return true;}/// <summary>
/// Attempts to skip the a C style line comment without capturing
/// </summary>
/// <returns>True if a comment was found and skipped, otherwise false</returns>
public bool TrySkipCLineComment(){EnsureStarted();if('/'!=Current)return false;if('/'!=Advance())return false;while(-1!=Advance()&&'\r'!=Current&&'\n'
!=Current);return true;}/// <summary>
/// Attempts to read a C style block comment into the capture buffer
/// </summary>
/// <returns>True if a valid comment was read, otherwise false</returns>
public bool TryReadCBlockComment(){EnsureStarted();if('/'!=Current)return false;Capture();if('*'!=Advance())return false;Capture();if(-1==Advance())return
 false;return TryReadUntil("*/");}/// <summary>
/// Attempts to skip the C style block comment without capturing
/// </summary>
/// <returns>True if a comment was found and skipped, otherwise false</returns>
public bool TrySkipCBlockComment(){EnsureStarted();if('/'!=Current)return false;if('*'!=Advance())return false;if(-1==Advance())return false;return TrySkipUntil("*/");
}/// <summary>
/// Attempts to read a C style comment into the capture buffer
/// </summary>
/// <returns>True if a valid comment was read, otherwise false</returns>
public bool TryReadCComment(){EnsureStarted();if('/'!=Current)return false;Capture();if('*'==Advance()){Capture();if(-1==Advance())return false;return
 TryReadUntil("*/");}if('/'==Current){Capture();while(-1!=Advance()&&'\r'!=Current&&'\n'!=Current)Capture();return true;}return false;}/// <summary>
/// Attempts to skip the a C style comment value without capturing
/// </summary>
/// <returns>True if a comment was found and skipped, otherwise false</returns>
public bool TrySkipCComment(){EnsureStarted();if('/'!=Current)return false;if('*'==Advance()){if(-1==Advance())return false;return TrySkipUntil("*/");
}if('/'==Current){while(-1!=Advance()&&'\r'!=Current&&'\n'!=Current);return true;}return false;}/// <summary>
/// Attempts to read C style comments or whitespace into the capture buffer
/// </summary>
/// <returns>True if a valid comment or whitespace was read, otherwise false</returns>
public bool TryReadCCommentsAndWhitespace(){bool result=false;while(-1!=Current){if(!TryReadWhiteSpace()&&!TryReadCComment())break;result=true;}if(TryReadWhiteSpace())
result=true;return result;}/// <summary>
/// Attempts to skip the a C style comment or whitespace value without capturing
/// </summary>
/// <returns>True if a comment or whitespace was found and skipped, otherwise false</returns>
public bool TrySkipCCommentsAndWhiteSpace(){bool result=false;while(-1!=Current){if(!TrySkipWhiteSpace()&&!TrySkipCComment())break;result=true;}if(TrySkipWhiteSpace())
result=true;return result;}/// <summary>
/// Attempts to read a C style identifier into the capture buffer
/// </summary>
/// <returns>True if a valid identifier was read, otherwise false</returns>
public bool TryReadCIdentifier(){EnsureStarted();if(-1==Current||!('_'==Current||char.IsLetter((char)Current)))return false;Capture();while(-1!=Advance()
&&('_'==Current||char.IsLetterOrDigit((char)Current)))Capture();return true;}/// <summary>
/// Attempts to skip the a C style identifier value without capturing
/// </summary>
/// <returns>True if an identifier was found and skipped, otherwise false</returns>
public bool TrySkipCIdentifier(){EnsureStarted();if(-1==Current||!('_'==Current||char.IsLetter((char)Current)))return false;while(-1!=Advance()&&('_'==
Current||char.IsLetterOrDigit((char)Current)));return true;}/// <summary>
/// Attempts to read a C style string into the capture buffer
/// </summary>
/// <returns>True if a valid string was read, otherwise false</returns>
public bool TryReadCString(){EnsureStarted();if('\"'!=Current)return false;Capture();while(-1!=Advance()&&'\r'!=Current&&'\n'!=Current&&'\"'!=Current)
{Capture();if('\\'==Current){if(-1==Advance()||'\r'==Current||'\n'==Current)return false;Capture();}}if('\"'==Current){Capture();Advance(); return true;
}return false;}/// <summary>
/// Attempts to skip a C style string literal without capturing
/// </summary>
/// <returns>True if a literal was found and skipped, otherwise false</returns>
public bool TrySkipCString(){EnsureStarted();if('\"'!=Current)return false;while(-1!=Advance()&&'\r'!=Current&&'\n'!=Current&&'\"'!=Current)if('\\'==Current)
if(-1==Advance()||'\r'==Current||'\n'==Current)return false;if('\"'==Current){Advance(); return true;}return false;}}}namespace LC{/// <summary>
/// Represents an exception encountered while lexing or parsing from an input source
/// </summary>
#if LCLIB
public
#endif
sealed class ExpectingException:Exception{/// <summary>
/// Creates a new <see cref="ExpectingException" />
/// </summary>
/// <param name="message">The error message - this will be appended with the location information</param>
/// <param name="line">The one based line number where the exception occured</param>
/// <param name="column">The one based column number where the exception occured</param>
/// <param name="position">The zero based position where the exception occured</param>
/// <param name="fileOrUrl">The file or URL in which the exception occured</param>
/// <param name="expecting">A list of expected symbols or characters</param>
public ExpectingException(string message,int line,int column,long position,string fileOrUrl,params string[]expecting):base(_GetMessage(message,line,column,position,fileOrUrl))
{Line=line;Column=column;Position=position;FileOrUrl=fileOrUrl;Expecting=expecting;}/// <summary>
/// Indicates the one based line where the exception occured
/// </summary>
public int Line{get;}/// <summary>
/// Indicates the one based column where the exception occured
/// </summary>
public int Column{get;}/// <summary>
/// Indicates the zero based position where the exception occured
/// </summary>
public long Position{get;}/// <summary>
/// Indicates the file or URL in which the exception occured
/// </summary>
public string FileOrUrl{get;}/// <summary>
/// Indicates a list of expecting characters or symbols
/// </summary>
public string[]Expecting{get;}static string _GetMessage(string m,int l,int c,long p,string f){var sb=new StringBuilder();sb.Append(m);sb.Append(" at line ");
sb.Append(l.ToString());sb.Append(", column ");sb.Append(c.ToString());sb.Append(", position ");sb.Append(p.ToString());if(!string.IsNullOrEmpty(f)){sb.Append(", in ");
sb.Append(f);}return sb.ToString();}}/// <summary>
/// Provides error reporting, location tracking, lifetime and start/end management over an input cursor
/// </summary>
public abstract partial class LexContext:IDisposable{/// <summary>
/// Indicates the default tab width of an input device
/// </summary>
public const int DefaultTabWidth=4;/// <summary>
/// Represents the end of input symbol
/// </summary>
public const int EndOfInput=-1;/// <summary>
/// Represents the before input symbol
/// </summary>
public const int BeforeInput=-2;/// <summary>
/// Represents a symbol for the disposed state
/// </summary>
public const int Disposed=-3;int _current=BeforeInput;int _line;int _column;long _position;string _fileOrUrl;/// <summary>
/// Indicates the tab width of the input device
/// </summary>
public int TabWidth{get;set;}=DefaultTabWidth;/// <summary>
/// Indicates the current one based line number
/// </summary>
public int Line{get{return _line;}}/// <summary>
/// Indicates the current one based column number
/// </summary>
public int Column{get{return _column;}}/// <summary>
/// Indicates the current zero based position
/// </summary>
public long Position{get{return _position;}}/// <summary>
/// Indicates the current filename or URL, if any could be discerned
/// </summary>
public string FileOrUrl{get{return _fileOrUrl;}}/// <summary>
/// Provides access to the capture buffer, a <see cref="StringBuilder" />
/// </summary>
public StringBuilder CaptureBuffer{get;}=new StringBuilder();/// <summary>
/// Gets the current character under the cursor or <see cref="BeforeInput"/>, <see cref="EndOfInput" />, or <see cref="Disposed" />
/// </summary>
public int Current{get{return _current;}}internal LexContext(){_line=1;_column=0;_position=0L;}~LexContext(){Close();}/// <summary>
/// Creates a <see cref="LexContext" /> over an enumeration of characters, which can be a string, character array, or other source
/// </summary>
/// <param name="input">The input characters</param>
/// <returns>A new <see cref="LexContext" /> over the input</returns>
public static LexContext Create(IEnumerable<char>input){return new _CharEnumeratorLexContext(input);}/// <summary>
/// Creates a <see cref="LexContext" /> over a <see cref="TextReader"/>
/// </summary>
/// <param name="input">The input reader</param>
/// <returns>A new <see cref="LexContext" /> over the input</returns>
public static LexContext CreateFrom(TextReader input){ string fn=null;var sr=input as StreamReader;if(null!=sr){var fstm=sr.BaseStream as FileStream;if(null!=fstm)
{fn=fstm.Name;}}var result=new _TextReaderLexContext(input);if(!string.IsNullOrEmpty(fn))result._fileOrUrl=fn;return result;}/// <summary>
/// Creates a <see cref="LexContext" /> over a file
/// </summary>
/// <param name="filename">The file</param>
/// <returns>A new <see cref="LexContext" /> over the file</returns>
public static LexContext CreateFrom(string filename){return CreateFrom(new StreamReader(filename));}/// <summary>
/// Creates a <see cref="LexContext" /> over an URL
/// </summary>
/// <param name="url">The URL</param>
/// <returns>A new <see cref="LexContext" /> over the URL</returns>
public static LexContext CreateFromUrl(string url){var wreq=WebRequest.Create(url);var wrsp=wreq.GetResponse();var result=CreateFrom(new StreamReader(wrsp.GetResponseStream()));
result._fileOrUrl=url;return result;}/// <summary>
/// Closes the current instance and releases any resources being held
/// </summary>
public void Close(){if(Disposed!=_current){_current=Disposed;GC.SuppressFinalize(this);CloseInner();CaptureBuffer.Clear();}}/// <summary>
/// Sets the location information for this instance
/// </summary>
/// <param name="line">The one based line number</param>
/// <param name="column">The one based column number</param>
/// <param name="position">The zero based position</param>
/// <param name="fileOrUrl">The file or URL</param>
public void SetLocation(int line,int column,long position,string fileOrUrl){_line=line;_column=column;_position=position;_fileOrUrl=fileOrUrl;}/// <summary>
/// Gets all or a subset of the current capture buffer
/// </summary>
/// <param name="startIndex">The start index</param>
/// <param name="length">The number of characters to retrieve, or zero to retrieve the remainder of the buffer</param>
/// <returns>A string containing the specified subset of the capture buffer</returns>
public string GetCapture(int startIndex=0,int length=0){_CheckDisposed();if(0==length)length=CaptureBuffer.Length-startIndex;return CaptureBuffer.ToString(startIndex,
length);}/// <summary>
/// Clears the capture buffer
/// </summary>
public void ClearCapture(){_CheckDisposed();CaptureBuffer.Clear();}/// <summary>
/// Captures the current character under the cursor, if any
/// </summary>
public void Capture(){_CheckDisposed();if(EndOfInput!=_current&&BeforeInput!=_current)CaptureBuffer.Append((char)_current);}/// <summary>
/// Verifies that one of the specified characters is under the input cursor. If it isn't, a <see cref="ExpectingException" /> is raised.
/// </summary>
/// <param name="expecting">The list of expected characters. If empty, anything but end of input is accepted. If <see cref="EndOfInput" /> is included, end of input is accepted.</param>
[System.Diagnostics.DebuggerHidden()]public void Expecting(params int[]expecting){_CheckDisposed();if(BeforeInput==_current)throw new ExpectingException("The cursor is before the beginning of the input",
_line,_column,_position,_fileOrUrl);switch(expecting.Length){case 0:if(EndOfInput==_current)throw new ExpectingException("Unexpected end of input",_line,_column,_position,_fileOrUrl);
break;case 1:if(expecting[0]!=_current)throw new ExpectingException(_GetErrorMessage(expecting),_line,_column,_position,_fileOrUrl,_GetErrorExpecting(expecting));
break;default:if(0>Array.IndexOf(expecting,_current))throw new ExpectingException(_GetErrorMessage(expecting),_line,_column,_position,_fileOrUrl,_GetErrorExpecting(expecting));
break;}}string[]_GetErrorExpecting(int[]expecting){var result=new string[expecting.Length];for(var i=0;i<expecting.Length;i++){if(-1!=expecting[i])result[i]
=Convert.ToString(expecting[i]);else result[i]="end of input";}return result;}string _GetErrorMessage(int[]expecting){StringBuilder sb=null;switch(expecting.Length)
{case 0:break;case 1:sb=new StringBuilder();if(-1==expecting[0])sb.Append("end of input");else{sb.Append("\"");sb.Append((char)expecting[0]);sb.Append("\"");
}break;case 2:sb=new StringBuilder();if(-1==expecting[0])sb.Append("end of input");else{sb.Append("\"");sb.Append((char)expecting[0]);sb.Append("\"");
}sb.Append(" or ");if(-1==expecting[1])sb.Append("end of input");else{sb.Append("\"");sb.Append((char)expecting[1]);sb.Append("\"");}break;default: sb
=new StringBuilder();if(-1==expecting[0])sb.Append("end of input");else{sb.Append("\"");sb.Append((char)expecting[0]);sb.Append("\"");}int l=expecting.Length
-1;int i=1;for(;i<l;++i){sb.Append(", ");if(-1==expecting[i])sb.Append("end of input");else{sb.Append("\"");sb.Append((char)expecting[i]);sb.Append("\"");
}}sb.Append(", or ");if(-1==expecting[i])sb.Append("end of input");else{sb.Append("\"");sb.Append((char)expecting[i]);sb.Append("\"");}break;}if(-1==Current)
{if(0==expecting.Length)return"Unexpected end of input";return string.Concat("Unexpected end of input. Expecting ",sb.ToString());}if(0==expecting.Length)
return string.Concat("Unexpected character \"",(char)Current,"\" in input");return string.Concat("Unexpected character \"",(char)Current,"\" in input. Expecting ",
sb.ToString());}void IDisposable.Dispose(){Close();}public void EnsureStarted(){_CheckDisposed();if(BeforeInput==_current)Advance();}public int Advance()
{_CheckDisposed();if(EndOfInput==_current)return EndOfInput;_current=AdvanceInner();switch(_current){case'\n':++_line;_column=0;break;case'\r':_column
=0;break;case'\t':_column+=TabWidth;break;default: if(!char.IsHighSurrogate(unchecked((char)_current)))++_column;break;}++_position;return _current;}protected
 abstract int AdvanceInner();protected abstract void CloseInner();void _CheckDisposed(){if(Disposed==_current)throw new ObjectDisposedException(typeof(LexContext).Name);
}
#region _TextReaderLexContext
private sealed class _TextReaderLexContext:LexContext{TextReader _inner;internal _TextReaderLexContext(TextReader inner){_inner=inner;}protected override
 int AdvanceInner(){return _inner.Read();}protected override void CloseInner(){_inner.Close();}}
#endregion
#region _CharEnumeratorLexContext
private sealed class _CharEnumeratorLexContext:LexContext{IEnumerator<char>_inner;internal _CharEnumeratorLexContext(IEnumerable<char>inner){_inner=inner.GetEnumerator();
}protected override int AdvanceInner(){if(!_inner.MoveNext())return EndOfInput;return _inner.Current;}protected override void CloseInner(){_inner.Dispose();
}}
#endregion
}}namespace LC{partial class LexContext{/// <summary>
/// Attempts to read a JSON string into the capture buffer
/// </summary>
/// <returns>True if a valid string was read, otherwise false</returns>
public bool TryReadJsonString(){EnsureStarted();if('\"'!=Current)return false;Capture();while(-1!=Advance()&&'\r'!=Current&&'\n'!=Current&&Current!='\"')
{Capture();if('\\'==Current){if(-1==Advance()||'\r'==Current||'\n'==Current)return false;Capture();}}if(Current=='\"'){Capture();Advance(); return true;
}return false;}/// <summary>
/// Attempts to skip a JSON string literal without capturing
/// </summary>
/// <returns>True if a literal was found and skipped, otherwise false</returns>
public bool TrySkipJsonString(){EnsureStarted();if('\"'!=Current)return false;while(-1!=Advance()&&'\r'!=Current&&'\n'!=Current&&Current!='\"')if('\\'
==Current)if(-1==Advance()||'\r'==Current||'\n'==Current)return false;if(Current=='\"'){Advance(); return true;}return false;}/// <summary>
/// Attempts to read a JSON string literal into the capture buffer while parsing it
/// </summary>
/// <param name="result">The value the literal represents</param>
/// <returns>True if the value was a valid literal, otherwise false</returns>
public bool TryParseJsonString(out string result){result=null;var sb=new StringBuilder();EnsureStarted();if('\"'!=Current)return false;Capture();while
(-1!=Advance()&&'\r'!=Current&&'\n'!=Current&&Current!='\"'){Capture();if('\\'==Current){if(-1==Advance()||'\r'==Current||'\n'==Current)return false;Capture();
switch(Current){case'f':sb.Append('\f');break;case'r':sb.Append('\r');break;case'n':sb.Append('\n');break;case't':sb.Append('\t');break;case'\\':sb.Append('\\');
break;case'/':sb.Append('/');break;case'\"':sb.Append('\"');break;case'b':sb.Append('\b');break;case'u':Capture();if(-1==Advance())return false;int ch
=0;if(!IsHexChar((char)Current))return false;ch<<=4;ch|=FromHexChar((char)Current);Capture();if(-1==Advance())return false;if(!IsHexChar((char)Current))
return false;ch<<=4;ch|=FromHexChar((char)Current);Capture();if(-1==Advance())return false;if(!IsHexChar((char)Current))return false;ch<<=4;ch|=FromHexChar((char)Current);
Capture();if(-1==Advance())return false;if(!IsHexChar((char)Current))return false;ch<<=4;ch|=FromHexChar((char)Current);sb.Append((char)ch);break;default:
return false;}}else sb.Append((char)Current);}if(Current=='\"'){Capture();Advance(); result=sb.ToString();return true;}return false;}/// <summary>
/// Reads a JSON string literal into the capture buffer while parsing it
/// </summary>
/// <returns>The value the literal represents</returns>
/// <exception cref="ExpectingException">The input was not valid</exception>
public string ParseJsonString(){var sb=new StringBuilder();EnsureStarted();Expecting('\"');while(-1!=Advance()&&'\r'!=Current&&'\n'!=Current&&Current!=
'\"'){if('\\'==Current){Advance();switch(Current){case'b':sb.Append('\b');break;case'f':sb.Append('\f');break;case'n':sb.Append('\n');break;case'r':sb.Append('\r');
break;case't':sb.Append('\t');break;case'\\':sb.Append('\\');break;case'\"':sb.Append('\"');break;case'u':int ch=0;Advance();Expecting('0','1','2','3',
'4','5','6','7','8','9','A','a','B','b','C','c','D','d','E','e','F','f');ch<<=4;ch|=FromHexChar((char)Current);Advance();Expecting('0','1','2','3','4',
'5','6','7','8','9','A','a','B','b','C','c','D','d','E','e','F','f');ch<<=4;ch|=FromHexChar((char)Current);Advance();Expecting('0','1','2','3','4','5',
'6','7','8','9','A','a','B','b','C','c','D','d','E','e','F','f');ch<<=4;ch|=FromHexChar((char)Current);Advance();Expecting('0','1','2','3','4','5','6',
'7','8','9','A','a','B','b','C','c','D','d','E','e','F','f');ch<<=4;ch|=FromHexChar((char)Current);sb.Append((char)ch);break;default:Expecting('b','n',
'r','t','\\','/','\"','u');break;}}else sb.Append((char)Current);}Expecting('\"');Advance();return sb.ToString();}/// <summary>
/// Attempts to read a JSON value into the capture buffer
/// </summary>
/// <returns>True if a valid value was read, otherwise false</returns>
public bool TryReadJsonValue(){TryReadWhiteSpace();if('t'==Current){Capture();if(Advance()!='r')return false;Capture();if(Advance()!='u')return false;
Capture();if(Advance()!='e')return false;if(-1!=Advance()&&char.IsLetterOrDigit((char)Current))return false;return true;}if('f'==Current){Capture();if
(Advance()!='a')return false;Capture();if(Advance()!='l')return false;Capture();if(Advance()!='s')return false;Capture();if(Advance()!='e')return false;
Capture();if(-1!=Advance()&&char.IsLetterOrDigit((char)Current))return false;return true;}if('n'==Current){Capture();if(Advance()!='u')return false;Capture();
if(Advance()!='l')return false;Capture();if(Advance()!='l')return false;Capture();if(-1!=Advance()&&char.IsLetterOrDigit((char)Current))return false;return
 true;}if('-'==Current||'.'==Current||char.IsDigit((char)Current))return TryReadReal();if('\"'==Current)return TryReadJsonString();if('['==Current){Capture();
Advance();if(TryReadJsonValue()){TryReadWhiteSpace();while(','==Current){Capture();Advance();if(!TryReadJsonValue())return false;TryReadWhiteSpace();}
}TryReadWhiteSpace();if(']'!=Current)return false;Capture();Advance();return true;}if('{'==Current){Capture();Advance();TryReadWhiteSpace();if(TryReadJsonString())
{TryReadWhiteSpace();if(':'!=Current)return false;Capture();Advance();if(!TryReadJsonValue())return false;TryReadWhiteSpace();while(','==Current){Capture();
Advance();TryReadWhiteSpace();if(!TryReadJsonString())return false;TryReadWhiteSpace();if(':'!=Current)return false;Capture();Advance();if(!TryReadJsonValue())
return false;TryReadWhiteSpace();}}TryReadWhiteSpace();if('}'!=Current)return false;Capture();Advance();return true;}return false;}/// <summary>
/// Attempts to skip the a JSON value without capturing
/// </summary>
/// <returns>True if a value was found and skipped, otherwise false</returns>
public bool TrySkipJsonValue(){TrySkipWhiteSpace();if('t'==Current){if(Advance()!='r')return false;if(Advance()!='u')return false;if(Advance()!='e')return
 false;if(-1!=Advance()&&char.IsLetterOrDigit((char)Current))return false;return true;}if('f'==Current){if(Advance()!='a')return false;if(Advance()!='l')
return false;if(Advance()!='s')return false;if(Advance()!='e')return false;if(-1!=Advance()&&char.IsLetterOrDigit((char)Current))return false;return true;
}if('n'==Current){if(Advance()!='u')return false;if(Advance()!='l')return false;if(Advance()!='l')return false;if(-1!=Advance()&&char.IsLetterOrDigit((char)Current))
return false;return true;}if('-'==Current||'.'==Current||char.IsDigit((char)Current))return TrySkipReal();if('\"'==Current)return TrySkipJsonString();
if('['==Current){Advance();if(TrySkipJsonValue()){TrySkipWhiteSpace();while(','==Current){Advance();if(!TrySkipJsonValue())return false;TrySkipWhiteSpace();
}}TrySkipWhiteSpace();if(']'!=Current)return false;Advance();return true;}if('{'==Current){Advance();TrySkipWhiteSpace();if(TrySkipJsonString()){TrySkipWhiteSpace();
if(':'!=Current)return false;Advance();if(!TrySkipJsonValue())return false;TrySkipWhiteSpace();while(','==Current){Advance();TrySkipWhiteSpace();if(!TrySkipJsonString())
return false;TrySkipWhiteSpace();if(':'!=Current)return false;Advance();if(!TrySkipJsonValue())return false;TrySkipWhiteSpace();}}TrySkipWhiteSpace();
if('}'!=Current)return false;Advance();return true;}return false;}/// <summary>
/// Attempts to read a JSON value into the capture buffer while parsing it
/// </summary>
/// <param name="result"><see cref="IDictionary{String,Object}"/> for a JSON object, <see cref="IList{Object}"/> for a JSON array, or the appropriate scalar value</param>
/// <returns>True if the value was a valid value, otherwise false</returns>
public bool TryParseJsonValue(out object result){result=null;TryReadWhiteSpace();if('t'==Current){Capture();if(Advance()!='r')return false;Capture();if
(Advance()!='u')return false;Capture();if(Advance()!='e')return false;if(-1!=Advance()&&char.IsLetterOrDigit((char)Current))return false;result=true;return
 true;}if('f'==Current){Capture();if(Advance()!='a')return false;Capture();if(Advance()!='l')return false;Capture();if(Advance()!='s')return false;Capture();
if(Advance()!='e')return false;Capture();if(-1!=Advance()&&char.IsLetterOrDigit((char)Current))return false;result=false;return true;}if('n'==Current)
{Capture();if(Advance()!='u')return false;Capture();if(Advance()!='l')return false;Capture();if(Advance()!='l')return false;Capture();if(-1!=Advance()
&&char.IsLetterOrDigit((char)Current))return false;return true;}if('-'==Current||'.'==Current||char.IsDigit((char)Current)){double r;if(TryParseReal(out
 r)){result=r;return true;}return false;}if('\"'==Current){string s;if(TryParseJsonString(out s)){result=s;return true;}return false;}if('['==Current)
{Capture();Advance();var l=new List<object>();object v;if(TryParseJsonValue(out v)){l.Add(v);TryReadWhiteSpace();while(','==Current){Capture();Advance();
if(!TryParseJsonValue(out v))return false;l.Add(v);TryReadWhiteSpace();}}TryReadWhiteSpace();if(']'!=Current)return false;Capture();Advance();result=l;
return true;}if('{'==Current){Capture();Advance();TryReadWhiteSpace();string n;object v;var d=new Dictionary<string,object>();if(TryParseJsonString(out
 n)){TryReadWhiteSpace();if(':'!=Current)return false;Capture();Advance();if(!TryParseJsonValue(out v))return false;d.Add(n,v);TryReadWhiteSpace();while
(','==Current){Capture();Advance();TryReadWhiteSpace();if(!TryParseJsonString(out n))return false;TryReadWhiteSpace();if(':'!=Current)return false;Capture();
Advance();if(!TryParseJsonValue(out v))return false;d.Add(n,v);TryReadWhiteSpace();}}TryReadWhiteSpace();if('}'!=Current)return false;Capture();Advance();
result=d;return true;}return false;}/// <summary>
/// Reads a JSON value into the capture buffer while parsing it
/// </summary>
/// <returns><see cref="IDictionary{String,Object}"/> for a JSON object, <see cref="IList{Object}"/> for a JSON array, or the appropriate scalar value</returns>
/// <exception cref="ExpectingException">The input was not valid</exception>
public object ParseJsonValue(){TrySkipWhiteSpace();if('t'==Current){Advance();Expecting('r');Advance();Expecting('u');Advance();Expecting('e');Advance();
return true;}if('f'==Current){Advance();Expecting('a');Advance();Expecting('l');Advance();Expecting('s');Advance();Expecting('e');Advance();return true;
}if('n'==Current){Advance();Expecting('u');Advance();Expecting('l');Advance();Expecting('l');Advance();return null;}if('-'==Current||'.'==Current||char.IsDigit((char)Current))
return ParseReal();if('\"'==Current)return ParseJsonString();if('['==Current){Advance();TrySkipWhiteSpace();var l=new List<object>();if(']'!=Current){
l.Add(ParseJsonValue());TrySkipWhiteSpace();while(','==Current){Advance();l.Add(ParseJsonValue());TrySkipWhiteSpace();}}TrySkipWhiteSpace();Expecting(']');
Advance();return l;}if('{'==Current){Advance();TrySkipWhiteSpace();var d=new Dictionary<string,object>();if('}'!=Current){string n=ParseJsonString();TrySkipWhiteSpace();
Expecting(':');Advance();object v=ParseJsonValue();d.Add(n,v);TrySkipWhiteSpace();while(','==Current){Advance();TrySkipWhiteSpace();n=ParseJsonString();
TrySkipWhiteSpace();Expecting(':');Advance();v=ParseJsonValue();d.Add(n,v);TrySkipWhiteSpace();}}TrySkipWhiteSpace();if('}'!=Current)return false;Advance();
return d;}return false;}}}