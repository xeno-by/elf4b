using System.Collections;
using System.Collections.Generic;
using Esath.Eval.Ver3.Core;

namespace Esath.Eval.Ver3.Snippets
{
    public class CompiledPropertyCollection : IEnumerable<ICompiledProperty>
    {
        private List<ICompiledProperty> _storage = new List<ICompiledProperty>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ICompiledProperty> GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        public void Add(CompiledProperty prop)
        {
            // override base properties with a child one
            _storage.RemoveAll(prev => prev.VPath == prop.VPath);
            _storage.Add(prop);
        }
    }
}