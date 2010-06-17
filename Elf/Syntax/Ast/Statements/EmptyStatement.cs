using System.Linq;

namespace Elf.Syntax.Ast.Statements
{
    public class EmptyStatement : Statement
    {
        public EmptyStatement()
            : base(AstNodeType.EmptyStatement, Enumerable.Empty<AstNode>()) 
        {
        }

        protected override string GetTPathNode() { return ";"; }
        protected override string GetTPathSuffix(int childIndex) { return null; }
        protected override string GetContent() { return null; }
    }
}