using System.Collections.Generic;
using System.Linq;
using Elf.Cola.Analysis;
using Elf.Cola.Parameters;

namespace Elf.Cola.Facta
{
    public class ParameterIsMutatedSeveralTimesFactum : Factum
    {
        public Parameter Param { get; private set; }
        public IEnumerable<ColaNode> Nodes { get; private set; }

        public ParameterIsMutatedSeveralTimesFactum(Parameter param, IEnumerable<ColaNode> nodes) 
            : base(Severity.Negligible, nodes.First())
        {
            Param = param;
            Nodes = nodes;
        }
    }
}