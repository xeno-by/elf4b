namespace ObjectMeet.Tiller.Entities
{
	using System;
	using System.IO;

	internal static class Manifest
	{
		/// <summary>
		/// The current version of storage structure of the application.
		/// </summary>
		/// <remarks>
		/// <list type="">
		/// <listheader>Rules on version part increment:</listheader>
		/// <item>Major - The whole storage structure is changed. Full convertation from the previous version is required.</item>
		/// <item>Minor - Default values of storage items are changed, items are renamed. Full convertation isn't required, but items be updated. Obsolete items should be removed.</item>
		/// <item>Build - Added new nodes with default values. Neither convertation nor amendment is required.</item>
		/// <item>Revision - Added new values with default values. Neither converation nor amendment is required.</item>
		/// </list>
		/// </remarks>
		public static Version StructureVersion
		{
			get { return new Version(2, 0, 0, 0); }
		}

		public static string NewScenarioName
		{
			get { return "Сценарий "; }
		}

		public static string NewReportName
		{
			get { return "Отчет "; }
		}

		internal const string ScenarioDevelopmentCategoryName = "Разработка сценария";
		internal const string ReportCreationCategoryName = "Работа над отчетом";
		internal const string ReportGenerationCategoryName = "Генерация отчета";

		public const string DeclarationTypeSourceDatum = "source";
		public const string DeclarationTypeFormula = "formula";

		public const string DataTypeString = "string";
		public const string DataTypeText = "text";
		public const string DataTypeNumber = "number";
		public const string DataTypePercent = "percent";
		public const string DataTypeDateTime = "datetime";
		public const string DataTypeCurrency = "currency";

		public static string GetHumanTextFromDataTypeToken(string token)
		{
			if (token == DataTypeText) return "Текст";
			if (token == DataTypeNumber) return "Число";
			if (token == DataTypePercent) return "Процент";
			if (token == DataTypeDateTime) return "Дата";
			if (token == DataTypeCurrency) return "Валюта";

			return "Строка";
		}


		public static DirectoryInfo DefaultDocumentDirectory
		{
			get { return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)); }
		}
	}
}