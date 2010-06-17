using System;

namespace Elf.Core.Assembler
{
    public class Jt : ElfVmInstruction
    {
        public String Label { get; private set; }

        public Jt(string label) 
        {
            Label = label;
        }

        public override string ToString() { return "jt " + Label; }
    }
}