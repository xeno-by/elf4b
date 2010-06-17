using System.Collections.Generic;
using DataVault.Core.Api;

namespace Esath.Eval.Ver2
{
    public class EvalStackOverflowException : BaseEvalException
    {
        public List<IBranch> Loop { get; private set; }

        public EvalStackOverflowException(IBranch offendingBranch, IEnumerable<IBranch> loop)
            : base(offendingBranch) 
        {
            Loop = new List<IBranch>(loop);
        }
    }
}