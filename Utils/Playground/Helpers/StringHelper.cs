using System;
using System.Linq;

namespace Playground.Helpers
{
    public static class StringHelper
    {
        public static String Indent(this String s, int indent)
        {
            return s.Indent(new String(' ', 2 * indent));
        }

        public static String Indent(this String s, String indent)
        {
            return s.Split(new String[]{Environment.NewLine}, StringSplitOptions.None).Select(
                line => line.IsNullOrEmpty() ? line : indent + line).StringJoin(Environment.NewLine);
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

        public static String[] SelectLines(this String s)
        {
            return s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
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
    }
}