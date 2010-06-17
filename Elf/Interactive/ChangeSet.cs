using System;
using Elf.Helpers;
using System.Linq;

namespace Elf.Interactive
{
    public class ChangeSet
    {
        private Func<PropertyBag> Get { get; set; }
        private Action<PropertyBag> Set { get; set; }
        public PropertyBag BaseLine { get; private set; }
        public PropertyBag NewVersion { get; private set; }
        public PropertyBag Admixture { get; private set; }
        public PropertyBag Changes { get; private set; }
        public PropertyBag Leakage { get; private set; }
        public bool Empty { get { return Admixture.IsNullOrEmpty() && Changes.IsNullOrEmpty() && Leakage.IsNullOrEmpty(); } }

        public ChangeSet(Func<PropertyBag> getter, Action<PropertyBag> setter)
        {
            Get = getter;
            Set = setter;
        }

        public ChangeSet StartRecording()
        {
            if (BaseLine != null)
                throw new NotSupportedException("Cannot call this method twice.");

            BaseLine = Get().AsReadOnly();
            return this;
        }

        public ChangeSet Capture()
        {
            if (Leakage != null || Admixture != null || Changes != null)
                throw new NotSupportedException("Cannot call this method twice.");

            NewVersion = Get().AsReadOnly();
            Admixture = new PropertyBag();
            Changes = new PropertyBag();
            Leakage = new PropertyBag();

            foreach(var key in BaseLine.Keys.Union(NewVersion.Keys))
            {
                if (BaseLine.ContainsKey(key) && NewVersion.ContainsKey(key))
                {
                    var bval = BaseLine[key];
                    var cval = NewVersion[key];

                    if (!Equals(bval, cval))
                    {
                        Changes.Add(key, cval);
                    }
                }
                else if (BaseLine.ContainsKey(key) && !NewVersion.ContainsKey(key))
                {
                    Leakage.Add(key, BaseLine[key]);
                }
                else if (!BaseLine.ContainsKey(key) && NewVersion.ContainsKey(key))
                {
                    Admixture.Add(key, NewVersion[key]);
                }
            }

            Admixture = Admixture.AsReadOnly();
            Changes = Changes.AsReadOnly();
            Leakage = Leakage.AsReadOnly();

            return this;
        }

        public void Accept()
        {
            var updated = new PropertyBag(Get());
            Admixture.ForEach(kvp => updated.Add(kvp.Key, kvp.Value));
            Changes.ForEach(kvp => updated[kvp.Key] = kvp.Value);
            Leakage.ForEach(kvp => updated.Remove(kvp.Key));
            Set(updated);
        }

        public void Reject()
        {
            Set(new PropertyBag(BaseLine));
        }

        public override string ToString()
        {
            return
                Empty ? "<empty>" :
                String.Format("[* = {0}, + = {1}, - = {2}]", Changes, Admixture, Leakage);
        }
    }
}