using System;
using System.Collections.Generic;
using DataVault.Core.Api;
using Esath.Eval.Ver3.Core;
using DataVault.Core.Helpers;
using Version=Esath.Eval.Ver3.Core.Version;

namespace Esath.Eval.Ver3.Snippets
{
	using DataVault.Core.Helpers.Assertions;

	public abstract class CompiledScenario : CompiledNode, ICompiledScenario
    {
        public abstract Version Version { get; }

        // design mode
        protected CompiledScenario(IVault scenario)
            : this(scenario, null)
        {
        }

        // runtime mode
        protected CompiledScenario(IVault scenario, IVault repository)
        {
            ValidateScenario(scenario);
            Scenario = new CachedVault(scenario.AssertNotNull());
            Repository = repository == null ? null : new CachedVault(repository);
        }

        private void ValidateScenario(IVault implScenario)
        {
            if (Version.Id != implScenario.Id)
            {
                throw new ArgumentException(String.Format(
                    "The compiled scenario has id '{0}' which is incompatible " +
                    "with the id '{1}' of a scenario being assigned.", Version.Id, implScenario.Id));
            }

            if (Version.Revision > implScenario.Revision)
            {
                throw new ArgumentException(String.Format(
                    "The compiled scenario has revision '{0}' which is earlier than " +
                    "the revision '{1}' of a scenario being assigned.", Version.Revision, implScenario.Revision));
            }
        }

        protected override CachedVault Scenario { get; set; }
        protected override CachedVault Repository { get; set; }

        IVault ICompiledScenario.Scenario
        {
            get { return Scenario == null ? null : Scenario.Vault; }
            set
            {
                value.AssertNotNull();
                ValidateScenario(value);
                Scenario = new CachedVault(value);
                FlushCaches();
            }
        }

        IVault ICompiledScenario.Repository
        {
            get { return Repository == null ? null : Repository.Vault; }
            set
            {
                Repository = value == null ? null : new CachedVault(value);
                FlushCaches();
            }
        }

        private NodeFactory _factory;

        protected abstract NodeFactory CreateNodeFactory();
        private NodeFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = CreateNodeFactory();
                }

                return _factory;
            }
        }

        public CompiledNode CreateNode(VPath vpath, CompiledNode parent)
        {
            var factoryMethod = Factory.FactoryFor(vpath);
            return factoryMethod == null ? null : factoryMethod(parent);
        }

        public Object ExpositionSyncRoot = new Object();

        private bool _isExposed;
        public bool IsExposed
        {
            get
            {
                lock (ExpositionSyncRoot)
                {
                    return _isExposed;
                }
            }

            set
            {
                lock (ExpositionSyncRoot)
                {
                    _isExposed = value;
                }
            }
        }

        public IDisposable Expose()
        {
            lock (ExpositionSyncRoot)
            {
                return new CompiledScenarioExposition(this);
            }
        }

        // indicates whether certain property of any node in the scenario is cached
        // implemented as a centralized store to provide very fast reinit of the scenario
        public HashSet<VPath> CachedPropertiesRegistry = new HashSet<VPath>();

        public void FlushCaches()
        {
            Scenario.FlushCaches();
            if (Repository != null) Repository.FlushCaches();
            CachedPropertiesRegistry.Clear();
        }
    }
}