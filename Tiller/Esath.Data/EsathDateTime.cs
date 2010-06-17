using System;
using System.ComponentModel;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;
using Elf.Helpers;
using Esath.Data.Converters;
using Esath.Data.Core;

namespace Esath.Data
{
    [Rtimpl("EsathDateTime"), ElfSerializable("datetime")]
    [Loc("ValueType_DateTime", "Conversion_BadFormatOfDateTimeValue")]
    [TypeDescriptionProvider(typeof(EsathTypeDescriptionProvider))]
    [TypeConverter(typeof(EsathDateTimeConverter))]
    public class EsathDateTime : ElfObjectImpl, IEsathObject
    {
        public DateTime? Val { get; private set; }

        public EsathDateTime(DateTime? val)
        {
            Val = val;
        }

#if ESATH_TYPES_WITH_IMPLICIT_CASTS
        public static implicit operator EsathDateTime(DateTime? d)
        {
            return new EsathDateTime(d);
        }

        public static implicit operator DateTime?(EsathDateTime d)
        {
            return d.Val;
        }
#endif

        public override string ToString()
        {
            return "<" + Val.ToInvariantString() + ">";
        }

        public override bool Equals(object obj)
        {
            var other = obj as EsathDateTime;
            return other != null && Val == other.Val;
        }

        public override int GetHashCode()
        {
            return Val == null ? 0 : Val.GetHashCode();
        }
    }
}