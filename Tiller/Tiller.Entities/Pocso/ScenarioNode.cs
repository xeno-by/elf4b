namespace ObjectMeet.Tiller.Entities.Pocso
{
	using System;
	using System.ComponentModel;
	using System.Linq;
	using Api;
	using Whit;

	internal abstract class ScenarioNode : TillerEntityBase, IScenarioNode
	{
		[Browsable(false)]
		public abstract Guid ParentNodeId { get; protected internal set; }

		[Browsable(false)]
		public abstract int ChildNodeCount { get; protected internal set; }

		[Browsable(false)]
		public abstract int SourceDatumDeclarationCount { get; protected internal set; }

		[Browsable(false)]
		public abstract int FormulaDeclarationCount { get; protected internal set; }

		[Browsable(false)]
		public abstract int GlossaryEntryCount { get; protected internal set; }

	[Browsable(false)]
		public abstract ScenarioNodeVariation NodeVariation { get; protected internal set; }

		private void FireNodeChangedEvent(ScenarioEventArgs scenarioEventArgs)
		{
			var scenario = Scenario as Scenario;
			if (scenario == null) return;
			scenarioEventArgs.ScenarioNode = this;
			scenario.OnNodeChanged(scenarioEventArgs);
		}

		#region public stuff
    
		[Browsable(false)]
		[WhitIgnorable]
		public abstract bool HasError { get; set; }


		protected void OnHasErrorChanged()
		{
			FireNodeChangedEvent(new ScenarioEventArgs { ChangeVariant = NodeChangeVariant.HasError, });
		}

		protected void OnNameChanged()
		{
			FireNodeChangedEvent(new ScenarioEventArgs {ChangeVariant = NodeChangeVariant.Name,});
		}

		[Browsable(true)]
		[DisplayName("Наименование заголовка")]
		[Description("Используется при генерации отчета как заголовок соответствующего раздела")]
		[Category(Manifest.ReportGenerationCategoryName)]
		public abstract string TopicTitle { get; set; }

		protected void OnTitleChanged()
		{
			FireNodeChangedEvent(new ScenarioEventArgs {ChangeVariant = NodeChangeVariant.Title,});
		}

		[Browsable(true)]
		[DisplayName("Вес сортировки")]
		[Description("Используется при генерации отчета, узлы с меньшими весами окажутся в отчете выше (допускается использование отрицательных значений)")]
		[Category(Manifest.ReportGenerationCategoryName)]
		public abstract int SortingWeight { get; set; }

		protected void OnSortingWeightChanged()
		{
			FireNodeChangedEvent(new ScenarioEventArgs {ChangeVariant = NodeChangeVariant.SortingWeight,});
		}

		[ReadOnly(true)]
		[Browsable(true)]
		[DisplayName("Уровень")]
		[Description("Уровень вложенности узла сценария")]
		[Category(Manifest.ScenarioDevelopmentCategoryName)]
		public abstract int Level { get; set; }

		[Browsable(true)]
		[DisplayName("Приложение")]
		[Description("Указывает системе, что этот узел попадет в \"Приложения к отчету\" при генерации документов")]
		[Category(Manifest.ReportGenerationCategoryName)]
		[TypeConverter("ObjectMeet.Couturier.Forms.BooleanToYesNoTypeConverter, ObjectMeet.Couturier")]
		public abstract bool IsAppendix { get; set; }

		protected void OnIsAppendixChanged()
		{
			FireNodeChangedEvent(new ScenarioEventArgs {ChangeVariant = NodeChangeVariant.IsAppendix,});
		}

		[Browsable(true)]
		[DisplayName("Узел выбора")]
		[Description("При выборе данного узла в отчете, пользователь может выбрать только один его дочерний узел")]
		[Category(Manifest.ReportCreationCategoryName)]
		[TypeConverter("ObjectMeet.Couturier.Forms.BooleanToYesNoTypeConverter, ObjectMeet.Couturier")]
		public abstract bool IsRadioOwner { get; set; }

		protected void OnIsRadioOwnerChanged()
		{
			FireNodeChangedEvent(new ScenarioEventArgs {ChangeVariant = NodeChangeVariant.IsAppendix,});
		}

		[Browsable(true)]
		[DisplayName("Тип узла")]
		[Description("Определяет как система обрабатывает данный узел")]
		[Category(Manifest.ReportGenerationCategoryName)]
		[TypeConverter("ObjectMeet.Couturier.Forms.EnumTypeConverter, ObjectMeet.Couturier")]
		public abstract LogicalTopicType TopicType { get; set; }

		protected void OnTopicTypeChanged()
		{
			FireNodeChangedEvent(new ScenarioEventArgs {ChangeVariant = NodeChangeVariant.TopicType,});
		}

		[Browsable(false)]
		public abstract bool ContainsTemplate { get; protected internal set; }

		[Browsable(false)]
		public abstract string Template { get; set; }

		protected bool OnTemplateChanging(string value)
		{
			return IsManagedByTool;
		}

		protected void OnTemplateChanged()
		{
			ContainsTemplate = !string.IsNullOrEmpty(Template);
		}

		protected void OnCommentChanged()
		{
			FireNodeChangedEvent(new ScenarioEventArgs {ChangeVariant = NodeChangeVariant.Comment,});
		}

		[Browsable(false)]
		public object Tag { get; set; }

		#endregion
	}
}