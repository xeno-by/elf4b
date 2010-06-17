using System;

namespace Esath.Eval.Ver3.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SeqAttribute : Attribute
    {
        public int Seq { get; private set; }

        public SeqAttribute(int seq)
        {
            Seq = seq;
        }
    }
}