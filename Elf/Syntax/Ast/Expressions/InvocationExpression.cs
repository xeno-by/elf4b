using System;
using System.Collections.Generic;
using System.Linq;
using Elf.Helpers;

namespace Elf.Syntax.Ast.Expressions
{
    public class InvocationExpression : Expression
    {
        public String Name { get; private set; }
        public IEnumerable<Expression> Args { get { return Children.Cast<Expression>(); } }

        public InvocationExpression(String name, IEnumerable<Expression> args)
            : base(AstNodeType.InvocationExpression, args.Cast<AstNode>()) 
        {
            Name = name;
        }

        protected override string GetTPathNode() { return Name; }
        protected override string GetTPathSuffix(int childIndex) { return "arg" + childIndex; }
        protected override string GetContent() { return String.Format("{0}({1})", Name, Args.Select(a => a.Content).StringJoin()); }
    }
}