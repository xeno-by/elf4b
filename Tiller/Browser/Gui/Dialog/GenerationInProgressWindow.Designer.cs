namespace Browser.Gui.Dialog
{
	partial class GenerationInProgressWindow
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
			this.buttonCancel = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.labelInfo = new System.Windows.Forms.Label();
			this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonCancel.Location = new System.Drawing.Point(186, 97);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(93, 24);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "Отменить";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Visible = false;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(12, 53);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(453, 17);
			this.progressBar.TabIndex = 1;
			this.progressBar.Visible = false;
			// 
			// labelInfo
			// 
			this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.labelInfo.Location = new System.Drawing.Point(16, 11);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(448, 29);
			this.labelInfo.TabIndex = 2;
			this.labelInfo.Text = "label1";
			this.labelInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// backgroundWorker
			// 
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.WorkerSupportsCancellation = true;
			this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
			// 
			// GenerationInProgressWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(477, 132);
			this.ControlBox = false;
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.buttonCancel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GenerationInProgressWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Shown += new System.EventHandler(this.GenerationInProgressWindow_Shown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label labelInfo;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
	}
}