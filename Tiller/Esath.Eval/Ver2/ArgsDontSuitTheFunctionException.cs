using System;
using DataVault.Core.Api;
using Elf.Core.Runtime.Contexts;

namespace Esath.Eval.Ver2
{
    public class ArgsDontSuitTheFunctionException : BaseEvalException
    {
        public ClrCallContext CallContext { get; private set; }

        public ArgsDontSuitTheFunctionException(IBranch offendingBranch, ClrCallContext callContext, Exception innerException)
            : base(offendingBranch, String.Empty, innerException) 
        {
            CallContext = callContext;
        }
    }
}