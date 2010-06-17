using System;
using System.ComponentModel;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Elf.Helpers;
using Esath.Data.Converters;
using Esath.Data.Core;

namespace Esath.Data
{
    [Rtimpl("EsathCurrency"), ElfSerializable("currency")]
    [Loc("ValueType_Currency", "Conversion_BadFormatOfCurrencyValue")]
    [TypeDescriptionProvider(typeof(EsathTypeDescriptionProvider))]
    [TypeConverter(typeof(EsathNumberRelatedConverter))]
    public class EsathCurrency : ElfNumber, IEsathObject
    {
        // todo. implement as Decimal, not double
        public EsathCurrency(Double val)
            : base(val)
        {
        }

#if ESATH_TYPES_WITH_IMPLICIT_CASTS
        public static implicit operator EsathCurrency(double d)
        {
            return new EsathCurrency(d);
        }

        public static implicit operator EsathCurrency(int i)
        {
            return new EsathCurrency(i);
        }

        public static implicit operator double(EsathCurrency d)
        {
            return d.Val;
        }
#endif

        public override ElfNumber Add(ElfNumber n)
        {
            if (n is EsathPercent)
            {
                return new EsathCurrency(Val * (1 + n.Val));
            }

            return new EsathCurrency(base.Add(n));
        }

        public override ElfNumber Subtract(ElfNumber n)
        {
            if (n is EsathPercent)
            {
                return new EsathCurrency(Val * (1 - n.Val));
            }

            return new EsathCurrency(base.Subtract(n));
        }

        public override ElfNumber Multiply(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, VM);
            }

            return new EsathCurrency(base.Multiply(n));
        }

        public override ElfNumber Divide(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, VM);
            }

            return new EsathCurrency(base.Divide(n));
        }

        public override ElfNumber Power(ElfNumber n)
        {
            if (n is EsathCurrency)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, VM);
            }

            return new EsathCurrency(base.Power(n));
        }

        public override string ToString()
        {
            return "<" + Val.ToInvariantString() + "$>";
        }
    }
}