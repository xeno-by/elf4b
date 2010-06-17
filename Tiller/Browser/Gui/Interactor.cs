namespace Browser.Gui
{
	using System;
	using System.Windows.Forms;
	using Dialog;

	public static class Interactor
	{
		public static string Prompt(string info, string value)
		{
			var f = new PromptDialog {Info = info, Value = value};

			if (f.ShowDialog() == DialogResult.OK)
			{
				if (f.Value != value)
					return f.Value;
			}

			return null;
		}

		public static void LongOperation(this Control source, Action action)
		{
//			var progress = new ProgressWindow {LongAction = action};
//			return progress.ShowDialog(source) == DialogResult.OK;

			var wait = new WaitWindow();
			source.Enabled = false;
			try
			{
				source.UseWaitCursor = true;
				wait.Show(source);
				Application.DoEvents();
				action();
				Application.DoEvents();
			}
			finally
			{
				wait.Hide();
				source.UseWaitCursor = false;
				source.Enabled = true;
			}
		}
	}
}