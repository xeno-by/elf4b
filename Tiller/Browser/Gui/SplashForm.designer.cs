namespace Browser.Gui
{
	partial class SplashForm
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
			this.labelDescription = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.labConfiguration = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.labelEdition = new System.Windows.Forms.Label();
			this.labCopyright = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// labelDescription
			// 
			this.labelDescription.BackColor = System.Drawing.Color.Transparent;
			this.labelDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelDescription.Location = new System.Drawing.Point(99, 49);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(344, 156);
			this.labelDescription.TabIndex = 0;
			this.labelDescription.Text = "Автоматизированное рабочее место оценщика для создания типовых отчетов и заключен" +
					"ий об оценке";
			this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(20, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 33);
			this.label2.TabIndex = 1;
			this.label2.Text = "АРМОСТО";
			// 
			// labelVersion
			// 
			this.labelVersion.BackColor = System.Drawing.Color.Transparent;
			this.labelVersion.Location = new System.Drawing.Point(178, 36);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(252, 13);
			this.labelVersion.TabIndex = 2;
			this.labelVersion.Text = "Версия 0.5.3 от 19.01.2009";
			// 
			// labConfiguration
			// 
			this.labConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labConfiguration.BackColor = System.Drawing.Color.Transparent;
			this.labConfiguration.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labConfiguration.ForeColor = System.Drawing.Color.Red;
			this.labConfiguration.Location = new System.Drawing.Point(305, 9);
			this.labConfiguration.Name = "labConfiguration";
			this.labConfiguration.Size = new System.Drawing.Size(133, 16);
			this.labConfiguration.TabIndex = 3;
			this.labConfiguration.Text = "Тестовая версия";
			this.labConfiguration.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = global::Browser.Properties.Resources._02_64x64;
			this.pictureBox1.Location = new System.Drawing.Point(12, 152);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(78, 69);
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// labelEdition
			// 
			this.labelEdition.AutoSize = true;
			this.labelEdition.BackColor = System.Drawing.Color.Transparent;
			this.labelEdition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelEdition.Location = new System.Drawing.Point(23, 10);
			this.labelEdition.Name = "labelEdition";
			this.labelEdition.Size = new System.Drawing.Size(206, 15);
			this.labelEdition.TabIndex = 5;
			this.labelEdition.Text = "Профессиональная редакция";
			// 
			// labCopyright
			// 
			this.labCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labCopyright.BackColor = System.Drawing.Color.Transparent;
			this.labCopyright.Location = new System.Drawing.Point(186, 213);
			this.labCopyright.Name = "labCopyright";
			this.labCopyright.Size = new System.Drawing.Size(252, 13);
			this.labCopyright.TabIndex = 6;
			this.labCopyright.Text = "copyright holder";
			this.labCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.Gainsboro;
			this.panel1.Location = new System.Drawing.Point(98, 208);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(345, 1);
			this.panel1.TabIndex = 7;
			// 
			// SplashForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(446, 233);
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.labCopyright);
			this.Controls.Add(this.labelEdition);
			this.Controls.Add(this.labConfiguration);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.pictureBox1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.Name = "SplashForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.SplashForm_Load);
			this.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.SplashForm_ControlAdded);
			this.Click += new System.EventHandler(this.SplashForm_Click);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SplashForm_KeyPress);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label labConfiguration;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label labelEdition;
		private System.Windows.Forms.Label labCopyright;
		private System.Windows.Forms.Panel panel1;
	}
}