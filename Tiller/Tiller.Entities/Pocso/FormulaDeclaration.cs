namespace ObjectMeet.Tiller.Entities.Pocso
{
	using System;
	using System.ComponentModel;
	using Api;
	using Whit;

	internal abstract class FormulaDeclaration : TillerEntityBase, IFormulaDeclaration
	{
		[Browsable(false)]
		public abstract Guid ScenarioNodeId { get; protected internal set; }

		[Browsable(false)]
		public string DeclarationType
		{
			get { return Manifest.DeclarationTypeFormula; }
		}

		#region public stuff

		[Browsable(false)]
		[MetaInfo(DefaultValue = Manifest.DataTypeString)]
		public abstract string DataType { get; set; }

		[Browsable(true)]
		[DisplayName("Выгружаемое значение")]
		[Description("Указывает системе будет ли выгружать итоговое значение данной формулы в репозиторий")]
		[Category(Manifest.ScenarioDevelopmentCategoryName)]
		public abstract string IsExportable { get; set; }


		[Browsable(true)]
		[DisplayName("Тип")]
		[Description("Характеристика области возможных значений объекта. Определяет, каким образом итоговые значения форматируются в отчете")]
		[Category(Manifest.ReportGenerationCategoryName)]
		public string HumanType
		{
			get { return Manifest.GetHumanTextFromDataTypeToken(DataType); }
		}

		[Browsable(false)]
		public abstract string HumanSource { get; set; }

		[Browsable(false)]
		public abstract string ElfSource { get; set; }

		#endregion
	}
}