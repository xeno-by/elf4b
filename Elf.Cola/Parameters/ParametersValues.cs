using System;
using System.Collections.Generic;
using Elf.Cola.Parameters;

namespace Elf.Cola.Parameters
{
    public class ParametersValues : TaggedParameterSet<Object>
    {
        public ParametersValues() 
        {
        }

        public ParametersValues(IDictionary<Parameter, object> dictionary) 
            : base(dictionary) 
        {
        }
    }
}