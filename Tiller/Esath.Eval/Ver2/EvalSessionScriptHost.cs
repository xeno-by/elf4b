using Elf.Core.ClrIntegration;

namespace Esath.Eval.Ver2
{
    [Rtimpl("EvalSessionScriptHost")]
    [CustomScopeResolver(typeof(EvalSessionScriptHostScopeResolver))]
    public class EvalSessionScriptHost
    {
        [Rtimpl]
        public EvalSessionScriptHost()
        {
        }
    }
}