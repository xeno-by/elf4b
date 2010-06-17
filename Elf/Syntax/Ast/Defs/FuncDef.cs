using System;
using System.Collections.Generic;
using System.Linq;
using Antlr.Runtime.Tree;
using Elf.Helpers;
using Elf.Syntax.Ast.Statements;

namespace Elf.Syntax.Ast.Defs
{
    public class FuncDef : AstNode
    {
        public Block Body { get { return (Block)Children.Single(); } }
        public String Name { get; private set; }
        public IEnumerable<String> Args { get; private set; }

        public FuncDef(String name, IEnumerable<String> args, Block body) 
            : base(AstNodeType.FuncDef, body.AsArray()) 
        {
            Name = name;
            Args = args;
        }

        protected override string GetTPathNode() { return "f:" + Name; }
        protected override string GetTPathSuffix(int childIndex) { return null; }

        protected override string GetContent()
        {
            return String.Format("{0}def {1}({2}){4}{3}{0}end{4}", 
                Indent, Name, Args.StringJoin(), Body.Content, Environment.NewLine);
        }

        public String SourceMethod
        {
            get
            {
                var methodSource = Source.Substring(GetEntireSpanOfAntlrNode());
                if (methodSource != null)
                {
                    var head = methodSource.SelectLines()[0];
                    var body = methodSource.SelectLines().Skip(1).StringJoin(Environment.NewLine);
                    return head + Environment.NewLine + body.TrimExcessiveIndendation();
                }
                else
                {
                    return null;
                }
            }
        }

        private Span GetEntireSpanOfAntlrNode()
        {
            var source = AntlrNode.GetSourceCode();
            if (source != null)
            {
                var flat = AntlrNode.Flatten(node2 => node2.Children.Cast<CommonTree>());
                var abses = flat.Where(node2 => node2.Token != null)
                    .ToDictionary(node2 => node2, node2 => source.GetAbsoluteIndex(node2.Line, node2.CharPositionInLine))
                    .Where(abs => abs.Value != -1);

                var span = Span.FromBounds(
                    abses.Min(kvp => kvp.Value),
                    abses.Max(kvp => kvp.Value + kvp.Key.Token.Text.Length));

                var pre = source.Substring(0, span.Start);
                var post = source.Substring(span.End);

                var expectedEnds = 0;
                AstNode current = this;
                while(current != null)
                {
                    if (current is FuncDef || current is IfStatement)
                    {
                        current = current.Children.Count() == 0 ? null : current.Children.Last();
                        while (current is Block && current.Children.Count() != 0) current = current.Children.Last();
                        ++expectedEnds;
                    }
                    else
                    {
                        break;
                    }
                }

                var startOfDef = pre.Substring(pre.LastIndexOf("def"));
                var endOfEnd = post.Substring(0, post.NthIndexOf("end", expectedEnds) + 3);
                return Span.FromBounds(span.Start - startOfDef.Length, span.End + endOfEnd.Length);
            }
            else
            {
                return null;
            }
        }
    }
}