using System;

namespace Elf.Core.Assembler.Literals
{
    public sealed class ElfStringLiteral : ElfLiteral<String>
    {
        public ElfStringLiteral(string val) 
            : base(val)
        {
        }
    }
}