using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using Esath.Eval.Ver3.Async;

namespace Esath.Eval.Ver3
{
    public class CompilationContext : CumulationAwareContext
    {
        public CompilationContext(VaultCompiler compiler)
            : base(compiler)
        {
        }

        // cba to pollute compiler variable namespace, so not 100% performant
        private static Object _nameRegistryLock = new Object();
        private static HashSet<String> _nameRegistry = new HashSet<String>();

        public String RequestName(String desiredName)
        {
            lock (_nameRegistryLock)
            {
                desiredName = desiredName + "_seq" + WorkerSeq;
                var name = desiredName;

                var i = 0;
                while (_nameRegistry.Contains(name))
                {
                    name = desiredName + "~" + ++i;
                }

                _nameRegistry.Add(name);
                return name;
            }
        }

        public TypeBuilder FactoryType { get; set; }
        public ConstructorBuilder Factory { get; set; }

        private Dictionary<TypeBuilder, MethodBuilder> _ccs = new Dictionary<TypeBuilder, MethodBuilder>();
        public Dictionary<TypeBuilder, MethodBuilder> CCs
        {
            get { return _ccs; }
            set { _ccs = value; }
        }

        private Dictionary<TypeBuilder, MethodBuilder> _cps = new Dictionary<TypeBuilder, MethodBuilder>();
        public Dictionary<TypeBuilder, MethodBuilder> CPs
        {
            get { return _cps; }
            set { _cps = value; }
        }
    }
}