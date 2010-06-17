using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Browser.Gui.DataVault.UI.Commands
{
    internal partial class TableImportForm : Form
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Table ExcelRange { get; private set; }

        public TableImportForm()
        {
            InitializeComponent();

            var timer = new Timer{Interval = 100, Enabled = true};
            timer.Tick += (o, e) =>
            {
                RichTextBoxWithHiddenCaret.HideCaret(richTextBox1.Handle); timer.Stop();
                RichTextBoxWithHiddenCaret.HideCaret(_tbExcelRange.Handle); timer.Stop();
            };
        }

        private void TableImportForm_KeyUp(Object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Control && !e.Alt && !e.Shift)
            {
                _tbExcelRange.Text = Clipboard.GetText();
                _okButton.Enabled = !String.IsNullOrEmpty(_tbExcelRange.Text);
            }

            if (e.KeyCode == Keys.Escape && !e.Control && !e.Alt && !e.Shift)
            {
                _cancelButton_Click(this, EventArgs.Empty);
            }

            if (e.KeyCode == Keys.Enter && !e.Control && !e.Alt && !e.Shift)
            {
                if (_okButton.Enabled)
                {
                    _okButton_Click(this, EventArgs.Empty);
                }
            }
        }

        private void _cancelButton_Click(Object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void _okButton_Click(Object sender, EventArgs e)
        {
            var table = _tbExcelRange.Text.AsTable();
            if (table != null)
            {
                ExcelRange = table;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
