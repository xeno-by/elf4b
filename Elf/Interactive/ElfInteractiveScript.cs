using Elf.Core.ClrIntegration;

namespace Elf.Interactive
{
    [Rtimpl("ElfInteractiveScript")]
    [CustomScopeResolver(typeof(ElfInteractiveScopeResolver))]
    public class ElfInteractiveScript
    {
        [Rtimpl]
        public ElfInteractiveScript()
        {
        }
    }
}