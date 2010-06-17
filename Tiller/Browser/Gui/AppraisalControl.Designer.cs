namespace Browser.Gui
{
//	partial class AppraisalControl
//	{
//		/// <summary> 
//		/// Required designer variable.
//		/// </summary>
//		private System.ComponentModel.IContainer components = null;
//
//		/// <summary> 
//		/// Clean up any resources being used.
//		/// </summary>
//		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//		protected override void Dispose(bool disposing)
//		{
//			if (disposing && (components != null))
//			{
//				components.Dispose();
//			}
//			base.Dispose(disposing);
//		}
//
//		#region Component Designer generated code
//
//		/// <summary> 
//		/// Required method for Designer support - do not modify 
//		/// the contents of this method with the code editor.
//		/// </summary>
//		private void InitializeComponent()
//		{
//			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
//			this.treeAppraisal = new System.Windows.Forms.TreeView();
//			this.tabControl1 = new System.Windows.Forms.TabControl();
//			this.tabData = new System.Windows.Forms.TabPage();
//			this.listSourceData = new System.Windows.Forms.ListView();
//			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
//			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
//			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
//			this.tabTemplate = new System.Windows.Forms.TabPage();
//			this.webBrowser = new System.Windows.Forms.WebBrowser();
//			this.splitContainer1.Panel1.SuspendLayout();
//			this.splitContainer1.Panel2.SuspendLayout();
//			this.splitContainer1.SuspendLayout();
//			this.tabControl1.SuspendLayout();
//			this.tabData.SuspendLayout();
//			this.tabTemplate.SuspendLayout();
//			this.SuspendLayout();
//			// 
//			// splitContainer1
//			// 
//			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
//			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
//			this.splitContainer1.Name = "splitContainer1";
//			// 
//			// splitContainer1.Panel1
//			// 
//			this.splitContainer1.Panel1.Controls.Add(this.treeAppraisal);
//			// 
//			// splitContainer1.Panel2
//			// 
//			this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
//			this.splitContainer1.Size = new System.Drawing.Size(631, 526);
//			this.splitContainer1.SplitterDistance = 210;
//			this.splitContainer1.TabIndex = 0;
//			// 
//			// treeAppraisal
//			// 
//			this.treeAppraisal.CheckBoxes = true;
//			this.treeAppraisal.Dock = System.Windows.Forms.DockStyle.Fill;
//			this.treeAppraisal.FullRowSelect = true;
//			this.treeAppraisal.HideSelection = false;
//			this.treeAppraisal.Location = new System.Drawing.Point(0, 0);
//			this.treeAppraisal.Name = "treeAppraisal";
//			this.treeAppraisal.Size = new System.Drawing.Size(210, 526);
//			this.treeAppraisal.TabIndex = 0;
//			this.treeAppraisal.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeAppraisal_AfterCheck);
//			this.treeAppraisal.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeAppraisal_AfterSelect);
//			this.treeAppraisal.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeAppraisal_BeforeCheck);
//			// 
//			// tabControl1
//			// 
//			this.tabControl1.Controls.Add(this.tabData);
//			this.tabControl1.Controls.Add(this.tabTemplate);
//			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//			this.tabControl1.Location = new System.Drawing.Point(0, 0);
//			this.tabControl1.Name = "tabControl1";
//			this.tabControl1.SelectedIndex = 0;
//			this.tabControl1.Size = new System.Drawing.Size(417, 526);
//			this.tabControl1.TabIndex = 0;
//			this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
//			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
//			// 
//			// tabData
//			// 
//			this.tabData.Controls.Add(this.listSourceData);
//			this.tabData.Location = new System.Drawing.Point(4, 22);
//			this.tabData.Name = "tabData";
//			this.tabData.Padding = new System.Windows.Forms.Padding(3);
//			this.tabData.Size = new System.Drawing.Size(409, 500);
//			this.tabData.TabIndex = 0;
//			this.tabData.Text = "Исходные Данные";
//			this.tabData.UseVisualStyleBackColor = true;
//			// 
//			// listSourceData
//			// 
//			this.listSourceData.Activation = System.Windows.Forms.ItemActivation.OneClick;
//			this.listSourceData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
//            this.columnHeader1,
//            this.columnHeader2,
//            this.columnHeader3});
//			this.listSourceData.Dock = System.Windows.Forms.DockStyle.Fill;
//			this.listSourceData.FullRowSelect = true;
//			this.listSourceData.GridLines = true;
//			this.listSourceData.HideSelection = false;
//			this.listSourceData.HotTracking = true;
//			this.listSourceData.HoverSelection = true;
//			this.listSourceData.Location = new System.Drawing.Point(3, 3);
//			this.listSourceData.Name = "listSourceData";
//			this.listSourceData.Size = new System.Drawing.Size(403, 494);
//			this.listSourceData.TabIndex = 0;
//			this.listSourceData.UseCompatibleStateImageBehavior = false;
//			this.listSourceData.View = System.Windows.Forms.View.Details;
//			this.listSourceData.DoubleClick += new System.EventHandler(this.listSourceData_DoubleClick);
//			// 
//			// columnHeader1
//			// 
//			this.columnHeader1.Text = "Имя";
//			this.columnHeader1.Width = 250;
//			// 
//			// columnHeader2
//			// 
//			this.columnHeader2.Text = "Единица измерения";
//			this.columnHeader2.Width = 130;
//			// 
//			// columnHeader3
//			// 
//			this.columnHeader3.Text = "Значение";
//			this.columnHeader3.Width = 200;
//			// 
//			// tabTemplate
//			// 
//			this.tabTemplate.Controls.Add(this.webBrowser);
//			this.tabTemplate.Location = new System.Drawing.Point(4, 22);
//			this.tabTemplate.Name = "tabTemplate";
//			this.tabTemplate.Padding = new System.Windows.Forms.Padding(3);
//			this.tabTemplate.Size = new System.Drawing.Size(409, 500);
//			this.tabTemplate.TabIndex = 1;
//			this.tabTemplate.Text = "Отчет";
//			this.tabTemplate.UseVisualStyleBackColor = true;
//			// 
//			// webBrowser
//			// 
//			this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
//			this.webBrowser.Location = new System.Drawing.Point(3, 3);
//			this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
//			this.webBrowser.Name = "webBrowser";
//			this.webBrowser.Size = new System.Drawing.Size(403, 494);
//			this.webBrowser.TabIndex = 1;
//			// 
//			// AppraisalControl
//			// 
//			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//			this.Controls.Add(this.splitContainer1);
//			this.Name = "AppraisalControl";
//			this.Size = new System.Drawing.Size(631, 526);
//			this.Load += new System.EventHandler(this.AppraisalControl_Load);
//			this.splitContainer1.Panel1.ResumeLayout(false);
//			this.splitContainer1.Panel2.ResumeLayout(false);
//			this.splitContainer1.ResumeLayout(false);
//			this.tabControl1.ResumeLayout(false);
//			this.tabData.ResumeLayout(false);
//			this.tabTemplate.ResumeLayout(false);
//			this.ResumeLayout(false);
//
//		}
//
//		#endregion
//
//		private System.Windows.Forms.SplitContainer splitContainer1;
//		private System.Windows.Forms.TabControl tabControl1;
//		private System.Windows.Forms.TabPage tabData;
//		private System.Windows.Forms.TabPage tabTemplate;
//		private System.Windows.Forms.ListView listSourceData;
//		private System.Windows.Forms.ColumnHeader columnHeader1;
//		private System.Windows.Forms.ColumnHeader columnHeader2;
//		private System.Windows.Forms.ColumnHeader columnHeader3;
//		private System.Windows.Forms.WebBrowser webBrowser;
//		internal System.Windows.Forms.TreeView treeAppraisal;
//	}
}
