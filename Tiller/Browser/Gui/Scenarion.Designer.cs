namespace Browser.Gui
{
	using Microsoft.ConsultingServices.HtmlEditor;

	partial class Scenarion
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Scenarion));
			this.tabScenarioTest = new System.Windows.Forms.TabControl();
			this.tabScenarioEditor = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.treeScenario = new System.Windows.Forms.TreeView();
			this.contextMenuTreeScenario = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.createBranchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteBranchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertyGridScenarioNode = new System.Windows.Forms.PropertyGrid();
			this.tabControlBranchOptions = new System.Windows.Forms.TabControl();
			this.sourceDataTab = new System.Windows.Forms.TabPage();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.listSourceValues = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuSourceValues = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemSourceDataDeclarationCreateString = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSourceDataDeclarationCreateText = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSourceDataDeclarationCreateNumber = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSourceDataDeclarationCreatePercent = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSourceDataDeclarationCreateDate = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSourceDataDeclarationCreateCurrency = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemSourceDataDeclarationEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSourceValueChangeType = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSourceValueChangeTypeToString = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSourceValueChangeTypeToText = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSourceValueChangeTypeToNumber = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSourceValueChangeTypeToPercent = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSourceValueChangeTypeToDateTime = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSourceValueChangeTypeToCurrency = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemSourceDataDeclarationDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemSourceDataDeclarationCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.propertyGridSourceValue = new System.Windows.Forms.PropertyGrid();
			this.templateTab = new System.Windows.Forms.TabPage();
			this.templateEditor = new Microsoft.ConsultingServices.HtmlEditor.HtmlEditorControl();
			this.tabFormulas = new System.Windows.Forms.TabPage();
			this.splitContainer4 = new System.Windows.Forms.SplitContainer();
			this.listDeclarations = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuDeclarations = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemDeclarationCreateCurrency = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemDeclarationCreateNumber = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemDeclarationCreatePercent = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemDeclarationCreateString = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemDeclarationCreateText = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemDeclarationCreateDate = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemDeclarationEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemDeclarationChangeType = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemDeclarationChangeTypeToCurrency = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemDeclarationChangeTypeToNumber = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemDeclarationChangeTypeToPercent = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemDeclarationDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemDeclarationCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.textFormulaView = new System.Windows.Forms.TextBox();
			this.tabConditions = new System.Windows.Forms.TabPage();
			this.splitContainer5 = new System.Windows.Forms.SplitContainer();
			this.listConditions = new System.Windows.Forms.ListView();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuConditions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.еслиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.узелВыбранToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemChildShouldBeSelected = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemDeleteCondition = new System.Windows.Forms.ToolStripMenuItem();
			this.textConditionView = new System.Windows.Forms.TextBox();
			this.previewTab = new System.Windows.Forms.TabPage();
			this.previewBrowser = new System.Windows.Forms.WebBrowser();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.apprasialX = new ObjectMeet.Appearance.Explorer.SimpleTabbedDocument();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuItemReport = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemReportGenerate = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemReportExport = new System.Windows.Forms.ToolStripMenuItem();
			this.tabScenarioTest.SuspendLayout();
			this.tabScenarioEditor.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.contextMenuTreeScenario.SuspendLayout();
			this.tabControlBranchOptions.SuspendLayout();
			this.sourceDataTab.SuspendLayout();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.contextMenuSourceValues.SuspendLayout();
			this.templateTab.SuspendLayout();
			this.tabFormulas.SuspendLayout();
			this.splitContainer4.Panel1.SuspendLayout();
			this.splitContainer4.Panel2.SuspendLayout();
			this.splitContainer4.SuspendLayout();
			this.contextMenuDeclarations.SuspendLayout();
			this.tabConditions.SuspendLayout();
			this.splitContainer5.Panel1.SuspendLayout();
			this.splitContainer5.Panel2.SuspendLayout();
			this.splitContainer5.SuspendLayout();
			this.contextMenuConditions.SuspendLayout();
			this.previewTab.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabScenarioTest
			// 
			this.tabScenarioTest.Controls.Add(this.tabScenarioEditor);
			this.tabScenarioTest.Controls.Add(this.tabPage2);
			this.tabScenarioTest.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabScenarioTest.Location = new System.Drawing.Point(0, 0);
			this.tabScenarioTest.Name = "tabScenarioTest";
			this.tabScenarioTest.SelectedIndex = 0;
			this.tabScenarioTest.Size = new System.Drawing.Size(907, 558);
			this.tabScenarioTest.TabIndex = 0;
			// 
			// tabScenarioEditor
			// 
			this.tabScenarioEditor.Controls.Add(this.splitContainer1);
			this.tabScenarioEditor.Controls.Add(this.toolStrip1);
			this.tabScenarioEditor.Location = new System.Drawing.Point(4, 22);
			this.tabScenarioEditor.Name = "tabScenarioEditor";
			this.tabScenarioEditor.Padding = new System.Windows.Forms.Padding(3);
			this.tabScenarioEditor.Size = new System.Drawing.Size(899, 532);
			this.tabScenarioEditor.TabIndex = 0;
			this.tabScenarioEditor.Text = "Редактор сценария";
			this.tabScenarioEditor.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControlBranchOptions);
			this.splitContainer1.Size = new System.Drawing.Size(893, 526);
			this.splitContainer1.SplitterDistance = 295;
			this.splitContainer1.TabIndex = 1;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.treeScenario);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.propertyGridScenarioNode);
			this.splitContainer2.Size = new System.Drawing.Size(295, 526);
			this.splitContainer2.SplitterDistance = 385;
			this.splitContainer2.TabIndex = 0;
			// 
			// treeScenario
			// 
			this.treeScenario.ContextMenuStrip = this.contextMenuTreeScenario;
			this.treeScenario.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeScenario.FullRowSelect = true;
			this.treeScenario.HideSelection = false;
			this.treeScenario.HotTracking = true;
			this.treeScenario.LabelEdit = true;
			this.treeScenario.Location = new System.Drawing.Point(0, 0);
			this.treeScenario.Name = "treeScenario";
			this.treeScenario.Size = new System.Drawing.Size(295, 385);
			this.treeScenario.TabIndex = 1;
			this.treeScenario.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeScenario_MouseClick);
			this.treeScenario.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeScenario_AfterSelect);
			this.treeScenario.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeScenario_BeforeSelect);
			// 
			// contextMenuTreeScenario
			// 
			this.contextMenuTreeScenario.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createBranchToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteBranchToolStripMenuItem});
			this.contextMenuTreeScenario.Name = "contextMenuTreeScenario";
			this.contextMenuTreeScenario.Size = new System.Drawing.Size(165, 70);
			// 
			// createBranchToolStripMenuItem
			// 
			this.createBranchToolStripMenuItem.Name = "createBranchToolStripMenuItem";
			this.createBranchToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.createBranchToolStripMenuItem.Text = "Создать узел";
			this.createBranchToolStripMenuItem.Click += new System.EventHandler(this.createBranchToolStripMenuItem_Click);
			// 
			// renameToolStripMenuItem
			// 
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.renameToolStripMenuItem.Text = "Переименовать";
			this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
			// 
			// deleteBranchToolStripMenuItem
			// 
			this.deleteBranchToolStripMenuItem.Name = "deleteBranchToolStripMenuItem";
			this.deleteBranchToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.deleteBranchToolStripMenuItem.Text = "Удалить узел";
			this.deleteBranchToolStripMenuItem.Click += new System.EventHandler(this.deleteBranchToolStripMenuItem_Click);
			// 
			// propertyGridScenarioNode
			// 
			this.propertyGridScenarioNode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGridScenarioNode.Location = new System.Drawing.Point(0, 0);
			this.propertyGridScenarioNode.Name = "propertyGridScenarioNode";
			this.propertyGridScenarioNode.Size = new System.Drawing.Size(295, 137);
			this.propertyGridScenarioNode.TabIndex = 0;
			// 
			// tabControlBranchOptions
			// 
			this.tabControlBranchOptions.Controls.Add(this.sourceDataTab);
			this.tabControlBranchOptions.Controls.Add(this.templateTab);
			this.tabControlBranchOptions.Controls.Add(this.tabFormulas);
			this.tabControlBranchOptions.Controls.Add(this.tabConditions);
			this.tabControlBranchOptions.Controls.Add(this.previewTab);
			this.tabControlBranchOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlBranchOptions.Location = new System.Drawing.Point(0, 0);
			this.tabControlBranchOptions.Name = "tabControlBranchOptions";
			this.tabControlBranchOptions.SelectedIndex = 0;
			this.tabControlBranchOptions.Size = new System.Drawing.Size(594, 526);
			this.tabControlBranchOptions.TabIndex = 0;
			this.tabControlBranchOptions.Visible = false;
			this.tabControlBranchOptions.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControlBranchOptions_Selecting);
			this.tabControlBranchOptions.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlBranchOptions_Selected);
			// 
			// sourceDataTab
			// 
			this.sourceDataTab.Controls.Add(this.splitContainer3);
			this.sourceDataTab.Location = new System.Drawing.Point(4, 22);
			this.sourceDataTab.Name = "sourceDataTab";
			this.sourceDataTab.Padding = new System.Windows.Forms.Padding(3);
			this.sourceDataTab.Size = new System.Drawing.Size(586, 500);
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
			this.splitContainer3.Size = new System.Drawing.Size(580, 494);
			this.splitContainer3.SplitterDistance = 353;
			this.splitContainer3.TabIndex = 0;
			// 
			// listSourceValues
			// 
			this.listSourceValues.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listSourceValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.listSourceValues.ContextMenuStrip = this.contextMenuSourceValues;
			this.listSourceValues.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listSourceValues.FullRowSelect = true;
			this.listSourceValues.GridLines = true;
			this.listSourceValues.HideSelection = false;
			this.listSourceValues.HotTracking = true;
			this.listSourceValues.HoverSelection = true;
			this.listSourceValues.Location = new System.Drawing.Point(0, 0);
			this.listSourceValues.Name = "listSourceValues";
			this.listSourceValues.Size = new System.Drawing.Size(353, 494);
			this.listSourceValues.TabIndex = 0;
			this.listSourceValues.UseCompatibleStateImageBehavior = false;
			this.listSourceValues.View = System.Windows.Forms.View.Details;
			this.listSourceValues.SelectedIndexChanged += new System.EventHandler(this.listSourceValues_SelectedIndexChanged);
			this.listSourceValues.DoubleClick += new System.EventHandler(this.listSourceValues_DoubleClick);
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
			// contextMenuSourceValues
			// 
			this.contextMenuSourceValues.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSourceDataDeclarationCreateString,
            this.menuItemSourceDataDeclarationCreateText,
            this.menuItemSourceDataDeclarationCreateNumber,
            this.menuItemSourceDataDeclarationCreatePercent,
            this.menuItemSourceDataDeclarationCreateDate,
            this.menuItemSourceDataDeclarationCreateCurrency,
            this.toolStripMenuItem2,
            this.menuItemSourceDataDeclarationEdit,
            this.toolStripMenuItemSourceValueChangeType,
            this.menuItemSourceDataDeclarationDelete,
            this.toolStripMenuItem1,
            this.menuItemSourceDataDeclarationCopy});
			this.contextMenuSourceValues.Name = "contextMenuSourceValues";
			this.contextMenuSourceValues.Size = new System.Drawing.Size(175, 236);
			// 
			// menuItemSourceDataDeclarationCreateString
			// 
			this.menuItemSourceDataDeclarationCreateString.Name = "menuItemSourceDataDeclarationCreateString";
			this.menuItemSourceDataDeclarationCreateString.Size = new System.Drawing.Size(174, 22);
			this.menuItemSourceDataDeclarationCreateString.Text = "Создать Строку";
			this.menuItemSourceDataDeclarationCreateString.Click += new System.EventHandler(this.menuItemSourceDataDeclarationCreateString_Click);
			// 
			// menuItemSourceDataDeclarationCreateText
			// 
			this.menuItemSourceDataDeclarationCreateText.Name = "menuItemSourceDataDeclarationCreateText";
			this.menuItemSourceDataDeclarationCreateText.Size = new System.Drawing.Size(174, 22);
			this.menuItemSourceDataDeclarationCreateText.Text = "Создать Текст";
			this.menuItemSourceDataDeclarationCreateText.Click += new System.EventHandler(this.menuItemSourceDataDeclarationCreateText_Click);
			// 
			// menuItemSourceDataDeclarationCreateNumber
			// 
			this.menuItemSourceDataDeclarationCreateNumber.Name = "menuItemSourceDataDeclarationCreateNumber";
			this.menuItemSourceDataDeclarationCreateNumber.Size = new System.Drawing.Size(174, 22);
			this.menuItemSourceDataDeclarationCreateNumber.Text = "Создать Число";
			this.menuItemSourceDataDeclarationCreateNumber.Click += new System.EventHandler(this.menuItemSourceDataDeclarationCreateNumber_Click);
			// 
			// menuItemSourceDataDeclarationCreatePercent
			// 
			this.menuItemSourceDataDeclarationCreatePercent.Name = "menuItemSourceDataDeclarationCreatePercent";
			this.menuItemSourceDataDeclarationCreatePercent.Size = new System.Drawing.Size(174, 22);
			this.menuItemSourceDataDeclarationCreatePercent.Text = "Создать Процент";
			this.menuItemSourceDataDeclarationCreatePercent.Click += new System.EventHandler(this.menuItemSourceDataDeclarationCreatePercent_Click);
			// 
			// menuItemSourceDataDeclarationCreateDate
			// 
			this.menuItemSourceDataDeclarationCreateDate.Name = "menuItemSourceDataDeclarationCreateDate";
			this.menuItemSourceDataDeclarationCreateDate.Size = new System.Drawing.Size(174, 22);
			this.menuItemSourceDataDeclarationCreateDate.Text = "Создать Дату";
			this.menuItemSourceDataDeclarationCreateDate.Click += new System.EventHandler(this.menuItemSourceDataDeclarationCreateDate_Click);
			// 
			// menuItemSourceDataDeclarationCreateCurrency
			// 
			this.menuItemSourceDataDeclarationCreateCurrency.Name = "menuItemSourceDataDeclarationCreateCurrency";
			this.menuItemSourceDataDeclarationCreateCurrency.Size = new System.Drawing.Size(174, 22);
			this.menuItemSourceDataDeclarationCreateCurrency.Text = "Создать Валюту";
			this.menuItemSourceDataDeclarationCreateCurrency.Click += new System.EventHandler(this.menuItemSourceDataDeclarationCreateCurrency_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(171, 6);
			// 
			// menuItemSourceDataDeclarationEdit
			// 
			this.menuItemSourceDataDeclarationEdit.Name = "menuItemSourceDataDeclarationEdit";
			this.menuItemSourceDataDeclarationEdit.Size = new System.Drawing.Size(174, 22);
			this.menuItemSourceDataDeclarationEdit.Text = "Редактировать";
			this.menuItemSourceDataDeclarationEdit.Click += new System.EventHandler(this.menuItemSourceDataDeclarationEdit_Click);
			// 
			// toolStripMenuItemSourceValueChangeType
			// 
			this.toolStripMenuItemSourceValueChangeType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSourceValueChangeTypeToString,
            this.toolStripMenuItemSourceValueChangeTypeToText,
            this.toolStripMenuItemSourceValueChangeTypeToNumber,
            this.toolStripMenuItemSourceValueChangeTypeToPercent,
            this.toolStripMenuItemSourceValueChangeTypeToDateTime,
            this.toolStripMenuItemSourceValueChangeTypeToCurrency});
			this.toolStripMenuItemSourceValueChangeType.Name = "toolStripMenuItemSourceValueChangeType";
			this.toolStripMenuItemSourceValueChangeType.Size = new System.Drawing.Size(174, 22);
			this.toolStripMenuItemSourceValueChangeType.Text = "Изменить Тип";
			// 
			// toolStripMenuItemSourceValueChangeTypeToString
			// 
			this.toolStripMenuItemSourceValueChangeTypeToString.Name = "toolStripMenuItemSourceValueChangeTypeToString";
			this.toolStripMenuItemSourceValueChangeTypeToString.Size = new System.Drawing.Size(128, 22);
			this.toolStripMenuItemSourceValueChangeTypeToString.Text = "Строка";
			this.toolStripMenuItemSourceValueChangeTypeToString.Click += new System.EventHandler(this.toolStripMenuItemSourceValueChangeTypeToString_Click);
			// 
			// toolStripMenuItemSourceValueChangeTypeToText
			// 
			this.toolStripMenuItemSourceValueChangeTypeToText.Name = "toolStripMenuItemSourceValueChangeTypeToText";
			this.toolStripMenuItemSourceValueChangeTypeToText.Size = new System.Drawing.Size(128, 22);
			this.toolStripMenuItemSourceValueChangeTypeToText.Text = "Текст";
			this.toolStripMenuItemSourceValueChangeTypeToText.Click += new System.EventHandler(this.toolStripMenuItemSourceValueChangeTypeToText_Click);
			// 
			// toolStripMenuItemSourceValueChangeTypeToNumber
			// 
			this.toolStripMenuItemSourceValueChangeTypeToNumber.Name = "toolStripMenuItemSourceValueChangeTypeToNumber";
			this.toolStripMenuItemSourceValueChangeTypeToNumber.Size = new System.Drawing.Size(128, 22);
			this.toolStripMenuItemSourceValueChangeTypeToNumber.Text = "Число";
			this.toolStripMenuItemSourceValueChangeTypeToNumber.Click += new System.EventHandler(this.toolStripMenuItemSourceValueChangeTypeToNumber_Click);
			// 
			// toolStripMenuItemSourceValueChangeTypeToPercent
			// 
			this.toolStripMenuItemSourceValueChangeTypeToPercent.Name = "toolStripMenuItemSourceValueChangeTypeToPercent";
			this.toolStripMenuItemSourceValueChangeTypeToPercent.Size = new System.Drawing.Size(128, 22);
			this.toolStripMenuItemSourceValueChangeTypeToPercent.Text = "Процент";
			this.toolStripMenuItemSourceValueChangeTypeToPercent.Click += new System.EventHandler(this.toolStripMenuItemSourceValueChangeTypeToPercent_Click);
			// 
			// toolStripMenuItemSourceValueChangeTypeToDateTime
			// 
			this.toolStripMenuItemSourceValueChangeTypeToDateTime.Name = "toolStripMenuItemSourceValueChangeTypeToDateTime";
			this.toolStripMenuItemSourceValueChangeTypeToDateTime.Size = new System.Drawing.Size(128, 22);
			this.toolStripMenuItemSourceValueChangeTypeToDateTime.Text = "Дата";
			this.toolStripMenuItemSourceValueChangeTypeToDateTime.Click += new System.EventHandler(this.toolStripMenuItemSourceValueChangeTypeToDateTime_Click);
			// 
			// toolStripMenuItemSourceValueChangeTypeToCurrency
			// 
			this.toolStripMenuItemSourceValueChangeTypeToCurrency.Name = "toolStripMenuItemSourceValueChangeTypeToCurrency";
			this.toolStripMenuItemSourceValueChangeTypeToCurrency.Size = new System.Drawing.Size(128, 22);
			this.toolStripMenuItemSourceValueChangeTypeToCurrency.Text = "Валюта";
			this.toolStripMenuItemSourceValueChangeTypeToCurrency.Click += new System.EventHandler(this.toolStripMenuItemSourceValueChangeTypeToCurrency_Click);
			// 
			// menuItemSourceDataDeclarationDelete
			// 
			this.menuItemSourceDataDeclarationDelete.Name = "menuItemSourceDataDeclarationDelete";
			this.menuItemSourceDataDeclarationDelete.Size = new System.Drawing.Size(174, 22);
			this.menuItemSourceDataDeclarationDelete.Text = "Удалить";
			this.menuItemSourceDataDeclarationDelete.Click += new System.EventHandler(this.menuItemSourceDataDeclarationDelete_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(171, 6);
			// 
			// menuItemSourceDataDeclarationCopy
			// 
			this.menuItemSourceDataDeclarationCopy.Name = "menuItemSourceDataDeclarationCopy";
			this.menuItemSourceDataDeclarationCopy.Size = new System.Drawing.Size(174, 22);
			this.menuItemSourceDataDeclarationCopy.Text = "Копировать";
			this.menuItemSourceDataDeclarationCopy.Click += new System.EventHandler(this.menuItemSourceDataDeclarationCopy_Click);
			// 
			// propertyGridSourceValue
			// 
			this.propertyGridSourceValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGridSourceValue.Location = new System.Drawing.Point(0, 0);
			this.propertyGridSourceValue.Name = "propertyGridSourceValue";
			this.propertyGridSourceValue.Size = new System.Drawing.Size(223, 494);
			this.propertyGridSourceValue.TabIndex = 0;
			// 
			// templateTab
			// 
			this.templateTab.Controls.Add(this.templateEditor);
			this.templateTab.Location = new System.Drawing.Point(4, 22);
			this.templateTab.Name = "templateTab";
			this.templateTab.Padding = new System.Windows.Forms.Padding(3);
			this.templateTab.Size = new System.Drawing.Size(586, 500);
			this.templateTab.TabIndex = 1;
			this.templateTab.Text = "Шаблон Отчета";
			this.templateTab.UseVisualStyleBackColor = true;
			// 
			// templateEditor
			// 
			this.templateEditor.BackColor = System.Drawing.Color.White;
			this.templateEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.templateEditor.InnerText = null;
			this.templateEditor.Location = new System.Drawing.Point(3, 3);
			this.templateEditor.Name = "templateEditor";
			this.templateEditor.Size = new System.Drawing.Size(580, 494);
			this.templateEditor.TabIndex = 1;
			this.templateEditor.ButtonRowSpecialClicked += new System.EventHandler<Microsoft.ConsultingServices.HtmlEditor.HtmlElementEventArgs>(this.templateEditor_ButtonRowSpecialClicked);
			this.templateEditor.HtmlChanged += new System.EventHandler(this.templateEditor_HtmlChanged);
			this.templateEditor.QueryRowIsSpecial += new System.EventHandler<Microsoft.ConsultingServices.HtmlEditor.HtmlElementEventArgs>(this.templateEditor_QueryRowIsSpecial);
			this.templateEditor.QueryRowIsTotal += new System.EventHandler<Microsoft.ConsultingServices.HtmlEditor.HtmlElementEventArgs>(this.templateEditor_QueryRowIsTotal);
			this.templateEditor.ButtonRowTotalClicked += new System.EventHandler<Microsoft.ConsultingServices.HtmlEditor.HtmlElementEventArgs>(this.templateEditor_ButtonRowTotalClicked);
			// 
			// tabFormulas
			// 
			this.tabFormulas.Controls.Add(this.splitContainer4);
			this.tabFormulas.Location = new System.Drawing.Point(4, 22);
			this.tabFormulas.Name = "tabFormulas";
			this.tabFormulas.Padding = new System.Windows.Forms.Padding(3);
			this.tabFormulas.Size = new System.Drawing.Size(586, 500);
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
			this.splitContainer4.Size = new System.Drawing.Size(580, 494);
			this.splitContainer4.SplitterDistance = 384;
			this.splitContainer4.TabIndex = 0;
			// 
			// listDeclarations
			// 
			this.listDeclarations.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listDeclarations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
			this.listDeclarations.ContextMenuStrip = this.contextMenuDeclarations;
			this.listDeclarations.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listDeclarations.FullRowSelect = true;
			this.listDeclarations.GridLines = true;
			this.listDeclarations.HideSelection = false;
			this.listDeclarations.HotTracking = true;
			this.listDeclarations.HoverSelection = true;
			this.listDeclarations.Location = new System.Drawing.Point(0, 0);
			this.listDeclarations.Name = "listDeclarations";
			this.listDeclarations.Size = new System.Drawing.Size(580, 384);
			this.listDeclarations.TabIndex = 1;
			this.listDeclarations.UseCompatibleStateImageBehavior = false;
			this.listDeclarations.View = System.Windows.Forms.View.Details;
			this.listDeclarations.SelectedIndexChanged += new System.EventHandler(this.listDeclarations_SelectedIndexChanged);
			this.listDeclarations.DoubleClick += new System.EventHandler(this.listDeclarations_DoubleClick);
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
			// contextMenuDeclarations
			// 
			this.contextMenuDeclarations.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDeclarationCreateCurrency,
            this.menuItemDeclarationCreateNumber,
            this.menuItemDeclarationCreatePercent,
            this.menuItemDeclarationCreateString,
            this.menuItemDeclarationCreateText,
            this.menuItemDeclarationCreateDate,
            this.toolStripSeparator1,
            this.menuItemDeclarationEdit,
            this.toolStripMenuItemDeclarationChangeType,
            this.menuItemDeclarationDelete,
            this.toolStripSeparator2,
            this.menuItemDeclarationCopy});
			this.contextMenuDeclarations.Name = "contextMenuSourceValues";
			this.contextMenuDeclarations.Size = new System.Drawing.Size(175, 236);
			// 
			// menuItemDeclarationCreateCurrency
			// 
			this.menuItemDeclarationCreateCurrency.Name = "menuItemDeclarationCreateCurrency";
			this.menuItemDeclarationCreateCurrency.Size = new System.Drawing.Size(174, 22);
			this.menuItemDeclarationCreateCurrency.Text = "Создать Валюту";
			this.menuItemDeclarationCreateCurrency.Click += new System.EventHandler(this.menuItemDeclarationCreateCurrency_Click);
			// 
			// menuItemDeclarationCreateNumber
			// 
			this.menuItemDeclarationCreateNumber.Name = "menuItemDeclarationCreateNumber";
			this.menuItemDeclarationCreateNumber.Size = new System.Drawing.Size(174, 22);
			this.menuItemDeclarationCreateNumber.Text = "Создать Число";
			this.menuItemDeclarationCreateNumber.Click += new System.EventHandler(this.menuItemDeclarationCreateNumber_Click);
			// 
			// menuItemDeclarationCreatePercent
			// 
			this.menuItemDeclarationCreatePercent.Name = "menuItemDeclarationCreatePercent";
			this.menuItemDeclarationCreatePercent.Size = new System.Drawing.Size(174, 22);
			this.menuItemDeclarationCreatePercent.Text = "Создать Процент";
			this.menuItemDeclarationCreatePercent.Click += new System.EventHandler(this.menuItemDeclarationCreatePercent_Click);
			// 
			// menuItemDeclarationCreateString
			// 
			this.menuItemDeclarationCreateString.Name = "menuItemDeclarationCreateString";
			this.menuItemDeclarationCreateString.Size = new System.Drawing.Size(174, 22);
			this.menuItemDeclarationCreateString.Text = "Создать Строку";
			this.menuItemDeclarationCreateString.Click += new System.EventHandler(this.menuItemDeclarationCreateString_Click);
			// 
			// menuItemDeclarationCreateText
			// 
			this.menuItemDeclarationCreateText.Name = "menuItemDeclarationCreateText";
			this.menuItemDeclarationCreateText.Size = new System.Drawing.Size(174, 22);
			this.menuItemDeclarationCreateText.Text = "Создать Текст";
			this.menuItemDeclarationCreateText.Visible = false;
			this.menuItemDeclarationCreateText.Click += new System.EventHandler(this.menuItemDeclarationCreateText_Click);
			// 
			// menuItemDeclarationCreateDate
			// 
			this.menuItemDeclarationCreateDate.Name = "menuItemDeclarationCreateDate";
			this.menuItemDeclarationCreateDate.Size = new System.Drawing.Size(174, 22);
			this.menuItemDeclarationCreateDate.Text = "Создать Дату";
			this.menuItemDeclarationCreateDate.Visible = false;
			this.menuItemDeclarationCreateDate.Click += new System.EventHandler(this.menuItemDeclarationCreateDate_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(171, 6);
			// 
			// menuItemDeclarationEdit
			// 
			this.menuItemDeclarationEdit.Name = "menuItemDeclarationEdit";
			this.menuItemDeclarationEdit.Size = new System.Drawing.Size(174, 22);
			this.menuItemDeclarationEdit.Text = "Редактировать";
			this.menuItemDeclarationEdit.Click += new System.EventHandler(this.menuItemDeclarationEdit_Click);
			// 
			// toolStripMenuItemDeclarationChangeType
			// 
			this.toolStripMenuItemDeclarationChangeType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDeclarationChangeTypeToCurrency,
            this.toolStripMenuItemDeclarationChangeTypeToNumber,
            this.toolStripMenuItemDeclarationChangeTypeToPercent});
			this.toolStripMenuItemDeclarationChangeType.Name = "toolStripMenuItemDeclarationChangeType";
			this.toolStripMenuItemDeclarationChangeType.Size = new System.Drawing.Size(174, 22);
			this.toolStripMenuItemDeclarationChangeType.Text = "Изменить Тип";
			// 
			// toolStripMenuItemDeclarationChangeTypeToCurrency
			// 
			this.toolStripMenuItemDeclarationChangeTypeToCurrency.Name = "toolStripMenuItemDeclarationChangeTypeToCurrency";
			this.toolStripMenuItemDeclarationChangeTypeToCurrency.Size = new System.Drawing.Size(128, 22);
			this.toolStripMenuItemDeclarationChangeTypeToCurrency.Text = "Валюта";
			this.toolStripMenuItemDeclarationChangeTypeToCurrency.Click += new System.EventHandler(this.toolStripMenuItemDeclarationChangeTypeToCurrency_Click);
			// 
			// toolStripMenuItemDeclarationChangeTypeToNumber
			// 
			this.toolStripMenuItemDeclarationChangeTypeToNumber.Name = "toolStripMenuItemDeclarationChangeTypeToNumber";
			this.toolStripMenuItemDeclarationChangeTypeToNumber.Size = new System.Drawing.Size(128, 22);
			this.toolStripMenuItemDeclarationChangeTypeToNumber.Text = "Число";
			this.toolStripMenuItemDeclarationChangeTypeToNumber.Click += new System.EventHandler(this.toolStripMenuItemDeclarationChangeTypeToNumber_Click);
			// 
			// toolStripMenuItemDeclarationChangeTypeToPercent
			// 
			this.toolStripMenuItemDeclarationChangeTypeToPercent.Name = "toolStripMenuItemDeclarationChangeTypeToPercent";
			this.toolStripMenuItemDeclarationChangeTypeToPercent.Size = new System.Drawing.Size(128, 22);
			this.toolStripMenuItemDeclarationChangeTypeToPercent.Text = "Процент";
			this.toolStripMenuItemDeclarationChangeTypeToPercent.Click += new System.EventHandler(this.toolStripMenuItemDeclarationChangeTypeToPercent_Click);
			// 
			// menuItemDeclarationDelete
			// 
			this.menuItemDeclarationDelete.Name = "menuItemDeclarationDelete";
			this.menuItemDeclarationDelete.Size = new System.Drawing.Size(174, 22);
			this.menuItemDeclarationDelete.Text = "Удалить";
			this.menuItemDeclarationDelete.Click += new System.EventHandler(this.menuItemDeclarationDelete_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(171, 6);
			// 
			// menuItemDeclarationCopy
			// 
			this.menuItemDeclarationCopy.Name = "menuItemDeclarationCopy";
			this.menuItemDeclarationCopy.Size = new System.Drawing.Size(174, 22);
			this.menuItemDeclarationCopy.Text = "Копировать";
			this.menuItemDeclarationCopy.Click += new System.EventHandler(this.menuItemDeclarationCopy_Click);
			// 
			// textFormulaView
			// 
			this.textFormulaView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textFormulaView.Location = new System.Drawing.Point(0, 0);
			this.textFormulaView.Multiline = true;
			this.textFormulaView.Name = "textFormulaView";
			this.textFormulaView.ReadOnly = true;
			this.textFormulaView.Size = new System.Drawing.Size(580, 106);
			this.textFormulaView.TabIndex = 0;
			// 
			// tabConditions
			// 
			this.tabConditions.Controls.Add(this.splitContainer5);
			this.tabConditions.Location = new System.Drawing.Point(4, 22);
			this.tabConditions.Name = "tabConditions";
			this.tabConditions.Size = new System.Drawing.Size(586, 500);
			this.tabConditions.TabIndex = 3;
			this.tabConditions.Text = "Условия";
			this.tabConditions.UseVisualStyleBackColor = true;
			// 
			// splitContainer5
			// 
			this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer5.Location = new System.Drawing.Point(0, 0);
			this.splitContainer5.Name = "splitContainer5";
			this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer5.Panel1
			// 
			this.splitContainer5.Panel1.Controls.Add(this.listConditions);
			// 
			// splitContainer5.Panel2
			// 
			this.splitContainer5.Panel2.Controls.Add(this.textConditionView);
			this.splitContainer5.Size = new System.Drawing.Size(586, 500);
			this.splitContainer5.SplitterDistance = 309;
			this.splitContainer5.TabIndex = 0;
			// 
			// listConditions
			// 
			this.listConditions.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listConditions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5});
			this.listConditions.ContextMenuStrip = this.contextMenuConditions;
			this.listConditions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listConditions.FullRowSelect = true;
			this.listConditions.GridLines = true;
			this.listConditions.HotTracking = true;
			this.listConditions.HoverSelection = true;
			this.listConditions.Location = new System.Drawing.Point(0, 0);
			this.listConditions.Name = "listConditions";
			this.listConditions.Size = new System.Drawing.Size(586, 309);
			this.listConditions.TabIndex = 0;
			this.listConditions.UseCompatibleStateImageBehavior = false;
			this.listConditions.View = System.Windows.Forms.View.Details;
			this.listConditions.SelectedIndexChanged += new System.EventHandler(this.listConditions_SelectedIndexChanged);
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Условие";
			this.columnHeader5.Width = 600;
			// 
			// contextMenuConditions
			// 
			this.contextMenuConditions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.еслиToolStripMenuItem,
            this.toolStripMenuItem3,
            this.menuItemDeleteCondition});
			this.contextMenuConditions.Name = "contextMenuConditions";
			this.contextMenuConditions.Size = new System.Drawing.Size(130, 54);
			// 
			// еслиToolStripMenuItem
			// 
			this.еслиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.узелВыбранToolStripMenuItem});
			this.еслиToolStripMenuItem.Name = "еслиToolStripMenuItem";
			this.еслиToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.еслиToolStripMenuItem.Text = "Если";
			// 
			// узелВыбранToolStripMenuItem
			// 
			this.узелВыбранToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemChildShouldBeSelected});
			this.узелВыбранToolStripMenuItem.Name = "узелВыбранToolStripMenuItem";
			this.узелВыбранToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.узелВыбранToolStripMenuItem.Text = "Узел Выбран";
			// 
			// menuItemChildShouldBeSelected
			// 
			this.menuItemChildShouldBeSelected.Name = "menuItemChildShouldBeSelected";
			this.menuItemChildShouldBeSelected.Size = new System.Drawing.Size(241, 22);
			this.menuItemChildShouldBeSelected.Text = "То Выбрать Один из Дочерних";
			this.menuItemChildShouldBeSelected.Click += new System.EventHandler(this.menuItemChildShouldBeSelected_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(126, 6);
			// 
			// menuItemDeleteCondition
			// 
			this.menuItemDeleteCondition.Name = "menuItemDeleteCondition";
			this.menuItemDeleteCondition.Size = new System.Drawing.Size(129, 22);
			this.menuItemDeleteCondition.Text = "Удалить";
			this.menuItemDeleteCondition.Click += new System.EventHandler(this.menuItemDeleteCondition_Click);
			// 
			// textConditionView
			// 
			this.textConditionView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textConditionView.Location = new System.Drawing.Point(0, 0);
			this.textConditionView.Multiline = true;
			this.textConditionView.Name = "textConditionView";
			this.textConditionView.ReadOnly = true;
			this.textConditionView.Size = new System.Drawing.Size(586, 187);
			this.textConditionView.TabIndex = 0;
			// 
			// previewTab
			// 
			this.previewTab.Controls.Add(this.previewBrowser);
			this.previewTab.Location = new System.Drawing.Point(4, 22);
			this.previewTab.Name = "previewTab";
			this.previewTab.Size = new System.Drawing.Size(586, 500);
			this.previewTab.TabIndex = 5;
			this.previewTab.Text = "Предосмотр";
			this.previewTab.UseVisualStyleBackColor = true;
			// 
			// previewBrowser
			// 
			this.previewBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.previewBrowser.Location = new System.Drawing.Point(0, 0);
			this.previewBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.previewBrowser.Name = "previewBrowser";
			this.previewBrowser.Size = new System.Drawing.Size(586, 500);
			this.previewBrowser.TabIndex = 0;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
			this.toolStrip1.Location = new System.Drawing.Point(3, 3);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(58, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "Сохранить";
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "toolStripButton2";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.apprasialX);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(899, 532);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Отчет";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// apprasialX
			// 
			this.apprasialX.Dock = System.Windows.Forms.DockStyle.Fill;
			this.apprasialX.Location = new System.Drawing.Point(3, 3);
			this.apprasialX.Name = "apprasialX";
			this.apprasialX.Size = new System.Drawing.Size(893, 526);
			this.apprasialX.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemReport});
			this.menuStrip1.Location = new System.Drawing.Point(144, 315);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(59, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuItemReport
			// 
			this.menuItemReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemReportGenerate,
            this.menuItemReportExport});
			this.menuItemReport.Name = "menuItemReport";
			this.menuItemReport.Size = new System.Drawing.Size(51, 20);
			this.menuItemReport.Text = "Отчет";
			// 
			// menuItemReportGenerate
			// 
			this.menuItemReportGenerate.Name = "menuItemReportGenerate";
			this.menuItemReportGenerate.Size = new System.Drawing.Size(216, 22);
			this.menuItemReportGenerate.Text = "Генерировать";
			// 
			// menuItemReportExport
			// 
			this.menuItemReportExport.Name = "menuItemReportExport";
			this.menuItemReportExport.Size = new System.Drawing.Size(216, 22);
			this.menuItemReportExport.Text = "Выгрузить в Репозиторий";
			// 
			// Scenarion
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(907, 558);
			this.Controls.Add(this.tabScenarioTest);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Scenarion";
			this.Text = "Scenarion";
			this.Load += new System.EventHandler(this.Scenarion_Load);
			this.tabScenarioTest.ResumeLayout(false);
			this.tabScenarioEditor.ResumeLayout(false);
			this.tabScenarioEditor.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.contextMenuTreeScenario.ResumeLayout(false);
			this.tabControlBranchOptions.ResumeLayout(false);
			this.sourceDataTab.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.ResumeLayout(false);
			this.contextMenuSourceValues.ResumeLayout(false);
			this.templateTab.ResumeLayout(false);
			this.tabFormulas.ResumeLayout(false);
			this.splitContainer4.Panel1.ResumeLayout(false);
			this.splitContainer4.Panel2.ResumeLayout(false);
			this.splitContainer4.Panel2.PerformLayout();
			this.splitContainer4.ResumeLayout(false);
			this.contextMenuDeclarations.ResumeLayout(false);
			this.tabConditions.ResumeLayout(false);
			this.splitContainer5.Panel1.ResumeLayout(false);
			this.splitContainer5.Panel2.ResumeLayout(false);
			this.splitContainer5.Panel2.PerformLayout();
			this.splitContainer5.ResumeLayout(false);
			this.contextMenuConditions.ResumeLayout(false);
			this.previewTab.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabScenarioTest;
		private System.Windows.Forms.TabPage tabScenarioEditor;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		internal System.Windows.Forms.TabControl tabControlBranchOptions;
		private System.Windows.Forms.TabPage sourceDataTab;
		private System.Windows.Forms.TabPage templateTab;
        internal System.Windows.Forms.TabPage tabFormulas;
		private System.Windows.Forms.TabPage tabConditions;
		internal System.Windows.Forms.TabPage previewTab;
		private System.Windows.Forms.ContextMenuStrip contextMenuTreeScenario;
		private System.Windows.Forms.ToolStripMenuItem createBranchToolStripMenuItem;
		internal HtmlEditorControl templateEditor;
		private System.Windows.Forms.WebBrowser previewBrowser;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripMenuItem deleteBranchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer2;
		internal System.Windows.Forms.TreeView treeScenario;
		private System.Windows.Forms.PropertyGrid propertyGridScenarioNode;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.ListView listSourceValues;
		private System.Windows.Forms.PropertyGrid propertyGridSourceValue;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ContextMenuStrip contextMenuSourceValues;
		private System.Windows.Forms.ToolStripMenuItem menuItemSourceDataDeclarationCreateString;
		private System.Windows.Forms.ToolStripMenuItem menuItemSourceDataDeclarationCreateText;
		private System.Windows.Forms.ToolStripMenuItem menuItemSourceDataDeclarationCreateNumber;
		private System.Windows.Forms.ToolStripMenuItem menuItemSourceDataDeclarationCreatePercent;
		private System.Windows.Forms.ToolStripMenuItem menuItemSourceDataDeclarationCreateDate;
		private System.Windows.Forms.ToolStripMenuItem menuItemSourceDataDeclarationCreateCurrency;
		private System.Windows.Forms.ToolStripMenuItem menuItemSourceDataDeclarationDelete;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem menuItemSourceDataDeclarationCopy;
		private System.Windows.Forms.ContextMenuStrip contextMenuDeclarations;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeclarationCreateString;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeclarationCreateText;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeclarationCreateNumber;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeclarationCreatePercent;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeclarationCreateDate;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeclarationCreateCurrency;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeclarationDelete;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeclarationCopy;
		private System.Windows.Forms.SplitContainer splitContainer4;
		private System.Windows.Forms.ListView listDeclarations;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.TextBox textFormulaView;
		private System.Windows.Forms.SplitContainer splitContainer5;
		private System.Windows.Forms.ListView listConditions;
		private System.Windows.Forms.TextBox textConditionView;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ContextMenuStrip contextMenuConditions;
		private System.Windows.Forms.ToolStripMenuItem еслиToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem узелВыбранToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem menuItemChildShouldBeSelected;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeleteCondition;
		private System.Windows.Forms.ToolStripMenuItem menuItemDeclarationEdit;
		private System.Windows.Forms.ToolStripMenuItem menuItemSourceDataDeclarationEdit;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSourceValueChangeType;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSourceValueChangeTypeToString;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSourceValueChangeTypeToText;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSourceValueChangeTypeToNumber;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSourceValueChangeTypeToPercent;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSourceValueChangeTypeToDateTime;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSourceValueChangeTypeToCurrency;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeclarationChangeType;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeclarationChangeTypeToCurrency;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeclarationChangeTypeToNumber;
				private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeclarationChangeTypeToPercent;
				private System.Windows.Forms.MenuStrip menuStrip1;
				private System.Windows.Forms.ToolStripMenuItem menuItemReport;
				private System.Windows.Forms.ToolStripMenuItem menuItemReportGenerate;
				private System.Windows.Forms.ToolStripMenuItem menuItemReportExport;
				internal ObjectMeet.Appearance.Explorer.SimpleTabbedDocument apprasialX;
	}
}