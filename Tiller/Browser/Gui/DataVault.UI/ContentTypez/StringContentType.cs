using System;
using Browser.Properties;
using DataVault.Core.Api;
using DataVault.UI.Api.ContentTypez;

namespace Browser.Gui.DataVault.UI.ContentTypez
{
    [ContentType("string"), ContentTypeLoc(typeof(Resources), "ValueType_String", "New_ValueString", null)]
    internal class StringContentType : CultureAwareContentTypeAppliedToValue<String>
    {
        public StringContentType(IBranch parent) 
            : base(parent) 
        {
        }

        public StringContentType(IValue untyped) 
            : base(untyped) 
        {
        }
    }
}