using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Elf.Cola.Analysis
{
    public class Findings : ReadOnlyCollection<Factum>
    {
        public Findings(IEnumerable<Factum> list)
            : base(list.ToArray())
        {
        }

        public bool AreFatal { get { return this.Any(f => f.Severity == Severity.Fatal); } }
    }
}