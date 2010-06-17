using System;
using System.Linq;
using Elf.Helpers;
using Elf.Syntax.Ast.Expressions;

namespace Elf.Syntax.Ast.Statements
{
    public class ReturnStatement : Statement
    {
        public Expression Expression { get { return (Expression)Children.SingleOrDefault(); } }

        public ReturnStatement()
            : base(AstNodeType.ReturnStatement, Enumerable.Empty<AstNode>())
        {
        }

        public ReturnStatement(Expression expression) 
            : base(AstNodeType.ReturnStatement, expression.AsArray()) 
        {
        }

        protected override string GetTPathNode() { return "ret"; }
        protected override string GetTPathSuffix(int childIndex) { return null; }
        protected override string GetContent() { return Indent + String.Format("ret {0}", Expression.Content); }
    }
}