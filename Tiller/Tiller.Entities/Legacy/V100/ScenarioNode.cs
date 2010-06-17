namespace ObjectMeet.Tiller.Entities.Legacy.V100
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using DataVault.Api;

	[Obsolete("Any given program, when running, is obsolete")]
	internal class ScenarioNode
	{
		[Browsable(false)]
		public object View { get; set; }

		[Browsable(false)]
		public IBranch Model { get; set; }

		[Browsable(false)]
		public bool IsSystemNode
		{
			get { return Id.StartsWith("@"); }
		}

		[Browsable(false)]
		public string Id
		{
			get { return Model.GetOrCreateValue("id", "").ContentString; }

			set { Model.GetOrCreateValue("id", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("ИД")]
		[Description("Внутренний идентификатор узла")]
		[Category("Разработка сценария")]
		public string VPath
		{
			get { return Model.VPath.ToString(); }
		}

		[Browsable(false)]
		public string Name
		{
			get { return Model.GetOrCreateValue("name", "").ContentString; }

			set { Model.GetOrCreateValue("name", value).UpdateContent(value); }
		}


		[Browsable(true)]
		[DisplayName("Наименование")]
		[Description("Используется при генерации отчета как заголовок соответствующего раздела")]
		[Category("Генерация отчета")]
		public string Title
		{
			get { return Model.GetOrCreateValue("title", "").ContentString; }

			set { Model.GetOrCreateValue("title", value).UpdateContent(value); }
		}


		[Browsable(true)]
		[DisplayName("Обязательный узел")]
		[Description(@"Выбор Обязательного узла пользователь не может отменить. (Например, ""Титульный лист"")")]
		[Category("Генерация отчета")]
		[TypeConverter("ObjectMeet.Couturier.Forms.BooleanToYesNoTypeConverter, ObjectMeet.Couturier")]
		public bool IsRequired
		{
			get { return Model.GetOrCreateValue("isRequired", "false").ContentString == "true"; }

			set { Model.GetOrCreateValue("isRequired", value ? "true" : "false").UpdateContent(value ? "true" : "false"); }
		}

		private int _level;

		[Browsable(true)]
		[DisplayName("Уровень")]
		[Description("Уровень вложенности узла сценария")]
		[Category("Генерация отчета")]
		public int Level
		{
			get
			{
				if (_level == 0)
				{
					for (var parent = Model.Parent; parent != null; parent = parent.Parent) _level++;
					_level--; // ignore the Scenario node
				}
				return _level;
			}
		}


		[Browsable(false)]
		public string Template
		{
			get { return Model.GetOrCreateValue("template", "").ContentString; }

			set { Model.GetOrCreateValue("template", value).UpdateContent(value); }
		}

		[Browsable(true)]
		[DisplayName("Вес сортировки")]
		[Description("Используется при генерации отчета, узлы с меньшими весами окажутся в отчете выше")]
		[Category("Генерация отчета")]
		public int SortingWeight
		{
			get { return int.Parse(Model.GetOrCreateValue("sortingWeight", "0").ContentString); }

			set { Model.GetOrCreateValue("sortingWeight", value.ToString()).UpdateContent(value.ToString()); }
		}

		[Browsable(true)]
		[DisplayName("Тип узла")]
		[Description("Указывает как системе обрабатывать данный узел")]
		[Category("Генерация отчета")]
		[TypeConverter("ObjectMeet.Couturier.Forms.EnumTypeConverter, ObjectMeet.Couturier")]
		public ScenarioNodeType NodeType
		{
			get { return (ScenarioNodeType)int.Parse(Model.GetOrCreateValue("nodeType", "0").ContentString); }

			set { Model.GetOrCreateValue("nodeType", ((int)value).ToString()).UpdateContent(((int)value).ToString()); }
		}

		[Browsable(true)]
		[DisplayName("Скрытый узел")]
		[Description("Скрытый узел не видим пользователю, но участвует в генерации отчета")]
		[Category("Разработка сценария")]
		[TypeConverter("ObjectMeet.Couturier.Forms.BooleanToYesNoTypeConverter, ObjectMeet.Couturier")]
		public bool IsHidden
		{
			get { return bool.Parse(Model.GetOrCreateValue("isHidden", "false").ContentString); }

			set { Model.GetOrCreateValue("isHidden", value.ToString()).UpdateContent(value.ToString()); }
		}

		[Browsable(true)]
		[DisplayName("Приложение")]
		[Description("Указывает системе, что этот узел попадет в Приложения к отчету при генерации документов")]
		[Category("Генерация отчета")]
		[TypeConverter("ObjectMeet.Couturier.Forms.BooleanToYesNoTypeConverter, ObjectMeet.Couturier")]
		public bool IsAppendix
		{
			get { return bool.Parse(Model.GetOrCreateValue("isAppendix", "false").ContentString); }

			set { Model.GetOrCreateValue("isAppendix", value.ToString()).UpdateContent(value.ToString()); }
		}

		[Browsable(false)]
		public IEnumerable<SourceValueDeclaration> SourceValueDeclarations
		{
			get
			{
				foreach (var branch in Model.GetOrCreateBranch("_sourceValueDeclarations").GetBranches())
				{
					yield return new SourceValueDeclaration { Model = branch };
				}
			}
		}

		public SourceValueDeclaration AddSourceValueDeclaration(string typeToken)
		{
			return new SourceValueDeclaration
			{
				Model = Model.GetOrCreateBranch("_sourceValueDeclarations").CreateBranch(Guid.NewGuid().ToString().Replace('-', '_')),
				Type = typeToken,
				DeclarationType = "source"
			}
				;
		}

		[Browsable(false)]
		public IEnumerable<FormulaDeclaration> FormulaDeclarations
		{
			get
			{
				foreach (var branch in Model.GetOrCreateBranch("_formulaDeclarations").GetBranches())
				{
					yield return new FormulaDeclaration { Model = branch };
				}
			}
		}

		public FormulaDeclaration AddFormulaDeclaration(string typeToken)
		{
			return new FormulaDeclaration
			{
				Model = Model.GetOrCreateBranch("_formulaDeclarations").CreateBranch(Guid.NewGuid().ToString().Replace('-', '_')),
				Type = typeToken,
				DeclarationType = "formula"
			}
				;
		}


		[Browsable(false)]
		public IEnumerable<ConditionDeclaration> ConditionDeclarations
		{
			get
			{
				foreach (var branch in Model.GetOrCreateBranch("_conditions").GetBranches())
				{
					yield return new ConditionDeclaration { Model = branch };
				}
			}
		}

		public ConditionDeclaration AddConditionDeclaration()
		{
			return new ConditionDeclaration
			{
				Model = Model.GetOrCreateBranch("_conditions").CreateBranch(Guid.NewGuid().ToString().Replace('-', '_')),
			}
				;
		}
	}

}