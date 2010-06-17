using System;
using Elf.Exceptions.Abstract;
using Elf.Syntax.Ast;
using Elf.Syntax.Ast.Defs;

namespace Elf.Exceptions.Compiler
{
    public class UnexpectedCompilerException : UnexpectedElfException
    {
        public FuncDef Func { get; private set; }
        public AstNode Node { get; private set; }

        public UnexpectedCompilerException(FuncDef func, AstNode node, Exception innerException) 
            : base(String.Empty, innerException) 
        {
            Func = func;
            Node = node;
        }

        public override string Message
        {
            get
            {
                return String.Format(
                    "Unexpected error compiling native fun:{0}{1}{0}{0}" + 
                    "Reason: unexpected error '{2}' occurred when processing '{3}'. ",
                    Environment.NewLine, Func, InnerException, Node);
            }
        }
    }
}