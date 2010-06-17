using System;
using Elf.Helpers;

namespace Elf.Core.Assembler.Literals
{
    public sealed class ElfNumberLiteral : ElfLiteral<Double>
    {
        // todo. is this precision enough?
        public ElfNumberLiteral(String val) 
            : base(val.FromInvariantString<double>())
        {
        }
    }
}