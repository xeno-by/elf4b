using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Browser.Properties;
using DataVault.Core.Api;
using DataVault.UI.Api.ContentTypez;
using DataVault.UI.Api.Exceptions;
using DataVault.Core.Helpers;

namespace Browser.Gui.DataVault.UI.ContentTypez
{
    [ContentType("datetime"), ContentTypeLoc(typeof(Resources), "ValueType_DateTime", "New_ValueDateTime", "Conversion_BadFormatOfDateTimeValue")]
    internal class DateTimeContentType : CultureAwareContentTypeAppliedToValue<DateTime>
    {
        public DateTimeContentType(IBranch parent)
            : base(parent)
        {
        }

        public DateTimeContentType(IValue untyped)
            : base(untyped)
        {
        }

        protected override String ConvertToString(DateTime t, CultureInfo culture)
        {
            return t == DateTime.MinValue ? String.Empty : t.ToShortDateString();
        }

        protected override DateTime ConvertFromString(String s, CultureInfo culture)
        {
            try
            {
                if (s.IsNullOrEmpty())
                {
                    return DateTime.MinValue;
                }
                else
                {
                    var match = Regex.Match(s, @"^(?<day>\d+)\.(?<month>\d+)\.(?<year>\d+)$");
                    if (match.Success)
                    {
                        return new DateTime(
                            int.Parse(match.Result("${year}"), CultureInfo.InvariantCulture),
                            int.Parse(match.Result("${month}"), CultureInfo.InvariantCulture),
                            int.Parse(match.Result("${day}"), CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        throw new ArgumentException("The String entered doesn't match the short date regex.");
                    }
                }
            }
            catch (Exception e)
            {
                throw new ValidationException(CType.LocValidationFailed, e, s);
            }
        }
    }
}