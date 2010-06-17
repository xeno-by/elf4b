using DataVault.Core.Api;

namespace Esath.Eval.Ver3
{
    public interface IVaultCompiler
    {
        IVault Vault { get; }

        CompiledScenarioCache GetCompiledSync();
        CompiledScenarioCache GetCompiledAsync();
    }
}