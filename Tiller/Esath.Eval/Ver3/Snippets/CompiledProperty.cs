using System;
using DataVault.Core.Api;
using Esath.Data.Core;
using Esath.Eval.Ver3.Core;

namespace Esath.Eval.Ver3.Snippets
{
    public class CompiledProperty : ICompiledProperty
    {
        public ICompiledNode Parent { get; private set; }
        public String Name { get; private set; }
        public VPath VPath { get; private set; }
        private Func<IEsathObject> _eval { get; set; }

        public CompiledProperty(CompiledNode parent, String name, VPath vpath, Func<IEsathObject> eval)
        {
            Parent = parent;
            Name = name;
            VPath = vpath;
            _eval = eval;
        }

        public IEsathObject Eval()
        {
            return _eval();
        }
    }
}