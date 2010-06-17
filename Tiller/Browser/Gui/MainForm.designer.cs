namespace Browser.Gui
{
	partial class MainForm
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
			System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabelMemoryUsed = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemNew = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemNewAppraisal = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemNewScenario = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSave = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBreak001 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemService = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemServiceRepositoryEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsSlowPreview = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.menuWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.openScenarioDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveAsDialog = new System.Windows.Forms.SaveFileDialog();
			this.menuItemServiceDocumentConverter = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.statusStrip.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripMenuItem1
			// 
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(143, 6);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripStatusLabelMemoryUsed});
			this.statusStrip.Location = new System.Drawing.Point(0, 481);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(683, 22);
			this.statusStrip.TabIndex = 0;
			this.statusStrip.Text = "statusStrip1";
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new System.Drawing.Size(638, 17);
			this.toolStripStatusLabel.Spring = true;
			this.toolStripStatusLabel.Text = "Готово";
			this.toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripStatusLabelMemoryUsed
			// 
			this.toolStripStatusLabelMemoryUsed.Name = "toolStripStatusLabelMemoryUsed";
			this.toolStripStatusLabelMemoryUsed.Size = new System.Drawing.Size(30, 17);
			this.toolStripStatusLabelMemoryUsed.Text = "0 Mb";
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuItemService,
            this.menuWindow});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(683, 24);
			this.menuStrip.TabIndex = 1;
			this.menuStrip.Text = "menuStrip1";
			// 
			// menuFile
			// 
			this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemNew,
            this.menuItemOpen,
            this.menuItemSave,
            this.menuBreak001,
            this.menuItemProperties,
            toolStripMenuItem1,
            this.menuItemExit});
			this.menuFile.Name = "menuFile";
			this.menuFile.Size = new System.Drawing.Size(45, 20);
			this.menuFile.Text = "Файл";
			this.menuFile.DropDownOpening += new System.EventHandler(this.menuFile_DropDownOpening);
			// 
			// menuItemNew
			// 
			this.menuItemNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemNewAppraisal,
            this.menuItemNewScenario});
			this.menuItemNew.Name = "menuItemNew";
			this.menuItemNew.Size = new System.Drawing.Size(146, 22);
			this.menuItemNew.Text = "Новый";
			// 
			// menuItemNewAppraisal
			// 
			this.menuItemNewAppraisal.Image = global::Browser.Properties.Resources._1;
			this.menuItemNewAppraisal.Name = "menuItemNewAppraisal";
			this.menuItemNewAppraisal.Size = new System.Drawing.Size(186, 22);
			this.menuItemNewAppraisal.Text = "Отчет об оценке ...";
			this.menuItemNewAppraisal.Click += new System.EventHandler(this.menuItemNewAppraisal_Click);
			// 
			// menuItemNewScenario
			// 
			this.menuItemNewScenario.Image = global::Browser.Properties.Resources._3;
			this.menuItemNewScenario.Name = "menuItemNewScenario";
			this.menuItemNewScenario.Size = new System.Drawing.Size(186, 22);
			this.menuItemNewScenario.Text = "Сценарий";
			this.menuItemNewScenario.Click += new System.EventHandler(this.menuItemNewScenario_Click);
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.Image = global::Browser.Properties.Resources._119;
			this.menuItemOpen.Name = "menuItemOpen";
			this.menuItemOpen.Size = new System.Drawing.Size(146, 22);
			this.menuItemOpen.Text = "Открыть ...";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
			// 
			// menuItemSave
			// 
			this.menuItemSave.Image = global::Browser.Properties.Resources._7;
			this.menuItemSave.Name = "menuItemSave";
			this.menuItemSave.Size = new System.Drawing.Size(146, 22);
			this.menuItemSave.Text = "Сохранить";
			this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
			// 
			// menuBreak001
			// 
			this.menuBreak001.Name = "menuBreak001";
			this.menuBreak001.Size = new System.Drawing.Size(143, 6);
			// 
			// menuItemProperties
			// 
			this.menuItemProperties.Enabled = false;
			this.menuItemProperties.Name = "menuItemProperties";
			this.menuItemProperties.Size = new System.Drawing.Size(146, 22);
			this.menuItemProperties.Text = "Свойства";
			this.menuItemProperties.Click += new System.EventHandler(this.menuItemProperties_Click);
			// 
			// menuItemExit
			// 
			this.menuItemExit.Name = "menuItemExit";
			this.menuItemExit.Size = new System.Drawing.Size(146, 22);
			this.menuItemExit.Text = "Выход";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// menuItemService
			// 
			this.menuItemService.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemServiceRepositoryEditor,
            this.menuItemOptions,
            this.menuItemServiceDocumentConverter,
            this.toolStripMenuItem2,
            this.menuItemAbout});
			this.menuItemService.Name = "menuItemService";
			this.menuItemService.Size = new System.Drawing.Size(55, 20);
			this.menuItemService.Text = "Сервис";
			// 
			// menuItemServiceRepositoryEditor
			// 
			this.menuItemServiceRepositoryEditor.Image = global::Browser.Properties.Resources._36;
			this.menuItemServiceRepositoryEditor.Name = "menuItemServiceRepositoryEditor";
			this.menuItemServiceRepositoryEditor.Size = new System.Drawing.Size(213, 22);
			this.menuItemServiceRepositoryEditor.Text = "Редактор Репозитория";
			this.menuItemServiceRepositoryEditor.Click += new System.EventHandler(this.menuItemServiceRepositoryEditor_Click);
			// 
			// menuItemOptions
			// 
			this.menuItemOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsSlowPreview});
			this.menuItemOptions.Name = "menuItemOptions";
			this.menuItemOptions.Size = new System.Drawing.Size(213, 22);
			this.menuItemOptions.Text = "Параметры";
			this.menuItemOptions.Visible = false;
			// 
			// optionsSlowPreview
			// 
			this.optionsSlowPreview.Checked = true;
			this.optionsSlowPreview.CheckOnClick = true;
			this.optionsSlowPreview.CheckState = System.Windows.Forms.CheckState.Checked;
			this.optionsSlowPreview.Name = "optionsSlowPreview";
			this.optionsSlowPreview.Size = new System.Drawing.Size(186, 22);
			this.optionsSlowPreview.Text = "Точный предосмотр";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(210, 6);
			// 
			// menuItemAbout
			// 
			this.menuItemAbout.Name = "menuItemAbout";
			this.menuItemAbout.Size = new System.Drawing.Size(213, 22);
			this.menuItemAbout.Text = "О программе ...";
			this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
			// 
			// menuWindow
			// 
			this.menuWindow.Name = "menuWindow";
			this.menuWindow.Size = new System.Drawing.Size(45, 20);
			this.menuWindow.Text = "Окна";
			this.menuWindow.Visible = false;
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "appraisal";
			this.openFileDialog.Filter = "Сценарии|*.scenario|Отчеты|*.appraisal";
			this.openFileDialog.RestoreDirectory = true;
			this.openFileDialog.SupportMultiDottedExtensions = true;
			this.openFileDialog.Title = "Открыть файл отчета либо сценария";
			// 
			// openScenarioDialog
			// 
			this.openScenarioDialog.AddExtension = false;
			this.openScenarioDialog.DefaultExt = "scenario";
			this.openScenarioDialog.Filter = "Сценарии|*.scenario";
			this.openScenarioDialog.RestoreDirectory = true;
			this.openScenarioDialog.SupportMultiDottedExtensions = true;
			// 
			// saveAsDialog
			// 
			this.saveAsDialog.AddExtension = false;
			this.saveAsDialog.DefaultExt = "scenario";
			this.saveAsDialog.Filter = "Сценарии|*.scenario";
			this.saveAsDialog.RestoreDirectory = true;
			this.saveAsDialog.SupportMultiDottedExtensions = true;
			// 
			// menuItemServiceDocumentConverter
			// 
			this.menuItemServiceDocumentConverter.Name = "menuItemServiceDocumentConverter";
			this.menuItemServiceDocumentConverter.Size = new System.Drawing.Size(213, 22);
			this.menuItemServiceDocumentConverter.Text = "Конвертор Сценариев ...";
			this.menuItemServiceDocumentConverter.Click += new System.EventHandler(this.menuItemServiceDocumentConverter_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(683, 503);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "Application title";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem menuFile;
		private System.Windows.Forms.ToolStripMenuItem menuItemExit;
		private System.Windows.Forms.ToolStripMenuItem menuItemSave;
		private System.Windows.Forms.ToolStripMenuItem menuItemOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemNew;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ToolStripMenuItem menuWindow;
		private System.Windows.Forms.ToolStripMenuItem menuItemNewAppraisal;
		private System.Windows.Forms.ToolStripMenuItem menuItemNewScenario;
		private System.Windows.Forms.ToolStripMenuItem menuItemService;
		private System.Windows.Forms.ToolStripMenuItem menuItemServiceRepositoryEditor;
		private System.Windows.Forms.OpenFileDialog openScenarioDialog;
		private System.Windows.Forms.SaveFileDialog saveAsDialog;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
				private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
				private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
				private System.Windows.Forms.ToolStripSeparator menuBreak001;
				private System.Windows.Forms.ToolStripMenuItem menuItemProperties;
				private System.Windows.Forms.ToolStripMenuItem menuItemOptions;
				internal System.Windows.Forms.ToolStripMenuItem optionsSlowPreview;
				private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMemoryUsed;
				private System.Windows.Forms.ToolStripMenuItem menuItemServiceDocumentConverter;
	}
}