using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Browser.Gui.Editor
{
	public partial class SourceValueDateEditor : SourceValueEditorBase
	{
		public SourceValueDateEditor()
		{
			InitializeComponent();
		}

		private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
		{
			textValue.Text = e.Start.ToString("dd.MM.yyyy");
			Value = textValue.Text;
		}

		public override bool IsValueValidForSaving(string value)
		{
			DateTime date;
			return DateTime.TryParse(value, out date);
		}

		protected override void InitByValue(string value)
		{
			DateTime date;
			if(DateTime.TryParse(value, out date))
			{
				monthCalendar.SelectionStart = date;
			}
		}

		private void SourceValueDateEditor_Load(object sender, EventArgs e)
		{
			monthCalendar.AnnuallyBoldedDates = new[]
			                                    	{
			                                    		new DateTime(2009, 01, 01),
			                                    		new DateTime(2009, 01, 07),
			                                    		new DateTime(2009, 03, 08),
			                                    		new DateTime(2009, 05, 01),
			                                    		new DateTime(2009, 05, 09),
			                                    		new DateTime(2009, 06, 03),
			                                    		new DateTime(2009, 12, 25),
			                                    	};
		}
	}
}