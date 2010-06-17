using DataVault.UI.Api;

namespace Browser.Gui
{
	partial class RepositoryEditor
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

		    CustomDispose();
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepositoryEditor));
            this.repoEditor = new DataVaultEditor();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.repositoryButton = new System.Windows.Forms.ToolStripMenuItem();
            this.importButton = new System.Windows.Forms.ToolStripMenuItem();
            this.exportButton = new System.Windows.Forms.ToolStripMenuItem();
            this.saveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // repoEditor
            // 
            this.repoEditor.AutoSize = true;
            this.repoEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.repoEditor.Location = new System.Drawing.Point(0, 24);
            this.repoEditor.Name = "repoEditor";
            this.repoEditor.ShowMainMenu = false;
            this.repoEditor.Size = new System.Drawing.Size(565, 311);
            this.repoEditor.TabIndex = 0;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.repositoryButton});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(565, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // repositoryButton
            // 
            this.repositoryButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importButton,
            this.exportButton,
            this.saveButton});
            this.repositoryButton.Name = "repositoryButton";
            this.repositoryButton.Size = new System.Drawing.Size(84, 20);
            this.repositoryButton.Text = "&–епозиторий";
            // 
            // importButton
            // 
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(178, 22);
            this.importButton.Text = "&»мпортировать из";
            // 
            // exportButton
            // 
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(178, 22);
            this.exportButton.Text = "&Ёкспортировать в";
            // 
            // saveButton
            // 
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(178, 22);
            this.saveButton.Text = "&—охранить";
            // 
            // RepositoryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 335);
            this.Controls.Add(this.repoEditor);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "RepositoryEditor";
            this.Text = "RepositoryEditor";
            this.Load += new System.EventHandler(this.RepositoryEditor_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private DataVaultEditor repoEditor;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem repositoryButton;
        private System.Windows.Forms.ToolStripMenuItem importButton;
        private System.Windows.Forms.ToolStripMenuItem exportButton;
        private System.Windows.Forms.ToolStripMenuItem saveButton;
	}
}