using System.Collections.Generic;
using Elf.Core.Assembler;
using Elf.Core.Reflection;
using Elf.Core.TypeSystem;

namespace Elf.Core.Runtime
{
    public interface IEntryPoint : IVmBound, IEnumerable<ElfVmInstruction>
    {
        NativeMethod CodePoint { get; }
        IElfObject This { get; }
        IElfObject[] Args { get; }
    }
}