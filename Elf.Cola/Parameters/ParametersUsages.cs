using System.Collections.Generic;
using Elf.Cola.Parameters;

namespace Elf.Cola.Parameters
{
    public class ParametersUsages : TaggedParameterSet<ParameterUsage>
    {
        public ParametersUsages() 
        {
        }

        public ParametersUsages(IDictionary<Parameter, ParameterUsage> dictionary) 
            : base(dictionary) 
        {
        }
    }
}