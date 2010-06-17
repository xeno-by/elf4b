using System;
using System.Collections.Generic;
using System.Diagnostics;
using Elf.Helpers;
using System.Linq;

namespace Elf.Cola.Parameters
{
    [DebuggerDisplay("{ThisToString}")]
    public class TaggedParameterSet<T> : Dictionary<Parameter, T>
    {
        public TaggedParameterSet() 
        {
        }

        public TaggedParameterSet(IDictionary<Parameter, T> dictionary) 
            : base(dictionary) 
        {
        }

        public T this[String name]
        {
            get { return this.Single(kvp => kvp.Key.Name == name).Value; }
            set { this[this.Single(kvp => kvp.Key.Name == name).Key] = value; }
        }

        public bool ContainsKey(String name)
        {
            return this.Where(kvp => kvp.Key.Name == name).Count() == 1;
        }

        public T this[Guid id]
        {
            get { return this.Single(kvp => kvp.Key.Id == id).Value; }
            set { this[this.Single(kvp => kvp.Key.Id == id).Key] = value; }
        }

        public bool ContainsKey(Guid id)
        {
            return this.Where(kvp => kvp.Key.Id == id).Count() == 1;
        }

        public override string ToString()
        {
            return 
                this.IsNullOrEmpty() ? "<empty>" :
                                                     "[" + this.Select(kvp => kvp.Key + " = " + kvp.Value).StringJoin() + "]";
        }

        private String ThisToString { get { return ToString(); } }

        public override bool Equals(object obj)
        {
            var other = obj as TaggedParameterSet<T>;
            return Keys.OrderBy(k => k.Id).AllMatch(other.Keys.OrderBy(k => k.Id), 
                (my, his) => Equals(this[my], other[his]));
        }

        public override int GetHashCode()
        {
            var num = 0x10cee8ad;
            Keys.OrderBy(k => k.Id).ForEach(p => {
                                                     num = (-1521134295 * num) + p.GetHashCode();
                                                     num = (-1521134295 * num) + EqualityComparer<T>.Default.GetHashCode(this[p]); });

            return num;
        }

        public static bool operator ==(TaggedParameterSet<T> o1, TaggedParameterSet<T> o2)
        {
            return Equals(o1, o2);
        }

        public static bool operator !=(TaggedParameterSet<T> o1, TaggedParameterSet<T> o2)
        {
            return !(o1 == o2);
        }
    }
}