using System.Collections.Generic;
using Elf.Core.Assembler;
using Elf.Core.Runtime.Contexts;
using Elf.Core.TypeSystem;

namespace Elf.Core.Runtime
{
    public interface IElfThread : IVmBound, IEnumerator<ElfVmInstruction>
    {
        ThreadStatus Status { get; }
        IElfObject ExecutionResult { get; }
        RuntimeContext RuntimeContext { get; }
        void Startup(IEntryPoint entryPoint);
    }
}