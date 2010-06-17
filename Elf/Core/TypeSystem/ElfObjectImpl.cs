using System;
using Elf.Core.ClrIntegration;
using Elf.Core.Reflection;
using System.Linq;
using Elf.Helpers;

namespace Elf.Core.TypeSystem
{
    public abstract class ElfObjectImpl : IElfObject
    {
        public VirtualMachine VM { get; private set; }
        protected virtual String TypeName { get { return GetType().RtimplOf(); } }
        public virtual ElfClass Type { get { return VM.Classes.Single(c => c.Name == TypeName); } }

        public virtual void Bind(VirtualMachine vm)
        {
            VM = vm;
        }

        [Rtimpl("==")]
        public static bool operator ==(ElfObjectImpl o1, ElfObjectImpl o2)
        {
            return Equals(o1, o2);
        }

        [Rtimpl("!=")]
        public static bool operator !=(ElfObjectImpl o1, ElfObjectImpl o2)
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
}