using Elf.Syntax.Ast;

namespace Elf.Core.Assembler
{
    public abstract class ElfVmInstruction
    {
        public AstNode AstNode { get; private set; }

        public ElfVmInstruction BindToAstNode(AstNode node)
        {
            // horrible, but works for now
            if (AstNode == null) 
                AstNode = node; 
            return this;
        }
    }
}