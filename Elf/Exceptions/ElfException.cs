using System;

namespace Elf.Exceptions
{
    public abstract class ElfException : ApplicationException
    {
        public ElfExceptionType Type { get; private set; }

        protected ElfException(ElfExceptionType type) 
        {
            Type = type;
        }

        protected ElfException(ElfExceptionType type, string message) 
            : base(message) 
        {
            Type = type;
        }

        protected ElfException(ElfExceptionType type, string message, Exception innerException)
            : base(message, innerException) 
        {
            Type = type;
        }
    }
}