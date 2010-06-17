using System;
using System.Runtime.Serialization;

namespace Esath.Eval.Ver1
{
    public abstract class BaseEvalException : Exception
    {
        protected BaseEvalException() {
        }

        protected BaseEvalException(string message)
            : base(message) 
        {
        }

        protected BaseEvalException(string message, Exception innerException)
            : base(message, innerException) 
        {
        }

        protected BaseEvalException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        {
        }
    }
}