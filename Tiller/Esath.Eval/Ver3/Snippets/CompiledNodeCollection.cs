using System.Collections;
using System.Collections.Generic;
using Esath.Eval.Ver3.Core;

namespace Esath.Eval.Ver3.Snippets
{
    public class CompiledNodeCollection : IEnumerable<ICompiledNode>
    {
        private List<ICompiledNode> _storage = new List<ICompiledNode>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ICompiledNode> GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        public void Add(CompiledNode node)
        {
            // so that null-results of node factory don't interfere
            if (node != null)
            {
                _storage.Add(node);
            }
        }
    }
}