using System.ComponentModel;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;
using Esath.Data.Converters;
using Esath.Data.Core;

namespace Esath.Data
{
    [Rtimpl("EsathString"), ElfSerializable("string")]
    [Loc("ValueType_String", null)]
    [TypeDescriptionProvider(typeof(EsathTypeDescriptionProvider))]
    [TypeConverter(typeof(EsathStringConverter))]
    public class EsathString : ElfString, IEsathObject
    {
        public EsathString(string val) 
            : base(val)
        {
        }

#if ESATH_TYPES_WITH_IMPLICIT_CASTS
        public static implicit operator EsathString(string s)
        {
            return new EsathString(s);
        }

        public static implicit operator string(EsathString s)
        {
            return s.Val;
        }
#endif
    }
}