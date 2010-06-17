using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Elf.Helpers;

namespace Esath.Data.Converters
{
    public class EsathDateTimeConverter : EsathConverter
    {
        protected override object ConvertValFrom(Type expectedType, string text, CultureInfo locale)
        {
            if (text.IsNullOrEmpty())
            {
                return null;
            }
            else
            {
                var match = Regex.Match(text, @"^(?<day>\d+)\.(?<month>\d+)\.(?<year>\d+)$");
                if (match.Success)
                {
                    return new DateTime(
                        int.Parse(match.Result("${year}"), CultureInfo.InvariantCulture),
                        int.Parse(match.Result("${month}"), CultureInfo.InvariantCulture),
                        int.Parse(match.Result("${day}"), CultureInfo.InvariantCulture));
                }
                else
                {
                    throw new ArgumentException("The string entered doesn't match the short date regex.");
                }
            }
        }

        protected override string ConvertValTo(object value, CultureInfo locale)
        {
            var dt = (DateTime?)value;
            return dt == null ? null : String.Format("{0:00}.{1:00}.{2:0000}",
                 dt.Value.Day, dt.Value.Month, dt.Value.Year);
        }
    }
}