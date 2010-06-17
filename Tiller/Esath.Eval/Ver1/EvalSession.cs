using System;
using System.Collections.Generic;
using DataVault.Core.Api;
using DataVault.Core.Helpers;

namespace Esath.Eval.Ver1
{
	using DataVault.Core.Helpers.Assertions;

	public class EvalSession : IDisposable
    {
        public IVault Vault { get; private set; }
        public IVault Repository { get; private set; }
        private List<IDisposable> _expositions = new List<IDisposable>();

        public EvalSession(IVault vault)
            : this(vault, null)
        {
        }

        public EvalSession(IVault vault, IVault repository)
        {
            Vault = vault.AssertNotNull();
            _expositions.Add(vault.ExposeReadOnly());

            Repository = repository;
            if (repository != null)
            {
                _expositions.Add(repository.ExposeReadOnly());
            }
        }

        public object Eval(IBranch b)
        {
            return b.Eval(Repository);
        }

        public void Dispose()
        {
            _expositions.ForEach(e => e.Dispose());
        }
    }
}