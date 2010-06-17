using System;
using Elf.Core.ClrIntegration;
using Elf.Helpers;

namespace Elf.Core.TypeSystem
{
    [Rtimpl("ElfString")]
    public class ElfString : ElfObjectImpl
    {
        public String Val { get; private set; }

        public ElfString(String val)
        {
            Val = val;
        }

        public static implicit operator ElfString(string s)
        {
            return new ElfString(s);
        }

        public static implicit operator string(ElfString s)
        {
            return s.Val;
        }

        public override string ToString()
        {
            return "<" + Val.ToInvariantString() + ">";
        }

        public override bool Equals(object obj)
        {
            var other = obj as ElfString;
            return other != null && Val == other.Val;
        }

        public override int GetHashCode()
        {
            return Val == null ? 0 : Val.GetHashCode();
        }
    }
}