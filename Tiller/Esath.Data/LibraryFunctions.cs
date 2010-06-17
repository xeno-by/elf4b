using System;
using Elf.Core.ClrIntegration;
using Elf.Core.TypeSystem;
using System.Linq;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Esath.Data.Core;

namespace Esath.Data
{
    using Util;

    [Rthelper]
    public static class LibraryFunctions
    {
        [Rtimpl("Срзнач")]
        public static ElfNumber Avg(params ElfNumber[] summands)
        {
            if (summands.Length == 0)
            {
                return 0;
            }
            else
            {
                if (summands.Select(s => s.GetType()).Distinct().Count() > 1)
                {
                    throw new ErroneousScriptRuntimeException(
                        ElfExceptionType.OperandsDontSuitMethod, summands[0].VM);
                }

                var avg = summands.Average(s => s.Val);
                return (ElfNumber) Activator.CreateInstance(summands[0].GetType(), avg);
            }
        }

        [Rtimpl("Округл")]
        public static ElfNumber Round(this ElfNumber n)
        {
            var val = n is EsathPercent ? n.Val*100 : n.Val;
            return (ElfNumber) Activator.CreateInstance(n.GetType(), val.Round(0));
        }

        [Rtimpl("Округлвверх")]
        public static ElfNumber RoundUp(this ElfNumber n)
        {
            var val = n is EsathPercent ? n.Val * 100 : n.Val;
            return (ElfNumber)Activator.CreateInstance(n.GetType(), val.RoundUp(0));
        }

        [Rtimpl("Округлвниз")]
        public static ElfNumber RoundDown(this ElfNumber n)
        {
            var val = n is EsathPercent ? n.Val * 100 : n.Val;
            return (ElfNumber)Activator.CreateInstance(n.GetType(), val.RoundDown(0));
        }

        [Rtimpl("Округл2")]
        public static ElfNumber Round(this ElfNumber n, EsathNumber digits)
        {
            var val = n is EsathPercent ? n.Val * 100 : n.Val;
            return (ElfNumber)Activator.CreateInstance(n.GetType(), val.Round((int)digits.Val));
        }

        [Rtimpl("Округлвверх2")]
        public static ElfNumber RoundUp(this ElfNumber n, EsathNumber digits)
        {
            var val = n is EsathPercent ? n.Val * 100 : n.Val;
            return (ElfNumber)Activator.CreateInstance(n.GetType(), val.RoundUp((int)digits.Val));
        }

        [Rtimpl("Округлвниз2")]
        public static ElfNumber RoundDown(this ElfNumber n, EsathNumber digits)
        {
            var val = n is EsathPercent ? n.Val * 100 : n.Val;
            return (ElfNumber)Activator.CreateInstance(n.GetType(), val.RoundDown((int)digits.Val));
        }

        [Rtimpl("Прописью")]
        public static EsathString SpelledOut(this ElfNumber n)
        {
            return new EsathString(NumberSpelledOut.Sum.SpellOut(n.Val, NumberSpelledOut.Currency.Rouble));
        }

        [Rtimpl("ДнейМеждуДатами")]
        public static EsathNumber DiffDays(this EsathDateTime d1, EsathDateTime d2)
        {
            return new EsathNumber(Math.Abs((d1.Val.Value - d2.Val.Value).Days));
        }

        [Rtimpl("ПЛТ")]
        public static EsathCurrency PMT(ElfNumber rate, EsathNumber nper, EsathCurrency pv, EsathCurrency fv, EsathNumber type)
        {
            if (type != 0 && type != 1)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, rate.VM);
            }

            if (rate is EsathCurrency)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, rate.VM);
            }

            double result;
            if (rate == 0)
            {
                if (nper == 0)
                {
                    throw new ErroneousScriptRuntimeException(ElfExceptionType.DivisionByZero, rate.VM);
                }

                result = (-pv - fv) / nper;
            }
            else
            {
                result = (-pv * Math.Pow(1 + rate, nper) - fv) / ((1 + rate * type) * ((Math.Pow(1 + rate, nper) - 1) / rate));
            }

            return new EsathCurrency(result);
        }

        [Rtimpl("ПС")]
        public static EsathCurrency PV(ElfNumber rate, EsathNumber nper, EsathCurrency pmt, EsathCurrency fv, EsathNumber type)
        {
            if (type != 0 && type != 1)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, rate.VM);
            }

            if (rate is EsathCurrency)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, rate.VM);
            }

            double result;
            if (rate == 0)
            {
                if (nper == 0)
                {
                    throw new ErroneousScriptRuntimeException(ElfExceptionType.DivisionByZero, rate.VM);
                }

                result = -pmt * nper - fv;
            }
            else
            {
                result = (-pmt * (1 + rate * type) * ((Math.Pow(1 + rate, nper) - 1) / rate) - fv) / (Math.Pow(1 + rate, nper));
            }

            return new EsathCurrency(result);
        }

        [Rtimpl("Mux")]
        public static IEsathObject MuxChildren(ScenarioNode node)
        {
            throw new NotImplementedException();
        }
    }
}