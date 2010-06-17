using System;
using System.Linq;

namespace Browser.Gui.Editor
{
	using System.Drawing.Design;
	using System.Windows.Forms;
	using System.Windows.Forms.Design;

	public class SourceValuePropertyEditor : UITypeEditor
	{
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if ((context == null) || (provider == null)) base.EditValue(context, provider, value);

			var editorService = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
			if (editorService == null) return base.EditValue(context, provider, value);

			var sourceValueDeclaration = context.Instance as SourceValueDeclaration;
			if (sourceValueDeclaration == null) return base.EditValue(context, provider, value);

			var editor = SourceValueEditorBase.CreateEditorFromTypeToken(sourceValueDeclaration.Type);
			if (editor == null) return base.EditValue(context, provider, value);

			editor.ValueDeclaration = sourceValueDeclaration;
			editor.Value = sourceValueDeclaration.ValueForTesting;
			if (editorService.ShowDialog(editor) == DialogResult.OK)
			{
				value = editor.Value;
			}

			return base.EditValue(context, provider, value);
		}

		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return context != null ? UITypeEditorEditStyle.Modal : base.GetEditStyle(context);
		}
	}
}