using System;

namespace Elf.Core.Assembler
{
    public class Jf : ElfVmInstruction
    {
        public String Label { get; private set; }

        public Jf(string label) 
        {
            Label = label;
        }

        public override string ToString() { return "jf " + Label; }
    }
}