using System;
using System.Collections.Generic;
using Elf.Core.TypeSystem;
using System.Linq;
using Elf.Helpers;

namespace Elf.Core.Runtime.Contexts
{
    public class Scope : Dictionary<String, IElfObject>
    {
        public String Dump()
        {
            return Count == 0 ? "<empty>" : this.Select(kvp => kvp.Key + " = " + kvp.Value).StringJoin();
        }
    }
}