using System.Collections.Generic;

namespace Elf.Syntax.Ast.Statements
{
    public abstract class Statement : AstNode
    {
        protected Statement(AstNodeType nodeType, IEnumerable<AstNode> children) 
            : base(nodeType, children) 
        {
        }
    }
}