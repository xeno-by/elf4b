namespace Esath.Playground
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._elfEditor = new Esath.Pie.ElfEditor();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._elfCode = new System.Windows.Forms.RichTextBox();
            this._publicText = new System.Windows.Forms.RichTextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _elfEditor
            // 
            resources.ApplyResources(this._elfEditor, "_elfEditor");
            this._elfEditor.Name = "_elfEditor";
            this._elfEditor.SelectedLine = null;
            this._elfEditor.SelectedSub = null;
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._elfEditor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this._elfCode);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this._publicText);
            // 
            // _elfCode
            // 
            this._elfCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this._elfCode, "_elfCode");
            this._elfCode.Name = "_elfCode";
            // 
            // _publicText
            // 
            this._publicText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this._publicText, "_publicText");
            this._publicText.Name = "_publicText";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Esath.Pie.ElfEditor _elfEditor;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox _elfCode;
        private System.Windows.Forms.RichTextBox _publicText;

    }
}

