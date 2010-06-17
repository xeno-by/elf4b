namespace ObjectMeet.Tiller.Entities.Api
{
	using System;

	public interface IDeclaration : ITillerEntity
	{
		Guid ScenarioNodeId { get; }

		string DeclarationType { get; }

		string DataType { get; set; }

		string IsExportable { get; set; }
	}
}