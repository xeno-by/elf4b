using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Playground.Helpers
{
    public static class StringDebugHelper
    {
        public static void Print(this String s, String fileName)
        {
            using(var sw = new StreamWriter(fileName, false))
            {
                sw.Write(s);
            }
        }

        public static void Print(this IEnumerable<String> s, String fileName)
        {
            using (var sw = new StreamWriter(fileName, false))
            {
                s.ForEach(s1 => sw.WriteLine(s1));
            }
        }

        public static void PrintSideBySide(IEnumerable<String> s1, IEnumerable<String> s2, String fileName)
        {
            using (var fs = new StreamWriter(fileName, false))
            {
                var width = s1.Select(evi => evi.Length).Max();
                Func<String, int, String> ensureWidth = (s, w) => String.Format("{0}{1}",
                    s, new String(' ', w - (s == null ? 0 : s.Length)));

                for (var i = 0; i < Math.Max(s1.Count(), s2.Count()); i++)
                {
                    fs.WriteLine(String.Format("{0}    {1}",
                        ensureWidth(i < s1.Count() ? s1.ElementAt(i) : null, width),
                        i < s2.Count() ? s2.ElementAt(i) : null));
                }
            }
        }
    }
}