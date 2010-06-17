using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Elf.Helpers;

namespace Esath.Data.Converters
{
    public class EsathConverter : TypeConverter
    {
        public Type ObjectType { get; internal set; }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(String);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(String);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is String)) return base.ConvertFrom(context, culture, value);

            var tval = ObjectType.GetProperty("Val").PropertyType;
            var val = ConvertValFrom(tval, (String)value, culture);
            var ctor = ObjectType.GetConstructors().Single();
            return ctor.Invoke(val.AsArray());
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(String)) return base.ConvertTo(context, culture, value, destinationType);

            var val = ObjectType.GetProperty("Val").GetValue(value, null);
            return ConvertValTo(val, culture);
        }

        protected virtual Object ConvertValFrom(Type expectedType, String text, CultureInfo locale)
        {
            return expectedType.FromLocalString(text, locale);
        }

        protected virtual String ConvertValTo(Object value, CultureInfo locale)
        {
            return value.ToLocalString(locale);
        }
    }
}