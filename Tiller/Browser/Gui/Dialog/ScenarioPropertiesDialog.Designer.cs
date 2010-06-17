namespace Browser.Gui.Dialog
{
	partial class ScenarioPropertiesDialog
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Свойства документа", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Пространство", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ColumnHeader columnHeader1;
			System.Windows.Forms.ColumnHeader columnHeader2;
			this.listView = new System.Windows.Forms.ListView();
			columnHeader1 = new System.Windows.Forms.ColumnHeader();
			columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listView.CausesValidation = false;
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2});
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			listViewGroup1.Header = "Свойства документа";
			listViewGroup1.Name = "common";
			listViewGroup2.Header = "Пространство";
			listViewGroup2.Name = "bytes";
			this.listView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
			this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(365, 302);
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			columnHeader1.Text = "";
			columnHeader1.Width = 200;
			// 
			// columnHeader2
			// 
			columnHeader2.Text = "";
			columnHeader2.Width = 100;
			// 
			// ScenarioPropertiesDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(365, 302);
			this.Controls.Add(this.listView);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimizeBox = false;
			this.Name = "ScenarioPropertiesDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Свойства сценария";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listView;
	}
}