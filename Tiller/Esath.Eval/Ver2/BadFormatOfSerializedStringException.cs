using System;
using DataVault.Core.Api;

namespace Esath.Eval.Ver2
{
    public class BadFormatOfSerializedStringException : BaseEvalException
    {
        public FormatException FormatException { get; private set; }

        public BadFormatOfSerializedStringException(IBranch offendingBranch, Exception innerException)
            : base(offendingBranch, String.Empty, innerException)
        {
            FormatException = (FormatException)innerException.InnerException;
        }
    }
}