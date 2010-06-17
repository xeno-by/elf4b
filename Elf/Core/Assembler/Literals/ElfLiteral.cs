using System.Collections.Generic;
using Elf.Helpers;

namespace Elf.Core.Assembler.Literals
{
    public abstract class ElfLiteral
    {
        public static bool operator ==(ElfLiteral o1, ElfLiteral o2)
        {
            return Equals(o1, o2);
        }

        public static bool operator !=(ElfLiteral o1, ElfLiteral o2)
        {
            return !(o1 == o2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public abstract class ElfLiteral<T> : ElfLiteral
    {
        public T Val { get; private set; }

        protected ElfLiteral(T val)
        {
            Val = val;
        }

        public override string ToString()
        {
            return "<" + Val.ToInvariantString() + ">";
        }

        public override bool Equals(object obj)
        {
            var other = obj as ElfLiteral<T>;
            return other != null && EqualityComparer<T>.Default.Equals(Val, other.Val);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Val);
        }
    }
}