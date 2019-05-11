using System;
using System.Collections.Generic;
using System.Text;

namespace AheuiDotnet
{
    enum First
    {
        ㄱ,
        ㄲ,
        ㄴ,
        ㄷ,
        ㄸ,
        ㄹ,
        ㅁ,
        ㅂ,
        ㅃ,
        ㅅ,
        ㅆ,
        ㅇ,
        ㅈ,
        ㅉ,
        ㅊ,
        ㅋ,
        ㅌ,
        ㅍ,
        ㅎ,
    }

    enum Middle
    {
        ㅏ,
        ㅐ,
        ㅑ,
        ㅒ,
        ㅓ,
        ㅔ,
        ㅕ,
        ㅖ,
        ㅗ,
        ㅘ,
        ㅙ,
        ㅚ,
        ㅛ,
        ㅜ,
        ㅝ,
        ㅞ,
        ㅟ,
        ㅠ,
        ㅡ,
        ㅢ,
        ㅣ,
    }

    enum Last
    {
        _,
        ㄱ,
        ㄲ,
        ㄳ,
        ㄴ,
        ㄵ,
        ㄶ,
        ㄷ,
        ㄹ,
        ㄺ,
        ㄻ,
        ㄼ,
        ㄽ,
        ㄾ,
        ㄿ,
        ㅀ,
        ㅁ,
        ㅂ,
        ㅄ,
        ㅅ,
        ㅆ,
        ㅇ,
        ㅈ,
        ㅊ,
        ㅋ,
        ㅌ,
        ㅍ,
        ㅎ,
    }

    struct Token
    {
        public First operation;
        public Middle speed;
        public Last storage;

        public Token(First f, Middle m, Last l)
        {
            operation = f;
            speed = m;
            storage = l;
        }
    }

    class CodeArray
    {
        List<List<Token>> codeTokens = new List<List<Token>>();

        public static CodeArray Parse(string code)
        {
            var toReturn = new CodeArray();
            toReturn.codeTokens.Add(new List<Token>());
            int current = 0;
            foreach(char c in code)
            {
                if(c == '\n')
                {
                    toReturn.codeTokens.Add(new List<Token>());
                    current += 1;
                    continue;
                }
                int offChar = c - 0xAC00;
                int 종성 = offChar % 28;
                offChar -= 종성;
                offChar /= 28;
                int 중성 = offChar % 21;
                offChar -= 중성;
                offChar /= 21;
                int 초성 = offChar % 21;

                var op = (First)초성;
                var sp = (Middle)중성;
                var st = (Last)종성;
                // Console.Error.WriteLine(op.ToString());
                // Console.Error.WriteLine(sp.ToString());
                // Console.Error.WriteLine(st.ToString());
                toReturn.codeTokens[current].Add(new Token(op, sp, st));
            }

            return toReturn;
        }

        public Token TokenAt(int x, int y)
        {
            return codeTokens[y][x];
        }
    }
}
