using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Esath.Pie.Api;
using System.Linq;

namespace Esath.Pie.Contexts
{
    public class DefaultEditorContext : IElfEditorContext
    {
        public IDictionary<String, String> Vars { get; private set; }

        public DefaultEditorContext(IDictionary<string, string> vars)
        {
            Vars = vars;
        }

        public CultureInfo Locale
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
        }

        public string GetDisplayName(string internalName)
        {
            return Vars.ContainsKey(internalName) ? Vars[internalName] : internalName;
        }

        IEnumerable<VarItem> IElfEditorContext.Vars
        {
            get
            {
                return Vars.Select(kvp => new VarItem(null, kvp.Value, kvp.Key));
            }
        }
    }
}