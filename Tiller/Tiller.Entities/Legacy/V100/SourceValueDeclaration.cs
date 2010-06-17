namespace ObjectMeet.Tiller.Entities.Legacy.V100
{
	using System;
	using System.ComponentModel;
	using System.IO;
	using DataVault.Api;

	[Obsolete("Any given program, when running, is obsolete")]
	public class SourceValueDeclaration
	{
		internal event Action<string> NameChanged;

		[Browsable(false)]
		public IBranch Model { get; set; }

		[Browsable(true)]
		[DisplayName("���")]
		[Description("������������� ���� �������� ������, ������������ � ��������")]
		[Category("��������� ������")]
		public string Name
		{
			get { return Model.GetOrCreateValue("name", "����� ��������").ContentString; }

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

		[Browsable(true)]
		[DisplayName("��")]
		[Description("���������� ������������� ������")]
		[Category("��������� ������")]
		public string VPath
		{
			get { return Model.VPath.ToString(); }
		}

		[Browsable(false)]
		public string Type
		{
			get { return Model.GetOrCreateValue("type", "string").ContentString; }

			set { Model.GetOrCreateValue("type", value).UpdateContent(value); }
		}

		[Browsable(false)]
		public string DeclarationType
		{
			get { return Model.GetOrCreateValue("declarationType", "source").ContentString; }

			set { Model.GetOrCreateValue("declarationType", value).UpdateContent(value); }
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

		[Browsable(false)]
		public string RepositoryValuePath
		{
			get { return Model.GetOrCreateValue("repositoryValue", "").ContentString; }

			set { Model.GetOrCreateValue("repositoryValue", value).UpdateContent(value); }
		}

		//#warning Should be removed in release
		//public String VaultFormat = typeof(FsVaultFormat).Name;
		public String Uri = Path.Combine("", "repository");

		//        private static Object _appWideVaultSyncRoot = new Object();
		//	    private static IVault _appWideVault;
		//
		//	    public IVault ExternalVault
		//	    {
		//	        get
		//	        {
		//                lock(_appWideVaultSyncRoot)
		//                {
		//                    if (_appWideVault == null)
		//                    {
		//                        _appWideVault = VaultApi.OpenFs(Path.Combine(Application.StartupPath, "repository"));
		//                    }
		//
		//                    return _appWideVault;
		//                }
		//	        }
		//	    }
		[Editor("DataVault.UI.Controls.ComponentModel.ValueEditor, DataVault.UI", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[TypeConverter("DataVault.UI.Controls.ComponentModel.ElementTypeConverter, DataVault.UI")]
		[Browsable(true)]
		[DisplayName("���� �����������")]
		[Description("���� � ����� �����������, ����������� �������� ����")]
		[Category("��������� ������")]
		public IValue RepositoryValue
		{
			get
			{
				if (!_isRepositoryValueCached)
				{
					_repositoryValueCached = RepositoryValuePath == ""
					                         	? null
					                         	:
					                         		VaultApi.OpenFs(Uri).GetValue(RepositoryValuePath);
					//                        ExternalVault.GetValue(RepositoryValuePath);
					_isRepositoryValueCached = true;
				}

				return _repositoryValueCached;
			}
			set
			{
				var newValuePath = value == null ? String.Empty : (String) value.VPath;
				if (RepositoryValuePath != newValuePath)
				{
					RepositoryValuePath = newValuePath;
					_repositoryValueCached = value;
					_isRepositoryValueCached = true;
				}
			}
		}

		private bool _isRepositoryValueCached;
		private IValue _repositoryValueCached;

		[Browsable(true)]
		[DisplayName("�������� ��������")]
		[Description("�������� ����, ����������� ��� ���������� ��������")]
		[Category("���������� ��������")]
		[Editor("ObjectMeet.Tiller.Gui.SourceDatumPropertyEditor, ObjectMeet.Tiller.Gui", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
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