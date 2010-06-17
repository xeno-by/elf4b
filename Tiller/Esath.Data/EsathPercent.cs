using System;
using System.ComponentModel;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Esath.Data.Converters;
using Esath.Data.Core;
using Elf.Helpers;

namespace Esath.Data
{
    [Rtimpl("EsathPercent"), ElfSerializable("percent")]
    [Loc("ValueType_Percent", "Conversion_BadFormatOfPercentValue")]
    [TypeDescriptionProvider(typeof(EsathTypeDescriptionProvider))]
    [TypeConverter(typeof(EsathPercentConverter))]
    public class EsathPercent : ElfNumber, IEsathObject
    {
        public EsathPercent(Double val)
            : base(val / 100)
        {
        }

#if ESATH_TYPES_WITH_IMPLICIT_CASTS
        public static implicit operator EsathPercent(double d)
        {
            return new EsathPercent(d);
        }

        public static implicit operator EsathPercent(int i)
        {
            return new EsathPercent(i);
        }

        public static implicit operator double(EsathPercent d)
        {
            return d.Val;
        }
#endif

        public override ElfNumber Add(ElfNumber n)
        {
            if (n is EsathPercent)
            {
                return new EsathPercent(base.Add(n) * 100);
            }

            return n.Add(this);
        }

        public override ElfNumber Subtract(ElfNumber n)
        {
            if (n is EsathPercent)
            {
                return new EsathPercent(base.Subtract(n) * 100);
            }

            throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, VM);
        }

        public override ElfNumber Multiply(ElfNumber n)
        {
            if (n is EsathPercent)
            {
                return new EsathPercent(((1 + Val) * (1 + n.Val) - 1) * 100);
            }

            return n.Multiply(this);
        }

        public override ElfNumber Divide(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, VM);
            }

            if (n is EsathPercent)
            {
                return new EsathPercent(((1 + Val) / (1 + n.Val) - 1) * 100);
            }

            return new EsathPercent(base.Divide(n) * 100);
        }

        public override ElfNumber Power(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, VM);
            }

            return new EsathPercent(base.Power(n) * 100);
        }

        public override string ToString()
        {
            return "<" + (Val * 100).ToInvariantString() + "%>";
        }
    }
}