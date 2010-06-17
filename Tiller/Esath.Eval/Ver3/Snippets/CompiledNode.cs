using System;
using System.Linq;
using System.Collections.Generic;
using DataVault.Core.Api;
using DataVault.Core.Helpers;
using Esath.Data;
using Esath.Data.Core;
using Esath.Eval.Ver3.Core;

namespace Esath.Eval.Ver3.Snippets
{
	using DataVault.Core.Helpers.Assertions;

	public abstract class CompiledNode : ICompiledNode
    {
        public abstract String Name { get; }
        public abstract VPath VPath { get; }

        // I've removed those from the interface because:
        // * Revision might be inaccurate for cumulative updates (since compilation doesn't lock the vault)
        // * Seq is merely an implementation detail

        public abstract UInt64 Revision { get; }
        public abstract int Seq { get; }

        public virtual ICompiledScenario Root { get; private set; }
        public virtual ICompiledNode Parent { get; private set; }
        private CompiledNodeCollection _children = new CompiledNodeCollection();
        public virtual IEnumerable<ICompiledNode> Children { get { return _children; } }
        private Dictionary<VPath, ICompiledNode> _allChildrenRecursive = new Dictionary<VPath, ICompiledNode>();
        private CompiledPropertyCollection _properties = new CompiledPropertyCollection();
        public virtual IEnumerable<ICompiledProperty> Properties { get { return _properties; } }
        private Dictionary<VPath, ICompiledProperty> _allPropertiesRecursive = new Dictionary<VPath, ICompiledProperty>();

        protected CompiledNode()
        {
            (this is ICompiledScenario).AssertTrue();
            Root = (ICompiledScenario)this;
            Parent = null;
            CreateChildrenAndProperties();
        }

        protected CompiledNode(CompiledNode parent)
        {
            parent.AssertNotNull();
            Root = parent.Root;
            Parent = parent;
            CreateChildrenAndProperties();
        }

        protected virtual CachedVault Scenario
        {
            get { return ((CompiledNode)Parent).Scenario; }
            set { ((CompiledNode)Parent).Scenario = value; }
        }

        protected virtual CachedVault Repository
        {
            get { return ((CompiledNode)Parent).Repository; }
            set { ((CompiledNode)Parent).Repository = value; }
        }

        protected abstract void CreateChildren();
        protected abstract void CreateProperties();

        private void CreateChildrenAndProperties()
        {
            CreateChildren();
            _allChildrenRecursive.AddRange(_children.ToDictionary(c => c.VPath, c => c));
            _children.Cast<CompiledNode>().ForEach(n1 => _allChildrenRecursive.AddRange(n1._allChildrenRecursive));

            CreateProperties();
            _allPropertiesRecursive.AddRange(_properties.ToDictionary(p => p.VPath, p => p));
            _children.Cast<CompiledNode>().ForEach(n1 => _allPropertiesRecursive.AddRange(n1._allPropertiesRecursive));
        }

        public ICompiledNode Child(VPath vpath)
        {
            if (!_allChildrenRecursive.ContainsKey(vpath))
            {
                throw new NotImplementedException(String.Format("There's no compiled node at VPath '{0}'.", vpath));
            }

            return _allChildrenRecursive[vpath];
        }

        public ICompiledProperty Property(VPath vpath)
        {
            if (!_allPropertiesRecursive.ContainsKey(vpath))
            {
                throw new NotImplementedException(String.Format("There's no compiled property at VPath '{0}'.", vpath));
            }

            return _allPropertiesRecursive[vpath];
        }

        public IEsathObject Eval(VPath vpath)
        {
            return Property(vpath).Eval();
        }
    }

    public static class ConvertHelper
    {
        public static EsathNumber AsEsathNumber(String s)
        {
            return null;
        }
    }
}