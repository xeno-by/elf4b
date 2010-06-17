using System;
using System.Windows.Forms;
using DataVault.UI.Api.Commands.WithHistory;
using DataVault.UI.Api.UIContext;
using DataVault.UI.Impl.Controls;

namespace Browser.Gui.DataVault.UI.Commands
{
    [GhostableInHistory]
    public class TableImportStartCommand : ContextBoundCommand
    {
        public TableImportStartCommand(DataVaultUIContext context)
            : base(context) 
        {
        }

        public override bool CanDoImpl()
        {
					return base.CanDoImpl() && Branch != null;
        }

        public override void DoImpl()
        {
            var importForm = new TableImportForm();
            importForm.StartPosition = FormStartPosition.CenterParent;

            if (importForm.ShowDialog() == DialogResult.OK)
            {
                var cmd = new TableImportFinishCommand(Ctx, importForm.ExcelRange);
                Ctx.Execute(cmd);
            }
        }

        public override void UndoImpl()
        {
            throw new NotSupportedException();
        }
    }
}