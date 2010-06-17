using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Browser.Gui.Editor
{
	using Elf.Syntax.Light;
	using Esath.Pie.AstRendering;

	public partial class FormulaEditor : Form
	{
		public FormulaEditor()
		{
			InitializeComponent();

			elfEditor.ElfCodeChanged += elfEditor_ElfCodeChanged;
		}

		void elfEditor_ElfCodeChanged(object sender, EventArgs e)
		{
			buttonOk.Enabled = !elfEditor.ElfCode.ToCanonicalElf().RenderCanonicalElfAsPublicText(elfEditor.Ctx).Contains("?");
		}
	}
}