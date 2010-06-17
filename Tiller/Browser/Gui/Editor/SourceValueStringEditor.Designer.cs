namespace Browser.Gui.Editor
{
	partial class SourceValueStringEditor
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
			this.textValue = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(438, 178);
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(357, 178);
			// 
			// buttonFromRepository
			// 
			this.buttonFromRepository.Location = new System.Drawing.Point(12, 178);
			// 
			// textValue
			// 
			this.textValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.textValue.Location = new System.Drawing.Point(118, 72);
			this.textValue.Name = "textValue";
			this.textValue.Size = new System.Drawing.Size(395, 20);
			this.textValue.TabIndex = 0;
			this.textValue.TextChanged += new System.EventHandler(this.textValue_TextChanged);
			// 
			// SourceValueStringEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(525, 213);
			this.Controls.Add(this.textValue);
			this.Name = "SourceValueStringEditor";
			this.Controls.SetChildIndex(this.buttonFromRepository, 0);
			this.Controls.SetChildIndex(this.textValue, 0);
			this.Controls.SetChildIndex(this.buttonOk, 0);
			this.Controls.SetChildIndex(this.buttonCancel, 0);
			this.Controls.SetChildIndex(this.labelName, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textValue;
	}
}
