// This file was generated using Reggie 0.9.6.0 from the
// TestLexer.rgg specification file on 11/8/2021 9:42:49 AM UTC
namespace Test {
    
    /// <summary>Represents a lexer/tokenizer for the regular expressions in TestLexer.rgg</summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reggie", "0.9.6.0")]
    partial class CSCompiledLexerWithLines {
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
        // Tokenizes the block end for CommentBlock
        static bool TokenizeCommentBlockBlockEnd(System.Collections.Generic.IEnumerator<char> cursor, System.Text.StringBuilder sb, ref long cursorPos, ref long absi, ref int ch, ref int lc, ref int cc, int tabWidth) {
            int adv = 0;
            bool matched;
            while(ch != -1) {
                matched = false;
            // q0
                if(ch == '*') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q1;
                }
                goto next;
            q1:
                if(ch == '/') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                goto next;
            q2:
                return true;
            next:
                if(!matched) {
                    switch (ch) {
                    case '\t':
                        cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                        break;
                    case '\r':
                        cc = 1;
                        break;
                    case '\n':
                        cc = 1;
                        ++lc;
                        break;
                    default:
                        if(ch > 31) ++cc;
                        break;
                    }
                    sb.Append(char.ConvertFromUtf32(ch));
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                }
            }
            return false;
        }
        /// <summary>Lexes tokens off of <paramref name="text"/></summary>
        /// <param name="text">The text to tokenize</param>
        /// <param name="position">The logical position in codepoints where the tokenizer started. By default assumes the beginning of the stream.</param>
        /// <param name="line">The 1 based line where the tokenizer is assumed to have started. Defaults to 1.</param>
        /// <param name="column">The 1 based column where the tokenizer is assumed to have started. Defaults to 1.</param>
        /// <param name="tabWidth">The tab width to assume when calculating the tab advance on a column. Defaults to 4.</param>
        /// <returns>An enumeration of tuples used to retrieve the tokens.</returns>
        /// <remarks>The token contains both the absolute native character position within <paramref name="text"/> and the logical position in UTF32 codepoints for each token. The former is useful for locating the token within a string programmatically while the latter is useful for locating text within a document based on its logical position.</remarks>
        public static System.Collections.Generic.IEnumerable<(long AbsolutePosition, int AbsoluteLength, long Position, int Length, int SymbolId, string Value, int Line, int Column)> Tokenize(System.Collections.Generic.IEnumerable<char> text, long position = 0, int line = 1, int column = 1, int tabWidth = 4) {
            int adv;
            var sb = new System.Text.StringBuilder();
            var cursor = text.GetEnumerator();
            var hasError = false;
            bool matched;
            var errorPos = position;
            var absoluteIndex = 0L;
            var errorIndex = absoluteIndex;
            var cursorPos = position;
            var absi = 0L;
            var lc = line;
            var cc = column;
            var errorLine = line;
            var errorColumn = column;
            int ai;
            int ch;
            ch = ReadUtf32(cursor, out adv);
            while(ch != -1) {
                position = cursorPos;
                absoluteIndex = absi;
                line = lc;
                column = cc;
                matched = false;
            // q0
                if(ch == '\t') {
                    sb.Append(unchecked((char)ch));
                    cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q1;
                }
                if(ch == '\n') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ++lc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q1;
                }
                if(ch == '\r') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q1;
                }
                if(ch == '\v' || ch == '\f' || ch == ' ') {
                    sb.Append(unchecked((char)ch));
                    if(ch > 31) ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q1;
                }
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\'') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q6;
                }
                if(ch == '.') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q11;
                }
                if(ch == '/') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q17;
                }
                if(ch == '0') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q19;
                }
                if(ch >= '1' && ch <= '9') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q20;
                }
                if(ch == '@') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q41;
                }
                if((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || ch == 170 || ch == 181 || ch == 186 || (ch >= 192 && ch <= 214) || (ch >= 216 && ch <= 246) || (ch >= 248 && ch <= 705) || (ch >= 710 && ch <= 721) || (ch >= 736 && ch <= 740) || ch == 748 || 
                        ch == 750 || (ch >= 880 && ch <= 884) || ch == 886 || ch == 887 || (ch >= 890 && ch <= 893) || ch == 895 || ch == 902 || (ch >= 904 && ch <= 906) || ch == 908 || (ch >= 910 && ch <= 929) || (ch >= 931 && ch <= 1013) || 
                        (ch >= 1015 && ch <= 1153) || (ch >= 1162 && ch <= 1327) || (ch >= 1329 && ch <= 1366) || ch == 1369 || (ch >= 1377 && ch <= 1415) || (ch >= 1488 && ch <= 1514) || (ch >= 1520 && ch <= 1522) || (ch >= 1568 && ch <= 1610) || ch == 1646 || ch == 1647 || (ch >= 1649 && ch <= 1747) || 
                        ch == 1749 || ch == 1765 || ch == 1766 || ch == 1774 || ch == 1775 || (ch >= 1786 && ch <= 1788) || ch == 1791 || ch == 1808 || (ch >= 1810 && ch <= 1839) || (ch >= 1869 && ch <= 1957) || ch == 1969 || (ch >= 1994 && ch <= 2026) || 
                        ch == 2036 || ch == 2037 || ch == 2042 || (ch >= 2048 && ch <= 2069) || ch == 2074 || ch == 2084 || ch == 2088 || (ch >= 2112 && ch <= 2136) || (ch >= 2208 && ch <= 2228) || (ch >= 2308 && ch <= 2361) || ch == 2365 || 
                        ch == 2384 || (ch >= 2392 && ch <= 2401) || (ch >= 2417 && ch <= 2432) || (ch >= 2437 && ch <= 2444) || ch == 2447 || ch == 2448 || (ch >= 2451 && ch <= 2472) || (ch >= 2474 && ch <= 2480) || ch == 2482 || (ch >= 2486 && ch <= 2489) || ch == 2493 || 
                        ch == 2510 || ch == 2524 || ch == 2525 || (ch >= 2527 && ch <= 2529) || ch == 2544 || ch == 2545 || (ch >= 2565 && ch <= 2570) || ch == 2575 || ch == 2576 || (ch >= 2579 && ch <= 2600) || (ch >= 2602 && ch <= 2608) || ch == 2610 || ch == 2611 || ch == 2613 || ch == 2614 || 
                        ch == 2616 || ch == 2617 || (ch >= 2649 && ch <= 2652) || ch == 2654 || (ch >= 2674 && ch <= 2676) || (ch >= 2693 && ch <= 2701) || (ch >= 2703 && ch <= 2705) || (ch >= 2707 && ch <= 2728) || (ch >= 2730 && ch <= 2736) || ch == 2738 || ch == 2739 || (ch >= 2741 && ch <= 2745) || 
                        ch == 2749 || ch == 2768 || ch == 2784 || ch == 2785 || ch == 2809 || (ch >= 2821 && ch <= 2828) || ch == 2831 || ch == 2832 || (ch >= 2835 && ch <= 2856) || (ch >= 2858 && ch <= 2864) || ch == 2866 || ch == 2867 || (ch >= 2869 && ch <= 2873) || 
                        ch == 2877 || ch == 2908 || ch == 2909 || (ch >= 2911 && ch <= 2913) || ch == 2929 || ch == 2947 || (ch >= 2949 && ch <= 2954) || (ch >= 2958 && ch <= 2960) || (ch >= 2962 && ch <= 2965) || ch == 2969 || ch == 2970 || ch == 2972 || 
                        ch == 2974 || ch == 2975 || ch == 2979 || ch == 2980 || (ch >= 2984 && ch <= 2986) || (ch >= 2990 && ch <= 3001) || ch == 3024 || (ch >= 3077 && ch <= 3084) || (ch >= 3086 && ch <= 3088) || (ch >= 3090 && ch <= 3112) || (ch >= 3114 && ch <= 3129) || ch == 3133 || 
                        (ch >= 3160 && ch <= 3162) || ch == 3168 || ch == 3169 || (ch >= 3205 && ch <= 3212) || (ch >= 3214 && ch <= 3216) || (ch >= 3218 && ch <= 3240) || (ch >= 3242 && ch <= 3251) || (ch >= 3253 && ch <= 3257) || ch == 3261 || ch == 3294 || ch == 3296 || ch == 3297 || 
                        ch == 3313 || ch == 3314 || (ch >= 3333 && ch <= 3340) || (ch >= 3342 && ch <= 3344) || (ch >= 3346 && ch <= 3386) || ch == 3389 || ch == 3406 || (ch >= 3423 && ch <= 3425) || (ch >= 3450 && ch <= 3455) || (ch >= 3461 && ch <= 3478) || (ch >= 3482 && ch <= 3505) || 
                        (ch >= 3507 && ch <= 3515) || ch == 3517 || (ch >= 3520 && ch <= 3526) || (ch >= 3585 && ch <= 3632) || ch == 3634 || ch == 3635 || (ch >= 3648 && ch <= 3654) || ch == 3713 || ch == 3714 || ch == 3716 || ch == 3719 || ch == 3720 || ch == 3722 || 
                        ch == 3725 || (ch >= 3732 && ch <= 3735) || (ch >= 3737 && ch <= 3743) || (ch >= 3745 && ch <= 3747) || ch == 3749 || ch == 3751 || ch == 3754 || ch == 3755 || (ch >= 3757 && ch <= 3760) || ch == 3762 || ch == 3763 || ch == 3773 || 
                        (ch >= 3776 && ch <= 3780) || ch == 3782 || (ch >= 3804 && ch <= 3807) || ch == 3840 || (ch >= 3904 && ch <= 3911) || (ch >= 3913 && ch <= 3948) || (ch >= 3976 && ch <= 3980) || (ch >= 4096 && ch <= 4138) || ch == 4159 || (ch >= 4176 && ch <= 4181) || 
                        (ch >= 4186 && ch <= 4189) || ch == 4193 || ch == 4197 || ch == 4198 || (ch >= 4206 && ch <= 4208) || (ch >= 4213 && ch <= 4225) || ch == 4238 || (ch >= 4256 && ch <= 4293) || ch == 4295 || ch == 4301 || (ch >= 4304 && ch <= 4346) || 
                        (ch >= 4348 && ch <= 4680) || (ch >= 4682 && ch <= 4685) || (ch >= 4688 && ch <= 4694) || ch == 4696 || (ch >= 4698 && ch <= 4701) || (ch >= 4704 && ch <= 4744) || (ch >= 4746 && ch <= 4749) || (ch >= 4752 && ch <= 4784) || (ch >= 4786 && ch <= 4789) || (ch >= 4792 && ch <= 4798) || 
                        ch == 4800 || (ch >= 4802 && ch <= 4805) || (ch >= 4808 && ch <= 4822) || (ch >= 4824 && ch <= 4880) || (ch >= 4882 && ch <= 4885) || (ch >= 4888 && ch <= 4954) || (ch >= 4992 && ch <= 5007) || (ch >= 5024 && ch <= 5109) || (ch >= 5112 && ch <= 5117) || (ch >= 5121 && ch <= 5740) || 
                        (ch >= 5743 && ch <= 5759) || (ch >= 5761 && ch <= 5786) || (ch >= 5792 && ch <= 5866) || (ch >= 5873 && ch <= 5880) || (ch >= 5888 && ch <= 5900) || (ch >= 5902 && ch <= 5905) || (ch >= 5920 && ch <= 5937) || (ch >= 5952 && ch <= 5969) || (ch >= 5984 && ch <= 5996) || (ch >= 5998 && ch <= 6000) || 
                        (ch >= 6016 && ch <= 6067) || ch == 6103 || ch == 6108 || (ch >= 6176 && ch <= 6263) || (ch >= 6272 && ch <= 6312) || ch == 6314 || (ch >= 6320 && ch <= 6389) || (ch >= 6400 && ch <= 6430) || (ch >= 6480 && ch <= 6509) || (ch >= 6512 && ch <= 6516) || 
                        (ch >= 6528 && ch <= 6571) || (ch >= 6576 && ch <= 6601) || (ch >= 6656 && ch <= 6678) || (ch >= 6688 && ch <= 6740) || ch == 6823 || (ch >= 6917 && ch <= 6963) || (ch >= 6981 && ch <= 6987) || (ch >= 7043 && ch <= 7072) || ch == 7086 || ch == 7087 || (ch >= 7098 && ch <= 7141) || 
                        (ch >= 7168 && ch <= 7203) || (ch >= 7245 && ch <= 7247) || (ch >= 7258 && ch <= 7293) || (ch >= 7401 && ch <= 7404) || (ch >= 7406 && ch <= 7409) || ch == 7413 || ch == 7414 || (ch >= 7424 && ch <= 7615) || (ch >= 7680 && ch <= 7957) || (ch >= 7960 && ch <= 7965) || (ch >= 7968 && ch <= 8005) || 
                        (ch >= 8008 && ch <= 8013) || (ch >= 8016 && ch <= 8023) || ch == 8025 || ch == 8027 || ch == 8029 || (ch >= 8031 && ch <= 8061) || (ch >= 8064 && ch <= 8116) || (ch >= 8118 && ch <= 8124) || ch == 8126 || (ch >= 8130 && ch <= 8132) || 
                        (ch >= 8134 && ch <= 8140) || (ch >= 8144 && ch <= 8147) || (ch >= 8150 && ch <= 8155) || (ch >= 8160 && ch <= 8172) || (ch >= 8178 && ch <= 8180) || (ch >= 8182 && ch <= 8188) || ch == 8305 || ch == 8319 || (ch >= 8336 && ch <= 8348) || ch == 8450 || 
                        ch == 8455 || (ch >= 8458 && ch <= 8467) || ch == 8469 || (ch >= 8473 && ch <= 8477) || ch == 8484 || ch == 8486 || ch == 8488 || (ch >= 8490 && ch <= 8493) || (ch >= 8495 && ch <= 8505) || (ch >= 8508 && ch <= 8511) || 
                        (ch >= 8517 && ch <= 8521) || ch == 8526 || ch == 8579 || ch == 8580 || (ch >= 11264 && ch <= 11310) || (ch >= 11312 && ch <= 11358) || (ch >= 11360 && ch <= 11492) || (ch >= 11499 && ch <= 11502) || ch == 11506 || ch == 11507 || (ch >= 11520 && ch <= 11557) || ch == 11559 || 
                        ch == 11565 || (ch >= 11568 && ch <= 11623) || ch == 11631 || (ch >= 11648 && ch <= 11670) || (ch >= 11680 && ch <= 11686) || (ch >= 11688 && ch <= 11694) || (ch >= 11696 && ch <= 11702) || (ch >= 11704 && ch <= 11710) || (ch >= 11712 && ch <= 11718) || (ch >= 11720 && ch <= 11726) || 
                        (ch >= 11728 && ch <= 11734) || (ch >= 11736 && ch <= 11742) || ch == 11823 || ch == 12293 || ch == 12294 || (ch >= 12337 && ch <= 12341) || ch == 12347 || ch == 12348 || (ch >= 12353 && ch <= 12438) || (ch >= 12445 && ch <= 12447) || (ch >= 12449 && ch <= 12538) || (ch >= 12540 && ch <= 12543) || 
                        (ch >= 12549 && ch <= 12589) || (ch >= 12593 && ch <= 12686) || (ch >= 12704 && ch <= 12730) || (ch >= 12784 && ch <= 12799) || (ch >= 13312 && ch <= 19893) || (ch >= 19968 && ch <= 40917) || (ch >= 40960 && ch <= 42124) || (ch >= 42192 && ch <= 42237) || (ch >= 42240 && ch <= 42508) || (ch >= 42512 && ch <= 42527) || 
                        ch == 42538 || ch == 42539 || (ch >= 42560 && ch <= 42606) || (ch >= 42623 && ch <= 42653) || (ch >= 42656 && ch <= 42725) || (ch >= 42775 && ch <= 42783) || (ch >= 42786 && ch <= 42888) || (ch >= 42891 && ch <= 42925) || (ch >= 42928 && ch <= 42935) || (ch >= 42999 && ch <= 43009) || (ch >= 43011 && ch <= 43013) || 
                        (ch >= 43015 && ch <= 43018) || (ch >= 43020 && ch <= 43042) || (ch >= 43072 && ch <= 43123) || (ch >= 43138 && ch <= 43187) || (ch >= 43250 && ch <= 43255) || ch == 43259 || ch == 43261 || (ch >= 43274 && ch <= 43301) || (ch >= 43312 && ch <= 43334) || (ch >= 43360 && ch <= 43388) || 
                        (ch >= 43396 && ch <= 43442) || ch == 43471 || (ch >= 43488 && ch <= 43492) || (ch >= 43494 && ch <= 43503) || (ch >= 43514 && ch <= 43518) || (ch >= 43520 && ch <= 43560) || (ch >= 43584 && ch <= 43586) || (ch >= 43588 && ch <= 43595) || (ch >= 43616 && ch <= 43638) || ch == 43642 || 
                        (ch >= 43646 && ch <= 43695) || ch == 43697 || ch == 43701 || ch == 43702 || (ch >= 43705 && ch <= 43709) || ch == 43712 || ch == 43714 || (ch >= 43739 && ch <= 43741) || (ch >= 43744 && ch <= 43754) || (ch >= 43762 && ch <= 43764) || (ch >= 43777 && ch <= 43782) || 
                        (ch >= 43785 && ch <= 43790) || (ch >= 43793 && ch <= 43798) || (ch >= 43808 && ch <= 43814) || (ch >= 43816 && ch <= 43822) || (ch >= 43824 && ch <= 43866) || (ch >= 43868 && ch <= 43877) || (ch >= 43888 && ch <= 44002) || (ch >= 44032 && ch <= 55203) || (ch >= 55216 && ch <= 55238) || (ch >= 55243 && ch <= 55291) || 
                        (ch >= 63744 && ch <= 64109) || (ch >= 64112 && ch <= 64217) || (ch >= 64256 && ch <= 64262) || (ch >= 64275 && ch <= 64279) || ch == 64285 || (ch >= 64287 && ch <= 64296) || (ch >= 64298 && ch <= 64310) || (ch >= 64312 && ch <= 64316) || ch == 64318 || ch == 64320 || ch == 64321 || 
                        ch == 64323 || ch == 64324 || (ch >= 64326 && ch <= 64433) || (ch >= 64467 && ch <= 64829) || (ch >= 64848 && ch <= 64911) || (ch >= 64914 && ch <= 64967) || (ch >= 65008 && ch <= 65019) || (ch >= 65136 && ch <= 65140) || (ch >= 65142 && ch <= 65276) || (ch >= 65313 && ch <= 65338) || (ch >= 65345 && ch <= 65370) || 
                        (ch >= 65382 && ch <= 65470) || (ch >= 65474 && ch <= 65479) || (ch >= 65482 && ch <= 65487) || (ch >= 65490 && ch <= 65495) || (ch >= 65498 && ch <= 65500) || (ch >= 65536 && ch <= 65547) || (ch >= 65549 && ch <= 65574) || (ch >= 65576 && ch <= 65594) || ch == 65596 || ch == 65597 || (ch >= 65599 && ch <= 65613) || 
                        (ch >= 65616 && ch <= 65629) || (ch >= 65664 && ch <= 65786) || (ch >= 66176 && ch <= 66204) || (ch >= 66208 && ch <= 66256) || (ch >= 66304 && ch <= 66335) || (ch >= 66352 && ch <= 66368) || (ch >= 66370 && ch <= 66377) || (ch >= 66384 && ch <= 66421) || (ch >= 66432 && ch <= 66461) || (ch >= 66464 && ch <= 66499) || 
                        (ch >= 66504 && ch <= 66511) || (ch >= 66560 && ch <= 66717) || (ch >= 66816 && ch <= 66855) || (ch >= 66864 && ch <= 66915) || (ch >= 67072 && ch <= 67382) || (ch >= 67392 && ch <= 67413) || (ch >= 67424 && ch <= 67431) || (ch >= 67584 && ch <= 67589) || ch == 67592 || (ch >= 67594 && ch <= 67637) || 
                        ch == 67639 || ch == 67640 || ch == 67644 || (ch >= 67647 && ch <= 67669) || (ch >= 67680 && ch <= 67702) || (ch >= 67712 && ch <= 67742) || (ch >= 67808 && ch <= 67826) || ch == 67828 || ch == 67829 || (ch >= 67840 && ch <= 67861) || (ch >= 67872 && ch <= 67897) || (ch >= 67968 && ch <= 68023) || 
                        ch == 68030 || ch == 68031 || ch == 68096 || (ch >= 68112 && ch <= 68115) || (ch >= 68117 && ch <= 68119) || (ch >= 68121 && ch <= 68147) || (ch >= 68192 && ch <= 68220) || (ch >= 68224 && ch <= 68252) || (ch >= 68288 && ch <= 68295) || (ch >= 68297 && ch <= 68324) || (ch >= 68352 && ch <= 68405) || 
                        (ch >= 68416 && ch <= 68437) || (ch >= 68448 && ch <= 68466) || (ch >= 68480 && ch <= 68497) || (ch >= 68608 && ch <= 68680) || (ch >= 68736 && ch <= 68786) || (ch >= 68800 && ch <= 68850) || (ch >= 69635 && ch <= 69687) || (ch >= 69763 && ch <= 69807) || (ch >= 69840 && ch <= 69864) || (ch >= 69891 && ch <= 69926) || 
                        (ch >= 69968 && ch <= 70002) || ch == 70006 || (ch >= 70019 && ch <= 70066) || (ch >= 70081 && ch <= 70084) || ch == 70106 || ch == 70108 || (ch >= 70144 && ch <= 70161) || (ch >= 70163 && ch <= 70187) || (ch >= 70272 && ch <= 70278) || ch == 70280 || 
                        (ch >= 70282 && ch <= 70285) || (ch >= 70287 && ch <= 70301) || (ch >= 70303 && ch <= 70312) || (ch >= 70320 && ch <= 70366) || (ch >= 70405 && ch <= 70412) || ch == 70415 || ch == 70416 || (ch >= 70419 && ch <= 70440) || (ch >= 70442 && ch <= 70448) || ch == 70450 || ch == 70451 || (ch >= 70453 && ch <= 70457) || 
                        ch == 70461 || ch == 70480 || (ch >= 70493 && ch <= 70497) || (ch >= 70784 && ch <= 70831) || ch == 70852 || ch == 70853 || ch == 70855 || (ch >= 71040 && ch <= 71086) || (ch >= 71128 && ch <= 71131) || (ch >= 71168 && ch <= 71215) || ch == 71236 || 
                        (ch >= 71296 && ch <= 71338) || (ch >= 71424 && ch <= 71449) || (ch >= 71840 && ch <= 71903) || ch == 71935 || (ch >= 72384 && ch <= 72440) || (ch >= 73728 && ch <= 74649) || (ch >= 74880 && ch <= 75075) || (ch >= 77824 && ch <= 78894) || (ch >= 82944 && ch <= 83526) || (ch >= 92160 && ch <= 92728) || 
                        (ch >= 92736 && ch <= 92766) || (ch >= 92880 && ch <= 92909) || (ch >= 92928 && ch <= 92975) || (ch >= 92992 && ch <= 92995) || (ch >= 93027 && ch <= 93047) || (ch >= 93053 && ch <= 93071) || (ch >= 93952 && ch <= 94020) || ch == 94032 || (ch >= 94099 && ch <= 94111) || ch == 110592 || ch == 110593 || 
                        (ch >= 113664 && ch <= 113770) || (ch >= 113776 && ch <= 113788) || (ch >= 113792 && ch <= 113800) || (ch >= 113808 && ch <= 113817) || (ch >= 119808 && ch <= 119892) || (ch >= 119894 && ch <= 119964) || ch == 119966 || ch == 119967 || ch == 119970 || ch == 119973 || ch == 119974 || (ch >= 119977 && ch <= 119980) || 
                        (ch >= 119982 && ch <= 119993) || ch == 119995 || (ch >= 119997 && ch <= 120003) || (ch >= 120005 && ch <= 120069) || (ch >= 120071 && ch <= 120074) || (ch >= 120077 && ch <= 120084) || (ch >= 120086 && ch <= 120092) || (ch >= 120094 && ch <= 120121) || (ch >= 120123 && ch <= 120126) || (ch >= 120128 && ch <= 120132) || 
                        ch == 120134 || (ch >= 120138 && ch <= 120144) || (ch >= 120146 && ch <= 120485) || (ch >= 120488 && ch <= 120512) || (ch >= 120514 && ch <= 120538) || (ch >= 120540 && ch <= 120570) || (ch >= 120572 && ch <= 120596) || (ch >= 120598 && ch <= 120628) || (ch >= 120630 && ch <= 120654) || (ch >= 120656 && ch <= 120686) || 
                        (ch >= 120688 && ch <= 120712) || (ch >= 120714 && ch <= 120744) || (ch >= 120746 && ch <= 120770) || (ch >= 120772 && ch <= 120779) || (ch >= 124928 && ch <= 125124) || (ch >= 126464 && ch <= 126467) || (ch >= 126469 && ch <= 126495) || ch == 126497 || ch == 126498 || ch == 126500 || ch == 126503 || 
                        (ch >= 126505 && ch <= 126514) || (ch >= 126516 && ch <= 126519) || ch == 126521 || ch == 126523 || ch == 126530 || ch == 126535 || ch == 126537 || ch == 126539 || (ch >= 126541 && ch <= 126543) || ch == 126545 || ch == 126546 || 
                        ch == 126548 || ch == 126551 || ch == 126553 || ch == 126555 || ch == 126557 || ch == 126559 || ch == 126561 || ch == 126562 || ch == 126564 || (ch >= 126567 && ch <= 126570) || (ch >= 126572 && ch <= 126578) || 
                        (ch >= 126580 && ch <= 126583) || (ch >= 126585 && ch <= 126588) || ch == 126590 || (ch >= 126592 && ch <= 126601) || (ch >= 126603 && ch <= 126619) || (ch >= 126625 && ch <= 126627) || (ch >= 126629 && ch <= 126633) || (ch >= 126635 && ch <= 126651) || (ch >= 131072 && ch <= 173782) || (ch >= 173824 && ch <= 177972) || 
                        (ch >= 177984 && ch <= 178205) || (ch >= 178208 && ch <= 183969) || (ch >= 194560 && ch <= 195101)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q44;
                }
                goto error;
            q1:
                if(ch == '\t') {
                    sb.Append(unchecked((char)ch));
                    cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q1;
                }
                if(ch == '\n') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ++lc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q1;
                }
                if(ch == '\r') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q1;
                }
                if(ch == '\v' || ch == '\f' || ch == ' ') {
                    sb.Append(unchecked((char)ch));
                    if(ch > 31) ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q1;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                sb.Clear();
                continue;
            q2:
                if(ch == '\t') {
                    sb.Append(unchecked((char)ch));
                    cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\n') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ++lc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\r') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if((ch >= '\0' && ch <= '\b') || ch == '\v' || ch == '\f' || (ch >= 14 && ch <= '!') || (ch >= '#' && ch <= '[') || (ch >= ']' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    if(ch > 31) ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q3;
                }
                if(ch == '\\') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q4;
                }
                goto error;
            q3:
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: StringLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q4:
                if(ch == '\t') {
                    sb.Append(unchecked((char)ch));
                    cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\n') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ++lc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\r') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if((ch >= '\0' && ch <= '\b') || ch == '\v' || ch == '\f' || (ch >= 14 && ch <= '!') || (ch >= '#' && ch <= '[') || (ch >= ']' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    if(ch > 31) ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q5;
                }
                if(ch == '\\') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q4;
                }
                goto error;
            q5:
                if(ch == '\t') {
                    sb.Append(unchecked((char)ch));
                    cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\n') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ++lc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\r') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if((ch >= '\0' && ch <= '\b') || ch == '\v' || ch == '\f' || (ch >= 14 && ch <= '!') || (ch >= '#' && ch <= '[') || (ch >= ']' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    if(ch > 31) ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q2;
                }
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q3;
                }
                if(ch == '\\') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q4;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: StringLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q6:
                if(ch == '\t') {
                    sb.Append(unchecked((char)ch));
                    cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q7;
                }
                if(ch == '\n') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ++lc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q7;
                }
                if(ch == '\r') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q7;
                }
                if((ch >= '\0' && ch <= '\b') || ch == '\v' || ch == '\f' || (ch >= 14 && ch <= '&') || (ch >= '(' && ch <= '[') || (ch >= ']' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    if(ch > 31) ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q7;
                }
                if(ch == '\\') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q9;
                }
                goto error;
            q7:
                if(ch == '\'') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q8;
                }
                goto error;
            q8:
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: CharacterLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q9:
                if(ch == '\t') {
                    sb.Append(unchecked((char)ch));
                    cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q7;
                }
                if(ch == '\n') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ++lc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q7;
                }
                if(ch == '\r') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q7;
                }
                if((ch >= '\0' && ch <= '\b') || ch == '\v' || ch == '\f' || (ch >= 14 && ch <= '&') || (ch >= '(' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    if(ch > 31) ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q7;
                }
                if(ch == '\'') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q10;
                }
                goto error;
            q10:
                if(ch == '\'') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q8;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: CharacterLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q11:
                if(ch >= '0' && ch <= '9') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q12;
                }
                goto error;
            q12:
                if(ch >= '0' && ch <= '9') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q12;
                }
                if(ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q13;
                }
                if(ch == 'E' || ch == 'e') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q14;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: FloatLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q13:
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: FloatLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q14:
                if(ch == 43 || ch == '-') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q15;
                }
                if(ch >= '0' && ch <= '9') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q16;
                }
                goto error;
            q15:
                if(ch >= '0' && ch <= '9') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q16;
                }
                goto error;
            q16:
                if(ch >= '0' && ch <= '9') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q16;
                }
                if(ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q13;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: FloatLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q17:
                if(ch == '*') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q18;
                }
                goto error;
            q18:
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(TokenizeCommentBlockBlockEnd(cursor, sb, ref cursorPos, ref absi, ref ch, ref lc, ref cc, tabWidth)) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: CommentBlock, Value: sb.ToString(), Line: line, Column: column);
                    sb.Clear();
                    continue;
                }
                yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: sb.Length, Position: position, Length: (int)(cursorPos - position), SymbolId: ERROR, Value: sb.ToString(), Line: line, Column: column);
                
                
                sb.Clear();
                continue;
            q19:
                if(ch == '.') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q11;
                }
                if(ch >= '0' && ch <= '9') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q20;
                }
                if(ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q13;
                }
                if(ch == 'E' || ch == 'e') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q14;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(ch == 'x') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q24;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q20:
                if(ch == '.') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q11;
                }
                if(ch >= '0' && ch <= '9') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q20;
                }
                if(ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q13;
                }
                if(ch == 'E' || ch == 'e') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q14;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q21:
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q22;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q22:
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q23:
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q22;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q24:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q25;
                }
                goto error;
            q25:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q26;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q26:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q27;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q27:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q28;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q28:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q29;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q29:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q30;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q30:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q31;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q31:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q32;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q32:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q33;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q33:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q34;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q34:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q35;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q35:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q36;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q36:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q37;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q37:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q38;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q38:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q39;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q39:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')) {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q40;
                }
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q40:
                if(ch == 'L' || ch == 'l') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q21;
                }
                if(ch == 'U' || ch == 'u') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q23;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: IntegerLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q41:
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q42;
                }
                goto error;
            q42:
                if(ch == '\t') {
                    sb.Append(unchecked((char)ch));
                    cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q42;
                }
                if(ch == '\n') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ++lc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q42;
                }
                if(ch == '\r') {
                    sb.Append(unchecked((char)ch));
                    cc = 1;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q42;
                }
                if((ch >= '\0' && ch <= '\b') || ch == '\v' || ch == '\f' || (ch >= 14 && ch <= '!') || (ch >= '#' && ch <= 1114111)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    if(ch > 31) ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q42;
                }
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q43;
                }
                goto error;
            q43:
                if(ch == '\"') {
                    sb.Append(unchecked((char)ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q42;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: VerbatimStringLiteral, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            q44:
                if((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || ch == 170 || ch == 181 || ch == 186 || (ch >= 192 && ch <= 214) || (ch >= 216 && ch <= 246) || (ch >= 248 && ch <= 705) || (ch >= 710 && ch <= 721) || (ch >= 736 && ch <= 740) || 
                        ch == 748 || ch == 750 || (ch >= 880 && ch <= 884) || ch == 886 || ch == 887 || (ch >= 890 && ch <= 893) || ch == 895 || ch == 902 || (ch >= 904 && ch <= 906) || ch == 908 || (ch >= 910 && ch <= 929) || 
                        (ch >= 931 && ch <= 1013) || (ch >= 1015 && ch <= 1153) || (ch >= 1162 && ch <= 1327) || (ch >= 1329 && ch <= 1366) || ch == 1369 || (ch >= 1377 && ch <= 1415) || (ch >= 1488 && ch <= 1514) || (ch >= 1520 && ch <= 1522) || (ch >= 1568 && ch <= 1610) || (ch >= 1632 && ch <= 1641) || 
                        ch == 1646 || ch == 1647 || (ch >= 1649 && ch <= 1747) || ch == 1749 || ch == 1765 || ch == 1766 || (ch >= 1774 && ch <= 1788) || ch == 1791 || ch == 1808 || (ch >= 1810 && ch <= 1839) || (ch >= 1869 && ch <= 1957) || ch == 1969 || 
                        (ch >= 1984 && ch <= 2026) || ch == 2036 || ch == 2037 || ch == 2042 || (ch >= 2048 && ch <= 2069) || ch == 2074 || ch == 2084 || ch == 2088 || (ch >= 2112 && ch <= 2136) || (ch >= 2208 && ch <= 2228) || (ch >= 2308 && ch <= 2361) || 
                        ch == 2365 || ch == 2384 || (ch >= 2392 && ch <= 2401) || (ch >= 2406 && ch <= 2415) || (ch >= 2417 && ch <= 2432) || (ch >= 2437 && ch <= 2444) || ch == 2447 || ch == 2448 || (ch >= 2451 && ch <= 2472) || (ch >= 2474 && ch <= 2480) || ch == 2482 || 
                        (ch >= 2486 && ch <= 2489) || ch == 2493 || ch == 2510 || ch == 2524 || ch == 2525 || (ch >= 2527 && ch <= 2529) || (ch >= 2534 && ch <= 2545) || (ch >= 2565 && ch <= 2570) || ch == 2575 || ch == 2576 || (ch >= 2579 && ch <= 2600) || (ch >= 2602 && ch <= 2608) || 
                        ch == 2610 || ch == 2611 || ch == 2613 || ch == 2614 || ch == 2616 || ch == 2617 || (ch >= 2649 && ch <= 2652) || ch == 2654 || (ch >= 2662 && ch <= 2671) || (ch >= 2674 && ch <= 2676) || (ch >= 2693 && ch <= 2701) || (ch >= 2703 && ch <= 2705) || (ch >= 2707 && ch <= 2728) || 
                        (ch >= 2730 && ch <= 2736) || ch == 2738 || ch == 2739 || (ch >= 2741 && ch <= 2745) || ch == 2749 || ch == 2768 || ch == 2784 || ch == 2785 || (ch >= 2790 && ch <= 2799) || ch == 2809 || (ch >= 2821 && ch <= 2828) || ch == 2831 || ch == 2832 || 
                        (ch >= 2835 && ch <= 2856) || (ch >= 2858 && ch <= 2864) || ch == 2866 || ch == 2867 || (ch >= 2869 && ch <= 2873) || ch == 2877 || ch == 2908 || ch == 2909 || (ch >= 2911 && ch <= 2913) || (ch >= 2918 && ch <= 2927) || ch == 2929 || ch == 2947 || 
                        (ch >= 2949 && ch <= 2954) || (ch >= 2958 && ch <= 2960) || (ch >= 2962 && ch <= 2965) || ch == 2969 || ch == 2970 || ch == 2972 || ch == 2974 || ch == 2975 || ch == 2979 || ch == 2980 || (ch >= 2984 && ch <= 2986) || (ch >= 2990 && ch <= 3001) || ch == 3024 || 
                        (ch >= 3046 && ch <= 3055) || (ch >= 3077 && ch <= 3084) || (ch >= 3086 && ch <= 3088) || (ch >= 3090 && ch <= 3112) || (ch >= 3114 && ch <= 3129) || ch == 3133 || (ch >= 3160 && ch <= 3162) || ch == 3168 || ch == 3169 || (ch >= 3174 && ch <= 3183) || (ch >= 3205 && ch <= 3212) || 
                        (ch >= 3214 && ch <= 3216) || (ch >= 3218 && ch <= 3240) || (ch >= 3242 && ch <= 3251) || (ch >= 3253 && ch <= 3257) || ch == 3261 || ch == 3294 || ch == 3296 || ch == 3297 || (ch >= 3302 && ch <= 3311) || ch == 3313 || ch == 3314 || (ch >= 3333 && ch <= 3340) || 
                        (ch >= 3342 && ch <= 3344) || (ch >= 3346 && ch <= 3386) || ch == 3389 || ch == 3406 || (ch >= 3423 && ch <= 3425) || (ch >= 3430 && ch <= 3439) || (ch >= 3450 && ch <= 3455) || (ch >= 3461 && ch <= 3478) || (ch >= 3482 && ch <= 3505) || (ch >= 3507 && ch <= 3515) || 
                        ch == 3517 || (ch >= 3520 && ch <= 3526) || (ch >= 3558 && ch <= 3567) || (ch >= 3585 && ch <= 3632) || ch == 3634 || ch == 3635 || (ch >= 3648 && ch <= 3654) || (ch >= 3664 && ch <= 3673) || ch == 3713 || ch == 3714 || ch == 3716 || ch == 3719 || ch == 3720 || 
                        ch == 3722 || ch == 3725 || (ch >= 3732 && ch <= 3735) || (ch >= 3737 && ch <= 3743) || (ch >= 3745 && ch <= 3747) || ch == 3749 || ch == 3751 || ch == 3754 || ch == 3755 || (ch >= 3757 && ch <= 3760) || ch == 3762 || ch == 3763 || 
                        ch == 3773 || (ch >= 3776 && ch <= 3780) || ch == 3782 || (ch >= 3792 && ch <= 3801) || (ch >= 3804 && ch <= 3807) || ch == 3840 || (ch >= 3872 && ch <= 3881) || (ch >= 3904 && ch <= 3911) || (ch >= 3913 && ch <= 3948) || (ch >= 3976 && ch <= 3980) || 
                        (ch >= 4096 && ch <= 4138) || (ch >= 4159 && ch <= 4169) || (ch >= 4176 && ch <= 4181) || (ch >= 4186 && ch <= 4189) || ch == 4193 || ch == 4197 || ch == 4198 || (ch >= 4206 && ch <= 4208) || (ch >= 4213 && ch <= 4225) || ch == 4238 || (ch >= 4240 && ch <= 4249) || 
                        (ch >= 4256 && ch <= 4293) || ch == 4295 || ch == 4301 || (ch >= 4304 && ch <= 4346) || (ch >= 4348 && ch <= 4680) || (ch >= 4682 && ch <= 4685) || (ch >= 4688 && ch <= 4694) || ch == 4696 || (ch >= 4698 && ch <= 4701) || (ch >= 4704 && ch <= 4744) || 
                        (ch >= 4746 && ch <= 4749) || (ch >= 4752 && ch <= 4784) || (ch >= 4786 && ch <= 4789) || (ch >= 4792 && ch <= 4798) || ch == 4800 || (ch >= 4802 && ch <= 4805) || (ch >= 4808 && ch <= 4822) || (ch >= 4824 && ch <= 4880) || (ch >= 4882 && ch <= 4885) || (ch >= 4888 && ch <= 4954) || 
                        (ch >= 4992 && ch <= 5007) || (ch >= 5024 && ch <= 5109) || (ch >= 5112 && ch <= 5117) || (ch >= 5121 && ch <= 5740) || (ch >= 5743 && ch <= 5759) || (ch >= 5761 && ch <= 5786) || (ch >= 5792 && ch <= 5866) || (ch >= 5873 && ch <= 5880) || (ch >= 5888 && ch <= 5900) || (ch >= 5902 && ch <= 5905) || 
                        (ch >= 5920 && ch <= 5937) || (ch >= 5952 && ch <= 5969) || (ch >= 5984 && ch <= 5996) || (ch >= 5998 && ch <= 6000) || (ch >= 6016 && ch <= 6067) || ch == 6103 || ch == 6108 || (ch >= 6112 && ch <= 6121) || (ch >= 6160 && ch <= 6169) || (ch >= 6176 && ch <= 6263) || 
                        (ch >= 6272 && ch <= 6312) || ch == 6314 || (ch >= 6320 && ch <= 6389) || (ch >= 6400 && ch <= 6430) || (ch >= 6470 && ch <= 6509) || (ch >= 6512 && ch <= 6516) || (ch >= 6528 && ch <= 6571) || (ch >= 6576 && ch <= 6601) || (ch >= 6608 && ch <= 6617) || (ch >= 6656 && ch <= 6678) || 
                        (ch >= 6688 && ch <= 6740) || (ch >= 6784 && ch <= 6793) || (ch >= 6800 && ch <= 6809) || ch == 6823 || (ch >= 6917 && ch <= 6963) || (ch >= 6981 && ch <= 6987) || (ch >= 6992 && ch <= 7001) || (ch >= 7043 && ch <= 7072) || (ch >= 7086 && ch <= 7141) || (ch >= 7168 && ch <= 7203) || 
                        (ch >= 7232 && ch <= 7241) || (ch >= 7245 && ch <= 7293) || (ch >= 7401 && ch <= 7404) || (ch >= 7406 && ch <= 7409) || ch == 7413 || ch == 7414 || (ch >= 7424 && ch <= 7615) || (ch >= 7680 && ch <= 7957) || (ch >= 7960 && ch <= 7965) || (ch >= 7968 && ch <= 8005) || (ch >= 8008 && ch <= 8013) || 
                        (ch >= 8016 && ch <= 8023) || ch == 8025 || ch == 8027 || ch == 8029 || (ch >= 8031 && ch <= 8061) || (ch >= 8064 && ch <= 8116) || (ch >= 8118 && ch <= 8124) || ch == 8126 || (ch >= 8130 && ch <= 8132) || (ch >= 8134 && ch <= 8140) || 
                        (ch >= 8144 && ch <= 8147) || (ch >= 8150 && ch <= 8155) || (ch >= 8160 && ch <= 8172) || (ch >= 8178 && ch <= 8180) || (ch >= 8182 && ch <= 8188) || ch == 8305 || ch == 8319 || (ch >= 8336 && ch <= 8348) || ch == 8450 || ch == 8455 || 
                        (ch >= 8458 && ch <= 8467) || ch == 8469 || (ch >= 8473 && ch <= 8477) || ch == 8484 || ch == 8486 || ch == 8488 || (ch >= 8490 && ch <= 8493) || (ch >= 8495 && ch <= 8505) || (ch >= 8508 && ch <= 8511) || (ch >= 8517 && ch <= 8521) || 
                        ch == 8526 || ch == 8579 || ch == 8580 || (ch >= 11264 && ch <= 11310) || (ch >= 11312 && ch <= 11358) || (ch >= 11360 && ch <= 11492) || (ch >= 11499 && ch <= 11502) || ch == 11506 || ch == 11507 || (ch >= 11520 && ch <= 11557) || ch == 11559 || ch == 11565 || 
                        (ch >= 11568 && ch <= 11623) || ch == 11631 || (ch >= 11648 && ch <= 11670) || (ch >= 11680 && ch <= 11686) || (ch >= 11688 && ch <= 11694) || (ch >= 11696 && ch <= 11702) || (ch >= 11704 && ch <= 11710) || (ch >= 11712 && ch <= 11718) || (ch >= 11720 && ch <= 11726) || (ch >= 11728 && ch <= 11734) || 
                        (ch >= 11736 && ch <= 11742) || ch == 11823 || ch == 12293 || ch == 12294 || (ch >= 12337 && ch <= 12341) || ch == 12347 || ch == 12348 || (ch >= 12353 && ch <= 12438) || (ch >= 12445 && ch <= 12447) || (ch >= 12449 && ch <= 12538) || (ch >= 12540 && ch <= 12543) || (ch >= 12549 && ch <= 12589) || 
                        (ch >= 12593 && ch <= 12686) || (ch >= 12704 && ch <= 12730) || (ch >= 12784 && ch <= 12799) || (ch >= 13312 && ch <= 19893) || (ch >= 19968 && ch <= 40917) || (ch >= 40960 && ch <= 42124) || (ch >= 42192 && ch <= 42237) || (ch >= 42240 && ch <= 42508) || (ch >= 42512 && ch <= 42539) || (ch >= 42560 && ch <= 42606) || 
                        (ch >= 42623 && ch <= 42653) || (ch >= 42656 && ch <= 42725) || (ch >= 42775 && ch <= 42783) || (ch >= 42786 && ch <= 42888) || (ch >= 42891 && ch <= 42925) || (ch >= 42928 && ch <= 42935) || (ch >= 42999 && ch <= 43009) || (ch >= 43011 && ch <= 43013) || (ch >= 43015 && ch <= 43018) || (ch >= 43020 && ch <= 43042) || 
                        (ch >= 43072 && ch <= 43123) || (ch >= 43138 && ch <= 43187) || (ch >= 43216 && ch <= 43225) || (ch >= 43250 && ch <= 43255) || ch == 43259 || ch == 43261 || (ch >= 43264 && ch <= 43301) || (ch >= 43312 && ch <= 43334) || (ch >= 43360 && ch <= 43388) || (ch >= 43396 && ch <= 43442) || 
                        (ch >= 43471 && ch <= 43481) || (ch >= 43488 && ch <= 43492) || (ch >= 43494 && ch <= 43518) || (ch >= 43520 && ch <= 43560) || (ch >= 43584 && ch <= 43586) || (ch >= 43588 && ch <= 43595) || (ch >= 43600 && ch <= 43609) || (ch >= 43616 && ch <= 43638) || ch == 43642 || (ch >= 43646 && ch <= 43695) || 
                        ch == 43697 || ch == 43701 || ch == 43702 || (ch >= 43705 && ch <= 43709) || ch == 43712 || ch == 43714 || (ch >= 43739 && ch <= 43741) || (ch >= 43744 && ch <= 43754) || (ch >= 43762 && ch <= 43764) || (ch >= 43777 && ch <= 43782) || (ch >= 43785 && ch <= 43790) || 
                        (ch >= 43793 && ch <= 43798) || (ch >= 43808 && ch <= 43814) || (ch >= 43816 && ch <= 43822) || (ch >= 43824 && ch <= 43866) || (ch >= 43868 && ch <= 43877) || (ch >= 43888 && ch <= 44002) || (ch >= 44016 && ch <= 44025) || (ch >= 44032 && ch <= 55203) || (ch >= 55216 && ch <= 55238) || (ch >= 55243 && ch <= 55291) || 
                        (ch >= 63744 && ch <= 64109) || (ch >= 64112 && ch <= 64217) || (ch >= 64256 && ch <= 64262) || (ch >= 64275 && ch <= 64279) || ch == 64285 || (ch >= 64287 && ch <= 64296) || (ch >= 64298 && ch <= 64310) || (ch >= 64312 && ch <= 64316) || ch == 64318 || ch == 64320 || ch == 64321 || 
                        ch == 64323 || ch == 64324 || (ch >= 64326 && ch <= 64433) || (ch >= 64467 && ch <= 64829) || (ch >= 64848 && ch <= 64911) || (ch >= 64914 && ch <= 64967) || (ch >= 65008 && ch <= 65019) || (ch >= 65136 && ch <= 65140) || (ch >= 65142 && ch <= 65276) || (ch >= 65296 && ch <= 65305) || (ch >= 65313 && ch <= 65338) || 
                        (ch >= 65345 && ch <= 65370) || (ch >= 65382 && ch <= 65470) || (ch >= 65474 && ch <= 65479) || (ch >= 65482 && ch <= 65487) || (ch >= 65490 && ch <= 65495) || (ch >= 65498 && ch <= 65500) || (ch >= 65536 && ch <= 65547) || (ch >= 65549 && ch <= 65574) || (ch >= 65576 && ch <= 65594) || ch == 65596 || ch == 65597 || 
                        (ch >= 65599 && ch <= 65613) || (ch >= 65616 && ch <= 65629) || (ch >= 65664 && ch <= 65786) || (ch >= 66176 && ch <= 66204) || (ch >= 66208 && ch <= 66256) || (ch >= 66304 && ch <= 66335) || (ch >= 66352 && ch <= 66368) || (ch >= 66370 && ch <= 66377) || (ch >= 66384 && ch <= 66421) || (ch >= 66432 && ch <= 66461) || 
                        (ch >= 66464 && ch <= 66499) || (ch >= 66504 && ch <= 66511) || (ch >= 66560 && ch <= 66717) || (ch >= 66720 && ch <= 66729) || (ch >= 66816 && ch <= 66855) || (ch >= 66864 && ch <= 66915) || (ch >= 67072 && ch <= 67382) || (ch >= 67392 && ch <= 67413) || (ch >= 67424 && ch <= 67431) || (ch >= 67584 && ch <= 67589) || 
                        ch == 67592 || (ch >= 67594 && ch <= 67637) || ch == 67639 || ch == 67640 || ch == 67644 || (ch >= 67647 && ch <= 67669) || (ch >= 67680 && ch <= 67702) || (ch >= 67712 && ch <= 67742) || (ch >= 67808 && ch <= 67826) || ch == 67828 || ch == 67829 || (ch >= 67840 && ch <= 67861) || 
                        (ch >= 67872 && ch <= 67897) || (ch >= 67968 && ch <= 68023) || ch == 68030 || ch == 68031 || ch == 68096 || (ch >= 68112 && ch <= 68115) || (ch >= 68117 && ch <= 68119) || (ch >= 68121 && ch <= 68147) || (ch >= 68192 && ch <= 68220) || (ch >= 68224 && ch <= 68252) || (ch >= 68288 && ch <= 68295) || 
                        (ch >= 68297 && ch <= 68324) || (ch >= 68352 && ch <= 68405) || (ch >= 68416 && ch <= 68437) || (ch >= 68448 && ch <= 68466) || (ch >= 68480 && ch <= 68497) || (ch >= 68608 && ch <= 68680) || (ch >= 68736 && ch <= 68786) || (ch >= 68800 && ch <= 68850) || (ch >= 69635 && ch <= 69687) || (ch >= 69734 && ch <= 69743) || 
                        (ch >= 69763 && ch <= 69807) || (ch >= 69840 && ch <= 69864) || (ch >= 69872 && ch <= 69881) || (ch >= 69891 && ch <= 69926) || (ch >= 69942 && ch <= 69951) || (ch >= 69968 && ch <= 70002) || ch == 70006 || (ch >= 70019 && ch <= 70066) || (ch >= 70081 && ch <= 70084) || (ch >= 70096 && ch <= 70106) || 
                        ch == 70108 || (ch >= 70144 && ch <= 70161) || (ch >= 70163 && ch <= 70187) || (ch >= 70272 && ch <= 70278) || ch == 70280 || (ch >= 70282 && ch <= 70285) || (ch >= 70287 && ch <= 70301) || (ch >= 70303 && ch <= 70312) || (ch >= 70320 && ch <= 70366) || (ch >= 70384 && ch <= 70393) || 
                        (ch >= 70405 && ch <= 70412) || ch == 70415 || ch == 70416 || (ch >= 70419 && ch <= 70440) || (ch >= 70442 && ch <= 70448) || ch == 70450 || ch == 70451 || (ch >= 70453 && ch <= 70457) || ch == 70461 || ch == 70480 || (ch >= 70493 && ch <= 70497) || (ch >= 70784 && ch <= 70831) || 
                        ch == 70852 || ch == 70853 || ch == 70855 || (ch >= 70864 && ch <= 70873) || (ch >= 71040 && ch <= 71086) || (ch >= 71128 && ch <= 71131) || (ch >= 71168 && ch <= 71215) || ch == 71236 || (ch >= 71248 && ch <= 71257) || (ch >= 71296 && ch <= 71338) || (ch >= 71360 && ch <= 71369) || 
                        (ch >= 71424 && ch <= 71449) || (ch >= 71472 && ch <= 71481) || (ch >= 71840 && ch <= 71913) || ch == 71935 || (ch >= 72384 && ch <= 72440) || (ch >= 73728 && ch <= 74649) || (ch >= 74880 && ch <= 75075) || (ch >= 77824 && ch <= 78894) || (ch >= 82944 && ch <= 83526) || (ch >= 92160 && ch <= 92728) || 
                        (ch >= 92736 && ch <= 92766) || (ch >= 92768 && ch <= 92777) || (ch >= 92880 && ch <= 92909) || (ch >= 92928 && ch <= 92975) || (ch >= 92992 && ch <= 92995) || (ch >= 93008 && ch <= 93017) || (ch >= 93027 && ch <= 93047) || (ch >= 93053 && ch <= 93071) || (ch >= 93952 && ch <= 94020) || ch == 94032 || 
                        (ch >= 94099 && ch <= 94111) || ch == 110592 || ch == 110593 || (ch >= 113664 && ch <= 113770) || (ch >= 113776 && ch <= 113788) || (ch >= 113792 && ch <= 113800) || (ch >= 113808 && ch <= 113817) || (ch >= 119808 && ch <= 119892) || (ch >= 119894 && ch <= 119964) || ch == 119966 || ch == 119967 || ch == 119970 || 
                        ch == 119973 || ch == 119974 || (ch >= 119977 && ch <= 119980) || (ch >= 119982 && ch <= 119993) || ch == 119995 || (ch >= 119997 && ch <= 120003) || (ch >= 120005 && ch <= 120069) || (ch >= 120071 && ch <= 120074) || (ch >= 120077 && ch <= 120084) || (ch >= 120086 && ch <= 120092) || (ch >= 120094 && ch <= 120121) || 
                        (ch >= 120123 && ch <= 120126) || (ch >= 120128 && ch <= 120132) || ch == 120134 || (ch >= 120138 && ch <= 120144) || (ch >= 120146 && ch <= 120485) || (ch >= 120488 && ch <= 120512) || (ch >= 120514 && ch <= 120538) || (ch >= 120540 && ch <= 120570) || (ch >= 120572 && ch <= 120596) || (ch >= 120598 && ch <= 120628) || 
                        (ch >= 120630 && ch <= 120654) || (ch >= 120656 && ch <= 120686) || (ch >= 120688 && ch <= 120712) || (ch >= 120714 && ch <= 120744) || (ch >= 120746 && ch <= 120770) || (ch >= 120772 && ch <= 120779) || (ch >= 120782 && ch <= 120831) || (ch >= 124928 && ch <= 125124) || (ch >= 126464 && ch <= 126467) || (ch >= 126469 && ch <= 126495) || 
                        ch == 126497 || ch == 126498 || ch == 126500 || ch == 126503 || (ch >= 126505 && ch <= 126514) || (ch >= 126516 && ch <= 126519) || ch == 126521 || ch == 126523 || ch == 126530 || ch == 126535 || ch == 126537 || 
                        ch == 126539 || (ch >= 126541 && ch <= 126543) || ch == 126545 || ch == 126546 || ch == 126548 || ch == 126551 || ch == 126553 || ch == 126555 || ch == 126557 || ch == 126559 || ch == 126561 || ch == 126562 || 
                        ch == 126564 || (ch >= 126567 && ch <= 126570) || (ch >= 126572 && ch <= 126578) || (ch >= 126580 && ch <= 126583) || (ch >= 126585 && ch <= 126588) || ch == 126590 || (ch >= 126592 && ch <= 126601) || (ch >= 126603 && ch <= 126619) || (ch >= 126625 && ch <= 126627) || (ch >= 126629 && ch <= 126633) || 
                        (ch >= 126635 && ch <= 126651) || (ch >= 131072 && ch <= 173782) || (ch >= 173824 && ch <= 177972) || (ch >= 177984 && ch <= 178205) || (ch >= 178208 && ch <= 183969) || (ch >= 194560 && ch <= 195101)) {
                    sb.Append(char.ConvertFromUtf32(ch));
                    ++cc;
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                    matched = true;
                    goto q44;
                }
                if(hasError) {
                    ai = (int)(absoluteIndex - errorIndex);
                    yield return (AbsolutePosition: errorIndex, AbsoluteLength: ai, Position: errorPos, Length: (int)(position - errorPos), SymbolId: ERROR, Value: sb.ToString(0, ai), Line: errorLine, Column: errorColumn);
                    sb.Remove(0, ai);
                    hasError = false;
                
                }
                if(sb.Length > 0) {
                    yield return (AbsolutePosition: absoluteIndex, AbsoluteLength: (int)(absi - absoluteIndex), Position: position, Length: (int)(cursorPos - position), SymbolId: Identifier, Value: sb.ToString(), Line: line, Column: column);
                }
                sb.Clear();
                continue;
            error:
                if(!matched && hasError) {
                    switch (ch) {
                    case '\t':
                        cc = (((cc - 1) / tabWidth) + 1) * tabWidth + 1;
                        break;
                    case '\r':
                        cc = 1;
                        break;
                    case '\n':
                        cc = 1;
                        ++lc;
                        break;
                    default:
                        if(ch > 31) ++cc;
                        break;
                    }
                    sb.Append(char.ConvertFromUtf32(ch));
                    ch = ReadUtf32(cursor, out adv);
                    absi += adv;
                    ++cursorPos;
                }
                if(!hasError) {
                    errorPos = position;
                    errorIndex = absoluteIndex;
                    errorColumn = column;
                    errorLine = line;
                }
                hasError = true;
            }
            if(hasError && sb.Length > 0) {
                yield return (AbsolutePosition: errorIndex, AbsoluteLength: sb.Length, Position: errorPos, Length: (int)(cursorPos - errorPos), SymbolId: ERROR, Value: sb.ToString(), Line: errorLine, Column: errorColumn);
            
            }
        }
        /// <summary>Indicates the symbol id for the ERROR symbol</summary>
        public const int ERROR = -1;
        /// <summary>Indicates the symbol id for the VerbatimStringLiteral symbol</summary>
        public const int VerbatimStringLiteral = 0;
        /// <summary>Indicates the symbol id for the StringLiteral symbol</summary>
        public const int StringLiteral = 1;
        /// <summary>Indicates the symbol id for the CharacterLiteral symbol</summary>
        public const int CharacterLiteral = 2;
        /// <summary>Indicates the symbol id for the IntegerLiteral symbol</summary>
        public const int IntegerLiteral = 3;
        /// <summary>Indicates the symbol id for the FloatLiteral symbol</summary>
        public const int FloatLiteral = 4;
        /// <summary>Indicates the symbol id for the Whitespace symbol</summary>
        public const int Whitespace = 5;
        /// <summary>Indicates the symbol id for the Identifier symbol</summary>
        public const int Identifier = 6;
        /// <summary>Indicates the symbol id for the CommentBlock symbol</summary>
        public const int CommentBlock = 40;
        
    }
}

