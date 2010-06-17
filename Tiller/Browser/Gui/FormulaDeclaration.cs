using DataVault.Core.Api;

namespace Browser.Gui
{
	using System;
	using System.ComponentModel;
	using System.Drawing.Design;
	using Util;

	public class FormulaDeclaration
	{
		internal event Action<string> NameChanged;

		[Browsable(false)]
		public IBranch Model { get; set; }

		[Browsable(false)]
		public string DeclarationType
		{
			get { return Model.GetOrCreateValue("declarationType", "formula").ContentString; }

			set { Model.GetOrCreateValue("declarationType", value).SetContent(value); }
		}

		private const string HUMAN_TEXT_NAME = "humanText";
		private const string HUMAN_TEXT_DEFAULT_VALUE = "";

		[Browsable(false)]
		public string HumanText
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(HUMAN_TEXT_NAME)) != null ? v.ContentString : HUMAN_TEXT_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(HUMAN_TEXT_NAME)) == null)
				{
					if (HUMAN_TEXT_DEFAULT_VALUE != value) Model.CreateValue(HUMAN_TEXT_NAME, value);
				}
				else
				{
					if (HUMAN_TEXT_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}

		private const string ELF_CODE_NAME = "elfCode";
		private const string ELF_CODE_DEFAULT_VALUE = "elfCode";

		[Browsable(false)]
		public string ElfCode
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(ELF_CODE_NAME)) != null ? v.ContentString : ELF_CODE_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(ELF_CODE_NAME)) == null)
				{
					if (ELF_CODE_DEFAULT_VALUE != value) Model.CreateValue(ELF_CODE_NAME, value);
				}
				else
				{
					if (ELF_CODE_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}

		private const string NAME_NAME = "name";
		private const string NAME_DEFAULT_VALUE = "Новое значение";

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
				if (NameChanged != null) NameChanged(value);
			}
		}

		[Browsable(false)]
		public string HumanType
		{
			get
			{
				var token = Type;
				if (token == "text") return "Текст";
				if (token == "number") return "Число";
				if (token == "percent") return "Процент";
				if (token == "datetime") return "Дата";
				if (token == "currency") return "Валюта";

				return "Строка";
			}
		}

		private const string TYPE_NAME = "type";
		private const string TYPE_DEFAULT_VALUE = "string";

		[Browsable(false)]
		public string Type
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(TYPE_NAME)) != null ? v.ContentString : TYPE_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(TYPE_NAME)) == null)
				{
					if (TYPE_DEFAULT_VALUE != value) Model.CreateValue(TYPE_NAME, value);
				}
				else
				{
					if (TYPE_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}


	}
}