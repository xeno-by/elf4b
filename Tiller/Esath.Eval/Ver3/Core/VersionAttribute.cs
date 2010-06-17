using System;

namespace Esath.Eval.Ver3.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        public Version Version { get; private set; }

        public VersionAttribute(String id, ulong revision)
        {
            Version = new Version(new Guid(id), revision);
        }
    }
}