using DataVault.Core.Api;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;

namespace Esath.Data
{
    [Rtimpl("ScenarioNode")]
    public class ScenarioNode : ElfObjectImpl
    {
        public IBranch Val { get; private set; }

        public ScenarioNode(IBranch val) 
        {
            Val = val;
        }

        // no UDT, since casts from/to interfaces are prohibited
    }
}