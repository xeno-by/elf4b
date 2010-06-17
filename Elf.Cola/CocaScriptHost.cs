using Elf.Core.ClrIntegration;

namespace Elf.Cola
{
    [Rtimpl("CocaScriptHost")]
    [CustomScopeResolver(typeof(NameResolver))]
    public class CocaScriptHost
    {
        [Rtimpl]
        public CocaScriptHost()
        {
        }
    }
}