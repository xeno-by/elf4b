namespace Browser.Gui.Editor
{
	partial class SourceValueDateEditor
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
			this.monthCalendar = new System.Windows.Forms.MonthCalendar();
			this.textValue = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(322, 301);
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(241, 301);
			// 
			// buttonFromRepository
			// 
			this.buttonFromRepository.Location = new System.Drawing.Point(12, 301);
			// 
			// monthCalendar
			// 
			this.monthCalendar.FirstDayOfWeek = System.Windows.Forms.Day.Monday;
			this.monthCalendar.Location = new System.Drawing.Point(118, 115);
			this.monthCalendar.MaxSelectionCount = 1;
			this.monthCalendar.Name = "monthCalendar";
			this.monthCalendar.TabIndex = 12;
			this.monthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_DateChanged);
			// 
			// textValue
			// 
			this.textValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.textValue.Location = new System.Drawing.Point(118, 72);
			this.textValue.Name = "textValue";
			this.textValue.ReadOnly = true;
			this.textValue.Size = new System.Drawing.Size(279, 20);
			this.textValue.TabIndex = 13;
			// 
			// SourceValueDateEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(408, 336);
			this.Controls.Add(this.textValue);
			this.Controls.Add(this.monthCalendar);
			this.Name = "SourceValueDateEditor";
			this.Load += new System.EventHandler(this.SourceValueDateEditor_Load);
			this.Controls.SetChildIndex(this.buttonFromRepository, 0);
			this.Controls.SetChildIndex(this.labelName, 0);
			this.Controls.SetChildIndex(this.buttonOk, 0);
			this.Controls.SetChildIndex(this.buttonCancel, 0);
			this.Controls.SetChildIndex(this.monthCalendar, 0);
			this.Controls.SetChildIndex(this.textValue, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MonthCalendar monthCalendar;
		private System.Windows.Forms.TextBox textValue;

	}
}
