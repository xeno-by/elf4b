using System;

namespace Esath.Eval.Ver3.Core
{
    public class Version
    {
        public Guid Id { get; private set; }
        public ulong Revision { get; private set; }

        public Version(Guid id, ulong revision)
        {
            Id = id;
            Revision = revision;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Version;
            return other != null && Id == other.Id && Revision == other.Revision;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * 3 ^ (int)Revision * 29;
        }

        public override string ToString()
        {
            return String.Format("'{0}' rev {1}", Id, Revision);
        }

        public static bool operator ==(Version ver1, Version ver2)
        {
            return Object.Equals(ver1, ver2);
        }

        public static bool operator !=(Version ver1, Version ver2)
        {
            return !(ver1 == ver2);
        }
    }
}