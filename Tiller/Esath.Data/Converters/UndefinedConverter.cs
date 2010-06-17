using System;
using System.ComponentModel;
using System.Globalization;

namespace Esath.Data.Converters
{
    public class UndefinedConverter : EsathConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is String)) return base.ConvertFrom(context, culture, value);
            return new EsathUndefined();
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(String)) return base.ConvertTo(context, culture, value, destinationType);
            return "?";
        }
    }
}