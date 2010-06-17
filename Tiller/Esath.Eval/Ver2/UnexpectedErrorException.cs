using System;
using DataVault.Core.Api;

namespace Esath.Eval.Ver2
{
    public class UnexpectedErrorException : BaseEvalException
    {
        public UnexpectedErrorException(IBranch offendingBranch, Exception innerException)
            : base(offendingBranch, String.Empty, innerException) 
        {
        }
    }
}