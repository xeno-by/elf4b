using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Browser.Gui.Editor
{
	using System.Globalization;
	using Esath.Data.Util;

	public partial class SourceValueNumericEditor : SourceValueEditorBase
	{
		public SourceValueNumericEditor()
		{
			InitializeComponent();
		}

		protected override void InitByValue(string value)
		{
			textValue.Text = value;
		}

		public override bool IsValueValidForSaving(string value)
		{
			double numeric;
			return double.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out numeric);
		}

		private void textValue_TextChanged(object sender, EventArgs e)
		{
			Value = textValue.Text;
			double numeric;
			if (double.TryParse(textValue.Text, out numeric))
			{
				try
				{
					labelInfo.Text = NumberSpelledOut.Sum.SpellOut(numeric, NumberSpelledOut.Currency.Rouble);
				}
				catch
				{
					labelInfo.Text = "";
				}
			}
			else
			{
				labelInfo.Text = "";
			}
		}
	}
}