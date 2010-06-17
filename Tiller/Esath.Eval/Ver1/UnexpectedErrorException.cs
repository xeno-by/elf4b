using System;

namespace Esath.Eval.Ver1
{
    public class UnexpectedErrorException : BaseEvalException
    {
        public UnexpectedErrorException(Exception innerException)
            : base(String.Empty, innerException) 
        {
        }
    }
}