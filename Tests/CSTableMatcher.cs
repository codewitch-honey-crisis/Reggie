// This file was generated using Reggie 0.9.6.0 from the
// Test.rgg specification file on 11/11/2021 7:34:20 AM UTC
namespace Test {
    
    /// <summary>Represents a matcher for the regular expressions in Test.rgg</summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reggie", "0.9.6.0")]
    partial class CSTableMatcher {
        // The error message to report when an invalid UTF32 stream is encountered
        const string UnicodeSurrogateError = "Invalid surrogate found in Unicode stream";
        // Reads the next UTF32 codepoint off an enumerator
        static int ReadUtf32(System.Collections.Generic.IEnumerator<char> cursor, out int adv) {
            adv = 1;
            if(!cursor.MoveNext()) return -1;
            var chh = cursor.Current;
            int result = chh;
            if(char.IsHighSurrogate(chh)) {
                ++adv;
                if(!cursor.MoveNext()) throw new System.IO.IOException(UnicodeSurrogateError);
                var chl = cursor.Current;
                if(!char.IsLowSurrogate(chl)) throw new System.IO.IOException(UnicodeSurrogateError);
                result = char.ConvertToUtf32(chh,chl);
            }
            return result;
        }
        // Matches text based on a DFA table and block end DFA table
        static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> TableMatch(int[] dfa, int[] blockEnd, System.Collections.Generic.IEnumerable<char> text, long position) {
            int adv;
            var sb = new System.Text.StringBuilder();
            var cursor = text.GetEnumerator();
            int tlen;
            int tto;
            int prlen;
            int pmin;
            int pmax;
            int i;
            int j;
            var absoluteIndex = 0L;
            var cursorPos = position;
            var absi = 0L;
            int ch;
            var state = 0;
            bool done;
            var acc = -1;
            ch = ReadUtf32(cursor, out adv);
            while(ch != -1) {
                sb.Clear();
                position = cursorPos;
                absoluteIndex = absi;
                acc = -1;
                done = false;
                while(!done) {
                start_dfa:
                    done = true;
                    acc = dfa[state++];
                    tlen = dfa[state++];
                    for (i = 0; i < tlen; ++i) {
                        tto = dfa[state++];
                        prlen = dfa[state++];
                        for (j = 0; j < prlen; ++j) {
                            pmin = dfa[state++];
                            pmax = dfa[state++];
                            if(ch < pmin) break;
                            if (ch <= pmax) {
                                sb.Append(char.ConvertFromUtf32(ch));
                                ch = ReadUtf32(cursor, out adv);
                                absi += adv;
                                ++cursorPos;
                                state = tto;
                                done = false;
                                
                                            goto start_dfa;
                            }
                        }
                    }
                }
                if(acc != -1) {
                    if(blockEnd != null) {
                        state = 0;
                        while(ch != -1) {
                            acc = -1;
                            done = false;
                            while(!done) {
                            start_block_end:
                                done = true;
                                acc = blockEnd[state++];
                                tlen = blockEnd[state++];
                                for (i = 0; i < tlen; ++i) {
                                    tto = blockEnd[state++];
                                    prlen = blockEnd[state++];
                                    for (j = 0; j < prlen; ++j) {
                                        pmin = blockEnd[state++];
                                        pmax = blockEnd[state++];
                                        if(ch < pmin) break;
                                        if (ch <= pmax) {
                                            sb.Append(char.ConvertFromUtf32(ch));
                                            ch = ReadUtf32(cursor, out adv);
                                            absi += adv;
                                            ++cursorPos;
                                            state = tto;
                                            done = false;
                                            
                                                        goto start_block_end;
                                        }
                                    }
                                }
                            }
                            if(acc != -1) {
                                yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), Value: sb.ToString());
                                break;
                            }
                            else {
                                sb.Append(char.ConvertFromUtf32(ch));
                                ch = ReadUtf32(cursor, out adv);
                                absi += adv;
                                ++cursorPos;
                            }
                            state = 0;
                        }
                        state = 0;
                        continue;
                    }
                    else {
                        if(sb.Length > 0) {
                            yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), Value: sb.ToString());
                        }
                    }
                }
                ch = ReadUtf32(cursor, out adv);
                absi += adv;
                ++cursorPos;
                state = 0;
            }
        }
        /// <summary>Returns all occurances of the expression indicated by VerbatimStringLiteral within <paramref name="text"/></summary>
        /// <param name="text">The text to search</param>
        /// <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
        /// <returns>An enumeration of tuples used to retrieve the match values.</returns>
        /// <remarks>The matches contain both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> MatchVerbatimStringLiteral(System.Collections.Generic.IEnumerable<char> text, long position = 0) {
            foreach(var result in TableMatch(VerbatimStringLiteralDfaStateTable, null, text, position)) {
            	yield return result;
            }
        }
        /// <summary>Returns all occurances of the expression indicated by StringLiteral within <paramref name="text"/></summary>
        /// <param name="text">The text to search</param>
        /// <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
        /// <returns>An enumeration of tuples used to retrieve the match values.</returns>
        /// <remarks>The matches contain both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> MatchStringLiteral(System.Collections.Generic.IEnumerable<char> text, long position = 0) {
            foreach(var result in TableMatch(StringLiteralDfaStateTable, null, text, position)) {
            	yield return result;
            }
        }
        /// <summary>Returns all occurances of the expression indicated by CharacterLiteral within <paramref name="text"/></summary>
        /// <param name="text">The text to search</param>
        /// <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
        /// <returns>An enumeration of tuples used to retrieve the match values.</returns>
        /// <remarks>The matches contain both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> MatchCharacterLiteral(System.Collections.Generic.IEnumerable<char> text, long position = 0) {
            foreach(var result in TableMatch(CharacterLiteralDfaStateTable, null, text, position)) {
            	yield return result;
            }
        }
        /// <summary>Returns all occurances of the expression indicated by IntegerLiteral within <paramref name="text"/></summary>
        /// <param name="text">The text to search</param>
        /// <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
        /// <returns>An enumeration of tuples used to retrieve the match values.</returns>
        /// <remarks>The matches contain both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> MatchIntegerLiteral(System.Collections.Generic.IEnumerable<char> text, long position = 0) {
            foreach(var result in TableMatch(IntegerLiteralDfaStateTable, null, text, position)) {
            	yield return result;
            }
        }
        /// <summary>Returns all occurances of the expression indicated by FloatLiteral within <paramref name="text"/></summary>
        /// <param name="text">The text to search</param>
        /// <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
        /// <returns>An enumeration of tuples used to retrieve the match values.</returns>
        /// <remarks>The matches contain both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> MatchFloatLiteral(System.Collections.Generic.IEnumerable<char> text, long position = 0) {
            foreach(var result in TableMatch(FloatLiteralDfaStateTable, null, text, position)) {
            	yield return result;
            }
        }
        /// <summary>Returns all occurances of the expression indicated by Whitespace within <paramref name="text"/></summary>
        /// <param name="text">The text to search</param>
        /// <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
        /// <returns>An enumeration of tuples used to retrieve the match values.</returns>
        /// <remarks>The matches contain both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> MatchWhitespace(System.Collections.Generic.IEnumerable<char> text, long position = 0) {
            foreach(var result in TableMatch(WhitespaceDfaStateTable, null, text, position)) {
            	yield return result;
            }
        }
        /// <summary>Returns all occurances of the expression indicated by Identifier within <paramref name="text"/></summary>
        /// <param name="text">The text to search</param>
        /// <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
        /// <returns>An enumeration of tuples used to retrieve the match values.</returns>
        /// <remarks>The matches contain both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> MatchIdentifier(System.Collections.Generic.IEnumerable<char> text, long position = 0) {
            foreach(var result in TableMatch(IdentifierDfaStateTable, null, text, position)) {
            	yield return result;
            }
        }
        /// <summary>Returns all occurances of the expression indicated by CommentBlock within <paramref name="text"/></summary>
        /// <param name="text">The text to search</param>
        /// <param name="position">The logical position in codepoints where the search started. By default assumes the beginning of the stream.</param>
        /// <returns>An enumeration of tuples used to retrieve the match values.</returns>
        /// <remarks>The matches contain both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each match. The former is useful for locating the match within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, string Value)> MatchCommentBlock(System.Collections.Generic.IEnumerable<char> text, long position = 0) {
            foreach(var result in TableMatch(CommentBlockDfaStateTable, CommentBlockBlockEndDfaStateTable, text, position)) {
            	yield return result;
            }
        }
        // DFA state machine table
        static readonly int[] VerbatimStringLiteralDfaStateTable = new int[] {
            -1, 1, 6, 1, 64, 64, -1, 1, 12, 1, 34, 34, -1, 2, 12, 2, 0, 33, 35, 1114111, 
            24, 1, 34, 34, 0, 1, 12, 1, 34, 34
        };
        // DFA state machine table
        static readonly int[] StringLiteralDfaStateTable = new int[] {
            -1, 1, 6, 1, 34, 34, -1, 3, 6, 3, 0, 33, 35, 91, 93, 1114111, 24, 1, 34, 34, 
            26, 1, 92, 92, 1, 0, -1, 3, 6, 3, 0, 33, 35, 91, 93, 1114111, 44, 1, 34, 34, 
            26, 1, 92, 92, 1, 3, 6, 3, 0, 33, 35, 91, 93, 1114111, 24, 1, 34, 34, 26, 1, 
            92, 92
        };
        // DFA state machine table
        static readonly int[] CharacterLiteralDfaStateTable = new int[] {
            -1, 1, 6, 1, 39, 39, -1, 2, 20, 3, 0, 38, 40, 91, 93, 1114111, 28, 1, 92, 92, 
            -1, 1, 26, 1, 39, 39, 2, 0, -1, 2, 20, 2, 0, 38, 40, 1114111, 40, 1, 39, 39, 
            2, 1, 26, 1, 39, 39
        };
        // DFA state machine table
        static readonly int[] IntegerLiteralDfaStateTable = new int[] {
            -1, 2, 10, 1, 48, 48, 32, 1, 49, 57, 3, 4, 32, 1, 48, 57, 50, 2, 76, 76, 
            108, 108, 60, 2, 85, 85, 117, 117, 68, 1, 120, 120, 3, 3, 32, 1, 48, 57, 50, 2, 
            76, 76, 108, 108, 60, 2, 85, 85, 117, 117, 3, 1, 58, 2, 85, 85, 117, 117, 3, 0, 
            3, 1, 58, 2, 76, 76, 108, 108, -1, 1, 78, 3, 48, 57, 65, 70, 97, 102, 3, 3, 
            100, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 60, 2, 85, 85, 117, 117, 
            3, 3, 122, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 60, 2, 85, 85, 
            117, 117, 3, 3, 144, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 60, 2, 
            85, 85, 117, 117, 3, 3, 166, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 
            60, 2, 85, 85, 117, 117, 3, 3, 188, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 
            108, 108, 60, 2, 85, 85, 117, 117, 3, 3, 210, 3, 48, 57, 65, 70, 97, 102, 50, 2, 
            76, 76, 108, 108, 60, 2, 85, 85, 117, 117, 3, 3, 232, 3, 48, 57, 65, 70, 97, 102, 
            50, 2, 76, 76, 108, 108, 60, 2, 85, 85, 117, 117, 3, 3, 254, 3, 48, 57, 65, 70, 
            97, 102, 50, 2, 76, 76, 108, 108, 60, 2, 85, 85, 117, 117, 3, 3, 276, 3, 48, 57, 
            65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 60, 2, 85, 85, 117, 117, 3, 3, 298, 3, 
            48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 60, 2, 85, 85, 117, 117, 3, 3, 
            320, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 60, 2, 85, 85, 117, 117, 
            3, 3, 342, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 60, 2, 85, 85, 
            117, 117, 3, 3, 364, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 60, 2, 
            85, 85, 117, 117, 3, 3, 386, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 108, 108, 
            60, 2, 85, 85, 117, 117, 3, 3, 408, 3, 48, 57, 65, 70, 97, 102, 50, 2, 76, 76, 
            108, 108, 60, 2, 85, 85, 117, 117, 3, 2, 50, 2, 76, 76, 108, 108, 60, 2, 85, 85, 
            117, 117
        };
        // DFA state machine table
        static readonly int[] FloatLiteralDfaStateTable = new int[] {
            -1, 2, 10, 1, 46, 46, 82, 1, 48, 57, -1, 1, 16, 1, 48, 57, 4, 3, 16, 1, 
            48, 57, 42, 6, 68, 68, 70, 70, 77, 77, 100, 100, 102, 102, 109, 109, 44, 2, 69, 69, 
            101, 101, 4, 0, -1, 2, 56, 2, 43, 43, 45, 45, 62, 1, 48, 57, -1, 1, 62, 1, 
            48, 57, 4, 2, 62, 1, 48, 57, 42, 6, 68, 68, 70, 70, 77, 77, 100, 100, 102, 102, 
            109, 109, 4, 4, 10, 1, 46, 46, 82, 1, 48, 57, 42, 6, 68, 68, 70, 70, 77, 77, 
            100, 100, 102, 102, 109, 109, 44, 2, 69, 69, 101, 101
        };
        // DFA state machine table
        static readonly int[] WhitespaceDfaStateTable = new int[] {
            -1, 1, 8, 2, 9, 13, 32, 32, 5, 1, 8, 2, 9, 13, 32, 32
        };
        // DFA state machine table
        static readonly int[] IdentifierDfaStateTable = new int[] {
            -1, 1, 1112, 554, 65, 90, 97, 122, 170, 170, 181, 181, 186, 186, 192, 214, 216, 246, 248, 705, 
            710, 721, 736, 740, 748, 748, 750, 750, 880, 884, 886, 887, 890, 893, 895, 895, 902, 902, 904, 906, 
            908, 908, 910, 929, 931, 1013, 1015, 1153, 1162, 1327, 1329, 1366, 1369, 1369, 1377, 1415, 1488, 1514, 1520, 1522, 
            1568, 1610, 1646, 1647, 1649, 1747, 1749, 1749, 1765, 1766, 1774, 1775, 1786, 1788, 1791, 1791, 1808, 1808, 1810, 1839, 
            1869, 1957, 1969, 1969, 1994, 2026, 2036, 2037, 2042, 2042, 2048, 2069, 2074, 2074, 2084, 2084, 2088, 2088, 2112, 2136, 
            2208, 2228, 2308, 2361, 2365, 2365, 2384, 2384, 2392, 2401, 2417, 2432, 2437, 2444, 2447, 2448, 2451, 2472, 2474, 2480, 
            2482, 2482, 2486, 2489, 2493, 2493, 2510, 2510, 2524, 2525, 2527, 2529, 2544, 2545, 2565, 2570, 2575, 2576, 2579, 2600, 
            2602, 2608, 2610, 2611, 2613, 2614, 2616, 2617, 2649, 2652, 2654, 2654, 2674, 2676, 2693, 2701, 2703, 2705, 2707, 2728, 
            2730, 2736, 2738, 2739, 2741, 2745, 2749, 2749, 2768, 2768, 2784, 2785, 2809, 2809, 2821, 2828, 2831, 2832, 2835, 2856, 
            2858, 2864, 2866, 2867, 2869, 2873, 2877, 2877, 2908, 2909, 2911, 2913, 2929, 2929, 2947, 2947, 2949, 2954, 2958, 2960, 
            2962, 2965, 2969, 2970, 2972, 2972, 2974, 2975, 2979, 2980, 2984, 2986, 2990, 3001, 3024, 3024, 3077, 3084, 3086, 3088, 
            3090, 3112, 3114, 3129, 3133, 3133, 3160, 3162, 3168, 3169, 3205, 3212, 3214, 3216, 3218, 3240, 3242, 3251, 3253, 3257, 
            3261, 3261, 3294, 3294, 3296, 3297, 3313, 3314, 3333, 3340, 3342, 3344, 3346, 3386, 3389, 3389, 3406, 3406, 3423, 3425, 
            3450, 3455, 3461, 3478, 3482, 3505, 3507, 3515, 3517, 3517, 3520, 3526, 3585, 3632, 3634, 3635, 3648, 3654, 3713, 3714, 
            3716, 3716, 3719, 3720, 3722, 3722, 3725, 3725, 3732, 3735, 3737, 3743, 3745, 3747, 3749, 3749, 3751, 3751, 3754, 3755, 
            3757, 3760, 3762, 3763, 3773, 3773, 3776, 3780, 3782, 3782, 3804, 3807, 3840, 3840, 3904, 3911, 3913, 3948, 3976, 3980, 
            4096, 4138, 4159, 4159, 4176, 4181, 4186, 4189, 4193, 4193, 4197, 4198, 4206, 4208, 4213, 4225, 4238, 4238, 4256, 4293, 
            4295, 4295, 4301, 4301, 4304, 4346, 4348, 4680, 4682, 4685, 4688, 4694, 4696, 4696, 4698, 4701, 4704, 4744, 4746, 4749, 
            4752, 4784, 4786, 4789, 4792, 4798, 4800, 4800, 4802, 4805, 4808, 4822, 4824, 4880, 4882, 4885, 4888, 4954, 4992, 5007, 
            5024, 5109, 5112, 5117, 5121, 5740, 5743, 5759, 5761, 5786, 5792, 5866, 5873, 5880, 5888, 5900, 5902, 5905, 5920, 5937, 
            5952, 5969, 5984, 5996, 5998, 6000, 6016, 6067, 6103, 6103, 6108, 6108, 6176, 6263, 6272, 6312, 6314, 6314, 6320, 6389, 
            6400, 6430, 6480, 6509, 6512, 6516, 6528, 6571, 6576, 6601, 6656, 6678, 6688, 6740, 6823, 6823, 6917, 6963, 6981, 6987, 
            7043, 7072, 7086, 7087, 7098, 7141, 7168, 7203, 7245, 7247, 7258, 7293, 7401, 7404, 7406, 7409, 7413, 7414, 7424, 7615, 
            7680, 7957, 7960, 7965, 7968, 8005, 8008, 8013, 8016, 8023, 8025, 8025, 8027, 8027, 8029, 8029, 8031, 8061, 8064, 8116, 
            8118, 8124, 8126, 8126, 8130, 8132, 8134, 8140, 8144, 8147, 8150, 8155, 8160, 8172, 8178, 8180, 8182, 8188, 8305, 8305, 
            8319, 8319, 8336, 8348, 8450, 8450, 8455, 8455, 8458, 8467, 8469, 8469, 8473, 8477, 8484, 8484, 8486, 8486, 8488, 8488, 
            8490, 8493, 8495, 8505, 8508, 8511, 8517, 8521, 8526, 8526, 8579, 8580, 11264, 11310, 11312, 11358, 11360, 11492, 11499, 11502, 
            11506, 11507, 11520, 11557, 11559, 11559, 11565, 11565, 11568, 11623, 11631, 11631, 11648, 11670, 11680, 11686, 11688, 11694, 11696, 11702, 
            11704, 11710, 11712, 11718, 11720, 11726, 11728, 11734, 11736, 11742, 11823, 11823, 12293, 12294, 12337, 12341, 12347, 12348, 12353, 12438, 
            12445, 12447, 12449, 12538, 12540, 12543, 12549, 12589, 12593, 12686, 12704, 12730, 12784, 12799, 13312, 19893, 19968, 40917, 40960, 42124, 
            42192, 42237, 42240, 42508, 42512, 42527, 42538, 42539, 42560, 42606, 42623, 42653, 42656, 42725, 42775, 42783, 42786, 42888, 42891, 42925, 
            42928, 42935, 42999, 43009, 43011, 43013, 43015, 43018, 43020, 43042, 43072, 43123, 43138, 43187, 43250, 43255, 43259, 43259, 43261, 43261, 
            43274, 43301, 43312, 43334, 43360, 43388, 43396, 43442, 43471, 43471, 43488, 43492, 43494, 43503, 43514, 43518, 43520, 43560, 43584, 43586, 
            43588, 43595, 43616, 43638, 43642, 43642, 43646, 43695, 43697, 43697, 43701, 43702, 43705, 43709, 43712, 43712, 43714, 43714, 43739, 43741, 
            43744, 43754, 43762, 43764, 43777, 43782, 43785, 43790, 43793, 43798, 43808, 43814, 43816, 43822, 43824, 43866, 43868, 43877, 43888, 44002, 
            44032, 55203, 55216, 55238, 55243, 55291, 63744, 64109, 64112, 64217, 64256, 64262, 64275, 64279, 64285, 64285, 64287, 64296, 64298, 64310, 
            64312, 64316, 64318, 64318, 64320, 64321, 64323, 64324, 64326, 64433, 64467, 64829, 64848, 64911, 64914, 64967, 65008, 65019, 65136, 65140, 
            65142, 65276, 65313, 65338, 65345, 65370, 65382, 65470, 65474, 65479, 65482, 65487, 65490, 65495, 65498, 65500, 65536, 65547, 65549, 65574, 
            65576, 65594, 65596, 65597, 65599, 65613, 65616, 65629, 65664, 65786, 66176, 66204, 66208, 66256, 66304, 66335, 66352, 66368, 66370, 66377, 
            66384, 66421, 66432, 66461, 66464, 66499, 66504, 66511, 66560, 66717, 66816, 66855, 66864, 66915, 67072, 67382, 67392, 67413, 67424, 67431, 
            67584, 67589, 67592, 67592, 67594, 67637, 67639, 67640, 67644, 67644, 67647, 67669, 67680, 67702, 67712, 67742, 67808, 67826, 67828, 67829, 
            67840, 67861, 67872, 67897, 67968, 68023, 68030, 68031, 68096, 68096, 68112, 68115, 68117, 68119, 68121, 68147, 68192, 68220, 68224, 68252, 
            68288, 68295, 68297, 68324, 68352, 68405, 68416, 68437, 68448, 68466, 68480, 68497, 68608, 68680, 68736, 68786, 68800, 68850, 69635, 69687, 
            69763, 69807, 69840, 69864, 69891, 69926, 69968, 70002, 70006, 70006, 70019, 70066, 70081, 70084, 70106, 70106, 70108, 70108, 70144, 70161, 
            70163, 70187, 70272, 70278, 70280, 70280, 70282, 70285, 70287, 70301, 70303, 70312, 70320, 70366, 70405, 70412, 70415, 70416, 70419, 70440, 
            70442, 70448, 70450, 70451, 70453, 70457, 70461, 70461, 70480, 70480, 70493, 70497, 70784, 70831, 70852, 70853, 70855, 70855, 71040, 71086, 
            71128, 71131, 71168, 71215, 71236, 71236, 71296, 71338, 71424, 71449, 71840, 71903, 71935, 71935, 72384, 72440, 73728, 74649, 74880, 75075, 
            77824, 78894, 82944, 83526, 92160, 92728, 92736, 92766, 92880, 92909, 92928, 92975, 92992, 92995, 93027, 93047, 93053, 93071, 93952, 94020, 
            94032, 94032, 94099, 94111, 110592, 110593, 113664, 113770, 113776, 113788, 113792, 113800, 113808, 113817, 119808, 119892, 119894, 119964, 119966, 119967, 
            119970, 119970, 119973, 119974, 119977, 119980, 119982, 119993, 119995, 119995, 119997, 120003, 120005, 120069, 120071, 120074, 120077, 120084, 120086, 120092, 
            120094, 120121, 120123, 120126, 120128, 120132, 120134, 120134, 120138, 120144, 120146, 120485, 120488, 120512, 120514, 120538, 120540, 120570, 120572, 120596, 
            120598, 120628, 120630, 120654, 120656, 120686, 120688, 120712, 120714, 120744, 120746, 120770, 120772, 120779, 124928, 125124, 126464, 126467, 126469, 126495, 
            126497, 126498, 126500, 126500, 126503, 126503, 126505, 126514, 126516, 126519, 126521, 126521, 126523, 126523, 126530, 126530, 126535, 126535, 126537, 126537, 
            126539, 126539, 126541, 126543, 126545, 126546, 126548, 126548, 126551, 126551, 126553, 126553, 126555, 126555, 126557, 126557, 126559, 126559, 126561, 126562, 
            126564, 126564, 126567, 126570, 126572, 126578, 126580, 126583, 126585, 126588, 126590, 126590, 126592, 126601, 126603, 126619, 126625, 126627, 126629, 126633, 
            126635, 126651, 131072, 173782, 173824, 177972, 177984, 178205, 178208, 183969, 194560, 195101, 6, 1, 1112, 587, 48, 57, 65, 90, 
            97, 122, 170, 170, 181, 181, 186, 186, 192, 214, 216, 246, 248, 705, 710, 721, 736, 740, 748, 748, 
            750, 750, 880, 884, 886, 887, 890, 893, 895, 895, 902, 902, 904, 906, 908, 908, 910, 929, 931, 1013, 
            1015, 1153, 1162, 1327, 1329, 1366, 1369, 1369, 1377, 1415, 1488, 1514, 1520, 1522, 1568, 1610, 1632, 1641, 1646, 1647, 
            1649, 1747, 1749, 1749, 1765, 1766, 1774, 1788, 1791, 1791, 1808, 1808, 1810, 1839, 1869, 1957, 1969, 1969, 1984, 2026, 
            2036, 2037, 2042, 2042, 2048, 2069, 2074, 2074, 2084, 2084, 2088, 2088, 2112, 2136, 2208, 2228, 2308, 2361, 2365, 2365, 
            2384, 2384, 2392, 2401, 2406, 2415, 2417, 2432, 2437, 2444, 2447, 2448, 2451, 2472, 2474, 2480, 2482, 2482, 2486, 2489, 
            2493, 2493, 2510, 2510, 2524, 2525, 2527, 2529, 2534, 2545, 2565, 2570, 2575, 2576, 2579, 2600, 2602, 2608, 2610, 2611, 
            2613, 2614, 2616, 2617, 2649, 2652, 2654, 2654, 2662, 2671, 2674, 2676, 2693, 2701, 2703, 2705, 2707, 2728, 2730, 2736, 
            2738, 2739, 2741, 2745, 2749, 2749, 2768, 2768, 2784, 2785, 2790, 2799, 2809, 2809, 2821, 2828, 2831, 2832, 2835, 2856, 
            2858, 2864, 2866, 2867, 2869, 2873, 2877, 2877, 2908, 2909, 2911, 2913, 2918, 2927, 2929, 2929, 2947, 2947, 2949, 2954, 
            2958, 2960, 2962, 2965, 2969, 2970, 2972, 2972, 2974, 2975, 2979, 2980, 2984, 2986, 2990, 3001, 3024, 3024, 3046, 3055, 
            3077, 3084, 3086, 3088, 3090, 3112, 3114, 3129, 3133, 3133, 3160, 3162, 3168, 3169, 3174, 3183, 3205, 3212, 3214, 3216, 
            3218, 3240, 3242, 3251, 3253, 3257, 3261, 3261, 3294, 3294, 3296, 3297, 3302, 3311, 3313, 3314, 3333, 3340, 3342, 3344, 
            3346, 3386, 3389, 3389, 3406, 3406, 3423, 3425, 3430, 3439, 3450, 3455, 3461, 3478, 3482, 3505, 3507, 3515, 3517, 3517, 
            3520, 3526, 3558, 3567, 3585, 3632, 3634, 3635, 3648, 3654, 3664, 3673, 3713, 3714, 3716, 3716, 3719, 3720, 3722, 3722, 
            3725, 3725, 3732, 3735, 3737, 3743, 3745, 3747, 3749, 3749, 3751, 3751, 3754, 3755, 3757, 3760, 3762, 3763, 3773, 3773, 
            3776, 3780, 3782, 3782, 3792, 3801, 3804, 3807, 3840, 3840, 3872, 3881, 3904, 3911, 3913, 3948, 3976, 3980, 4096, 4138, 
            4159, 4169, 4176, 4181, 4186, 4189, 4193, 4193, 4197, 4198, 4206, 4208, 4213, 4225, 4238, 4238, 4240, 4249, 4256, 4293, 
            4295, 4295, 4301, 4301, 4304, 4346, 4348, 4680, 4682, 4685, 4688, 4694, 4696, 4696, 4698, 4701, 4704, 4744, 4746, 4749, 
            4752, 4784, 4786, 4789, 4792, 4798, 4800, 4800, 4802, 4805, 4808, 4822, 4824, 4880, 4882, 4885, 4888, 4954, 4992, 5007, 
            5024, 5109, 5112, 5117, 5121, 5740, 5743, 5759, 5761, 5786, 5792, 5866, 5873, 5880, 5888, 5900, 5902, 5905, 5920, 5937, 
            5952, 5969, 5984, 5996, 5998, 6000, 6016, 6067, 6103, 6103, 6108, 6108, 6112, 6121, 6160, 6169, 6176, 6263, 6272, 6312, 
            6314, 6314, 6320, 6389, 6400, 6430, 6470, 6509, 6512, 6516, 6528, 6571, 6576, 6601, 6608, 6617, 6656, 6678, 6688, 6740, 
            6784, 6793, 6800, 6809, 6823, 6823, 6917, 6963, 6981, 6987, 6992, 7001, 7043, 7072, 7086, 7141, 7168, 7203, 7232, 7241, 
            7245, 7293, 7401, 7404, 7406, 7409, 7413, 7414, 7424, 7615, 7680, 7957, 7960, 7965, 7968, 8005, 8008, 8013, 8016, 8023, 
            8025, 8025, 8027, 8027, 8029, 8029, 8031, 8061, 8064, 8116, 8118, 8124, 8126, 8126, 8130, 8132, 8134, 8140, 8144, 8147, 
            8150, 8155, 8160, 8172, 8178, 8180, 8182, 8188, 8305, 8305, 8319, 8319, 8336, 8348, 8450, 8450, 8455, 8455, 8458, 8467, 
            8469, 8469, 8473, 8477, 8484, 8484, 8486, 8486, 8488, 8488, 8490, 8493, 8495, 8505, 8508, 8511, 8517, 8521, 8526, 8526, 
            8579, 8580, 11264, 11310, 11312, 11358, 11360, 11492, 11499, 11502, 11506, 11507, 11520, 11557, 11559, 11559, 11565, 11565, 11568, 11623, 
            11631, 11631, 11648, 11670, 11680, 11686, 11688, 11694, 11696, 11702, 11704, 11710, 11712, 11718, 11720, 11726, 11728, 11734, 11736, 11742, 
            11823, 11823, 12293, 12294, 12337, 12341, 12347, 12348, 12353, 12438, 12445, 12447, 12449, 12538, 12540, 12543, 12549, 12589, 12593, 12686, 
            12704, 12730, 12784, 12799, 13312, 19893, 19968, 40917, 40960, 42124, 42192, 42237, 42240, 42508, 42512, 42539, 42560, 42606, 42623, 42653, 
            42656, 42725, 42775, 42783, 42786, 42888, 42891, 42925, 42928, 42935, 42999, 43009, 43011, 43013, 43015, 43018, 43020, 43042, 43072, 43123, 
            43138, 43187, 43216, 43225, 43250, 43255, 43259, 43259, 43261, 43261, 43264, 43301, 43312, 43334, 43360, 43388, 43396, 43442, 43471, 43481, 
            43488, 43492, 43494, 43518, 43520, 43560, 43584, 43586, 43588, 43595, 43600, 43609, 43616, 43638, 43642, 43642, 43646, 43695, 43697, 43697, 
            43701, 43702, 43705, 43709, 43712, 43712, 43714, 43714, 43739, 43741, 43744, 43754, 43762, 43764, 43777, 43782, 43785, 43790, 43793, 43798, 
            43808, 43814, 43816, 43822, 43824, 43866, 43868, 43877, 43888, 44002, 44016, 44025, 44032, 55203, 55216, 55238, 55243, 55291, 63744, 64109, 
            64112, 64217, 64256, 64262, 64275, 64279, 64285, 64285, 64287, 64296, 64298, 64310, 64312, 64316, 64318, 64318, 64320, 64321, 64323, 64324, 
            64326, 64433, 64467, 64829, 64848, 64911, 64914, 64967, 65008, 65019, 65136, 65140, 65142, 65276, 65296, 65305, 65313, 65338, 65345, 65370, 
            65382, 65470, 65474, 65479, 65482, 65487, 65490, 65495, 65498, 65500, 65536, 65547, 65549, 65574, 65576, 65594, 65596, 65597, 65599, 65613, 
            65616, 65629, 65664, 65786, 66176, 66204, 66208, 66256, 66304, 66335, 66352, 66368, 66370, 66377, 66384, 66421, 66432, 66461, 66464, 66499, 
            66504, 66511, 66560, 66717, 66720, 66729, 66816, 66855, 66864, 66915, 67072, 67382, 67392, 67413, 67424, 67431, 67584, 67589, 67592, 67592, 
            67594, 67637, 67639, 67640, 67644, 67644, 67647, 67669, 67680, 67702, 67712, 67742, 67808, 67826, 67828, 67829, 67840, 67861, 67872, 67897, 
            67968, 68023, 68030, 68031, 68096, 68096, 68112, 68115, 68117, 68119, 68121, 68147, 68192, 68220, 68224, 68252, 68288, 68295, 68297, 68324, 
            68352, 68405, 68416, 68437, 68448, 68466, 68480, 68497, 68608, 68680, 68736, 68786, 68800, 68850, 69635, 69687, 69734, 69743, 69763, 69807, 
            69840, 69864, 69872, 69881, 69891, 69926, 69942, 69951, 69968, 70002, 70006, 70006, 70019, 70066, 70081, 70084, 70096, 70106, 70108, 70108, 
            70144, 70161, 70163, 70187, 70272, 70278, 70280, 70280, 70282, 70285, 70287, 70301, 70303, 70312, 70320, 70366, 70384, 70393, 70405, 70412, 
            70415, 70416, 70419, 70440, 70442, 70448, 70450, 70451, 70453, 70457, 70461, 70461, 70480, 70480, 70493, 70497, 70784, 70831, 70852, 70853, 
            70855, 70855, 70864, 70873, 71040, 71086, 71128, 71131, 71168, 71215, 71236, 71236, 71248, 71257, 71296, 71338, 71360, 71369, 71424, 71449, 
            71472, 71481, 71840, 71913, 71935, 71935, 72384, 72440, 73728, 74649, 74880, 75075, 77824, 78894, 82944, 83526, 92160, 92728, 92736, 92766, 
            92768, 92777, 92880, 92909, 92928, 92975, 92992, 92995, 93008, 93017, 93027, 93047, 93053, 93071, 93952, 94020, 94032, 94032, 94099, 94111, 
            110592, 110593, 113664, 113770, 113776, 113788, 113792, 113800, 113808, 113817, 119808, 119892, 119894, 119964, 119966, 119967, 119970, 119970, 119973, 119974, 
            119977, 119980, 119982, 119993, 119995, 119995, 119997, 120003, 120005, 120069, 120071, 120074, 120077, 120084, 120086, 120092, 120094, 120121, 120123, 120126, 
            120128, 120132, 120134, 120134, 120138, 120144, 120146, 120485, 120488, 120512, 120514, 120538, 120540, 120570, 120572, 120596, 120598, 120628, 120630, 120654, 
            120656, 120686, 120688, 120712, 120714, 120744, 120746, 120770, 120772, 120779, 120782, 120831, 124928, 125124, 126464, 126467, 126469, 126495, 126497, 126498, 
            126500, 126500, 126503, 126503, 126505, 126514, 126516, 126519, 126521, 126521, 126523, 126523, 126530, 126530, 126535, 126535, 126537, 126537, 126539, 126539, 
            126541, 126543, 126545, 126546, 126548, 126548, 126551, 126551, 126553, 126553, 126555, 126555, 126557, 126557, 126559, 126559, 126561, 126562, 126564, 126564, 
            126567, 126570, 126572, 126578, 126580, 126583, 126585, 126588, 126590, 126590, 126592, 126601, 126603, 126619, 126625, 126627, 126629, 126633, 126635, 126651, 
            131072, 173782, 173824, 177972, 177984, 178205, 178208, 183969, 194560, 195101
        };
        // DFA state machine table
        static readonly int[] CommentBlockDfaStateTable = new int[] {
            -1, 1, 6, 1, 47, 47, -1, 1, 12, 1, 42, 42, 7, 0
        };
        // DFA state machine table
        static readonly int[] CommentBlockBlockEndDfaStateTable = new int[] {
            -1, 1, 6, 1, 42, 42, -1, 1, 12, 1, 47, 47, 0, 0
        };
        
    }
}

