namespace Browser.Gui.DataVault.UI.Commands
{
    partial class TableImportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableImportForm));
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new RichTextBoxWithHiddenCaret();
            this.panel1 = new System.Windows.Forms.Panel();
            this._tbExcelRange = new RichTextBoxWithHiddenCaret();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _cancelButton
            // 
            this._cancelButton.AccessibleDescription = null;
            this._cancelButton.AccessibleName = null;
            resources.ApplyResources(this._cancelButton, "_cancelButton");
            this._cancelButton.BackgroundImage = null;
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Font = null;
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _okButton
            // 
            this._okButton.AccessibleDescription = null;
            this._okButton.AccessibleName = null;
            resources.ApplyResources(this._okButton, "_okButton");
            this._okButton.BackgroundImage = null;
            this._okButton.Font = null;
            this._okButton.Name = "_okButton";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // panel2
            // 
            this.panel2.AccessibleDescription = null;
            this.panel2.AccessibleName = null;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackgroundImage = null;
            this.panel2.Controls.Add(this._cancelButton);
            this.panel2.Controls.Add(this._okButton);
            this.panel2.Font = null;
            this.panel2.Name = "panel2";
            // 
            // panel3
            // 
            this.panel3.AccessibleDescription = null;
            this.panel3.AccessibleName = null;
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.BackgroundImage = null;
            this.panel3.Controls.Add(this.richTextBox1);
            this.panel3.Font = null;
            this.panel3.Name = "panel3";
            // 
            // richTextBox1
            // 
            this.richTextBox1.AccessibleDescription = null;
            this.richTextBox1.AccessibleName = null;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BackgroundImage = null;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = null;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this._tbExcelRange);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // _tbExcelRange
            // 
            this._tbExcelRange.AccessibleDescription = null;
            this._tbExcelRange.AccessibleName = null;
            resources.ApplyResources(this._tbExcelRange, "_tbExcelRange");
            this._tbExcelRange.BackgroundImage = null;
            this._tbExcelRange.Font = null;
            this._tbExcelRange.Name = "_tbExcelRange";
            this._tbExcelRange.ReadOnly = true;
            // 
            // TableImportForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Font = null;
            this.Icon = null;
            this.KeyPreview = true;
            this.Name = "TableImportForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TableImportForm_KeyUp);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox _tbExcelRange;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}