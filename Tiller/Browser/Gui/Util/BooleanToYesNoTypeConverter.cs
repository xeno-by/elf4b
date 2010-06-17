namespace Browser.Gui.Util
{
	using System;
	using System.ComponentModel;
	using System.Globalization;

	internal class BooleanToYesNoTypeConverter : BooleanConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context,
		                                 CultureInfo culture,
		                                 object value,
		                                 Type destType)
		{
			return (bool) value
			       	?
			       		"��"
			       	: "���";
		}

		public override object ConvertFrom(ITypeDescriptorContext context,
		                                   CultureInfo culture,
		                                   object value)
		{
			return (string) value == "��";
		}
	}
}