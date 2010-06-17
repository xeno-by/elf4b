using System;
using System.Collections.Generic;
using DataVault.Core.Api;

namespace Esath.Eval.Ver3.Snippets
{
    // todo. it would be great if datavault itself cached searching by vpath
    // by the way it's possible to index stuff during the loading phase
    // tho it requires extreme caution to keep cache consistent through renames and deletes

    public class CachedVault
    {
        private readonly IVault _vault;
        private readonly Dictionary<VPath, IBranch> _branches = new Dictionary<VPath, IBranch>();
        private readonly Dictionary<VPath, IValue> _values = new Dictionary<VPath, IValue>();

        public CachedVault(IVault vault)
        {
            _vault = vault;
        }

        public IVault Vault
        {
            get { return _vault; }
        }

        public IBranch GetBranch(VPath vpath)
        {
            if (!_branches.ContainsKey(vpath))
            {
                _branches.Add(vpath, _vault.GetBranch(vpath));
            }

            return _branches[vpath];
        }

        public IValue GetValue(VPath vpath)
        {
            if (!_values.ContainsKey(vpath))
            {
                _values.Add(vpath, _vault.GetValue(vpath));
            }

            return _values[vpath];
        }

        public void FlushCaches()
        {
            _branches.Clear();
            _values.Clear();
        }
    }
}