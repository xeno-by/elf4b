using System;
using System.Collections.Generic;
using System.Linq;
using Elf.Helpers;

namespace Elf.Syntax.Ast.Statements
{
    public class Block : AstNode
    {
        public IEnumerable<Statement> Statements { get { return Children.Cast<Statement>(); } }

        public Block(IEnumerable<Statement> statements) 
            : base(AstNodeType.Block, statements.Cast<AstNode>()) 
        {
        }

        protected override string GetTPathNode() { return "b"; }
        protected override string GetTPathSuffix(int childIndex) { return childIndex.ToString(); }
        protected override string GetContent() { return Statements.Select(s => s.Content).StringJoin(Environment.NewLine) + Environment.NewLine; }
    }
}