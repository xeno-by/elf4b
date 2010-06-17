using Esath.Eval.Ver2;

namespace Browser.Gui
{
	using System.Text.RegularExpressions;

	public partial class Scenarion
	{
		private bool ConditionsTabSelected(ScenarioNode node)
		{
			listConditions.Items.Clear();
			textConditionView.Text = "";
			foreach (var condition in SelectedNode.ConditionDeclarations)
			{
				var item = listConditions.Items.Add(condition.Name);
				item.Tag = condition;
			}
			return true;
		}

		private bool GlossaryTabSelected(ScenarioNode node)
		{
			return true;
		}

		private bool FormulasTabSelected(ScenarioNode node)
		{
			listDeclarations.Items.Clear();
			textFormulaView.Text = "";
			foreach (var declaration in SelectedNode.FormulaDeclarations)
			{
				var item = listDeclarations.Items.Add(declaration.Name);
				item.Tag = declaration;
				declaration.NameChanged += x => item.Text = x;
				item.SubItems.Add(declaration.HumanType);
			}
			return true;
		}

		internal bool PreviewTabSelected(ScenarioNode node)
		{
			if (templateEditor.InnerHtml == null)
			{
				previewBrowser.DocumentText = "";
				return true;
			}
			var topicNumber = "";
			var title = "";

			var tableCounter = 1;

#warning Review this: eval sessions now lock the vault for reading

#if VAULT_EVAL_1
            using (_evalSession1 = new Esath.Eval.Ver1.EvalSession(Scenario.Vault))
            {
#endif

#if VAULT_EVAL_2
			using (var repo = RepositoryEditor.Repository())
			using (_evalSession2 = new Esath.Eval.Ver2.EvalSession(Scenario.Vault, repo, null))
			{
#endif

#if VAULT_EVAL_3
            using (_evalSession3 = new Esath.Eval.Ver3.EvalSession(Scenario.VaultCompiler))
            {
#endif

				var preview = Regex.Replace(templateEditor.InnerHtml, StartUp.VFIELD_PATTERN, vpathEvaluator, RegexOptions.Compiled);
				if (node.Title != "")
				{
					title = node.Title;
					switch (node.NodeType)
					{
						case ScenarioNodeType.Topic:
							topicNumber = "1";
							title = string.Format("<h{1}>{2} {0}</h{1}>", title, "1", topicNumber);
							break;
						case ScenarioNodeType.Subtopic2:
							topicNumber = "1.1";
							title = string.Format("<h{1}>{2} {0}</h{1}>", title, "2", topicNumber);
							break;
						case ScenarioNodeType.Subtopic3:
							topicNumber = "1.1.1";
							title = string.Format("<h{1}>{2} {0}</h{1}>", title, "3", topicNumber);
							break;
						case ScenarioNodeType.Subtopic4:
							topicNumber = "1.1.1.1";
							title = string.Format("<h{1}>{2} {0}</h{1}>", title, "4", topicNumber);
							break;
						case ScenarioNodeType.Subtopic5:
							topicNumber = "1.1.1.1.1";
							title = string.Format("<h{1}>{2} {0}</h{1}>", title, "5", topicNumber);
							break;
						default:
							break;
					}
					topicNumber += "."; // for tables only
				}

				preview = Regex.Replace(preview, @"<table\s", match => string.Format(@"<div style='text-align:right'>Таблица {1}{0}</div><table ", tableCounter++, topicNumber), RegexOptions.Compiled | RegexOptions.IgnoreCase);
				if (title != "") preview = title + preview;
				previewBrowser.DocumentText = preview;

#if VAULT_EVAL_1
            }
#endif

#if VAULT_EVAL_2
			}
#endif

#if VAULT_EVAL_3
            }
#endif

			return true;
		}

		private bool SourceDataTabSelected(ScenarioNode node)
		{
			listSourceValues.Items.Clear();
			propertyGridSourceValue.SelectedObject = null;
			foreach (var sourceValueDeclaration in SelectedNode.SourceValueDeclarations)
			{
				var item = listSourceValues.Items.Add(sourceValueDeclaration.Name);
				item.Tag = sourceValueDeclaration;
				sourceValueDeclaration.NameChanged += x => item.Text = x;
				item.SubItems.Add(sourceValueDeclaration.HumanType);
			}
			return true;
		}

		private bool TemplateTabSelected(ScenarioNode node)
		{
			return true;
		}
	}
}