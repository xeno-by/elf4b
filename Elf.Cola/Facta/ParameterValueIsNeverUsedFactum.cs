using Elf.Cola.Analysis;
using Elf.Cola.Parameters;

namespace Elf.Cola.Facta
{
    public class ParameterValueIsNeverUsedFactum : Factum
    {
        public Parameter Param { get; private set; }

        public ParameterValueIsNeverUsedFactum(Parameter param, ColaBottle bottle)
            : base(Severity.Negligible, bottle)
        {
            Param = param;
        }
    }
}