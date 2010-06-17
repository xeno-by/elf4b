using System;
using System.Reflection;
using Elf.Core.ClrIntegration;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Elf.Helpers;

namespace Elf.Core.TypeSystem
{
    [Rtimpl("ElfNumber")]
    public class ElfNumber : ElfObjectImpl
    {
        public Double Val { get; private set; }

        public ElfNumber(double val)
        {
            Val = val;
        }

        public static implicit operator ElfNumber(double d)
        {
            return new ElfNumber(d);
        }

        public static implicit operator ElfNumber(int i)
        {
            return new ElfNumber(i);
        }

        public static implicit operator double(ElfNumber d)
        {
            return d.Val;
        }

        public virtual ElfNumber Add(ElfNumber n)
        {
            return Val + n.Val;
        }

        public virtual ElfNumber Subtract(ElfNumber n)
        {
            return Val - n.Val;
        }

        public virtual ElfNumber Multiply(ElfNumber n)
        {
            return Val * n.Val;
        }

        public virtual ElfNumber Divide(ElfNumber n)
        {
            if (n.Val == 0)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.DivisionByZero, VM);
            }

            return Val / n.Val;
        }

        public virtual ElfNumber Power(ElfNumber n)
        {
            return Math.Pow(Val, n.Val);
        }

        [Rtimpl("+")]
        public static ElfNumber operator +(ElfNumber n1, ElfNumber n2)
        {
            return n1.Add(n2);
        }

        [Rtimpl("-")]
        public static ElfNumber operator -(ElfNumber n1, ElfNumber n2)
        {
            return n1.Subtract(n2);
        }

        [Rtimpl("*")]
        public static ElfNumber operator *(ElfNumber n1, ElfNumber n2)
        {
            return n1.Multiply(n2);
        }

        [Rtimpl("/")]
        public static ElfNumber operator /(ElfNumber n1, ElfNumber n2)
        {
            return n1.Divide(n2);
        }

        [Rtimpl("^")]
        public static ElfNumber operator ^(ElfNumber n1, ElfNumber n2)
        {
            return n1.Power(n2);
        }

        [Rtimpl(">=")]
        public static ElfBoolean operator >=(ElfNumber n1, ElfNumber n2)
        {
            if (n1.GetType().Name != MethodBase.GetCurrentMethod().DeclaringType.Name)
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, n1.VM);
            if (n2.GetType().Name != MethodBase.GetCurrentMethod().DeclaringType.Name)
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, n1.VM);

            return n1.Val >= n2.Val;
        }

        [Rtimpl("<=")]
        public static ElfBoolean operator <=(ElfNumber n1, ElfNumber n2)
        {
            if (n1.GetType().Name != MethodBase.GetCurrentMethod().DeclaringType.Name)
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, n1.VM);
            if (n2.GetType().Name != MethodBase.GetCurrentMethod().DeclaringType.Name)
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, n1.VM);

            return n1.Val <= n2.Val;
        }

        [Rtimpl("<")]
        public static ElfBoolean operator <(ElfNumber n1, ElfNumber n2)
        {
            if (n1.GetType().Name != MethodBase.GetCurrentMethod().DeclaringType.Name)
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, n1.VM);
            if (n2.GetType().Name != MethodBase.GetCurrentMethod().DeclaringType.Name)
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, n1.VM);

            return n1.Val < n2.Val;
        }

        [Rtimpl(">")]
        public static ElfBoolean operator >(ElfNumber n1, ElfNumber n2)
        {
            if (n1.GetType().Name != MethodBase.GetCurrentMethod().DeclaringType.Name)
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, n1.VM);
            if (n2.GetType().Name != MethodBase.GetCurrentMethod().DeclaringType.Name)
                throw new ErroneousScriptRuntimeException(ElfExceptionType.OperandsDontSuitMethod, n1.VM);

            return n1.Val > n2.Val;
        }

        public override string ToString()
        {
            return "<" + Val.ToInvariantString() + ">";
        }

        public override bool Equals(object obj)
        {
            var other = obj as ElfNumber;
            return other != null && Val == other.Val;
        }

        public override int GetHashCode()
        {
            return Val.GetHashCode();
        }
    }
}
