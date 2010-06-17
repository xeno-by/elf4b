using System;

namespace Esath.Eval.Ver3.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RevisionAttribute : Attribute
    {
        public UInt64 Revision { get; private set; }

        public RevisionAttribute(ulong revision)
        {
            Revision = revision;
        }
    }
}