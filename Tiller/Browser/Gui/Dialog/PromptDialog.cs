using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Browser.Gui.Dialog
{
	public partial class PromptDialog : Form
	{
		public PromptDialog()
		{
			InitializeComponent();
		}

		public string Info
		{
			get { return labelInfo.Text; }
			set { labelInfo.Text = value; }
		}

		public string Value
		{
			get { return textPrompt.Text; }
			set { textPrompt.Text = value; }
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
		}

		private void textPrompt_TextChanged(object sender, EventArgs e)
		{
			buttonOk.Enabled = !string.IsNullOrEmpty(textPrompt.Text);
		}
	}
}