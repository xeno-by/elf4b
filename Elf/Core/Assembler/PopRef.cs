using System;

namespace Elf.Core.Assembler
{
    public class PopRef : ElfVmInstruction
    {
        public String Ref { get; private set; }

        public PopRef(String @ref)
        {
            Ref = @ref;
        }

        public override string ToString() { return "pop " + Ref; }
    }
}