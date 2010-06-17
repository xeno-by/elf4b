using System;
using System.Globalization;
using Browser.Properties;
using DataVault.Core.Api;
using DataVault.UI.Api.ContentTypez;

namespace Browser.Gui.DataVault.UI.ContentTypez
{
    [ContentType("currency"), ContentTypeLoc(typeof(Resources), "ValueType_Currency", "New_ValueCurrency", "Conversion_BadFormatOfCurrencyValue")]
    internal class CurrencyContentType : CultureAwareContentTypeAppliedToValue<Decimal>
    {
        public CurrencyContentType(IBranch parent) 
            : base(parent) 
        {
        }

        public CurrencyContentType(IValue untyped) 
            : base(untyped) 
        {
        }

        protected override Decimal ConvertFromString(String s, CultureInfo culture)
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