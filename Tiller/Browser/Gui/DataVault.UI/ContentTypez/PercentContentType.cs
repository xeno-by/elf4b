using System;
using System.Globalization;
using Browser.Properties;
using DataVault.Core.Api;
using DataVault.UI.Api.ContentTypez;

namespace Browser.Gui.DataVault.UI.ContentTypez
{
    [ContentType("percent"), ContentTypeLoc(typeof(Resources), "ValueType_Percent", "New_ValuePercent", "Conversion_BadFormatOfPercentValue")]
    internal class PercentContentType : CultureAwareContentTypeAppliedToValue<Double>
    {
        public PercentContentType(IBranch parent) 
            : base(parent) 
        {
        }

        public PercentContentType(IValue untyped) 
            : base(untyped) 
        {
        }

        protected override double ConvertFromString(String s, CultureInfo culture)
        {
            if (culture == CultureInfo.GetCultureInfo("ru-RU"))
            {
                return base.ConvertFromString(s.Replace(".", ","), culture);
            }
            else if (culture == CultureInfo.InvariantCulture)
            {
                // this is for backward compatibility only
                return base.ConvertFromString(s.Replace(",", "."), culture);
            }
            else
            {
                return base.ConvertFromString(s, culture);
            }
        }
    }
}