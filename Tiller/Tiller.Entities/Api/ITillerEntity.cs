namespace ObjectMeet.Tiller.Entities.Api
{
	using System;

	public interface ITillerEntity
	{
		Guid Id { get; }

		int SiblingWeight { get; }

		bool IsUnderCommonRootNode { get; }

		bool IsUnderParticularRootNode { get; }

		bool IsManagedByTool { get; }

		IScenario Scenario { get; }

		string Name { get; set; }

		string Comment { get; set; }
	}
}