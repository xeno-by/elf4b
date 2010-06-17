using System;

namespace Elf.Cola.Parameters
{
    public class Parameter
    {
        public Guid Id { get; private set; }
        public String Name { get; private set; }

        public Parameter(string name)
            : this(Guid.NewGuid(), name)
        {
        }

        public Parameter(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Parameter;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Parameter o1, Parameter o2)
        {
            return Equals(o1, o2);
        }

        public static bool operator !=(Parameter o1, Parameter o2)
        {
            return !(o1 == o2);
        }
    }
}