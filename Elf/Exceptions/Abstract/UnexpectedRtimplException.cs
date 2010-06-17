using System;

namespace Elf.Exceptions.Abstract
{
    public abstract class UnexpectedRtimplException : ElfException
    {
        protected UnexpectedRtimplException(string message, Exception innerException)
            : base(ElfExceptionType.UnexpectedElf, message, innerException)
        {
        }
    }
}