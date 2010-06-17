using System;
using System.Linq;

namespace Elf.Syntax.Ast.Expressions
{
    public class VariableExpression : Expression
    {
        public String Name { get; private set; }

        public VariableExpression(String name) 
            : base(AstNodeType.VariableExpression, Enumerable.Empty<AstNode>()) 
        {
            Name = name;
        }

        protected override string GetTPathNode() { return "v:" + Name; }
        protected override string GetTPathSuffix(int childIndex) { return null; }
        protected override string GetContent() { return Name; }
    }
}