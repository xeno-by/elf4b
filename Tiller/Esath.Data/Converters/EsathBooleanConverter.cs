using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Elf.Helpers;
using Esath.Data.Properties;

namespace Esath.Data.Converters
{
    public class EsathBooleanConverter : EsathConverter
    {
        private IEnumerable<String> SplitList(string s)
        {
            return s.Split(';').Select(el => el.Trim());
        }

        private IDictionary<String, bool> Xlate(CultureInfo culture)
        {
            var xlate = new Dictionary<String, bool>();
            var cultureBak = Resources.Culture;

            try
            {
                Resources.Culture = culture;
                SplitList(Resources.Boolean_True).ForEach(xl => xlate.Add(xl, true));
                SplitList(Resources.Boolean_True).ForEach(xl => xlate.Add(xl.ToLower(culture), true));
                SplitList(Resources.Boolean_False).ForEach(xl => xlate.Add(xl, false));
                SplitList(Resources.Boolean_False).ForEach(xl => xlate.Add(xl.ToLower(culture), false));
            }
            finally
            {
                Resources.Culture = cultureBak;
            }

            return xlate;
        }

        protected override object ConvertValFrom(Type expectedType, string text, CultureInfo locale)
        {
            var xlate = Xlate(locale);
            if (xlate.ContainsKey(text))
            {
                return xlate[text];
            }
            else
            {
                throw new FormatException(String.Format(
                    "'{0}' is not a valid boolean value in the locale '{1}'", text, locale));
            }
        }

        protected override string ConvertValTo(object value, CultureInfo locale)
        {
            var cultureBak = Resources.Culture;

            try
            {
                Resources.Culture = locale;
                var locKey = ((bool)value) ? Resources.Boolean_True : Resources.Boolean_False;
                return SplitList(locKey).First();
            }
            finally
            {
                Resources.Culture = cultureBak;
            }
        }
    }
}