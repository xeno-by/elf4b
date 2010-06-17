using System;

namespace Elf.Core.Assembler
{
    public class Decl : ElfVmInstruction
    {
        public String Name { get; private set; }

        public Decl(string name) 
        {
            Name = name;
        }

        public override string ToString() { return "decl " + Name; }
    }
}