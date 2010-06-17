using System;
using System.Collections.Generic;
using DataVault.Core.Api;
using Esath.Data.Core;

namespace Esath.Eval.Ver3.Core
{
    public interface ICompiledNode
    {
        String Name { get; }
        VPath VPath { get; }

        ICompiledScenario Root { get; }
        ICompiledNode Parent { get; }
        IEnumerable<ICompiledNode> Children { get; }
        IEnumerable<ICompiledProperty> Properties { get; }

        ICompiledNode Child(VPath vpath);
        ICompiledProperty Property(VPath vpath);
        IEsathObject Eval(VPath vpath);
    }
}