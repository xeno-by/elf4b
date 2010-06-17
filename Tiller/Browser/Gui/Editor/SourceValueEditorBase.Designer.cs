namespace Browser.Gui.Editor
{
	partial class SourceValueEditorBase
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceValueEditorBase));
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.labelName = new System.Windows.Forms.Label();
			this.buttonFromRepository = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(438, 212);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 8;
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Enabled = false;
			this.buttonOk.Location = new System.Drawing.Point(357, 212);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 7;
			this.buttonOk.Text = "Далее";
			this.buttonOk.UseVisualStyleBackColor = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Browser.Properties.Resources._15;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(79, 79);
			this.pictureBox1.TabIndex = 9;
			this.pictureBox1.TabStop = false;
			// 
			// labelName
			// 
			this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelName.Location = new System.Drawing.Point(115, 39);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(398, 30);
			this.labelName.TabIndex = 10;
			this.labelName.Text = "Имя переменной";
			// 
			// buttonFromRepository
			// 
			this.buttonFromRepository.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonFromRepository.Location = new System.Drawing.Point(12, 212);
			this.buttonFromRepository.Name = "buttonFromRepository";
			this.buttonFromRepository.Size = new System.Drawing.Size(129, 23);
			this.buttonFromRepository.TabIndex = 11;
			this.buttonFromRepository.Text = "Из Репозитория";
			this.buttonFromRepository.UseVisualStyleBackColor = true;
			this.buttonFromRepository.Click += new System.EventHandler(this.buttonFromRepository_Click);
			// 
			// SourceValueEditorBase
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(525, 247);
			this.Controls.Add(this.buttonFromRepository);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SourceValueEditorBase";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Редактор Исходных Данных";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		protected System.Windows.Forms.Button buttonCancel;
		protected System.Windows.Forms.Button buttonOk;
		protected System.Windows.Forms.Label labelName;
		protected System.Windows.Forms.Button buttonFromRepository;
	}
}