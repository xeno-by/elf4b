using System;
using System.Linq;
using System.Windows.Forms;
using Browser.Properties;
using DataVault.Core.Helpers;
using DataVault.UI.Api.Exceptions;

namespace Browser.Gui.DataVault.UI.Commands
{
    public static class TableHelper
    {
        public static Table AsTable(this String s)
        {
            s = s.Replace(Environment.NewLine, "\n");
            var lines = s.Split("\n".MkArray(), StringSplitOptions.RemoveEmptyEntries);

            var rows = lines.Length - 1;
            var cols = lines[0].Where(c => c == '\t').Count();

            if (rows <= 0)
            {
                MessageBox.Show(Resources.TableImport_BadNumberOfLines, Resources.TableImport_Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (cols <= 0)
            {
                MessageBox.Show(Resources.TableImport_BadNumberOfColumns, Resources.TableImport_Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            foreach (var line in lines.Skip(0))
            {
                var lineCols = line.Where(c => c == '\t').Count();
                if (lineCols != cols)
                {
                    MessageBox.Show(String.Format(Resources.TableImport_RowLengthDiscrepancy,
                        line, lineCols, cols), Resources.TableImport_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            var table = new String[rows + 1, cols + 1];
            for(var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var frags = line.Split('\t'.MkArray(), StringSplitOptions.None);

                for(var j = 0; j < frags.Length; j++)
                {
                    var c = frags[j];
                    table[i, j] = c;
                }
            }

            try
            {
                return new Table(table);
            }
            catch(ValidationException vex)
            {
                MessageBox.Show(vex.Message, Resources.TableImport_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}