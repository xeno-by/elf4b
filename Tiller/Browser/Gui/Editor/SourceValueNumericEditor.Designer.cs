namespace Browser.Gui.Editor
{
	partial class SourceValueNumericEditor
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
			this.labelInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textValue
			// 
			this.textValue.Location = new System.Drawing.Point(118, 72);
			this.textValue.MaxLength = 20;
			this.textValue.Name = "textValue";
			this.textValue.Size = new System.Drawing.Size(393, 20);
			this.textValue.TabIndex = 11;
			this.textValue.TextChanged += new System.EventHandler(this.textValue_TextChanged);
			// 
			// labelInfo
			// 
			this.labelInfo.Location = new System.Drawing.Point(16, 115);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(495, 79);
			this.labelInfo.TabIndex = 12;
			this.labelInfo.Text = "прописью";
			// 
			// SourceValueNumericEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(525, 247);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.textValue);
			this.Name = "SourceValueNumericEditor";
			this.Controls.SetChildIndex(this.textValue, 0);
			this.Controls.SetChildIndex(this.labelInfo, 0);
			this.Controls.SetChildIndex(this.buttonOk, 0);
			this.Controls.SetChildIndex(this.buttonCancel, 0);
			this.Controls.SetChildIndex(this.labelName, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textValue;
		private System.Windows.Forms.Label labelInfo;
	}
}
