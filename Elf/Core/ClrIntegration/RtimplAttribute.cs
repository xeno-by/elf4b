using System;

namespace Elf.Core.ClrIntegration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method,
        AllowMultiple = false, Inherited = false)]
    public class RtimplAttribute : Attribute
    {
        public String Name { get; private set; }

        public RtimplAttribute() 
        {
        }

        public RtimplAttribute(string name) 
        {
            Name = name;
        }
    }
}
