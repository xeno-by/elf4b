using System;
using System.Linq;

namespace Elf.Syntax.Ast.Expressions
{
    public class AssignmentExpression : Expression
    {
        public VariableExpression Target { get { return (VariableExpression)Children.ElementAt(0); } }
        public Expression Expression { get { return (Expression)Children.ElementAt(1); } }

        public AssignmentExpression(VariableExpression target, Expression expression) 
            : base(AstNodeType.AssignmentExpression, new AstNode[]{target, expression})
        {
        }

        protected override string GetTPathNode() { return "="; }
        protected override string GetTPathSuffix(int childIndex)
        {
            if (childIndex == 0) return "l";
            if (childIndex == 1) return "r";
            throw new NotSupportedException(childIndex.ToString());
        }

        protected override string GetContent() { return String.Format("{0} = {1}", Target.Content, Expression.Content); }
    }
}