using System;

namespace Esath.Eval.Ver3.Exceptions
{
    public class ExpectedVaultCompilerException : Exception
    {
        public ExpectedVaultCompilerException()
        {
        }

        public ExpectedVaultCompilerException(string message)
            : base(message)
        {
        }

        public ExpectedVaultCompilerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
