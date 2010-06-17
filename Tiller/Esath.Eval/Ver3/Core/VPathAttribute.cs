using System;
using DataVault.Core.Api;

namespace Esath.Eval.Ver3.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class VPathAttribute : Attribute
    {
        public VPath VPath { get; private set; }

        public VPathAttribute(String vpath)
        {
            VPath = vpath;
        }
    }
}