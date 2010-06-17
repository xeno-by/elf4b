using System;
using System.Collections.Generic;
using DataVault.Core.Api;
using DataVault.Core.Helpers;
using System.Linq;

namespace Esath.Eval.Ver3.Snippets
{
    public abstract class NodeFactory
    {
        private Dictionary<VPath, Func<CompiledNode, CompiledNode>> _registry;
        private Dictionary<VPath, Func<CompiledNode, CompiledNode>> Registry
        {
            get
            {
                if (_registry == null)
                {
                    _registry = new Dictionary<VPath, Func<CompiledNode, CompiledNode>>();
                }

                return _registry;
            }
        }

        protected void Register(VPath vpath, Func<CompiledNode, CompiledNode> factory)
        {
            Registry[vpath] = factory;
        }

        protected void Unregister(VPath vpath)
        {
            Registry.Remove(vpath);
        }

        protected void UnregisterRecursively(VPath vpath)
        {
            Registry.RemoveRange(Registry.Keys.Where(path => path > vpath));
        }

        public Func<CompiledNode, CompiledNode> FactoryFor(VPath vpath)
        {
            return Registry.ContainsKey(vpath) ? Registry[vpath] : null;
        }
    }
}