using System;

namespace Elf.Helpers
{
    public class Span
    {
        public int Start{ get; private set; }
        public int End { get; private set; }

        private Span(int start, int end)
        {
            Start = start;
            End = end;
        }

        public static Span FromBounds(int start, int end)
        {
            return new Span(start, end);
        }

        public static Span FromLength(int start, int len)
        {
            return new Span(start, start + len);
        }

        public static Span Invalid
        {
            get
            {
                return new Span(-1, -1);
            }
        }

        public override string ToString()
        {
            return this == Invalid ? "<invalid>" :
                String.Format("[{0}..{1}]", Start, End);
        }

        public static bool operator ==(Span o1, Span o2)
        {
            return Equals(o1, o2);
        }

        public static bool operator !=(Span o1, Span o2)
        {
            return !(o1 == o2);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Span;
            return Start == other.Start && End == other.End;
        }

        public override int GetHashCode()
        {
            var num = 0x10cee8ad;
            num = (-1521134295 * num) + Start.GetHashCode();
            num = (-1521134295 * num) + End.GetHashCode();
            return num;
        }
    }
}