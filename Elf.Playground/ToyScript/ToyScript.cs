using System;
using System.Text;
using Elf.Core.TypeSystem;
using Elf.Core.ClrIntegration;

namespace Elf.Playground.ToyScript
{
    [Rtimpl("ToyScript")]
    [CustomScopeResolver(typeof(ToyScriptScopeResolver))]
    public class ToyScript : ElfObjectImpl
    {
        [Rtimpl]
        public ToyScript()
        {
        }

        public double? this[String url]
        {
            get
            {
                if (url.Contains("."))
                {
                    ((StringBuilder)VM.Context["ToyLog"]).AppendLine(String.Format(
                        "Acquire({0}) called", url));

                    var num = url.Substring(url.LastIndexOf('.') + 1);
                    return int.Parse(num);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ((StringBuilder)VM.Context["ToyLog"]).AppendLine(String.Format(
                    "Store({0}, {1}) called", url, value));
            }
        }
    }
}
