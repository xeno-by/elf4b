namespace ObjectMeet.Tiller.Entities.Api
{
	using System.ComponentModel;

	public enum LogicalTopicType
	{
		[Description("Обычный")] Default = 0,
		[Description("Глоссарий")] Glossary = 0x10,
		[Description("Раздел 1")] Topic1 = 0x100,
		[Description("Раздел 2")] Topic2 = Topic1 + 1,
		[Description("Раздел 3")] Topic3 = Topic1 + 2,
		[Description("Раздел 4")] Topic4 = Topic1 + 3,
		[Description("Раздел 5")] Topic5 = Topic1 + 4,
		[Description("Для выгрузки")] ForUpload = 0x200,
	}
}