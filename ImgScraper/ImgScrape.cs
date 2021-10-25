namespace ImgScraper
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reggie", "0.5.0.0")]
    partial class ImgScrape
    {
        public struct Match
        {
            internal Match(string text, long position) {
                Text = text;
                Position = position;
            }
            public string Text { get; } 
            public long Position { get; }
        }
        static int _FetchNextInput(System.Collections.Generic.IEnumerator<char> cursor) {
            if(!cursor.MoveNext()) return -1;
            var chh = cursor.Current;
            int ch = chh;
            if(char.IsHighSurrogate(chh)) {
                if(!cursor.MoveNext()) throw new System.IO.IOException("Invalid surrogate found in Unicode stream");
                var chl = cursor.Current;
                if(!char.IsLowSurrogate(chl)) throw new System.IO.IOException("Invalid surrogate found in Unicode stream");
                ch = char.ConvertToUtf32(chh,chl);
            }
            return ch;
        }
        static int _FetchNextInput(System.IO.TextReader reader) {
            var result = reader.Read();
            if (-1 != result) {
                if (char.IsHighSurrogate((char)result)) {
                    var chl = reader.Read();
                    if (-1 == chl) throw new System.IO.IOException("Invalid surrogate found in Unicode stream");
                    if (!char.IsLowSurrogate((char)chl)) throw new System.IO.IOException("Invalid surrogate found in Unicode stream");
                    result = char.ConvertToUtf32((char)result, (char)chl);
                }
            }
            return result;
        }
        /// <summary>Validates that input character stream contains content that matches the ImgUrl expression.</summary>
        /// <param name="text">The text stream to validate. The entire stream must match the expression.</param>
        /// <returns>True if <paramref name="text"/> matches the expression indicated by ImgUrl, otherwise false.</returns>
        /// <remarks>ImgUrl is defined as '"((https?:/)?/)[^.]*\.(png|jpg|gif|svg)"'</remarks>
        public static bool IsImgUrl(System.Collections.Generic.IEnumerable<char> text) {
            var cursor = text.GetEnumerator();
            var ch = _FetchNextInput(cursor);
            if(ch == -1) return false;
        // q0
            if(ch == '\"') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q1;
            }
            return false;
        q1:
            if(ch == '/') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q2;
            }
            if(ch == 'h') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q12;
            }
            return false;
        q2:
            if((ch >= '\0' && ch <= '-') || (ch >= '/' && ch <= 1114111)) {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q2;
            }
            if(ch == '.') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q3;
            }
            return false;
        q3:
            if(ch == 'g') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q4;
            }
            if(ch == 'j') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q8;
            }
            if(ch == 'p') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q10;
            }
            if(ch == 's') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q11;
            }
            return false;
        q4:
            if(ch == 'i') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q5;
            }
            return false;
        q5:
            if(ch == 'f') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q6;
            }
            return false;
        q6:
            if(ch == '\"') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return true;
                goto q7;
            }
            return false;
        q7:
            return false;
        q8:
            if(ch == 'p') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q9;
            }
            return false;
        q9:
            if(ch == 'g') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q6;
            }
            return false;
        q10:
            if(ch == 'n') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q9;
            }
            return false;
        q11:
            if(ch == 'v') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q9;
            }
            return false;
        q12:
            if(ch == 't') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q13;
            }
            return false;
        q13:
            if(ch == 't') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q14;
            }
            return false;
        q14:
            if(ch == 'p') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q15;
            }
            return false;
        q15:
            if(ch == ':') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q16;
            }
            if(ch == 's') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q18;
            }
            return false;
        q16:
            if(ch == '/') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q17;
            }
            return false;
        q17:
            if(ch == '/') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q2;
            }
            return false;
        q18:
            if(ch == ':') {
                ch = _FetchNextInput(cursor);
                if(ch == -1)
                    return false;
                goto q16;
            }
            return false;
        }
        /// <summary>Finds occurrances of a string matching the ImgUrl expression.</summary>
        /// <param name="text">The text stream to match on.</param>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerable{Match}"/> object that enumerates the match information.</returns>
        /// <remarks>ImgUrl is defined as '"((https?:/)?/)[^.]*\.(png|jpg|gif|svg)"'</remarks>
        public static System.Collections.Generic.IEnumerable<Match> MatchImgUrl(System.Collections.Generic.IEnumerable<char> text) {
            var sb = new System.Text.StringBuilder();
            var position = 0L;
            var cursor = text.GetEnumerator();
            var cursorPos = 0L;
            var ch = _FetchNextInput(cursor);
            while (ch != -1) {
                sb.Clear();
                position = cursorPos;
            // q0
                if(ch == '\"') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q1;
                }
                goto next;
            q1:
                if(ch == '/') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q2;
                }
                if(ch == 'h') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q12;
                }
                goto next;
            q2:
                if((ch >= '\0' && ch <= '-') || (ch >= '/' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q2;
                }
                if(ch == '.') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q3;
                }
                goto next;
            q3:
                if(ch == 'g') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q4;
                }
                if(ch == 'j') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q8;
                }
                if(ch == 'p') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q10;
                }
                if(ch == 's') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q11;
                }
                goto next;
            q4:
                if(ch == 'i') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q5;
                }
                goto next;
            q5:
                if(ch == 'f') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q6;
                }
                goto next;
            q6:
                if(ch == '\"') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q7;
                }
                goto next;
            q7:
                if (sb.Length > 0) yield return new Match(sb.ToString(), position);
                goto next;
            q8:
                if(ch == 'p') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q9;
                }
                goto next;
            q9:
                if(ch == 'g') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q6;
                }
                goto next;
            q10:
                if(ch == 'n') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q9;
                }
                goto next;
            q11:
                if(ch == 'v') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q9;
                }
                goto next;
            q12:
                if(ch == 't') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q13;
                }
                goto next;
            q13:
                if(ch == 't') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q14;
                }
                goto next;
            q14:
                if(ch == 'p') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q15;
                }
                goto next;
            q15:
                if(ch == ':') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q16;
                }
                if(ch == 's') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q18;
                }
                goto next;
            q16:
                if(ch == '/') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q17;
                }
                goto next;
            q17:
                if(ch == '/') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q2;
                }
                goto next;
            q18:
                if(ch == ':') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(cursor);
                    ++cursorPos;
                    goto q16;
                }
            next:
                ch = _FetchNextInput(cursor);
                ++cursorPos;
            }
            yield break;
        }
        /// <summary>Finds occurrances of a string matching the ImgUrl expression.</summary>
        /// <param name="text">The text stream to match on.</param>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerable{Match}"/> object that enumerates the match information.</returns>
        /// <remarks>ImgUrl is defined as '"((https?:/)?/)[^.]*\.(png|jpg|gif|svg)"'</remarks>
        public static System.Collections.Generic.IEnumerable<Match> MatchImgUrl(System.IO.TextReader text) {
            var sb = new System.Text.StringBuilder();
            var position = 0L;
            var cursorPos = 0L;
            var ch = _FetchNextInput(text);
            while (ch != -1) {
                sb.Clear();
                position = cursorPos;
            // q0
                if(ch == '\"') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q1;
                }
                goto next;
            q1:
                if(ch == '/') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q2;
                }
                if(ch == 'h') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q12;
                }
                goto next;
            q2:
                if((ch >= '\0' && ch <= '-') || (ch >= '/' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q2;
                }
                if(ch == '.') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q3;
                }
                goto next;
            q3:
                if(ch == 'g') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q4;
                }
                if(ch == 'j') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q8;
                }
                if(ch == 'p') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q10;
                }
                if(ch == 's') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q11;
                }
                goto next;
            q4:
                if(ch == 'i') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q5;
                }
                goto next;
            q5:
                if(ch == 'f') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q6;
                }
                goto next;
            q6:
                if(ch == '\"') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q7;
                }
                goto next;
            q7:
                if (sb.Length > 0) yield return new Match(sb.ToString(), position);
                goto next;
            q8:
                if(ch == 'p') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q9;
                }
                goto next;
            q9:
                if(ch == 'g') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q6;
                }
                goto next;
            q10:
                if(ch == 'n') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q9;
                }
                goto next;
            q11:
                if(ch == 'v') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q9;
                }
                goto next;
            q12:
                if(ch == 't') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q13;
                }
                goto next;
            q13:
                if(ch == 't') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q14;
                }
                goto next;
            q14:
                if(ch == 'p') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q15;
                }
                goto next;
            q15:
                if(ch == ':') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q16;
                }
                if(ch == 's') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q18;
                }
                goto next;
            q16:
                if(ch == '/') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q17;
                }
                goto next;
            q17:
                if(ch == '/') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q2;
                }
                goto next;
            q18:
                if(ch == ':') {
                    sb.Append((char)ch);
                    ch = _FetchNextInput(text);
                    ++cursorPos;
                    goto q16;
                }
            next:
                ch = _FetchNextInput(text);
                ++cursorPos;
            }
            yield break;
        }
    }
}
