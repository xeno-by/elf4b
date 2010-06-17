namespace ObjectMeet.Tiller.Gui
{
	using System.Windows.Forms;
	using Entities.Service;

	public class Interactor : IInteractionProvider
	{
		public Interactor(IWin32Window win32Window)
		{
			Win32Window = win32Window;
		}

		public IWin32Window Win32Window { get; private set; }

		public bool AskRetryCancel(string title, string message)
		{
			return MessageBox.Show(
			       	Win32Window,
			       	message,
			       	title,
			       	MessageBoxButtons.RetryCancel,
			       	MessageBoxIcon.Asterisk,
			       	MessageBoxDefaultButton.Button1
			       	) == DialogResult.Retry;
		}

		public bool AskConfirmation(string title, string message)
		{
			return MessageBox.Show(
			       	Win32Window,
			       	message,
			       	title,
			       	MessageBoxButtons.YesNo,
			       	MessageBoxIcon.Question,
			       	MessageBoxDefaultButton.Button1
			       	) == DialogResult.Yes;
		}

		public void Alert(string title, string message)
		{
			MessageBox.Show(
				Win32Window,
				message,
				title,
				MessageBoxButtons.OK,
				MessageBoxIcon.Information,
				MessageBoxDefaultButton.Button1
				);
		}
	}
}