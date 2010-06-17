using System;

namespace Elf.Cola.Exceptions
{
    public abstract class CocacolaException : ApplicationException
    {
        public CocacolaExceptionType Type { get; private set; }

        protected CocacolaException(CocacolaExceptionType type)
        {
            Type = type;
        }

        protected CocacolaException(CocacolaExceptionType type, string message)
            : base(message)
        {
            Type = type;
        }

        protected CocacolaException(CocacolaExceptionType type, string message, Exception innerException)
            : base(message, innerException)
        {
            Type = type;
        }
    }
}