using System;
using System.Windows.Forms;
using Browser.Properties;
using DataVault.UI.Api.UIContext;
using DataVault.UI.Api.UIExtensionz;
using Browser.Gui.DataVault.UI.Commands;

namespace Browser.Gui.DataVault.UI
{
	using Commands;

	[DataVaultUIExtension]
    public class EsathDataVaultUIExtension : DataVaultUIExtension
    {
        protected override void InitializeImpl()
        {
            Func<ToolStripMenuItem> createImportFromExcel = () =>
            {
                var tsmi = new ToolStripMenuItem(Resources.Menu_Branch_ImportFromExcel);
                _ctx.BindCommand(tsmi, () => new TableImportStartCommand(_ctx));
                return tsmi;
            };

            _ctx.BranchMenu().DropDownItems.Insert(1, createImportFromExcel());
            _ctx.BranchPopup().Items.Insert(1, createImportFromExcel());
        }
    }
}
