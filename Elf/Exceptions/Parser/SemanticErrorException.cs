using System;
using Antlr.Runtime.Tree;
using Elf.Exceptions.Abstract;
using Elf.Helpers;
using Elf.Syntax.Ast;

namespace Elf.Exceptions.Parser
{
    public class SemanticErrorException : ErroneousScriptException
    {
        public String Input { get; private set; }
        public int LineNumber { get { return Node.Line; } }
        public int CharNumberInLine { get { return Node.CharPositionInLine; } }
        public CommonTree Node { get; private set; }

        public SemanticErrorException(string input, CommonTree node, ElfExceptionType type)
            : base(type)
        {
            Input = input;
            Node = node;
        }

        public override string Message
        {
            get
            {
                var span = Node.Token == null ? -1 : Node.Token.Text.Length;
                var prettyInput = Input.InjectErrorMarker(LineNumber, CharNumberInLine, span).InjectLineNumbers1();

                return String.Format(
                    "Error parsing script:{0}{1}{0}{0}" +
                    "Reason: semantic error '{2}' occurred when parsing '{3}'.",
                    Environment.NewLine, prettyInput, Type, Node.ToStringTree());
            }
        }

        public override string SourceCode
        {
            get { return Input; }
        }

        public override Span ErrorSpan
        {
            get { return Node.GetOwnSpan(); }
        }

        public override CommonTree AntlrNode
        {
            get { return Node; }
        }

        public override AstNode ElfNode
        {
            get { return null; }
        }
    }
}