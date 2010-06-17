namespace ObjectMeet.Tiller.Entities.Legacy.V100
{
	using System;
	using System.ComponentModel;
	using DataVault.Api;

	[Obsolete("Any given program, when running, is obsolete")]
	internal class FormulaDeclaration
	{
		internal event Action<string> NameChanged;

		[Browsable(false)]
		public IBranch Model { get; set; }

		[Browsable(false)]
		public string DeclarationType
		{
			get { return Model.GetOrCreateValue("declarationType", "formula").ContentString; }

			set { Model.GetOrCreateValue("declarationType", value).UpdateContent(value); }
		}

		[Browsable(false)]
		public string HumanText
		{
			get { return Model.GetOrCreateValue("humanText", "").ContentString; }

			set { Model.GetOrCreateValue("humanText", value).UpdateContent(value); }
		}

		[Browsable(false)]
		public string ElfCode
		{
			get { return Model.GetOrCreateValue("elfCode", "").ContentString; }

			set { Model.GetOrCreateValue("elfCode", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("���")]
		[Description("������������� ���� �������� ������, ������������ � ��������")]
		[Category("��������� ������")]
		public string Name
		{
			get { return Model.GetOrCreateValue("name", "��������� ��� ��� ����� :)").ContentString; }

			set
			{
				Model.GetOrCreateValue("name", value).UpdateContent(value);
				if (NameChanged != null) NameChanged(value);
			}
		}

		[Browsable(true)]
		[DisplayName("���")]
		[Description("�������������� ������� ��������� �������� �������. �������� ������� � �� �������� ���������")]
		[Category("��������� ������")]
		public string HumanType
		{
			get
			{
				var token = Type;
				if (token == "text") return "�����";
				if (token == "number") return "�����";
				if (token == "percent") return "�������";
				if (token == "datetime") return "����";
				if (token == "currency") return "������";

				return "������";
			}
		}

		[Browsable(false)]
		public string Type
		{
			get { return Model.GetOrCreateValue("type", "string").ContentString; }

			set { Model.GetOrCreateValue("type", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("����������")]
		[Description("����� ����������, ������� ������ ������������ ��� ���������� ��������")]
		[Category("��������")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Comment
		{
			get { return Model.GetOrCreateValue("comment", "").ContentString; }

			set { Model.GetOrCreateValue("comment", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("������� ���������")]
		[Description("���������� ��� ������������, ���������� ���������� ������� ��������� ��������")]
		[Category("��������")]
		public string MeasurementUnit
		{
			get { return Model.GetOrCreateValue("measurementUnit", "").ContentString; }

			set { Model.GetOrCreateValue("measurementUnit", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("�������� ��-���������")]
		[Description("�������������� �������� ����")]
		[Category("��������� ������")]
		public string DefaultValue
		{
			get { return Model.GetOrCreateValue("defaultValue", "").ContentString; }

			set { Model.GetOrCreateValue("defaultValue", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("�������� ��������")]
		[Description("�������� ����, ����������� ��� ���������� ��������")]
		[Category("���������� ��������")]
		public string ValueForTesting
		{
			get { return Model.GetOrCreateValue("valueForTesting", "").ContentString; }

			set { Model.GetOrCreateValue("valueForTesting", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("������� ����")]
		[Description("������� ���� �� ������ ������������, �� ����� ����������� � ���������")]
		[Category("���������� ��������")]
		[TypeConverter("ObjectMeet.Couturier.Forms.BooleanToYesNoTypeConverter, ObjectMeet.Couturier")]
		public bool IsHidden
		{
			get { return bool.Parse(Model.GetOrCreateValue("isHidden", "false").ContentString); }

			set { Model.GetOrCreateValue("isHidden", value.ToString()).UpdateContent(value.ToString()); }
		}
	}

}