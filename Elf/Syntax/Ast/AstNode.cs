using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime.Tree;
using Elf.Helpers;
using Elf.Syntax.Ast.Defs;
using Elf.Syntax.Ast.Statements;

namespace Elf.Syntax.Ast
{
    public abstract class AstNode
    {
        public AstNodeType NodeType { get; private set; }

        public AstNode Parent { get; private set; }
        public IEnumerable<AstNode> Parents 
        {
            get
            {
                for (var current = this.Parent; current != null; current = current.Parent)
                    yield return current;
            } 
        }

        public int ChildIndex { get; private set; }
        public IEnumerable<AstNode> Children { get; private set; }

        public CommonTree AntlrNode { get; private set; }
        public String Source { get { return AntlrNode.GetSourceCode(); } }
        public AstNode BindToAntlrNode(CommonTree node)
        {
            // horrible, but works for now
            if (AntlrNode == null)
                AntlrNode = node;
            return this;
        }

        protected AstNode(AstNodeType nodeType, IEnumerable<AstNode> children)
        {
            NodeType = nodeType;
            Children = new List<AstNode>(children).AsReadOnly();

            var childIndex = 0;
            foreach (var child in Children)
            {
                child.Parent = this;
                child.ChildIndex = childIndex++;
            }
        }

        protected abstract string GetTPathNode();
        protected abstract string GetTPathSuffix(int childIndex);
        protected abstract string GetContent();

        public string ShortTPath { get { return GenerateTPath(false); } }
        public string FullTPath { get { return GenerateTPath(true); } }
        public string Content { get { return GetContent(); } }

        private string GenerateTPath(bool full)
        {
            var stack = new Stack<String>();
            for (var current = this; current != null; current = current.Parent)
            {
                var suffix = current.Parent == null ? null :
                    current.Parent.GetTPathSuffix(current.ChildIndex);
                suffix = full && !suffix.IsNullOrEmpty() ? suffix + ":" : null;

                var parentHasNullTPathNode = current.Parent == null ? false :
                    current.Parent.GetTPathNode() == null;
                var prefix = parentHasNullTPathNode ? "" : ("/" + suffix);

                stack.Push(prefix + current.GetTPathNode());
            }

            var sb = new StringBuilder();
            while (stack.Count != 0) sb.Append(stack.Pop());
            return sb.ToString();
        }


        public String Indent
        {
            get
            {
                var tabCounter = 0;
                for (var current = this; current != null; current = current.Parent)
                    if (current is Block || current is FuncDef) ++tabCounter;

                return new String(' ', 2 * tabCounter);
            }
        }

        public override string ToString()
        {
            var content = Content.TrimExcessiveIndendation().TrimWrappingNewLines();
            return String.Format("[{0} ->{1}{2}]", FullTPath, Environment.NewLine, content.InjectLineNumbers1());
        }
    }
}
