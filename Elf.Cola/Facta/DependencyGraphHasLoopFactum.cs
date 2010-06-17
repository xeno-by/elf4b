using System.Collections.Generic;
using Elf.Cola.Analysis;

namespace Elf.Cola.Facta
{
    public class DependencyGraphHasLoopFactum : Factum
    {
        public IEnumerable<ColaNode> Loop { get; private set; }

        public DependencyGraphHasLoopFactum(ColaBottle bottle, IEnumerable<ColaNode> loop) 
            : base(Severity.Fatal, bottle) 
        {
            Loop = loop;
        }
    }
}