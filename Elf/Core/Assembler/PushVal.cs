using Elf.Core.Assembler.Literals;

namespace Elf.Core.Assembler
{
    public class PushVal : ElfVmInstruction
    {
        public ElfLiteral Val { get; private set; }

        public PushVal(ElfLiteral val) 
        {
            Val = val;
        }

        public override string ToString() { return "push " + Val
            // hack to get rid of <>'s when showing ELFA
            .ToString().Substring(1, Val.ToString().Length - 2); }
    }
}