namespace Browser.Gui.Dialog
{
	partial class ProgressWindow
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
			System.Windows.Forms.PictureBox pictureBox1;
			System.Windows.Forms.Label label1;
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			pictureBox1.Image = global::Browser.Properties.Resources._12;
			pictureBox1.Location = new System.Drawing.Point(113, 22);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(69, 70);
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			pictureBox1.UseWaitCursor = true;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(115, 109);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(64, 13);
			label1.TabIndex = 1;
			label1.Text = "Подождите";
			label1.UseWaitCursor = true;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(17, 141);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(256, 16);
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar.TabIndex = 2;
			this.progressBar.UseWaitCursor = true;
			// 
			// backgroundWorker
			// 
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.WorkerSupportsCancellation = true;
			this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			// 
			// ProgressWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.ClientSize = new System.Drawing.Size(288, 171);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar);
			this.Controls.Add(label1);
			this.Controls.Add(pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ProgressWindow";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.UseWaitCursor = true;
			this.Load += new System.EventHandler(this.ProgressWindow_Load);
			this.Shown += new System.EventHandler(this.ProgressWindow_Shown);
			((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
	}
}