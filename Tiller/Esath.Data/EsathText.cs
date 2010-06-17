using System.ComponentModel;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;
using Esath.Data.Converters;
using Esath.Data.Core;

namespace Esath.Data
{
    [Rtimpl("EsathText"), ElfSerializable("text")]
    [Loc("ValueType_Text", null)]
    [TypeDescriptionProvider(typeof(EsathTypeDescriptionProvider))]
    [TypeConverter(typeof(EsathStringConverter))]
    public class EsathText : ElfString, IEsathObject
    {
        public EsathText(string val)
            : base(val)
        {
        }

#if ESATH_TYPES_WITH_IMPLICIT_CASTS
        public static implicit operator EsathText(string s)
        {
            return new EsathText(s);
        }

        public static implicit operator string(EsathText s)
        {
            return s.Val;
        }
#endif
    }
}