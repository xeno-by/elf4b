using System;
using DataVault.Core.Api;
using Esath.Data.Core;

namespace Esath.Eval.Ver3.Core
{
    public interface ICompiledProperty
    {
        ICompiledNode Parent { get; }

        String Name { get; }
        VPath VPath { get; }

        IEsathObject Eval();
    }
}