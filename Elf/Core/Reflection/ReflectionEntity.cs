using Elf.Syntax.Ast;

namespace Elf.Core.Reflection
{
    public abstract class ReflectionEntity
    {
        public AstNode AstNode { get; private set; }

        protected ReflectionEntity(AstNode astNode) 
        {
            AstNode = astNode;
        }
    }
}