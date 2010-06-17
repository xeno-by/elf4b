using System;
using System.Collections.Generic;
using Elf.Helpers;
using System.Linq;

namespace Elf.Cola.Parameters
{
    public class ParameterUsage
    {
        public Parameter Param { get; private set; }
        public List<ColaNode> In { get; private set; }
        public List<ColaNode> Out { get; private set; }

        public ParameterUsage(Parameter param)
        {
            Param = param;
            In = new List<ColaNode>();
            Out = new List<ColaNode>();
        }

        public int UsedIn(ColaNode n)
        {
            if (In.Contains(n)) return +1;
            if (Out.Contains(n)) return -1;
            return 0;
        }

        public override string ToString()
        {
            return String.Format("[{0}, + = [{1}], - = [{2}]]", Param, 
                In.Select(@in => @in.TPath).StringJoin(),
                Out.Select(@out => @out.TPath).StringJoin());
        }
    }
}