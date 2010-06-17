using System.ComponentModel;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;
using Esath.Data.Converters;
using Esath.Data.Core;

namespace Esath.Data
{
    [Rtimpl("EsathBoolean"), ElfSerializable("boolean")]
    [Loc("ValueType_Boolean", "Conversion_BadFormatOfBooleanValue")]
    [TypeDescriptionProvider(typeof(EsathTypeDescriptionProvider))]
    [TypeConverter(typeof(EsathBooleanConverter))]
    public class EsathBoolean : ElfBoolean, IEsathObject
    {
        public EsathBoolean(bool val)
            : base(val)
        {
        }

#if ESATH_TYPES_WITH_IMPLICIT_CASTS
        public static implicit operator EsathBoolean(bool b)
        {
            return new EsathBoolean(b);
        }

        public static implicit operator bool(EsathBoolean b)
        {
            return b.Val;
        }
#endif
    }
}