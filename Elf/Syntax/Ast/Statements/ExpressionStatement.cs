using Elf.Helpers;
using Elf.Syntax.Ast.Expressions;
using System.Linq;

namespace Elf.Syntax.Ast.Statements
{
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get { return (Expression)Children.ElementAt(0); } }

        public ExpressionStatement(Expression expression) 
            : base(AstNodeType.ExpressionStatement, expression.AsArray()) 
        {
        }

        protected override string GetTPathNode() { return null; }
        protected override string GetTPathSuffix(int childIndex) { return null; }
        protected override string GetContent() { return Indent + Expression.Content; }
    }
}