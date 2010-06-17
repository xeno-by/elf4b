namespace ObjectMeet.Tiller.Entities.Api
{
	using System;

	[Flags]
	public enum NodeChangeVariant
	{
		Property = 0x10000000,
		Collection = 0x20000000,

		Name = 0x0001 | Property,
		Title = 0x0002 | Property,
		SortingWeight = 0x0004 | Property,
		IsAppendix = 0x0008 | Property,
		TopicType = 0x0010 | Property,
		IsRadioOwner = 0x0020 | Property,
		Comment = 0x0040 | Property,
		HasError = 0x0080 | Property,
	}

	[Flags]
	public enum SourceDatumChangeVariant
	{
		Property = 0x10000000,
		Collection = 0x20000000,

		Name = 0x0001 | Property,
		DataType = 0x0002 | Property,
		Comment = 0x0004 | Property,
		UnitOfMeasurement = 0x0008 | Property,
		IsExportable = 0x0010 | Property,
	}
}