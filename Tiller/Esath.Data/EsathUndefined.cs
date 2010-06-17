using System.ComponentModel;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;
using Esath.Data.Converters;
using Esath.Data.Core;

namespace Esath.Data
{
    [Rtimpl("EsathUndefined"), ElfSerializable("?")]
    [Loc("ValueType_Undefined", null)]
    [TypeDescriptionProvider(typeof(EsathTypeDescriptionProvider))]
    [TypeConverter(typeof(UndefinedConverter))]
    public class EsathUndefined : ElfObjectImpl, IEsathObject
    {
    }
}