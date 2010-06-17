using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Browser.Gui
{
	using System.Reflection;
	using Properties;

	public partial class SplashForm : Form
	{
		private readonly bool _inAboutMode;

		public SplashForm()
		{
			InitializeComponent();

			var va = (AssemblyFileVersionAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyFileVersionAttribute), false)[0];
			labelVersion.Text = "Версия " + va.Version;
#if EDITION_LIGHT
			labelEdition.Visible = false;
#else
			var resources = new ComponentResourceManager(typeof (SplashForm));
// ReSharper disable DoNotCallOverridableMethodsInConstructor
			BackgroundImage = ((Image) (resources.GetObject("$this.BackgroundImage")));
// ReSharper restore DoNotCallOverridableMethodsInConstructor
#endif
			var ca = (AssemblyConfigurationAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyConfigurationAttribute), false)[0];
			labConfiguration.Visible = ca.Configuration.IndexOf("TEST") > -1;
			var cra = (AssemblyCopyrightAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false)[0];
			labCopyright.Text = cra.Copyright;

			labelDescription.Text += Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (SpalshInfoAttribute), false).Cast<SpalshInfoAttribute>().OrderBy(x => x.SiblingWeigth).Select(x => x.ToString()).ToArray());
		}

		public SplashForm(bool inAboutMode) : this()
		{
			_inAboutMode = inAboutMode;
		}

		private void SplashForm_Click(object sender, EventArgs e)
		{
			if (!_inAboutMode) return;
			Hide();
		}

		private void SplashForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!_inAboutMode) return;
			Hide();
		}

		private void SplashForm_Load(object sender, EventArgs e)
		{
			Application.DoEvents();
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			// ea
		}

		private void SplashForm_ControlAdded(object sender, ControlEventArgs e)
		{
			e.Control.Click += Control_Click;
		}

		void Control_Click(object sender, EventArgs e)
		{
			if (!_inAboutMode) return;
			Hide();
		}
	}
}