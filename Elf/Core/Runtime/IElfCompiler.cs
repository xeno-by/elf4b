using Elf.Core.Assembler;
using Elf.Syntax.Ast.Defs;

namespace Elf.Core.Runtime
{
    public interface IElfCompiler : IVmBound
    {
        ElfVmInstruction[] Compile(FuncDef func);
    }
}