using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DataVault.Core.Api;
using DataVault.UI.Api;
using System.Linq;
using DataVault.Core.Helpers;

namespace Browser.Gui
{
	public partial class RepositoryEditor : Form
	{
		public RepositoryEditor()
		{
			InitializeComponent();

			mainMenu.Visible = false;
			repoEditor.ShowMainMenu = false;
			repoEditor.TextChanged += (o, e) => this.Text = repoEditor.Text;
		}

		private void CustomDispose()
		{
			try
			{
				repoEditor.Dispose();
			}
			catch (ArgumentNullException)
			{
			}
		}

		private void RepositoryEditor_Load(object sender, EventArgs e)
		{
			this.LongOperation(() => repoEditor.Ctx.SetVault(Repository(), true));
			var import = (ToolStripMenuItem) typeof (DataVaultEditor).GetField("_vaultImport", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(repoEditor);
			var export = (ToolStripMenuItem) typeof (DataVaultEditor).GetField("_vaultExport", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(repoEditor);
			var save = (ToolStripMenuItem) typeof (DataVaultEditor).GetField("_vaultSave", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(repoEditor);

			Action<ToolStripMenuItem, ToolStripMenuItem> importmi = (imported, host) =>
				{
					if (imported.Name.Contains("Dummy")) return;
					var ddi = host.DropDownItems.Add(imported.Text);
					ddi.Click += (o, args) => imported.PerformClick();
				};

			import.DropDownItems.Cast<ToolStripMenuItem>().ForEach(ddi => importmi(ddi, importButton));
			export.DropDownItems.Cast<ToolStripMenuItem>().ForEach(ddi => importmi(ddi, exportButton));

			saveButton.Click += (o, args) => save.PerformClick();
			saveButton.Enabled = save.Enabled;
			save.EnabledChanged += (o, args) => saveButton.Enabled = save.Enabled;
		}

		public static Func<IVault> Repository
		{
			get { return () => VaultApi.OpenFs(Path.Combine(Application.StartupPath, "repository")); }
		}
	}
}