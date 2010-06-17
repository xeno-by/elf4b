using System;
using System.Collections.Generic;
using DataVault.Core.Api;
using Esath.Eval.Ver3.Core;
using System.Linq;

namespace Esath.Eval.Ver3
{
    public class CompiledScenarioCache
    {
        public IVault Vault { get; private set; }
        public ulong Revision { get; private set; }
        private Type ScenarioType { get; set; }

        private Object PoolSyncRoot = new Object();
        private List<ICompiledScenario> _pool = new List<ICompiledScenario>();

        public CompiledScenarioCache(IVault vault, Type t)
        {
            Vault = vault;
            Revision = vault.Revision;
            ScenarioType = t;

            lock (PoolSyncRoot)
            {
                _pool.Add(SpawnNewInstance());
                _pool.Add(SpawnNewInstance());
            }
        }

        private ICompiledScenario SpawnNewInstance()
        {
            return (ICompiledScenario)Activator.CreateInstance(ScenarioType, Vault);
        }

        private ICompiledScenario GetFromPool()
        {
            lock (PoolSyncRoot)
            {
                var avail = _pool.FirstOrDefault(i => !i.IsExposed);
                if (avail == null)
                {
                    avail = SpawnNewInstance();
                    _pool.Add(avail);
                }

                // ugly: we need this to prevent other threads from hijacking this instance
                avail.Expose();

                return avail;
            }
        }

        private ICompiledScenario RequestDesignTime()
        {
            var scenario = GetFromPool();
            scenario.Repository = null;
            return scenario;
        }

        private ICompiledScenario RequestRuntime(IVault repository)
        {
            var scenario = GetFromPool();
            scenario.Repository = repository;
            return scenario;
        }

        public ICompiledScenario RequestInstance()
        {
            return RequestInstance(null);
        }

        public ICompiledScenario RequestInstance(IVault repository)
        {
            return repository == null ? RequestDesignTime() : RequestRuntime(repository);
        }
    }
}