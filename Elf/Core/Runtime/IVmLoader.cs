using Elf.Core.Runtime;
using Elf.Syntax.Ast.Defs;

namespace Elf.Core.Runtime
{
    public interface IVmLoader : IVmBound
    {
        void Load(Script script);
    }
}