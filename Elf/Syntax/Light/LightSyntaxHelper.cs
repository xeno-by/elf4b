using System;
using System.Linq;
using Elf.Helpers;

namespace Elf.Syntax.Light
{
    public static class LightSyntaxHelper
    {
        public static String ToCanonicalElf(this String elfLight)
        {
            return elfLight.ToCanonicalElf("LightSyntaxHost");
        }

        // todo. check correctness of EOLs (so that every eol is an Environment.NewLine)
        // todo. check encoding (so that it's UTF-8)
        public static String ToCanonicalElf(this String elfLight, String rtimplOf)
        {
            var funcBody = elfLight;
//            if (funcBody.SelectLines().Length == 1 && !funcBody.Contains(";")) funcBody = "ret " + funcBody;

            var funcDef = String.Format("def Main(){0}{1}{0}end",
                Environment.NewLine, funcBody.Indent(1));

            var className = ("Host_" + Guid.NewGuid()).Where(c => c != '-').ToArray();
            return String.Format("def {1} rtimpl {2}{0}{3}{0}end",
                Environment.NewLine, new String(className), rtimplOf, funcDef.Indent(1));
        }

        public static String RecalculateSourceCode(this String canonicalElf)
        {
            var lines = canonicalElf.SelectLines().AsEnumerable();
            lines = lines.Skip(2).Take(lines.Count() - 4).Select(line => line.Substring(4));
            return lines.StringJoin(Environment.NewLine);
        }

        public static Span RecalculateErrorSpan(this Span errorSpan, String canonicalElf)
        {
            var lines = canonicalElf.SelectLines();
            var lci = LineCharIndex.FromAbsolute(canonicalElf, errorSpan.Start);

            var recalculatedStart = errorSpan.Start - lines[0].Length - 2 - lines[1].Length - 2;
            recalculatedStart -= (lci.LineNumber - 2) * 4;
            return Span.FromLength(recalculatedStart, errorSpan.End - errorSpan.Start);
        }
    }
}