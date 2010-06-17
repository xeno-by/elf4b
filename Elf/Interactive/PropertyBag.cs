using System;
using System.Collections.Generic;
using System.Diagnostics;
using Elf.Helpers;
using System.Linq;

namespace Elf.Interactive
{
    [DebuggerDisplay("{ThisToString}")]
    public class PropertyBag : Dictionary<String, Object>
    {
        public PropertyBag() 
        {
        }

        public PropertyBag(IDictionary<String, Object> dictionary) 
            : base(dictionary) 
        {
        }

        public PropertyBag AsReadOnly()
        {
            return new PropertyBag(new ReadOnlyDictionary<String, Object>(this));
        }

        public override string ToString()
        {
            return 
                this.IsNullOrEmpty() ? "<empty>" :
                "[" + this.Select(kvp => kvp.Key + " = " + kvp.Value).StringJoin() + "]";
        }

        private String ThisToString { get { return ToString(); } }
    }
}