namespace ObjectMeet.Tiller.Entities.Legacy.V100
{
	using System;
	using System.ComponentModel;

	[Obsolete("Any given program, when running, is obsolete")]
	internal enum ScenarioNodeType
	{
		[Description("�������")] Default = 0,
		//[Description("���������")] Glossary = 10,
		[Description("������ 1")] Topic = 20,
		[Description("��������� 2")] Subtopic2 = 21,
		[Description("��������� 3")] Subtopic3 = 22,
		[Description("��������� 4")] Subtopic4 = 23,
		[Description("��������� 5")] Subtopic5 = 24,
		//[Description("�������� ������")] SourceData = 40,
		//[Description("����������")] Appendix = 50,
		//[Description("�������")] Hidden = 0x100,
		[Description("��� ��������")] ForExport = 0x200,
	}
}