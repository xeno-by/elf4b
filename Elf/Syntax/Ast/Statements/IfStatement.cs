using System;
using Elf.Syntax.Ast.Expressions;
using System.Linq;

namespace Elf.Syntax.Ast.Statements
{
    public class IfStatement : Statement
    {
        public Expression Test { get { return (Expression)Children.ElementAt(0); } }
        public Block Then { get { return (Block)Children.ElementAt(1); } }
        public Block Else { get { return (Block)Children.ElementAtOrDefault(2); } }

        public IfStatement(Expression test, Block then)
            : base(AstNodeType.IfStatement, new AstNode[]{test, then})
        {
        }

        public IfStatement(Expression test, Block then, Block @else) 
            : base(AstNodeType.IfStatement, new AstNode[]{test, then, @else})
        {
        }

        protected override string GetTPathNode() { return "if"; }
        protected override string GetTPathSuffix(int childIndex)
        {
            if (childIndex == 0) return "?";
            if (childIndex == 1) return "1";
            if (childIndex == 2) return "0";
            throw new NotSupportedException(childIndex.ToString());
        }

        protected override string GetContent()
        {
            String content;
            if (Else == null)
            {
                content = String.Format("{0}if {1} then{3}{2}{0}end", 
                    Indent, Test.Content, Then.Content, Environment.NewLine);
            }
            else
            {
                content = String.Format("{0}if {1} then{4}{2}{0}else{4}{3}{0}end",
                    Indent, Test.Content, Then.Content, Else.Content, Environment.NewLine);
            }

            var pre = ChildIndex == 0 ? null : Environment.NewLine;
            var post = ChildIndex == Parent.Children.Count() - 1 ? null : Environment.NewLine;
            return String.Format("{0}{1}{2}", pre, content, post);
        }
    }
}