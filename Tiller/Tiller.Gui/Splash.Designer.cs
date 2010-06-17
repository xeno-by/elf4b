namespace ObjectMeet.Tiller.Gui
{
	partial class Splash
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
			this.customShape1 = new Telerik.WinControls.CustomShape(this.components);
			this.tabIEShape1 = new Telerik.WinControls.UI.TabIEShape();
			this.SuspendLayout();
			// 
			// customShape1
			// 
			this.customShape1.AsString = "20,20,200,100:20,20,False,0,0,0,0,0:220,20,False,0,0,0,0,0:220,120,False,0,0,0,0," +
					"0:20,120,False,0,0,0,0,0:";
			// 
			// Splash
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Red;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(395, 299);
			this.ControlBox = false;
			this.Name = "Splash";
			this.Shape = this.tabIEShape1;
			this.ShowIcon = false;
			this.TransparencyKey = System.Drawing.Color.Red;
			this.ResumeLayout(false);

		}

		#endregion

		private Telerik.WinControls.CustomShape customShape1;
		private Telerik.WinControls.UI.TabIEShape tabIEShape1;
	}
}