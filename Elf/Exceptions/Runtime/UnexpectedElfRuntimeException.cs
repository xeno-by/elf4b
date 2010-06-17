using System;
using Elf.Core;
using Elf.Exceptions.Abstract;

namespace Elf.Exceptions.Runtime
{
    public class UnexpectedElfRuntimeException : UnexpectedElfException
    {
        public VirtualMachine VM { get; private set; }

        public UnexpectedElfRuntimeException(VirtualMachine vm, string message)
            : base(message)
        {
            VM = vm;
        }

        public UnexpectedElfRuntimeException(VirtualMachine vm, string message, Exception innerException)
            : base(message, innerException)
        {
            VM = vm;
        }

        public override string Message
        {
            get
            {
                return String.Format("Unexpected runtime error: '{0}'. VM dump:{1}{2}{1}",
                    base.Message, Environment.NewLine, VM.DumpAll());
            }
        }
    }
}