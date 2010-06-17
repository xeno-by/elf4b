using System;
using Antlr.Runtime.Tree;
using Elf.Exceptions.Abstract;
using Elf.Helpers;

namespace Elf.Exceptions.Parser
{
    public class UnexpectedParserException : UnexpectedElfException
    {
        public String Input { get; private set; }
        public CommonTree Node { get; private set; }

        public UnexpectedParserException(string input, CommonTree node, Exception innerException) 
            : base(String.Empty, innerException) 
        {
            Input = input;
            Node = node;
        }

        public override string Message
        {
            get
            {
                return String.Format(
                    "Error parsing script:{0}{1}{0}{0}" +
                    "Reason: unexpected exception '{2}' occurred when parsing '{3}'. ",
                    Environment.NewLine, Input.InjectLineNumbers1(), InnerException, Node.ToStringTree());
            }
        }
    }
}