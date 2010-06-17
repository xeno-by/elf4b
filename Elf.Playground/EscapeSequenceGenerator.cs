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
            DumpJavaEscapeSequence("����������");
            DumpJavaEscapeSequence("�����");
            DumpJavaEscapeSequence("��������");
            DumpJavaEscapeSequence("��������");
            DumpJavaEscapeSequence("��������");
            DumpJavaEscapeSequence("�������");
            DumpJavaEscapeSequence("����");
            DumpJavaEscapeSequence("��");
            DumpJavaEscapeSequence("�����");
            DumpJavaEscapeSequence("�����");
            DumpJavaEscapeSequence("��");
            DumpJavaEscapeSequence("�");
            DumpJavaEscapeSequence("���");

            DumpJavaEscapeSequence("�"); // 0x0410
            DumpJavaEscapeSequence("�"); // 0x042f
            DumpJavaEscapeSequence("�"); // 0x0430
            DumpJavaEscapeSequence("�"); // 0x044f

            // http://www.utf8-chartable.de/unicode-utf8-table.pl
            // the aforementioned character range (32+32) leaves behind the � letter
            // which resides at the following positions: 0x0451 (�) and 0x0401 (�)
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