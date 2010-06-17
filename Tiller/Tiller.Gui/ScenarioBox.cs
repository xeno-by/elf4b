namespace ObjectMeet.Tiller.Gui
{
	using System;
	using System.Drawing;
	using Telerik.WinControls.Docking;
	using Telerik.WinControls.UI;

	public partial class ScenarioBox : DocumentPane
	{
		internal const string EMPTY_HTML = @"
<html><head>
<title>Template</title>
<style>
P {padding:0px;margin:0px;}
body {background-color:white}
</style></head>
<body style=""font-family:times;font-size:14pt;text-decoration:none;vertical-align:baseline;font-weight:normal;font-style:normal;"">
</body>
</html>
" + "\0";

		public ScenarioBox()
		{
			InitializeComponent();

			BeginInit();
			DockState = Telerik.WinControls.Docking.DockState.TabbedDocument;
			ID = Guid.NewGuid();
			Location = new System.Drawing.Point(1, 31);
			Name = ID.ToString();
			PreferredDockSize = new System.Drawing.Size(100, 200);
			PreferredFloatSize = new System.Drawing.Size(150, 300);
			//this.documentPane1.Size = new System.Drawing.Size(827, 375);
			//this.documentPane1.TabIndex = 7;
			//this.documentPane1.Text = "documentPane1";
			EndInit();
			webBrowser.DocumentText = EMPTY_HTML;
			webBrowser.Document.ExecCommand("EditMode", true, null);
		}


	}
}