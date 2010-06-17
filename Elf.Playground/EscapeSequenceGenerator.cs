using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace Elf.Playground
{
    [TestFixture]
    public class EscapeSequenceGenerator
    {
        [Test]
        public void GenerateJavaEscapeSequencesForRussianTokens()
        {
            DumpJavaEscapeSequence("определить");
            DumpJavaEscapeSequence("пакет");
            DumpJavaEscapeSequence("сценарий");
            DumpJavaEscapeSequence("прототип");
            DumpJavaEscapeSequence("объявить");
            DumpJavaEscapeSequence("вернуть");
            DumpJavaEscapeSequence("если");
            DumpJavaEscapeSequence("то");
            DumpJavaEscapeSequence("иначе");
            DumpJavaEscapeSequence("конец");
            DumpJavaEscapeSequence("не");
            DumpJavaEscapeSequence("и");
            DumpJavaEscapeSequence("или");

            DumpJavaEscapeSequence("А"); // 0x0410
            DumpJavaEscapeSequence("Я"); // 0x042f
            DumpJavaEscapeSequence("а"); // 0x0430
            DumpJavaEscapeSequence("я"); // 0x044f

            // http://www.utf8-chartable.de/unicode-utf8-table.pl
            // the aforementioned character range (32+32) leaves behind the Ё letter
            // which resides at the following positions: 0x0451 (ё) and 0x0401 (Ё)
        }

        private void DumpJavaEscapeSequence(String s)
        {
            var sb = new StringBuilder();
            foreach(var c in s)
            {
                sb.AppendFormat("\\u{0:x4}", (int)c);
            }

            Trace.WriteLine(String.Format("{0} -> {1}", s, sb));
        }
    }
}