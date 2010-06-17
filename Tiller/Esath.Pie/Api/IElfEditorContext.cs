using System;
using System.Collections.Generic;
using DataVault.Core.Api;
using Esath.Pie.AstRendering;

namespace Esath.Pie.Api
{
    public interface IElfEditorContext : IRendererContext
    {
        IEnumerable<VarItem> Vars { get; }
    }

    public class VarItem
    {
        public IBranch Branch { get; private set; }
        public String PrettyText { get; private set; }
        public String InternalName { get; private set; }

        public VarItem(IBranch branch, string prettyText, string internalName)
        {
            Branch = branch;
            PrettyText = prettyText;
            InternalName = internalName;
        }

        public override string ToString()
        {
            return PrettyText;
        }
    }
}