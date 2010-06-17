using System;

namespace Elf.Core.Assembler
{
    public class Invoke : ElfVmInstruction
    {
        public String Name { get; private set; }
        public int Argc { get; private set; }

        public Invoke(string name, int argc) 
        {
            Name = name;
            Argc = argc;
        }

        public override string ToString() { return "invoke" + Argc + " " + Name; }
    }
}