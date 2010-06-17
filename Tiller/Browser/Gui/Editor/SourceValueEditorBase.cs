using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataVault.Core.Api;

namespace Browser.Gui.Editor
{
	using System.IO;
	using global::DataVault.UI.Api;
	using global::DataVault.UI.Api.UIContext;

	public partial class SourceValueEditorBase : Form
	{
		public SourceValueEditorBase()
		{
			InitializeComponent();
		}

		private SourceValueDeclaration _valueDeclaration;

		public SourceValueDeclaration ValueDeclaration
		{
			get { return _valueDeclaration; }
			set
			{
				_valueDeclaration = value;
				labelName.Text = _valueDeclaration.Name;
			}
		}

		private string _value;

		public virtual string Value
		{
			get { return _value; }
			set
			{
				if (_value == null) InitByValue(value);
				_value = value;
				buttonOk.Enabled = IsValueValidForSaving(value);
			}
		}

		protected virtual void InitByValue(string value)
		{
		}

		public virtual bool IsValueValidForSaving(string value)
		{
			return true;
		}

		public static SourceValueEditorBase CreateEditorFromTypeToken(string token)
		{
			if (token == "string") return new SourceValueStringEditor();
			if (token == "text") return new SourceValueTextEditor();
			if (token == "number") return new SourceValueNumericEditor();
			if (token == "percent") return new SourceValueNumericEditor();
			if (token == "datetime") return new SourceValueDateEditor();
			if (token == "currency") return new SourceValueNumericEditor();

			return null;
		}

		private string _valueVPath;

		public string ValueVPath
		{
			get { return _valueVPath; }
			set
			{
				_valueVPath = value;
				//buttonFromRepository.Visible = buttonToRepository.Visible = !string.IsNullOrEmpty(value);
			}
		}

		private void buttonFromRepository_Click(object sender, EventArgs e)
		{
			var text = "";
			using (var repo = RepositoryEditor.Repository())
			{
				var dvbf = new DataVaultBrowserForm(repo) {Approver = Acceptor, StartPosition = FormStartPosition.CenterScreen,};
				if (dvbf.ShowDialog(this) == DialogResult.OK)
				{
					var element = dvbf.SelectedElement as IValue;
					text = element.ContentString;
				}
			}
			_value = null;
			Value = text;
		}

		private bool Acceptor(DataVaultUIContext context, IElement element)
		{
			var value = element as IValue;
			if (value == null) return false;
			if (value.Name.StartsWith("$")) return false;
			if (value.Metadata["default"] == "binary") return false;

			return true;
		}

		private void buttonFromRepository_ClickOld(object sender, EventArgs e)
		{
			var text = "";
			using (var repo = RepositoryEditor.Repository())
			{
				var value = repo.GetValue(ValueVPath);
				if (value == null)
				{
					//buttonFromRepository.Enabled = buttonToRepository.Enabled = false;
					return;
				}
				text = value.ContentString ?? "";
			}
			_value = null;
			Value = text;
		}

		private void buttonToRepository_Click(object sender, EventArgs e)
		{
			try
			{
				Enabled = false;
				UseWaitCursor = true;
				using (var repo = RepositoryEditor.Repository())
				{
					var value = repo.GetValue(ValueVPath);
					if (value == null)
					{
						//buttonFromRepository.Enabled = buttonToRepository.Enabled = false;
						return;
					}
					value.SetContent(Value ?? "");
					repo.Save();
				}
			}
			finally
			{
				UseWaitCursor = false;
				Enabled = true;
			}
		}
	}
}