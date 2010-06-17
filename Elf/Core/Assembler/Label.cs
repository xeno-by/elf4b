using System;

namespace Elf.Core.Assembler
{
    public class Label : ElfVmInstruction
    {
        public String Name { get; private set; }

        public Label(string name) 
        {
            Name = name;
        }

        public override string ToString() { return "label " + Name; }
    }
}