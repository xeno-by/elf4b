namespace Browser.Gui.Dialog
{
	partial class GenerateReportDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateReportDialog));
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonGenerate = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.webBrowser = new System.Windows.Forms.WebBrowser();
			this.buttonExport = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(401, 335);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonGenerate
			// 
			this.buttonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonGenerate.Location = new System.Drawing.Point(320, 335);
			this.buttonGenerate.Name = "buttonGenerate";
			this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
			this.buttonGenerate.TabIndex = 3;
			this.buttonGenerate.Text = "Далее";
			this.buttonGenerate.UseVisualStyleBackColor = true;
			this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.webBrowser);
			this.panel1.Location = new System.Drawing.Point(12, 23);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(464, 294);
			this.panel1.TabIndex = 5;
			// 
			// webBrowser
			// 
			this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser.Location = new System.Drawing.Point(0, 0);
			this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.Size = new System.Drawing.Size(464, 294);
			this.webBrowser.TabIndex = 0;
			this.webBrowser.Url = new System.Uri("about:blank", System.UriKind.Absolute);
			// 
			// buttonExport
			// 
			this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonExport.Location = new System.Drawing.Point(320, 335);
			this.buttonExport.Name = "buttonExport";
			this.buttonExport.Size = new System.Drawing.Size(75, 23);
			this.buttonExport.TabIndex = 6;
			this.buttonExport.Text = "Экспорт";
			this.buttonExport.UseVisualStyleBackColor = true;
			this.buttonExport.Visible = false;
			this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
			// 
			// GenerateReportDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(488, 370);
			this.Controls.Add(this.buttonExport);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonGenerate);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GenerateReportDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Модуль Генерации Отчетов";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonGenerate;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.WebBrowser webBrowser;
		private System.Windows.Forms.Button buttonExport;
	}
}