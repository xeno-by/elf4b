using System.Collections.Generic;
using System.Linq;
using Elf.Cola.Analysis;
using Elf.Cola.Parameters;

namespace Elf.Cola.Facta
{
    public class ParameterValueIsMissingFactum : Factum
    {
        public Parameter Param { get; private set; }
        public IEnumerable<ColaNode> Nodes { get; private set; }

        public ParameterValueIsMissingFactum(Parameter param, IEnumerable<ColaNode> nodes)
            : base(Severity.Fatal, nodes.First())
        {
            Param = param;
            Nodes = nodes;
        }
    }
}