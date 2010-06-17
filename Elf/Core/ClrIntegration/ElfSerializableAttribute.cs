using System;

namespace Elf.Core.ClrIntegration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ElfSerializableAttribute : Attribute
    {
        public String TypeToken { get; private set; }

        public ElfSerializableAttribute(String typeToken)
        {
            TypeToken = typeToken;
        }
    }
}