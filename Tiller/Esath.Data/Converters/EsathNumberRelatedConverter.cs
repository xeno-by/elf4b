using System;
using System.Globalization;

namespace Esath.Data.Converters
{
    public class EsathNumberRelatedConverter : EsathConverter
    {
        protected override object ConvertValFrom(Type expectedType, string text, CultureInfo locale)
        {
            if (locale == CultureInfo.GetCultureInfo("ru-RU")) text = text.Replace(".", ",");
            if (locale != CultureInfo.GetCultureInfo("ru-RU")) text = text.Replace(",", ".");
            return base.ConvertValFrom(expectedType, text, locale);
        }
    }
}