using System;

namespace Elf.Interactive
{
    public class EvalResult
    {
        public Object Retval { get; private set; }
        public ChangeSet SideEffects { get; private set; }

        public EvalResult(object retval, ChangeSet sideEffects)
        {
            Retval = retval;
            SideEffects = sideEffects;
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}]", Retval, SideEffects);
        }
    }
}