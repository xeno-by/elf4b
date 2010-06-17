namespace ObjectMeet.Tiller.Entities.Api
{
	using System.IO;
	using Service;

	public interface IScenarioService
	{
		IInteractionProvider InteractionProvider { get; set; }
		bool CreateScenario(out IScenario scenario);
		bool LoadScenario(FileInfo fileInfo, out IScenario scenario);
		bool SaveScenario(IScenario scenario);
		bool SaveScenarioAs(IScenario scenario, FileInfo newFile);

		IScenarioNode NewNode();
		bool AttachNode(IScenarioNode parentNode, ref IScenarioNode scenarioNode);

		ISourceDatumDeclaration NewSourceDatumDeclaration();
		bool AttachSourceDatumDeclaration(IScenarioNode parentNode, ref ISourceDatumDeclaration sourceDatumDeclaration);

		IFormulaDeclaration NewFormulaDeclaration();
		bool AttachFormulaDeclaration(IScenarioNode parentNode, ref IFormulaDeclaration formulaDeclaration);

	}
}