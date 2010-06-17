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
		[DisplayName("Имя")]
		[Description("Идентификатор поля исходных данных, используемый в формулах")]
		[Category("Генерация отчета")]
		public string Name
		{
			get { return Model.GetOrCreateValue("name", "Новое значение").ContentString; }

			set
			{
				Model.GetOrCreateValue("name", value).UpdateContent(value);
				if (NameChanged != null) NameChanged(value);
			}
		}

		[Browsable(true)]
		[DisplayName("Тип")]
		[Description("Характеристика области возможных значений объекта. Задается однажды и не подлежит изменению")]
		[Category("Генерация отчета")]
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

		[Browsable(true)]
		[DisplayName("ИД")]
		[Description("Внутренний идентификатор записи")]
		[Category("Генерация отчета")]
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
		[DisplayName("Коментарий")]
		[Description("Текст коментария, который увидит пользователь при заполнении значения")]
		[Category("Удобства")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Comment
		{
			get { return Model.GetOrCreateValue("comment", "").ContentString; }

			set { Model.GetOrCreateValue("comment", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("Единица измерения")]
		[Description("Информация для пользователя, помогающая определить единицу измерения значения")]
		[Category("Удобства")]
		public string MeasurementUnit
		{
			get { return Model.GetOrCreateValue("measurementUnit", "").ContentString; }

			set { Model.GetOrCreateValue("measurementUnit", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("Значение по-умолчанию")]
		[Description("Первоначальное значение поля")]
		[Category("Генерация отчета")]
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
		[DisplayName("Ключ репозитория")]
		[Description("Путь к ключу репозитория, содержащему значение поля")]
		[Category("Генерация отчета")]
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
		[DisplayName("Тестовое значение")]
		[Description("Значение поля, применяемое при разработке сценария")]
		[Category("Разработка сценария")]
		[Editor("ObjectMeet.Tiller.Gui.SourceDatumPropertyEditor, ObjectMeet.Tiller.Gui", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string ValueForTesting
		{
			get { return Model.GetOrCreateValue("valueForTesting", "").ContentString; }

			set { Model.GetOrCreateValue("valueForTesting", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("Скрытое поле")]
		[Description("Скрытое поле не видимо пользователю, но может применяться в сценариях")]
		[Category("Разработка сценария")]
		[TypeConverter("ObjectMeet.Couturier.Forms.BooleanToYesNoTypeConverter, ObjectMeet.Couturier")]
		public bool IsHidden
		{
			get { return bool.Parse(Model.GetOrCreateValue("isHidden", "false").ContentString); }

			set { Model.GetOrCreateValue("isHidden", value.ToString()).UpdateContent(value.ToString()); }
		}
	}
}