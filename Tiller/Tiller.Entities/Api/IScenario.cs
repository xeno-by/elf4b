namespace ObjectMeet.Tiller.Entities.Api
{
	using System;
	using System.Linq;

	public interface IScenario : IStorageUnit
	{
		Guid ScenarioId { get; }

		int Revision { get; }

		IQueryable<IScenarioNode> ScenarioNodes { get; }
		int ScenarioNodeCount { get; }

		IQueryable<ISourceDatumDeclaration> SourceDatumDeclarations { get; }
		int SourceDatumDeclarationCount { get; }

		IQueryable<IFormulaDeclaration> FormulaDeclarations { get; }
		int FormulaDeclarationCount { get; }

		string Name { get; }

		IScenarioNode CommonRootNode { get; }

		IScenarioNode ParticularRootNode { get; }

		event EventHandler<ScenarioEventArgs> NodeChanged;
	}
}