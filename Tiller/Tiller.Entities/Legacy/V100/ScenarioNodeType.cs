namespace ObjectMeet.Tiller.Entities.Legacy.V100
{
	using System;
	using System.ComponentModel;

	[Obsolete("Any given program, when running, is obsolete")]
	internal enum ScenarioNodeType
	{
		[Description("Обычный")] Default = 0,
		//[Description("Глоссарий")] Glossary = 10,
		[Description("Раздел 1")] Topic = 20,
		[Description("Подраздел 2")] Subtopic2 = 21,
		[Description("Подраздел 3")] Subtopic3 = 22,
		[Description("Подраздел 4")] Subtopic4 = 23,
		[Description("Подраздел 5")] Subtopic5 = 24,
		//[Description("Исходные данные")] SourceData = 40,
		//[Description("Приложение")] Appendix = 50,
		//[Description("Скрытый")] Hidden = 0x100,
		[Description("Для выгрузки")] ForExport = 0x200,
	}
}