using System;
using Antlr.Runtime.Tree;
using Elf.Helpers;
using Elf.Syntax.Ast;

namespace Elf.Exceptions.Abstract
{
    public abstract class ErroneousScriptException : ElfException
    {
        protected ErroneousScriptException(ElfExceptionType type)
            : base(type)
        {
        }

        protected ErroneousScriptException(ElfExceptionType type, string message)
            : base(type, message)
        {
        }

        protected ErroneousScriptException(ElfExceptionType type, string message, Exception innerException)
            : base(type, message, innerException)
        {
        }

        public abstract String SourceCode { get; }
        public abstract Span ErrorSpan { get; }
        public abstract CommonTree AntlrNode { get; }
        public abstract AstNode ElfNode { get; }
    }
}