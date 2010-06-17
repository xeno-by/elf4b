using System;
using System.ComponentModel;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Esath.Data.Converters;
using Esath.Data.Core;

namespace Esath.Data
{
    [Rtimpl("EsathNumber"), ElfSerializable("number")]
    [Loc("ValueType_Numeric", "Conversion_BadFormatOfNumericValue")]
    [TypeDescriptionProvider(typeof(EsathTypeDescriptionProvider))]
    [TypeConverter(typeof(EsathNumberRelatedConverter))]
    public class EsathNumber : ElfNumber, IEsathObject
    {
        public EsathNumber(Double val)
            : base(val)
        {
        }

#if ESATH_TYPES_WITH_IMPLICIT_CASTS
        public static implicit operator EsathNumber(double d)
        {
            return new EsathNumber(d);
        }

        public static implicit operator EsathNumber(int i)
        {
            return new EsathNumber(i);
        }

        public static implicit operator double(EsathNumber d)
        {
            return d.Val;
        }
#endif

        public override ElfNumber Add(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                return new EsathCurrency(base.Add(n));
            }
            else if (n is EsathPercent)
            {
                return new EsathNumber(Val * (1 + n.Val));
            }

            return new EsathNumber(base.Add(n));
        }

        public override ElfNumber Subtract(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                return new EsathCurrency(base.Subtract(n));
            }
            else if (n is EsathPercent)
            {
                return new EsathNumber(Val * (1 - n.Val));
            }

            return new EsathNumber(base.Subtract(n));
        }

        public override ElfNumber Multiply(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                return new EsathCurrency(base.Multiply(n));
            }

            return new EsathNumber(base.Multiply(n));
        }

        public override ElfNumber Divide(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                return new EsathCurrency(base.Divide(n));
            }

            return new EsathNumber(base.Divide(n));
        }

        public override ElfNumber Power(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, VM);
            }

            return new EsathNumber(base.Power(n));
        }
    }
}