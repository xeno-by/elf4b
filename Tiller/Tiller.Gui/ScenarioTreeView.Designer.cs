namespace ObjectMeet.Tiller.Gui
{
	partial class ScenarioTreeView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenarioTreeView));
			this.imagesScenario = new System.Windows.Forms.ImageList(this.components);
			this.treeBox = new ObjectMeet.Couturier.Forms.TreeBox();
			((System.ComponentModel.ISupportInitialize)(this.treeBox)).BeginInit();
			this.SuspendLayout();
			// 
			// imagesScenario
			// 
			this.imagesScenario.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesScenario.ImageStream")));
			this.imagesScenario.TransparentColor = System.Drawing.Color.Transparent;
			this.imagesScenario.Images.SetKeyName(0, "special-common.png");
			this.imagesScenario.Images.SetKeyName(1, "special-object.png");
			this.imagesScenario.Images.SetKeyName(2, "special-glossary.png");
			this.imagesScenario.Images.SetKeyName(3, "tpl-portrait.png");
			this.imagesScenario.Images.SetKeyName(4, "tpl-landscape.png");
			this.imagesScenario.Images.SetKeyName(5, "empty.png");
			// 
			// treeBox
			// 
			this.treeBox.CheckBoxes = true;
			this.treeBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeBox.ImageIndex = 0;
			this.treeBox.ImageList = this.imagesScenario;
			this.treeBox.Location = new System.Drawing.Point(0, 0);
			this.treeBox.Name = "treeBox";
			this.treeBox.NorthEastImage = ((System.Drawing.Image)(resources.GetObject("treeBox.NorthEastImage")));
			this.treeBox.SelectedNode = null;
			this.treeBox.Size = new System.Drawing.Size(337, 246);
			this.treeBox.SouthEastImage = ((System.Drawing.Image)(resources.GetObject("treeBox.SouthEastImage")));
			this.treeBox.TabIndex = 0;
			this.treeBox.TaggedImage = ((System.Drawing.Image)(resources.GetObject("treeBox.TaggedImage")));
			// 
			// ScenarioTreeView
			// 
			this.Controls.Add(this.treeBox);
			this.Name = "ScenarioTreeView";
			this.Size = new System.Drawing.Size(337, 246);
			((System.ComponentModel.ISupportInitialize)(this.treeBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Couturier.Forms.TreeBox treeBox;
		private System.Windows.Forms.ImageList imagesScenario;
	}
}
