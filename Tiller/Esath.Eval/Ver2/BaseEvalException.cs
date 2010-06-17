using System;
using DataVault.Core.Api;

namespace Esath.Eval.Ver2
{
    public abstract class BaseEvalException : Exception
    {
        public IBranch OffendingBranch { get; private set; }

        protected BaseEvalException(IBranch offendingBranch) 
        {
            OffendingBranch = offendingBranch;
        }

        protected BaseEvalException(IBranch offendingBranch, string message)
            : base(message) 
        {
            OffendingBranch = offendingBranch;
        }

        protected BaseEvalException(IBranch offendingBranch, string message, Exception innerException)
            : base(message, innerException) 
        {
            OffendingBranch = offendingBranch;
        }
    }
}