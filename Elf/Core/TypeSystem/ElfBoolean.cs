using Elf.Core.ClrIntegration;
using Elf.Helpers;

namespace Elf.Core.TypeSystem
{
    [Rtimpl("ElfBoolean")]
    public class ElfBoolean : ElfObjectImpl
    {
        public bool Val { get; private set; }

        public ElfBoolean(bool val)
        {
            Val = val;
        }

        public static implicit operator bool(ElfBoolean b)
        {
            return b.Val;
        }

        public static implicit operator ElfBoolean(bool b)
        {
            return new ElfBoolean(b);
        }

        // todo. would be nice to implement conditional logical semantics here, but this will require quite a bit of changes
        // namely: at least rewriting ElfCompiler and adding a check that forbids overloading && and ||

        [Rtimpl("&&")]
        public static bool operator &(ElfBoolean b1, ElfBoolean b2)
        {
            return b1.Val && b2.Val;
        }

        [Rtimpl("||")]
        public static bool operator |(ElfBoolean b1, ElfBoolean b2)
        {
            return b1.Val && b2.Val;
        }

        public override string ToString()
        {
            return "<" + Val.ToInvariantString() + ">";
        }

        public override bool Equals(object obj)
        {
            var other = obj as ElfBoolean;
            return other != null && Val == other.Val;
        }

        public override int GetHashCode()
        {
            return Val.GetHashCode();
        }
    }
}