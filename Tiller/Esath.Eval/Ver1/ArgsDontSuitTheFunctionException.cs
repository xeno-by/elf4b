using System;
using Elf.Core.Runtime.Contexts;

namespace Esath.Eval.Ver1
{
    public class ArgsDontSuitTheFunctionException : BaseEvalException
    {
        public ClrCallContext CallContext { get; private set; }

        public ArgsDontSuitTheFunctionException(ClrCallContext callContext, Exception innerException)
            : base(String.Empty, innerException) 
        {
            CallContext = callContext;
        }
    }
}