using System;
using System.Globalization;

namespace Esath.Pie.AstRendering
{
    public interface IRendererContext
    {
        CultureInfo Locale { get; }
        String GetDisplayName(String internalName);
    }
}