namespace ObjectMeet.Tiller.Entities.Service.Versioning
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Api;
	using DataVault.Api;
	using Pocso;
	using OLD = Legacy.V100;

	internal class ConvertorV1000V2000 : IModelConverter
	{
		IEnumerable<string> IModelConverter.FromVersions
		{
			get
			{
				yield return "0.*.*.*";
				yield return "1.*.*.*";
			}
		}

		Version IModelConverter.ToVersion
		{
			get { return new Version(2, 0); }
		}

		public IScenarioService ScenarioService { get; set; }

		bool IModelConverter.Convert(FileInfo source, FileInfo destination)
		{
			if (ScenarioService == null) throw new BusinessRuleViolationException(8, 10);

			using (var previous = VaultApi.OpenZip(source.FullName))
			{
				IScenario scenarioBean;
				if (!ScenarioService.CreateScenario(out scenarioBean)) return false;
				if (!ScenarioService.SaveScenarioAs(scenarioBean, destination)) return false;
				var scenario = (Scenario) scenarioBean;
#pragma warning disable 618,612
				var scenarioDepot = new OLD.ScenarioDepot {Vault = previous};
#pragma warning restore 618,612

				scenario.Revision = scenarioDepot.Version;
				if (!ScenarioService.SaveScenario(scenario)) return false;

				LoadBranch(scenarioDepot.CommonPart, scenario.CommonRootNode);
				LoadBranch(scenarioDepot.PartucilarPart, scenario.ParticularRootNode);

				if (!ScenarioService.SaveScenario(scenario)) return false;
			}
			return true;
		}
#pragma warning disable 618,612
		private void LoadBranch(IBranch branch, IScenarioNode parent)
		{
			foreach (var b in branch.GetBranches())
			{
				if (b.Name.StartsWith("_")) continue; // service node
				var scenarioNode = ScenarioService.NewNode();
				var oldNode = new OLD.ScenarioNode {Model = b,};

				scenarioNode.Name = oldNode.Name;
				scenarioNode.IsAppendix = oldNode.IsAppendix;
				scenarioNode.TopicTitle = oldNode.Title;
				scenarioNode.SortingWeight = oldNode.SortingWeight;
				scenarioNode.IsRadioOwner = oldNode.ConditionDeclarations.Count() > 0;
				switch (oldNode.NodeType)
				{
					case OLD.ScenarioNodeType.Default:
						scenarioNode.TopicType = LogicalTopicType.Default;
						break;
					case OLD.ScenarioNodeType.Topic:
						scenarioNode.TopicType = LogicalTopicType.Topic1;
						break;
					case OLD.ScenarioNodeType.Subtopic2:
						scenarioNode.TopicType = LogicalTopicType.Topic2;
						break;
					case OLD.ScenarioNodeType.Subtopic3:
						scenarioNode.TopicType = LogicalTopicType.Topic3;
						break;
					case OLD.ScenarioNodeType.Subtopic4:
						scenarioNode.TopicType = LogicalTopicType.Topic4;
						break;
					case OLD.ScenarioNodeType.Subtopic5:
						scenarioNode.TopicType = LogicalTopicType.Topic5;
						break;
					case OLD.ScenarioNodeType.ForExport:
						scenarioNode.TopicType = LogicalTopicType.ForUpload;
						break;
					default:
						scenarioNode.TopicType = LogicalTopicType.Default;
						break;
				}

				if (!ScenarioService.AttachNode(parent, ref scenarioNode)) continue;
				//TODO: strip word tags
				scenarioNode.Template = oldNode.Template;

				foreach (var declaration in oldNode.SourceValueDeclarations)
				{
					var sourceDatumDeclaration = ScenarioService.NewSourceDatumDeclaration();
					sourceDatumDeclaration.Comment = declaration.Comment;
					sourceDatumDeclaration.DataType = declaration.Type;
					sourceDatumDeclaration.MeasurementUnit = declaration.MeasurementUnit;
					sourceDatumDeclaration.Name = declaration.Name;
					sourceDatumDeclaration.RepositoryValuePath = declaration.RepositoryValuePath;
					sourceDatumDeclaration.ValueForTesting = declaration.ValueForTesting;
					if (!ScenarioService.AttachSourceDatumDeclaration(scenarioNode, ref sourceDatumDeclaration)) continue;
				}

				foreach (var declaration in oldNode.FormulaDeclarations)
				{
					var formulaDeclaration = ScenarioService.NewFormulaDeclaration();
					formulaDeclaration.Comment = declaration.Comment;
					formulaDeclaration.DataType = declaration.Type;
					formulaDeclaration.Name = declaration.Name;
					formulaDeclaration.ElfSource = declaration.ElfCode;
					formulaDeclaration.HumanSource = declaration.HumanText;
					if (!ScenarioService.AttachFormulaDeclaration(scenarioNode, ref formulaDeclaration)) continue;
				}

				LoadBranch(b, scenarioNode);
			}
		}
#pragma warning restore 618,612
	}
}