using System;

namespace Elf.Cola.Exceptions
{
    // todo. no prettyprint here so far
    public class UnexpectedCocacolaException : CocacolaException
    {
        public UnexpectedCocacolaException(string message, Exception innerException)
            : base(CocacolaExceptionType.Unexpected, message, innerException)
        {
        }
    }
}