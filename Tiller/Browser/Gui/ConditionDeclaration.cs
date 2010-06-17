using DataVault.Core.Api;

namespace Browser.Gui
{
	using System.ComponentModel;

	public class ConditionDeclaration
	{
		[Browsable(false)]
		public IBranch Model { get; set; }

		private const string NAME_NAME = "name";
		private const string NAME_DEFAULT_VALUE = "";

		[Browsable(false)]
		public string Name
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(NAME_NAME)) != null ? v.ContentString : NAME_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(NAME_NAME)) == null)
				{
					if (NAME_DEFAULT_VALUE != value) Model.CreateValue(NAME_NAME, value);
				}
				else
				{
					if (NAME_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}

		private const string TEXT_NAME = "text";
		private const string TEXT_DEFAULT_VALUE = "";

		[Browsable(false)]
		public string Text
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(TEXT_NAME)) != null ? v.ContentString : TEXT_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(TEXT_NAME)) == null)
				{
					if (TEXT_DEFAULT_VALUE != value) Model.CreateValue(TEXT_NAME, value);
				}
				else
				{
					if (TEXT_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}

		private const string HANDLER_NAME = "handler";
		private const string HANDLER_DEFAULT_VALUE = "";

		[Browsable(false)]
		public string Handler
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(HANDLER_NAME)) != null ? v.ContentString : HANDLER_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(HANDLER_NAME)) == null)
				{
					if (HANDLER_DEFAULT_VALUE != value) Model.CreateValue(HANDLER_NAME, value);
				}
				else
				{
					if (HANDLER_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}
	}
}