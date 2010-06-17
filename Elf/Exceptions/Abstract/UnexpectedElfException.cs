using System;

namespace Elf.Exceptions.Abstract
{
    public abstract class UnexpectedElfException : ElfException
    {
        protected UnexpectedElfException()
            : base(ElfExceptionType.UnexpectedElf)
        {
        }

        protected UnexpectedElfException(string message)
            : base(ElfExceptionType.UnexpectedElf, message) 
        {
        }

        protected UnexpectedElfException(string message, Exception innerException)
            : base(ElfExceptionType.UnexpectedElf, message, innerException) 
        {
        }
    }
}