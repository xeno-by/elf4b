namespace ObjectMeet.Tiller.Entities.Api
{
	using System.ComponentModel;

	public enum LogicalTopicType
	{
		[Description("�������")] Default = 0,
		[Description("���������")] Glossary = 0x10,
		[Description("������ 1")] Topic1 = 0x100,
		[Description("������ 2")] Topic2 = Topic1 + 1,
		[Description("������ 3")] Topic3 = Topic1 + 2,
		[Description("������ 4")] Topic4 = Topic1 + 3,
		[Description("������ 5")] Topic5 = Topic1 + 4,
		[Description("��� ��������")] ForUpload = 0x200,
	}
}