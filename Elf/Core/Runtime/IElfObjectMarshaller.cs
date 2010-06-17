using System;
using Elf.Core.TypeSystem;
using Elf.Core.Runtime;

namespace Elf.Core.Runtime
{
    public interface IElfObjectMarshaller : IVmBound
    {
        Object Marshal(IElfObject elf);
        IElfObject Unmarshal(Object clr);
    }
}