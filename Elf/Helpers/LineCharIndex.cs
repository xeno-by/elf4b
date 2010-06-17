using System;

namespace Elf.Helpers
{
    public class LineCharIndex
    {
        // line numbering starts from 1 (ANTLR-compatible)
        public int LineNumber { get; private set; }

        // char numbering starts from 0
        public int CharPosition { get; private set; }

        public LineCharIndex(int lineNumber, int charPosition)
        {
            LineNumber = lineNumber;
            CharPosition = charPosition;
        }

        public int ToAbsolute(String s)
        {
            var lineNumber = 1;
            var charPosition = 0;

            for (var i = 0; i < s.Length; ++i)
            {
                var c = s[i];

                if (lineNumber == LineNumber &&
                    charPosition == CharPosition)
                {
                    return i;
                }

                // todo. introduce better support for newlines
                if (c == '\n')
                {
                    ++lineNumber;
                    charPosition = 0;
                }
                else
                {
                    ++charPosition;
                }
            }

            return -1;
        }

        public static LineCharIndex FromAbsolute(String s, int index)
        {
            if (index < 0 || s.Length <= index)
            {
                return null;
            }
            else
            {
                if (index == 0)
                {
                    return new LineCharIndex(1, 0);
                }
                else
                {
                    var pre = s.Substring(0, index - 1);
                    // todo. introduce better support for newlines
                    var line = pre.Substring(pre.LastIndexOf('\n') + 1);
                    return new LineCharIndex(pre.Split('\n').Length, line.Length);
                }
            }
        }
    }
}