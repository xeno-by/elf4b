using System;

namespace Esath.Data.Util
{
    public static class MathExtensions
    {
        public static double Round(this double d, int digits)
        {
            var mul = Math.Pow(10, digits);
            var sign = Math.Sign(d);
            return sign * Math.Round(Math.Abs(d) * mul, MidpointRounding.AwayFromZero) / mul;
        }

        public static double RoundUp(this double d, int digits)
        {
            var mul = Math.Pow(10, digits);
            var sign = Math.Sign(d);
            return sign * Math.Ceiling(Math.Abs(d) * mul) / mul;
        }

        public static double RoundDown(this double d, int digits)
        {
            var mul = Math.Pow(10, digits);
            var sign = Math.Sign(d);
            return sign * Math.Floor(Math.Abs(d) * mul) / mul;
        }
    }
}