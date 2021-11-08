// This file was generated using Reggie 0.9.6.0 from the
// ImgScrape.rl specification file on 11/8/2021 6:24:56 AM UTC
namespace ImgScraper {
    
    /// <summary>Represents a matcher for the regular expressions in ImgScrape.rl</summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reggie", "0.9.6.0")]
    partial class ImgScrape {
        // The error message to report when an invalid UTF32 stream is encountered
        const string UnicodeSurrogateError = "Invalid surrogate found in Unicode stream";
        // Reads the next UTF32 codepoint off a text reader
        static int ReadUtf32(System.IO.TextReader reader, out int adv) {
            adv=1;
            var result = reader.Read();
            if (-1 != result) {
                if (char.IsHighSurrogate(unchecked((char)result))) {
                    ++adv;
                    var chl = reader.Read();
                    if (-1 == chl) throw new System.IO.IOException(UnicodeSurrogateError);
                    if (!char.IsLowSurrogate(unchecked((char)chl))) throw new System.IO.IOException(UnicodeSurrogateError);
                    result = char.ConvertToUtf32(unchecked((char)result), unchecked((char)chl));
                }
            }
            return result;
        }
        /// <summary>Returns all occurances of the expression indicated by ImgUrl within <paramref name="text"/></summary>
        /// <param name="text">The text to search</param>
        /// <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
        /// <returns>An enumeration of tuples used to retrieve the match values.</returns>
        /// <remarks>The matches contain both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> MatchImgUrl(System.IO.TextReader text, long position = 0) {
            int adv;
            var sb = new System.Text.StringBuilder();
            var absoluteIndex = 0L;
            var cursorPos = position;
            var absi = 0L;
            int ch;
            ch = ReadUtf32(text, out adv);
            while(ch != -1) {
                sb.Clear();
                position = cursorPos;
                absoluteIndex = absi;
            // q0
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q1;
                }
                goto next;
            q1:
                if((ch >= '\0' && ch <= '!') || (ch >= '#' && ch <= '[') || (ch >= ']' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q1;
                }
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q2;
                }
                if(ch == '\\') {
                    sb.Append(unchecked((char)ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q3;
                }
                goto next;
            q2:
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), Value: sb.ToString());
                }
                goto next;
            q3:
                if((ch >= '\0' && ch <= '!') || (ch >= '#' && ch <= '[') || (ch >= ']' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q1;
                }
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q4;
                }
                if(ch == '\\') {
                    sb.Append(unchecked((char)ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q3;
                }
                goto next;
            q4:
                if((ch >= '\0' && ch <= '!') || (ch >= '#' && ch <= '[') || (ch >= ']' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q1;
                }
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q2;
                }
                if(ch == '\\') {
                    sb.Append(unchecked((char)ch));
                    ch = ReadUtf32(text, out adv);
                    absi += adv;
                    ++cursorPos;
                    goto q3;
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), Value: sb.ToString());
                }
            next:
                ch = ReadUtf32(text, out adv);
                absi += adv;
                ++cursorPos;
            }
        }
    }
}

