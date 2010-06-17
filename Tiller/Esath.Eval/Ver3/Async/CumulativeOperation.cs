using DataVault.Core.Api;
using DataVault.Core.Api.Events;

namespace Esath.Eval.Ver3.Async
{
    public class CumulativeOperation
    {
        public IBranch Host { get; private set; }
        public IElement Subject { get; private set; }
        public EventReason Reason { get; private set; }

        public CumulativeOperation(IBranch host, IElement subject, EventReason reason)
        {
            Host = host;
            Subject = subject;
            Reason = reason;
        }
    }
}