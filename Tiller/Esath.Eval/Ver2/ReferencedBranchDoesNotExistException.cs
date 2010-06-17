using System;
using DataVault.Core.Api;

namespace Esath.Eval.Ver2
{
    public class ReferencedBranchDoesNotExistException : BaseEvalException
    {
        public String BranchRef { get; private set; }

        public ReferencedBranchDoesNotExistException(IBranch offendingBranch, string branchRef)
            : base(offendingBranch, String.Empty) 
        {
        }
    }
}