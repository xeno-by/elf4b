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
	public partial class ProgressWindow : Form
	{
		public ProgressWindow()
		{
			InitializeComponent();
		}


		public Action LongAction { get; set; }

		private void ProgressWindow_Shown(object sender, EventArgs e)
		{
			if (LongAction == null)
			{
				DialogResult = DialogResult.Ignore;
				return;
			}
			backgroundWorker.DoWork += (s, ea) => LongAction();
			backgroundWorker.RunWorkerAsync();
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			DialogResult = e.Error == null ? DialogResult.OK : DialogResult.Cancel;
		}

		private void ProgressWindow_Load(object sender, EventArgs e)
		{

		}
	}
}