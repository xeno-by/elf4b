using System;
using Elf.Helpers;
using System.Linq;

namespace Elf.Cola.Parameters
{
    public class ChangeSet
    {
        private Func<ParametersValues> Get { get; set; }
        private Action<ParametersValues> Set { get; set; }
        public ParametersValues BaseLine { get; private set; }
        public ParametersValues NewVersion { get; private set; }
        public ParametersValues Admixture { get; private set; }
        public ParametersValues Changes { get; private set; }
        public ParametersValues Leakage { get; private set; }
        public bool Empty { get { return Admixture.IsNullOrEmpty() && Changes.IsNullOrEmpty() && Leakage.IsNullOrEmpty(); } }

        public ChangeSet(Func<ParametersValues> getter, Action<ParametersValues> setter)
        {
            Get = getter;
            Set = setter;
        }

        public ChangeSet StartRecording()
        {
            if (BaseLine != null)
                throw new NotSupportedException("Cannot call this method twice.");

            BaseLine = new ParametersValues(Get().AsReadOnly());
            return this;
        }

        public ChangeSet Capture()
        {
            if (Leakage != null || Admixture != null || Changes != null)
                throw new NotSupportedException("Cannot call this method twice.");

            NewVersion = new ParametersValues(Get().AsReadOnly());
            CaptureImpl();

            return this;
        }

        private void CaptureImpl()
        {
            if (Leakage != null || Admixture != null || Changes != null)
                throw new NotSupportedException("Cannot call this method twice.");

            Admixture = new ParametersValues();
            Changes = new ParametersValues();
            Leakage = new ParametersValues();

            foreach (var key in BaseLine.Keys.Union(NewVersion.Keys))
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

            Admixture = new ParametersValues(Admixture.AsReadOnly());
            Changes = new ParametersValues(Changes.AsReadOnly());
            Leakage = new ParametersValues(Leakage.AsReadOnly());
        }

        public ChangeSet Accept()
        {
            var updated = new ParametersValues(Get());
            Admixture.ForEach(kvp => updated.Add(kvp.Key, kvp.Value));
            Changes.ForEach(kvp => updated[kvp.Key] = kvp.Value);
            Leakage.ForEach(kvp => updated.Remove(kvp.Key));
            Set(updated);
            return this;
        }

        public ChangeSet Reject()
        {
            Set(new ParametersValues(BaseLine));
            return this;
        }

        public ChangeSet Merge(ChangeSet nextVersion)
        {
            if (this.NewVersion == nextVersion.BaseLine)
            {
                var merged = new ChangeSet(Get, Set);
                merged.BaseLine = this.BaseLine;
                merged.NewVersion = nextVersion.NewVersion;
                merged.CaptureImpl();
                return merged;
            }
            else
            {
                throw new NotSupportedException(String.Format(
                    "Cannot merge '{0}' with '{1}': incompatible revisions."));
            }
        }

        public override string ToString()
        {
            return
                Empty ? "<empty>" :
                String.Format("[* = {0}, + = {1}, - = {2}, base = {3}]", Changes, Admixture, Leakage, BaseLine);
        }
    }
}