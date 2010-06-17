using System;
using System.Linq;
using System.Text.RegularExpressions;
using DataVault.Core.Api;
using DataVault.Core.Helpers;
using DataVault.UI.Api.ApiExtensions;
using DataVault.UI.Api.Exceptions;
using DataVault.UI.Api.ContentTypez;
using DataVault.UI.Api.UIContext;
using DataVault.UI.Properties;

namespace Browser.Gui.DataVault.UI.Commands
{
	using global::DataVault.Core.Helpers.Assertions;

	public class TableImportFinishCommand : ContextBoundCommand
    {
        private Table ExcelRange { get; set; }
        private IBranch CreatedBranch { get; set; }

        public TableImportFinishCommand(DataVaultUIContext context, Table excelRange)
            : base(context) 
        {
            ExcelRange = excelRange;
        }

				public override bool CanDoImpl()
        {
            return base.CanDo() && Branch != null && ExcelRange != null;
        }

				public override void DoImpl()
        {
            Func<int, String> namegen = i => String.Format(Resources.New_BranchDefaultName, i);
            var lastUsedIndex = 1.Seq(i => i + 1, i => Branch.GetBranches().Any(b => b.Name == namegen(i))).LastOrDefault();
            var unusedName = namegen(lastUsedIndex + 1);
            CreatedBranch = Branch.CreateBranch(unusedName).SetDefault2();
            CreatedBranch.Delete();

            try
            {
                for (var i = 0; i < ExcelRange.Rows.Length; i++)
                {
                    var row = ExcelRange.Rows[i];
                    var rowBranch = CreatedBranch.CreateBranch(row).SetDefault2();

                    for (var j = 0; j < ExcelRange.Data[i].Length; j++)
                    {
                        var col = ExcelRange.Columns[j];
                        var data = ExcelRange.Data[i][j];

                        var v = rowBranch.CreateValue(col, data).SetDefault2();
                        v.SetTypeToken2(ExcelRange.ColumnTypes[j].TypeToken);

                        // validate the data format
                        var aux = ContentTypes.ApplyCType(v).Value;
                    }
                }
            }
            catch(ArgumentException aex)
            {
                if (aex.Message.StartsWith("VPath"))
                {
                    var match = Regex.Match(aex.Message, 
                        @"VPath '(?<name>.*?)' is of invalid format \(expected '.*?'\)");

                    match.Success.AssertTrue();
                    throw new ValidationException(Resources.Validation_InvalidName, match.Result("${name}"));
                }
            }

            Branch.AttachBranch(CreatedBranch);
            Tree.SelectedNode = Tree.Nodes[0].SelectNode(Branch.VPath);
            var tn = Ctx.CreateTreeNodesRecursive(Tree.SelectedNode, CreatedBranch);

            Tree.SelectedNode = tn;
            tn.Expand();
            tn.BeginEdit();
        }

				public override void UndoImpl()
        {
            CreatedBranch.Delete();

            var tn = Tree.Nodes[0].SelectNode(CreatedBranch.VPath).AssertNotNull();
            Tree.SelectedNode = tn.Parent;
            tn.Remove();
        }
    }
}