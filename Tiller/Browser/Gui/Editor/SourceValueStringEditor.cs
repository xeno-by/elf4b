using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Browser.Gui.Editor
{
	public partial class SourceValueStringEditor : SourceValueEditorBase
	{
		public SourceValueStringEditor()
		{
			InitializeComponent();
		}

		private void textValue_TextChanged(object sender, EventArgs e)
		{
			Value = textValue.Text;
		}

		protected override void InitByValue(string value)
		{
			textValue.Text = value;
		}
	}
}