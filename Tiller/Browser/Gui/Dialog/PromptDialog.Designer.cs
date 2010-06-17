namespace Browser.Gui.Dialog
{
	partial class PromptDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PromptDialog));
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelInfo = new System.Windows.Forms.Label();
			this.textPrompt = new System.Windows.Forms.TextBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Enabled = false;
			this.buttonOk.Location = new System.Drawing.Point(328, 123);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "Далее";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(409, 123);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelInfo
			// 
			this.labelInfo.Location = new System.Drawing.Point(90, 20);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(394, 23);
			this.labelInfo.TabIndex = 2;
			this.labelInfo.Text = "info";
			// 
			// textPrompt
			// 
			this.textPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.textPrompt.Location = new System.Drawing.Point(90, 46);
			this.textPrompt.Name = "textPrompt";
			this.textPrompt.Size = new System.Drawing.Size(395, 20);
			this.textPrompt.TabIndex = 0;
			this.textPrompt.TextChanged += new System.EventHandler(this.textPrompt_TextChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Browser.Properties.Resources._11;
			this.pictureBox1.Location = new System.Drawing.Point(12, 20);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(69, 67);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// PromptDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(500, 158);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.textPrompt);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PromptDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ввод данных";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelInfo;
		private System.Windows.Forms.TextBox textPrompt;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}