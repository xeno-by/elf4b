using System.Collections.Generic;

namespace Elf.Syntax.Ast.Expressions
{
    public abstract class Expression : AstNode
    {
        protected Expression(AstNodeType nodeType, IEnumerable<AstNode> children) 
            : base(nodeType, children) 
        {
        }
    }
}