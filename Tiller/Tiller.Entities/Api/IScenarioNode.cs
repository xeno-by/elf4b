namespace ObjectMeet.Tiller.Entities.Api
{
	using System;
	using System.Linq;
	using Pocso;
	using Whit;

	[DefaultImplementation(typeof (ScenarioNode))]
	public interface IScenarioNode : ITillerEntity
	{
		Guid ParentNodeId { get; }

		ScenarioNodeVariation NodeVariation { get; }

		int Level { get; }

		object Tag { get; set; }
		bool HasError { get; set; }

		string Template { get; set; }
		bool ContainsTemplate { get; }

		int ChildNodeCount { get; }
		int SourceDatumDeclarationCount { get; }
		int FormulaDeclarationCount { get; }
		int GlossaryEntryCount { get; }

		#region public stuff

		string TopicTitle { get; set; }

		bool IsAppendix { get; set; }

		bool IsRadioOwner { get; set; }

		int SortingWeight { get; set; }

		LogicalTopicType TopicType { get; set; }

		#endregion
	}
}