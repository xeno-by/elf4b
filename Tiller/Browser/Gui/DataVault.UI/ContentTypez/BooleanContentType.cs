using System;
using System.Collections.Generic;
using System.Globalization;
using Browser.Properties;
using DataVault.Core.Api;
using DataVault.UI.Api.Exceptions;
using DataVault.UI.Api.ContentTypez;
using System.Linq;
using DataVault.Core.Helpers;

namespace Browser.Gui.DataVault.UI.ContentTypez
{
    [ContentType("boolean"), ContentTypeLoc(typeof(Resources), "ValueType_Boolean", "New_ValueBoolean", "Conversion_BadFormatOfBooleanValue")]
    internal class BooleanContentType : CultureAwareContentTypeAppliedToValue<bool>
    {
        public BooleanContentType(IBranch parent)
            : base(parent)
        {
        }

        public BooleanContentType(IValue untyped)
            : base(untyped)
        {
        }

        private IEnumerable<String> SplitList(String s)
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

        protected override bool ConvertFromString(String s, CultureInfo culture)
        {
            var xlate = Xlate(culture);
            if (xlate.ContainsKey(s))
            {
                return xlate[s];
            }
            else
            {
                throw new ValidationException(CType.LocValidationFailed, s);
            }
        }

        protected override String ConvertToString(bool t, CultureInfo culture)
        {
            var cultureBak = Resources.Culture;

            try
            {
                Resources.Culture = culture;
                var locKey = t ? Resources.Boolean_True : Resources.Boolean_False;
                return SplitList(locKey).First();
            }
            finally
            {
                Resources.Culture = cultureBak;
            }
        }
    }
}