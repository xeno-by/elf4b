using System;

namespace Elf.Core.Assembler
{
    public class PushRef : ElfVmInstruction
    {
        public String Ref { get; private set; }

        public PushRef(String @ref)
        {
            Ref = @ref;
        }

        public override string ToString() { return "push " + Ref; }
    }
}