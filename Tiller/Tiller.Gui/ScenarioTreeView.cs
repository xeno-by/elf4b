using System.Linq;

namespace ObjectMeet.Tiller.Gui
{
	using System;
	using System.Windows.Forms;
	using Couturier.Forms;
	using Entities.Api;

	public partial class ScenarioTreeView : Control
	{
		public ScenarioTreeView()
		{
			InitializeComponent();
			//treeBox.CheckBoxes = false;
		}

		public IScenarioNode SelectedNode
		{
			get { return treeBox.SelectedNode == null ? null : (IScenarioNode) treeBox.SelectedNode.Tag; }
			set { treeBox.SelectedNode = value == null ? null : (TreeBox.Node) value.Tag; }
		}

		private IScenario _scenario;

		public IScenario Scenario
		{
			get { return _scenario; }
			set
			{
				if (_scenario == value) return;

				treeBox.SuspendLayout();
				treeBox.Clear();
				if (_scenario != null) _scenario.NodeChanged -= _scenario_OnNodeChanged;
				_scenario = value;
				_scenario.NodeChanged += _scenario_OnNodeChanged;

				var scenarioNodes = _scenario.ScenarioNodes;
				foreach (var scenarioNode in scenarioNodes)
				{
					var node = new TreeBox.Node
					           	{
					           		Tag = scenarioNode,
					           		Text = scenarioNode.Name,
					           		IsRadioOwner = scenarioNode.IsRadioOwner,
					           		Id = scenarioNode.Id,
												SortingWeight = scenarioNode.SiblingWeight,
												IsBold = scenarioNode.IsManagedByTool,
					           	};
					scenarioNode.Tag = node;
					UpdateImageIndicesForNode(scenarioNode, node);
					treeBox[scenarioNode.Id] = node;
				}
				foreach (var scenarioNode in scenarioNodes)
				{
					(scenarioNode.ParentNodeId == Guid.Empty ? treeBox.Nodes : treeBox[scenarioNode.ParentNodeId].Nodes).Add(treeBox[scenarioNode.Id]);
				}

				treeBox.Nodes[1].Remove();
				treeBox.Invalidate(false);
				treeBox.ResumeLayout(false);
			}
		}

		private void _scenario_OnNodeChanged(object sender, ScenarioEventArgs e)
		{
			switch (e.ChangeVariant)
			{
				case NodeChangeVariant.Property:
					break;
				case NodeChangeVariant.Collection:
					break;
				case NodeChangeVariant.Name:
					break;
				case NodeChangeVariant.Title:
					break;
				case NodeChangeVariant.SortingWeight:
					break;
				case NodeChangeVariant.IsAppendix:
					break;
				case NodeChangeVariant.TopicType:
					break;
				case NodeChangeVariant.IsRadioOwner:
					break;
				case NodeChangeVariant.Comment:
					break;
				case NodeChangeVariant.HasError:
					treeBox[e.ScenarioNode.Id].IsTagged = e.ScenarioNode.HasError;
					break;
				default:
					throw new ArgumentOutOfRangeException("e", e.ChangeVariant, "Unable to recognize change variant");
			}
		}

		private void UpdateImageIndicesForNode(IScenarioNode scenarioNode, TreeBox.Node node)
		{
			if (scenarioNode.Id == scenarioNode.Scenario.CommonRootNode.Id)
			{
				node.ImageAppearance.ImageIndex = (int) NodeImageIndex.RootCommonNormal;
				return;
			}
			if (scenarioNode.Id == scenarioNode.Scenario.ParticularRootNode.Id)
			{
				node.ImageAppearance.ImageIndex = (int) NodeImageIndex.RootParticularNormal;
				return;
			}
			if (scenarioNode.TopicType == LogicalTopicType.Glossary)
			{
				node.ImageAppearance.ImageIndex = (int) NodeImageIndex.NodeGlossaryNormal;
				return;
			}

			if (scenarioNode.ContainsTemplate)
			{
				node.ImageAppearance.ImageIndex = (int) (scenarioNode.IsAppendix ? NodeImageIndex.NodeLandscapeNormal : NodeImageIndex.NodePortraitNormal);
			}
			else
			{
				node.ImageAppearance.ImageIndex = (int) NodeImageIndex.NodeEmptyNormal;
			}

			node.ImageAppearance.SouthEast = scenarioNode.SourceDatumDeclarationCount > 0;
			node.ImageAppearance.NorthEast = scenarioNode.FormulaDeclarationCount > 0;

//			if (scenarioNode.ContainsTemplate && scenarioNode.SourceDatumDeclarationCount > 0)
//			{
//				node.ImageAppearance. .ImageIndex = (int) (scenarioNode.IsAppendix ? NodeImageIndex.NodeLandscapeSourceDataNormal : NodeImageIndex.NodePortraitSourceDataNormal);
//				return;
//			}
//			if (scenarioNode.ContainsTemplate)
//			{
//				node.ImageAppearance.ImageIndex = (int) (scenarioNode.IsAppendix ? NodeImageIndex.NodeLandscapeNormal : NodeImageIndex.NodePortraitNormal);
//				return;
//			}
//			if (scenarioNode.SourceDatumDeclarationCount > 0)
//			{
//				node.ImageAppearance.ImageIndex = (int) NodeImageIndex.NodeSourceDataNormal;
//				return;
//			}
//			node.ImageAppearance.ImageIndex = (int) NodeImageIndex.NodeEmptyNormal;
		}
	}

	internal enum NodeImageIndex
	{
		RootCommonNormal = 0,
		RootParticularNormal = 1,
		NodeGlossaryNormal = 2,
		NodeEmptyNormal = 5,
		NodeLandscapeNormal = 4,
		NodePortraitNormal = 3,
	}
}