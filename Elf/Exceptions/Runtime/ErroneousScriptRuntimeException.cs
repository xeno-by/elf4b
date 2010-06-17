using System;
using Antlr.Runtime.Tree;
using Elf.Core;
using Elf.Core.Assembler;
using Elf.Core.Runtime;
using Elf.Exceptions.Abstract;
using Elf.Helpers;
using Elf.Syntax.Ast;

namespace Elf.Exceptions.Runtime
{
    public class ErroneousScriptRuntimeException : ErroneousScriptException
    {
        public VirtualMachine VM { get; private set; }
        public IElfThread Thread { get; private set; }

        public ErroneousScriptRuntimeException(ElfExceptionType type, VirtualMachine vm)
            : base(type, String.Empty)
        {
            VM = vm;
            Thread = VM.CurrentThread;
        }

        public ErroneousScriptRuntimeException(ElfExceptionType type, VirtualMachine vm, Exception innerException)
            : base(type, String.Empty, innerException)
        {
            VM = vm;
            Thread = VM.CurrentThread;
        }

        public override string Message
        {
            get
            {
                return String.Format("A runtime exception of type '{0}' has occurred. VM dump:{1}{2}{1}",
                    Type, Environment.NewLine, VM.DumpAll());
            }
        }

        private ElfVmInstruction CurrentEvi
        {
            get
            {
                var currentFrame = Thread.RuntimeContext.CallStack.Peek();
                if (Type == ElfExceptionType.UsingVoidValue)
                {
                    return currentFrame.Source.Body[currentFrame.PrevEvi];
                }
                else
                {
                    return currentFrame.Source.Body[currentFrame.CurrentEvi];
                }
            }
        }

        public override string SourceCode
        {
            get { return CurrentEvi.AstNode.AntlrNode.GetSourceCode(); }
        }

        public override Span ErrorSpan
        {
            get { return CurrentEvi.AstNode.AntlrNode.GetOwnSpan(); }
        }

        public override CommonTree AntlrNode
        {
            get { return CurrentEvi.AstNode.AntlrNode; }
        }

        public override AstNode ElfNode
        {
            get { return CurrentEvi.AstNode; }
        }
    }
}