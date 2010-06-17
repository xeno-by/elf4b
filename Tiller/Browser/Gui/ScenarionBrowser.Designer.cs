namespace Browser.Gui
{
    partial class ScenarionBrowser
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
            this.panel1 = new System.Windows.Forms.Panel();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeScenario = new System.Windows.Forms.TreeView();
            this.tabControlBranchOptions = new System.Windows.Forms.TabControl();
            this.sourceDataTab = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.listSourceValues = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.propertyGridSourceValue = new System.Windows.Forms.PropertyGrid();
            this.tabFormulas = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.listDeclarations = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.textFormulaView = new System.Windows.Forms.TextBox();
            this._emptySelectionTipRtb = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlBranchOptions.SuspendLayout();
            this.sourceDataTab.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabFormulas.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._okButton);
            this.panel1.Controls.Add(this._cancelButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 427);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(916, 35);
            this.panel1.TabIndex = 2;
            // 
            // _okButton
            // 
            this._okButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._okButton.Enabled = false;
            this._okButton.Location = new System.Drawing.Point(761, 5);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 25);
            this._okButton.TabIndex = 2;
            this._okButton.Text = "Выбрать";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._cancelButton.Location = new System.Drawing.Point(836, 5);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 25);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Отмена";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeScenario);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlBranchOptions);
            this.splitContainer1.Panel2.Controls.Add(this._emptySelectionTipRtb);
            this.splitContainer1.Size = new System.Drawing.Size(916, 427);
            this.splitContainer1.SplitterDistance = 230;
            this.splitContainer1.TabIndex = 3;
            // 
            // treeScenario
            // 
            this.treeScenario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeScenario.FullRowSelect = true;
            this.treeScenario.HideSelection = false;
            this.treeScenario.HotTracking = true;
            this.treeScenario.LabelEdit = true;
            this.treeScenario.Location = new System.Drawing.Point(0, 0);
            this.treeScenario.Name = "treeScenario";
            this.treeScenario.Size = new System.Drawing.Size(230, 427);
            this.treeScenario.TabIndex = 2;
            this.treeScenario.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeScenario_MouseDoubleClick);
            this.treeScenario.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeScenario_AfterSelect);
            // 
            // tabControlBranchOptions
            // 
            this.tabControlBranchOptions.Controls.Add(this.sourceDataTab);
            this.tabControlBranchOptions.Controls.Add(this.tabFormulas);
            this.tabControlBranchOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlBranchOptions.Location = new System.Drawing.Point(0, 0);
            this.tabControlBranchOptions.Name = "tabControlBranchOptions";
            this.tabControlBranchOptions.SelectedIndex = 0;
            this.tabControlBranchOptions.Size = new System.Drawing.Size(682, 427);
            this.tabControlBranchOptions.TabIndex = 5;
            this.tabControlBranchOptions.Visible = false;
            this.tabControlBranchOptions.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlBranchOptions_Selected);
            // 
            // sourceDataTab
            // 
            this.sourceDataTab.Controls.Add(this.splitContainer3);
            this.sourceDataTab.Location = new System.Drawing.Point(4, 22);
            this.sourceDataTab.Name = "sourceDataTab";
            this.sourceDataTab.Padding = new System.Windows.Forms.Padding(3);
            this.sourceDataTab.Size = new System.Drawing.Size(674, 401);
            this.sourceDataTab.TabIndex = 0;
            this.sourceDataTab.Text = "Исходные Данные";
            this.sourceDataTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.listSourceValues);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.propertyGridSourceValue);
            this.splitContainer3.Size = new System.Drawing.Size(668, 395);
            this.splitContainer3.SplitterDistance = 405;
            this.splitContainer3.TabIndex = 0;
            // 
            // listSourceValues
            // 
            this.listSourceValues.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listSourceValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listSourceValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSourceValues.FullRowSelect = true;
            this.listSourceValues.GridLines = true;
            this.listSourceValues.HideSelection = false;
            this.listSourceValues.HotTracking = true;
            this.listSourceValues.HoverSelection = true;
            this.listSourceValues.Location = new System.Drawing.Point(0, 0);
            this.listSourceValues.Name = "listSourceValues";
            this.listSourceValues.Size = new System.Drawing.Size(405, 395);
            this.listSourceValues.TabIndex = 0;
            this.listSourceValues.UseCompatibleStateImageBehavior = false;
            this.listSourceValues.View = System.Windows.Forms.View.Details;
            this.listSourceValues.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listSourceValues_MouseDoubleClick);
            this.listSourceValues.SelectedIndexChanged += new System.EventHandler(this.listSourceValues_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Имя";
            this.columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Тип";
            this.columnHeader2.Width = 100;
            // 
            // propertyGridSourceValue
            // 
            this.propertyGridSourceValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridSourceValue.Location = new System.Drawing.Point(0, 0);
            this.propertyGridSourceValue.Name = "propertyGridSourceValue";
            this.propertyGridSourceValue.Size = new System.Drawing.Size(259, 395);
            this.propertyGridSourceValue.TabIndex = 0;
            // 
            // tabFormulas
            // 
            this.tabFormulas.Controls.Add(this.splitContainer4);
            this.tabFormulas.Location = new System.Drawing.Point(4, 22);
            this.tabFormulas.Name = "tabFormulas";
            this.tabFormulas.Padding = new System.Windows.Forms.Padding(3);
            this.tabFormulas.Size = new System.Drawing.Size(674, 401);
            this.tabFormulas.TabIndex = 2;
            this.tabFormulas.Text = "Формулы";
            this.tabFormulas.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.listDeclarations);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.textFormulaView);
            this.splitContainer4.Size = new System.Drawing.Size(668, 395);
            this.splitContainer4.SplitterDistance = 305;
            this.splitContainer4.TabIndex = 0;
            // 
            // listDeclarations
            // 
            this.listDeclarations.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listDeclarations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.listDeclarations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listDeclarations.FullRowSelect = true;
            this.listDeclarations.GridLines = true;
            this.listDeclarations.HideSelection = false;
            this.listDeclarations.HotTracking = true;
            this.listDeclarations.HoverSelection = true;
            this.listDeclarations.Location = new System.Drawing.Point(0, 0);
            this.listDeclarations.Name = "listDeclarations";
            this.listDeclarations.Size = new System.Drawing.Size(668, 305);
            this.listDeclarations.TabIndex = 1;
            this.listDeclarations.UseCompatibleStateImageBehavior = false;
            this.listDeclarations.View = System.Windows.Forms.View.Details;
            this.listDeclarations.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listDeclarations_MouseDoubleClick);
            this.listDeclarations.SelectedIndexChanged += new System.EventHandler(this.listDeclarations_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Имя";
            this.columnHeader3.Width = 300;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Тип";
            this.columnHeader4.Width = 100;
            // 
            // textFormulaView
            // 
            this.textFormulaView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textFormulaView.Location = new System.Drawing.Point(0, 0);
            this.textFormulaView.Multiline = true;
            this.textFormulaView.Name = "textFormulaView";
            this.textFormulaView.ReadOnly = true;
            this.textFormulaView.Size = new System.Drawing.Size(668, 86);
            this.textFormulaView.TabIndex = 0;
            // 
            // _emptySelectionTipRtb
            // 
            this._emptySelectionTipRtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._emptySelectionTipRtb.Dock = System.Windows.Forms.DockStyle.Fill;
            this._emptySelectionTipRtb.Location = new System.Drawing.Point(0, 0);
            this._emptySelectionTipRtb.Name = "_emptySelectionTipRtb";
            this._emptySelectionTipRtb.ReadOnly = true;
            this._emptySelectionTipRtb.Size = new System.Drawing.Size(682, 427);
            this._emptySelectionTipRtb.TabIndex = 4;
            this._emptySelectionTipRtb.Text = "Для выбора исходных значений или формул, щелкните по одному из неслужебных узлов " +
                "дерева слева.";
            // 
            // ScenarionBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 462);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "ScenarionBrowser";
            this.Text = "Навигатор сценария";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScenarionBrowser_KeyDown);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControlBranchOptions.ResumeLayout(false);
            this.sourceDataTab.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.tabFormulas.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeScenario;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.TabControl tabControlBranchOptions;
        private System.Windows.Forms.TabPage sourceDataTab;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ListView listSourceValues;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.PropertyGrid propertyGridSourceValue;
        private System.Windows.Forms.TabPage tabFormulas;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ListView listDeclarations;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox textFormulaView;
        private System.Windows.Forms.RichTextBox _emptySelectionTipRtb;

    }
}