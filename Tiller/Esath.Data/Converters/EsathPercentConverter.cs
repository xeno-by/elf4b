using System;
using System.Globalization;

namespace Esath.Data.Converters
{
    public class EsathPercentConverter : EsathNumberRelatedConverter
    {
        protected override string ConvertValTo(object value, CultureInfo locale)
        {
            return base.ConvertValTo(((double)value) * 100, locale) + "%";
        }

        protected override object ConvertValFrom(Type expectedType, string text, CultureInfo locale)
        {
            if (text.EndsWith("%")) text = text.Substring(0, text.Length - 1);
            return base.ConvertValFrom(expectedType, text, locale);
        }
    }
}