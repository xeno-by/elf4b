using System;
using System.Linq;

namespace Elf.Helpers
{
    public static class StringHelper
    {
        public static String ToVerbatimCSharpCopy(this String s)
        {
            return s.Select(c => (c != '\\' && c != '\"') ? new String(c, 1) : "\\" + c)
                .StringJoin(String.Empty);
        }

        public static int DetectExcessiveIndentation(this String s)
        {
            var statistic = s.SelectLines().Where(line => !line.IsNullOrEmpty())
                .Select(line => line.DetectIndentation());
            return statistic.Any() ? statistic.Min() : 0;
        }

        private static int DetectIndentation(this String line)
        {
            int numOfSpaces;
            for (numOfSpaces = 0; numOfSpaces < line.Length && line[numOfSpaces] == ' '; numOfSpaces++) ;
            return numOfSpaces;
        }

        public static String TrimExcessiveIndendation(this String s)
        {
            var excessive = s.DetectExcessiveIndentation();
            return s.SelectLines().Select(line => 
                line.IsNullOrEmpty() ? line : line.Substring(excessive)).StringJoin(Environment.NewLine);
        }

        public static String TrimWrappingNewLines(this String s)
        {
            if (s.StartsWith(Environment.NewLine)) s = s.Substring(2);
            if (s.EndsWith(Environment.NewLine)) s = s.Substring(0, s.Length - 2);
            return s;
        }

        public static String InjectLineNumbers0(this String elfCode)
        {
            var lines = elfCode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            lines.ForEach((line, i) => lines[i] =
                i.ToString("D" + (lines.Length - 1).ToString().Length) + ": " + line);
            return lines.StringJoin(Environment.NewLine);
        }

        public static String InjectLineNumbers1(this String elfCode)
        {
            var lines = elfCode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            lines.ForEach((line, i) => lines[i] =
                (i + 1).ToString("D" + lines.Length.ToString().Length) + ": " + line);
            return lines.StringJoin(Environment.NewLine);
        }

        public static String InjectErrorMarker(this String elfCode, 
            int errorLineNumber, int errorCharNumber, int errorSpan)
        {
            var abs = new LineCharIndex(errorLineNumber, errorCharNumber).ToAbsolute(elfCode);
            if (errorSpan <= 0)
            {
                return elfCode.Substring(0, abs) + "[ERROR>>>]" + elfCode.Substring(abs);
            }
            else
            {
                var thres = Math.Min(abs + errorSpan, elfCode.Length);
                return elfCode.Substring(0, abs) + "[ERROR>>> " + 
                    elfCode.Substring(abs, thres - abs) + "]" + elfCode.Substring(thres);
            }
        }

        public static String Indent(this String s, int indent)
        {
            return s.Indent(new String(' ', 2 * indent));
        }

        public static String Indent(this String s, String indent)
        {
            if (s == null) return indent;
            return s.Split(new String[]{Environment.NewLine}, StringSplitOptions.None).Select(
                line => line.IsNullOrEmpty() ? line : indent + line).StringJoin(Environment.NewLine);
        }

        public static String[] SelectLines(this String s)
        {
            return s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public static String Substring(this String s, Span span)
        {
            if (s == null)
            {
                return null;
            }
            else
            {
                if (span == Span.Invalid)
                {
                    return null;
                }
                else
                {
                    var thres = Math.Min(span.End, s.Length - 1);
                    return s.Substring(span.Start, thres - span.Start);
                }
            }
        }

        public static int GetAbsoluteIndex(this String s, int lineNumber, int charPosition)
        {
            return new LineCharIndex(lineNumber, charPosition).ToAbsolute(s);
        }

        public static int NthIndexOf(this String s, String substring, int n)
        {
            if (n <= 0)
            {
                return -1;
            }
            else
            {
                var shift = 0;
                Enumerable.Range(1, n).ForEach(i => {
                    var index = s.IndexOf(substring);
                    if (i != n)
                    {
                        shift += index + 3;
                        s = s.Substring(index + 3);
                    }
                });

                return shift + s.IndexOf(substring);
            }
        }

        // todo. handle escape sequences here as well
        public static String Quote(this String s)
        {
            return "'" + s + "'";
        }

        // todo. handle escape sequences here as well
        public static String Unquote(this String s)
        {
            if (s.Length >= 2)
            {
                if (s[0] == '\'' && s[s.Length - 1] == '\'')
                {
                    return s.Substring(1, s.Length - 2);
                }

                if (s[0] == '\"' && s[s.Length - 1] == '\"')
                {
                    return s.Substring(1, s.Length - 2);
                }
            }

            return s;
        }
    }
}