using Elf.Core.Reflection;

namespace Elf.Core.TypeSystem
{
    public class ElfVoid : ElfObjectImpl
    {
        protected override string TypeName { get { return null; } }
        public override ElfClass Type { get { return null; } }

        public override string ToString()
        {
            return "<void>";
        }

        public override bool Equals(object obj)
        {
            return obj is ElfVoid;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}