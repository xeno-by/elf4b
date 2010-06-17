using System;

namespace Esath.Eval.Ver1
{
    public class ReferencedBranchDoesNotExistException : BaseEvalException
    {
        public String BranchRef { get; private set; }

        public ReferencedBranchDoesNotExistException(string branchRef)
            : base(String.Empty) 
        {
        }
    }
}