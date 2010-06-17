namespace ObjectMeet.Tiller.Gui
{
	partial class ScenarioBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenarioBox));
			this.label1 = new Telerik.WinControls.UI.RadTabStrip();
			this.tabItem3 = new Telerik.WinControls.UI.TabItem();
			this.tabItem4 = new Telerik.WinControls.UI.TabItem();
			this.tabItem5 = new Telerik.WinControls.UI.TabItem();
			this.tabItem6 = new Telerik.WinControls.UI.TabItem();
			this.webBrowser = new System.Windows.Forms.WebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			this.label1.SuspendLayout();
			this.tabItem3.ContentPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.label1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(254)))));
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.tabItem3,
            this.tabItem4,
            this.tabItem5,
            this.tabItem6});
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.ScrollOffsetStep = 5;
			this.label1.Size = new System.Drawing.Size(647, 453);
			this.label1.TabIndex = 1;
			this.label1.TabScrollButtonsPosition = Telerik.WinControls.UI.TabScrollButtonsPosition.RightBottom;
			this.label1.Text = "label1";
			// 
			// tabItem3
			// 
			this.tabItem3.Alignment = System.Drawing.ContentAlignment.BottomLeft;
			this.tabItem3.CanFocus = true;
			this.tabItem3.Class = "TabItem";
			// 
			// tabItem3.ContentPanel
			// 
			this.tabItem3.ContentPanel.BackColor = System.Drawing.Color.Transparent;
			this.tabItem3.ContentPanel.CausesValidation = true;
			this.tabItem3.ContentPanel.Controls.Add(this.webBrowser);
			this.tabItem3.IsSelected = true;
			this.tabItem3.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this.tabItem3.Name = "tabItem3";
			this.tabItem3.StretchHorizontally = false;
			this.tabItem3.Text = "Шаблон";
			// 
			// tabItem4
			// 
			this.tabItem4.Alignment = System.Drawing.ContentAlignment.BottomLeft;
			this.tabItem4.CanFocus = true;
			this.tabItem4.Class = "TabItem";
			// 
			// tabItem4.ContentPanel
			// 
			this.tabItem4.ContentPanel.BackColor = System.Drawing.Color.Transparent;
			this.tabItem4.ContentPanel.CausesValidation = true;
			this.tabItem4.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this.tabItem4.Name = "tabItem4";
			this.tabItem4.StretchHorizontally = false;
			this.tabItem4.Text = "Исходные Данные";
			// 
			// tabItem5
			// 
			this.tabItem5.Alignment = System.Drawing.ContentAlignment.BottomLeft;
			this.tabItem5.CanFocus = true;
			this.tabItem5.Class = "TabItem";
			// 
			// tabItem5.ContentPanel
			// 
			this.tabItem5.ContentPanel.BackColor = System.Drawing.Color.Transparent;
			this.tabItem5.ContentPanel.CausesValidation = true;
			this.tabItem5.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this.tabItem5.Name = "tabItem5";
			this.tabItem5.StretchHorizontally = false;
			this.tabItem5.Text = "Формулы";
			// 
			// tabItem6
			// 
			this.tabItem6.Alignment = System.Drawing.ContentAlignment.BottomLeft;
			this.tabItem6.CanFocus = true;
			this.tabItem6.Class = "TabItem";
			// 
			// tabItem6.ContentPanel
			// 
			this.tabItem6.ContentPanel.BackColor = System.Drawing.Color.Transparent;
			this.tabItem6.ContentPanel.CausesValidation = true;
			this.tabItem6.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this.tabItem6.Name = "tabItem6";
			this.tabItem6.StretchHorizontally = false;
			this.tabItem6.Text = "Глоссарий";
			// 
			// webBrowser
			// 
			this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser.Location = new System.Drawing.Point(0, 0);
			this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.Size = new System.Drawing.Size(645, 430);
			this.webBrowser.TabIndex = 0;
			// 
			// ScenarioBox
			// 
			this.BackColor = System.Drawing.Color.Transparent;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.Controls.Add(this.label1);
			this.DockState = Telerik.WinControls.Docking.DockState.TabbedDocument;
			this.DropDownButtonVisible = false;
			this.Image = ((System.Drawing.Image)(resources.GetObject("$this.Image")));
			this.Name = "ScenarioBox";
			this.PreferredDockSize = new System.Drawing.Size(647, 453);
			this.Size = new System.Drawing.Size(647, 453);
			this.Text = "ScenarioBox";
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			this.label1.ResumeLayout(false);
			this.tabItem3.ContentPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Telerik.WinControls.UI.RadTabStrip label1;
		private Telerik.WinControls.UI.TabItem tabItem3;
		private Telerik.WinControls.UI.TabItem tabItem4;
		private Telerik.WinControls.UI.TabItem tabItem5;
		private Telerik.WinControls.UI.TabItem tabItem6;
		private System.Windows.Forms.WebBrowser webBrowser;

	}
}