using System;
using System.Linq;

namespace Elf.Syntax.Ast.Statements
{
    public class VarStatement : Statement
    {
        public String Name { get; private set; }

        public VarStatement(String name) 
            : base(AstNodeType.VarStatement, Enumerable.Empty<AstNode>())
        {
            Name = name;
        }

        protected override string GetTPathNode() { return "var:" + Name; }
        protected override string GetTPathSuffix(int childIndex) { return null; }
        protected override string GetContent() { return Indent + String.Format("var {0}", Name); }
    }
}