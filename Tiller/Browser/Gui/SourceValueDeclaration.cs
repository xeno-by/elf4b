using DataVault.UI.Api.ComponentModel;
using DataVault.UI.Impl.VaultFormatz;
using DataVault.Core.Api;

namespace Browser.Gui
{
	using System;
	using System.ComponentModel;
	using System.Drawing.Design;
	using System.IO;
	using System.Windows.Forms;
	using Editor;

	public class SourceValueDeclaration
	{
		internal event Action<string> NameChanged;

		[Browsable(false)]
		public IBranch Model { get; set; }


		private const string NAME_NAME = "name";
		internal const string NAME_DEFAULT_VALUE = "Новое значение";

		[Browsable(true)]
		[DisplayName("Имя")]
		[Description("Идентификатор поля исходных данных, используемый в формулах")]
		[Category("Генерация отчета")]
		public string Name
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(NAME_NAME)) == null ? NAME_DEFAULT_VALUE : v.ContentString;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(NAME_NAME)) == null)
				{
					Model.CreateValue(NAME_NAME, value);
				}
				else
				{
					v.SetContent(value);
				}
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
		public string VPath { get { return Model.VPath.ToString(); } }

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

		[Browsable(false)]
		public string DeclarationType { get { return Model.GetOrCreateValue("declarationType", "source").ContentString; } set { Model.GetOrCreateValue("declarationType", value).SetContent(value); } }


		private const string COMMENT_NAME = "comment";
		private const string COMMENT_DEFAULT_VALUE = "";


		[Browsable(true)]
		[DisplayName("Коментарий")]
		[Description("Текст коментария, который увидит пользователь при заполнении значения")]
		[Category("Удобства")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		public string Comment
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(COMMENT_NAME)) != null ? v.ContentString : COMMENT_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(COMMENT_NAME)) == null)
				{
					if (COMMENT_DEFAULT_VALUE != value) Model.CreateValue(COMMENT_NAME, value);
				}
				else
				{
					if (COMMENT_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}

		private const string MEASUREMENT_UNIT_NAME = "measurementUnit";
		private const string MEASUREMENT_UNIT_DEFAULT_VALUE = "";

		[Browsable(true)]
		[DisplayName("Единица измерения")]
		[Description("Информация для пользователя, помогающая определить единицу измерения значения")]
		[Category("Удобства")]
		public string MeasurementUnit
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(MEASUREMENT_UNIT_NAME)) != null ? v.ContentString : MEASUREMENT_UNIT_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(MEASUREMENT_UNIT_NAME)) == null)
				{
					if (MEASUREMENT_UNIT_DEFAULT_VALUE != value) Model.CreateValue(MEASUREMENT_UNIT_NAME, value);
				}
				else
				{
					if (MEASUREMENT_UNIT_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}

		private const string DEFAULT_VALUE_NAME = "defaultValue";
		private const string DEFAULT_VALUE_DEFAULT_VALUE = "";

		[Browsable(true)]
		[DisplayName("Значение по-умолчанию")]
		[Description("Первоначальное значение поля")]
		[Category("Генерация отчета")]
		public string DefaultValue
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(DEFAULT_VALUE_NAME)) != null ? v.ContentString : DEFAULT_VALUE_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(DEFAULT_VALUE_NAME)) == null)
				{
					if (DEFAULT_VALUE_DEFAULT_VALUE != value) Model.CreateValue(DEFAULT_VALUE_NAME, value);
				}
				else
				{
					if (DEFAULT_VALUE_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}

		private const string REPOSITORY_VALUE_NAME = "repositoryValue";
		private const string REPOSITORY_VALUE_DEFAULT_VALUE = "";

		[Browsable(false)]
		private string RepositoryValuePath
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(REPOSITORY_VALUE_NAME)) != null ? v.ContentString : REPOSITORY_VALUE_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(REPOSITORY_VALUE_NAME)) == null)
				{
					if (REPOSITORY_VALUE_DEFAULT_VALUE != value) Model.CreateValue(REPOSITORY_VALUE_NAME, value);
				}
				else
				{
					if (REPOSITORY_VALUE_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}

		// #warning Should be removed in release:: see comment on RepositoryValue
		private String VaultFormat = typeof (FsVaultFormat).Name;
		private String Uri = Path.Combine(Application.StartupPath, "repository");

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

		[Editor(typeof (ValueEditor), typeof (UITypeEditor))]
		[TypeConverter(typeof (ElementTypeConverter))]
		[Browsable(!true)] // the current implementation of UITypeEditor doesn't dispose external vault correctly
		[DisplayName("Ключ репозитория")]
		[Description("Путь к ключу репозитория, содержащему значение поля")]
		[Category("Генерация отчета")]
		private IValue RepositoryValue
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

		private bool _isRepositoryValueCached = false;
		private IValue _repositoryValueCached = null;


		private const string VALUE_FOR_TESTING_NAME = "valueForTesting";
		private const string VALUE_FOR_TESTING_DEFAULT_VALUE = "";

		[Browsable(true)]
		[DisplayName("Тестовое значение")]
		[Description("Значение поля, применяемое при разработке сценария")]
		[Category("Разработка сценария")]
		[Editor(typeof (SourceValuePropertyEditor), typeof (UITypeEditor))]
		public string ValueForTesting
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(VALUE_FOR_TESTING_NAME)) != null ? v.ContentString : Model.CreateValue(VALUE_FOR_TESTING_NAME, VALUE_FOR_TESTING_DEFAULT_VALUE).ContentString;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(VALUE_FOR_TESTING_NAME)) == null)
					Model.CreateValue(VALUE_FOR_TESTING_NAME, value);
				else
					v.SetContent(value);
			}
		}

		internal string Value
		{
			get
			{
				var selfValue = ValueForTesting;

				if (string.IsNullOrEmpty(selfValue))
				{
					var repositoryVPath = RepositoryValuePath;

					if (!string.IsNullOrEmpty(repositoryVPath))
					{
						using (var repo = RepositoryEditor.Repository())
						{
							var v = repo.GetValue(repositoryVPath);
							if (v == null) return "";
							return v.ContentString ?? "";
						}
					}
				}
				return selfValue;
			}
		}

//		[Browsable(true)]
//		[DisplayName("Скрытое поле")]
//		[Description("Скрытое поле не видимо пользователю, но может применяться в сценариях")]
//		[Category("Разработка сценария")]
//		[TypeConverter(typeof (BooleanToYesNoTypeConverter))]
//		public bool IsHidden
//		{
//			get { return bool.Parse(Model.GetOrCreateValue("isHidden", "false").ContentString); }
//
//			set { Model.GetOrCreateValue("isHidden", value.ToString()).SetContent(value.ToString()); }
//		}
//
//		[Browsable(true)]
//		[DisplayName("Константа")]
//		[Description("Константное значение будет копироваться из сценария в каждый создаваемый на его основе отчет")]
//		[Category("Разработка сценария")]
//		[TypeConverter(typeof (BooleanToYesNoTypeConverter))]
//		public bool IsConstant
//		{
//			get { return bool.Parse(Model.GetOrCreateValue("isConstant", "true").ContentString); }
//
//			set { Model.GetOrCreateValue("isConstant", value.ToString()).SetContent(value.ToString()); }
//		}
	}
}