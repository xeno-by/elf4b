using System;
using System.Collections.Generic;
using DataVault.Core.Api;
using Esath.Eval.Ver3.Core;
using DataVault.Core.Helpers;

namespace Esath.Eval.Ver3
{
#if VAULT_EVAL_3

    public class EvalSession : IDisposable
    {
        private ICompiledScenario Scenario { get; set; }
        private Ver2.EvalSession Eval2 { get; set; }
        private List<IDisposable> _expositions = new List<IDisposable>();
        private IDisposable _evalExposition = null;

        public EvalSession(IVaultCompiler compiler)
            : this(compiler, null)
        {
        }

        public EvalSession(IVaultCompiler compiler, IVault repository)
        {
            _expositions.Add(compiler.AssertNotNull().Vault.ExposeReadOnly());
            if (repository != null)
            {
                _expositions.Add(repository.AssertNotNull().ExposeReadOnly());
            }

            var cache = compiler.GetCompiledAsync();
            if (cache != null)
            {
                Scenario = cache.RequestInstance(repository);
                _evalExposition = Scenario.Expose();
            }
            else
            {
                // if cache returned is null, this means that the compiler is busy generating an assembly
                // however, we don't want to make user wait so let's just fall back to 5-10x times slower Eval2
                Eval2 = new Ver2.EvalSession(compiler.Vault, repository);
                _evalExposition = Eval2;
            }
        }

        public object Eval(IBranch b)
        {
            if (Scenario != null)
            {
                var esath = Scenario.AssertNotNull().Eval(b.VPath);
                return esath.GetType().GetProperty("Val").GetValue(esath, null);
            }
            else
            {
                return Eval2.AssertNotNull().Eval(b);
            }
        }

        public void Dispose()
        {
            _evalExposition.Dispose();
            _expositions.ForEach(e => e.Dispose());
        }
    }

#endif
}