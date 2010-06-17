using System;

namespace Esath.Eval.Ver1
{
    public class BadFormatOfSerializedStringException : BaseEvalException
    {
        public FormatException FormatException { get; private set; }

        public BadFormatOfSerializedStringException(Exception innerException)
            : base(String.Empty, innerException)
        {
            FormatException = (FormatException)innerException.InnerException;
        }
    }
}