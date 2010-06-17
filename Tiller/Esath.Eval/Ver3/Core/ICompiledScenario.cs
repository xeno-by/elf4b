using System;
using DataVault.Core.Api;

namespace Esath.Eval.Ver3.Core
{
    public interface ICompiledScenario : ICompiledNode
    {
        Version Version { get; }

        IVault Scenario { get; set; }
        IVault Repository { get; set; }

        bool IsExposed { get; }
        IDisposable Expose();
    }
}