using System.Linq;
using Elf.Helpers;

namespace Elf.Cola.Analysis
{
    // todo. no prettyprint for factum so far
    public abstract class Factum
    {
        public Severity Severity { get; private set; }
        public ColaBottle Bottle { get; private set; }

        protected Factum(Severity severity, ColaBottle bottle)
        {
            Severity = severity;
            Bottle = bottle;
        }

        protected Factum(Severity severity, ColaNode bubble)
            : this(severity, new ColaBottle(bubble.Parents.Reverse().Concat(bubble.AsArray()).First()))
        {
        }
    }
}