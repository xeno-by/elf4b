using Elf.Core.Reflection;
using Elf.Core.Runtime;

namespace Elf.Core.TypeSystem
{
    public interface IElfObject : IVmBound
    {
        ElfClass Type { get; }
    }
}