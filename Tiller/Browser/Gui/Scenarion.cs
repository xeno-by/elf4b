using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Esath.Pie.Helpers;
using DataVault.Core.Api;
using DataVault.UI.Api.ApiExtensions;

namespace Browser.Gui
{
	using System.IO;
	using System.Text.RegularExpressions;
	using Dialog;
	using Editor;
	using Elf.Syntax.Light;
	using Esath.Pie.AstRendering;
	using Esath.Pie.Contexts;
	using Util;

	public partial class Scenarion : Form
	{
		public Scenarion()
		{
			InitializeComponent();

			templateEditor.InnerHtml = "<html></html>";

			menuItemReportExport.Click += menuItemReportExport_Click;
			menuItemReportGenerate.Click += menuItemReportGenerate_Click;

			apprasialX.OpenStaticMenuCaption = "Исходные Данные";
			apprasialX.OpenDynamicMenuCaption = "Предосмотр";

			Disposed += Scenarion_Disposed;
		}

		private void Scenarion_Disposed(object sender, EventArgs e)
		{
			if (Scenario != null)
			{
				Scenario.Vault.Dispose();
				Scenario = null;
			}
		}

		private ScenarioDepot _scenario;

		public ScenarioDepot Scenario
		{
			get { return _scenario; }
			set
			{
				_scenario = value;
				InitScenario();
			}
		}

		private void InitScenario()
		{
			if (Scenario == null) return;

			foreach (ScenarioNode root in Scenario)
			{
				LoadBranch(root, null);
			}

			apprasialX.DefineModel(Scenario);

			//old: appraisalOne.ScenarioVault = Scenario.Vault;

#if EDITION_LIGHT
			if (tabScenarioTest.TabCount > 1) tabScenarioTest.TabPages.RemoveAt(0);
#endif

#if VAULT_EVAL_3
            appraisalOne.ScenarioCompiler = Scenario.VaultCompiler;
#endif
		}

		private void LoadBranch(ScenarioNode scenarioNode, TreeNode parent)
		{
			if (parent == null) // root
			{
				parent = new TreeNode {Text = scenarioNode.Name, Tag = scenarioNode, NodeFont = new Font("Tahoma", 8, FontStyle.Bold),};
				treeScenario.Nodes.Add(parent);
			}
			foreach (ScenarioNode node in scenarioNode.GetChildren())
			{
				var n = new TreeNode {Text = node.Name, Tag = node,};
				parent.Nodes.Add(n);
				LoadBranch(node, n);
			}
		}

		private void LoadBranchOld(IBranch branch, TreeNode parent)
		{
			if (parent == null) // root
			{
				parent = new TreeNode
				         	{
				         		Text = branch.GetOrCreateValue(new VPath("name"), branch.Name).ContentString,
				         		Tag = branch,
				         		//Name = branch.GetOrCreateValue(new VPath("id"), Guid.NewGuid().ToString()).ContentString,
				         		NodeFont = new Font("Tahoma", 8, FontStyle.Bold),
				         	};
				treeScenario.Nodes.Add(parent);
			}
			foreach (var b in branch.GetBranches())
			{
				if (b.Name.StartsWith("_")) continue; // service node
				var n = new TreeNode
				        	{
				        		Text = b.GetOrCreateValue("name", Guid.NewGuid().ToString().Replace('-', '_')).ContentString,
				        		Tag = b,
				        		//Name = b.GetOrCreateValue(new VPath("id"), Guid.NewGuid().ToString().Replace('-', '_')).ContentString
				        	};
				parent.Nodes.Add(n);
				LoadBranchOld(b, n);
			}
		}

		public ScenarioNode SelectedNode { get { return treeScenario.SelectedNode == null ? null : (ScenarioNode) treeScenario.SelectedNode.Tag; } }

		private void treeScenario_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (SelectedNode.IsSystemNode)
			{
				tabControlBranchOptions.Visible = deleteBranchToolStripMenuItem.Enabled = renameToolStripMenuItem.Enabled = false;
				propertyGridScenarioNode.SelectedObject = null;
			}
			else
			{
				propertyGridScenarioNode.SelectedObject = SelectedNode;
				templateEditor.InnerHtml = SelectedNode.Template;
				tabControlBranchOptions.SelectedTab = templateTab;
				tabControlBranchOptions.Visible = deleteBranchToolStripMenuItem.Enabled = renameToolStripMenuItem.Enabled = true;
			}

			if (AfterAfterSelect != null) AfterAfterSelect(this, EventArgs.Empty);

//			if (e.Node.Name != null && e.Node.Name.StartsWith("@"))
//			{
//				tabControlBranchOptions.Visible =
//					deleteBranchToolStripMenuItem.Enabled =
//					renameToolStripMenuItem.Enabled =
//					false;
//			}
//			else
//			{
//				var b = e.Node.Tag as IBranch;
//				templateEditor.InnerHtml = b.GetOrCreateValue("template", "").ContentString;
//				tabControlBranchOptions.SelectedTab = templateTab;
//				tabControlBranchOptions.Visible = deleteBranchToolStripMenuItem.Enabled = renameToolStripMenuItem.Enabled = true;
//			}
		}

		internal event EventHandler AfterAfterSelect;

		private void treeScenario_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			//MessageBox.Show(this, e.Node.Name);
			if (treeScenario.SelectedNode == null || treeScenario.SelectedNode.Name.StartsWith("@"))
			{
				return;
			}

			SelectedNode.Template = templateEditor.InnerHtml;

			//			var b = treeScenario.SelectedNode.Tag as IBranch;
			//			b.GetValue("template").SetContent(templateEditor.InnerHtml);
		}

		private void createBranchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeScenario.SelectedNode == null) return;
			var name = Interactor.Prompt("Введите имя нового узла", "");
			if (string.IsNullOrEmpty(name)) return;

			var n = new TreeNode {Text = name, Tag = SelectedNode.AddChild(name, Guid.NewGuid())};
			treeScenario.SelectedNode.Nodes.Add(n);
			n.EnsureVisible();
		}


		private void deleteBranchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeScenario.SelectedNode != null && treeScenario.SelectedNode.Name.StartsWith("@"))
			{
				return;
			}
			var b = treeScenario.SelectedNode.Tag as ScenarioNode;
			b.Model.Delete();
			treeScenario.SelectedNode.Remove();
		}

		private void treeScenario_MouseClick(object sender, MouseEventArgs e)
		{
			var n = treeScenario.GetNodeAt(e.Location);
			if (n == null) return;
			if (treeScenario.SelectedNode != n) treeScenario.SelectedNode = n;
		}

		private void tabControlBranchOptions_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.TabPage == templateTab)
			{
				if (treeScenario.SelectedNode != null && treeScenario.SelectedNode.Name.StartsWith("@"))
				{
					return;
				}
				var b = treeScenario.SelectedNode.Tag as ScenarioNode;
				templateEditor.InnerHtml = b.Template;
			}
			else
			{
				if (treeScenario.SelectedNode == null || treeScenario.SelectedNode.Name.StartsWith("@"))
				{
					return;
				}
				var b = treeScenario.SelectedNode.Tag as ScenarioNode;
				b.Template = templateEditor.InnerHtml;
			}
		}

		private void tabControlBranchOptions_Selected(object sender, TabControlEventArgs e)
		{
			var node = SelectedNode;

			if (e.TabPage == templateTab)
				if (TemplateTabSelected(node)) return;

			if (e.TabPage == previewTab)
				if (PreviewTabSelected(node)) return;

			if (e.TabPage == sourceDataTab)
				if (SourceDataTabSelected(node)) return;

			if (e.TabPage == tabFormulas)
				if (FormulasTabSelected(node)) return;

			if (e.TabPage == tabConditions)
				if (ConditionsTabSelected(node)) return;

//			if (e.TabPage == glossaryTab)
//				if (GlossaryTabSelected(node)) return;
		}

#if VAULT_EVAL_1
        private Esath.Eval.Ver1.EvalSession _evalSession1;
#endif

#if VAULT_EVAL_2
		private Esath.Eval.Ver2.EvalSession _evalSession2;
#endif

#if VAULT_EVAL_3
        private Esath.Eval.Ver3.EvalSession _evalSession3;
#endif

		private string vpathEvaluator(Match match)
		{
			if (Scenario == null) return "";

			var vpath = match.Groups["vpath"].Value;
			if (string.IsNullOrEmpty(vpath)) return "<span style='background-color:red'>Узел не существует</span>";

			var branch = Scenario.Vault.GetBranch(vpath);
			if (branch == null) return "<span style='background-color:red'>Узел не существует</span>";

			var value = branch.GetValue("declarationType");
			if (value != null)
			{
				var type = value.ContentString;
				if (type == "formula")
				{
					string eval;
					try
					{
#if VAULT_EVAL_1
                        eval = _evalSession1.Eval(branch).ToString();
#endif

#if VAULT_EVAL_2
						eval = _evalSession2.Eval(branch).ToString();
#endif

#if VAULT_EVAL_3
                        eval = _evalSession3.Eval(branch).ToString();
#endif
					}
					catch (Exception e)
					{
						var message = e.Message;
						if (string.IsNullOrEmpty(message) && e.InnerException != null) message = e.InnerException.Message;
						eval = string.Format("<span onclick=\"with(this.nextSibling.style)display=display=='none'?'':'none'\" contenteditable=false style='font:bold 8pt tahoma;border:1px solid red;background-color:white'>Ошибка вычисления формулы</span><pre style='display:none;border:1px solid red;margin:10px;'>{0}</pre>", message);
					}

					eval = Regex.Replace(match.Groups["body"].Value, @"([^&]*)&lt;[^&]+&gt;([.\n\r]*)", x => x.Groups[1].Value + eval + x.Groups[2].Value, RegexOptions.Compiled);

					return eval;
				}
			}

			var selfValue = branch.GetValue("valueForTesting").ContentString;

			if (string.IsNullOrEmpty(selfValue))
			{
				var repositoryValue = branch.GetValue("repositoryValue"); // subject to refactor or remove at all
				var repositoryVPath = repositoryValue != null ? repositoryValue.ContentString : "";

				if (!string.IsNullOrEmpty(repositoryVPath))
				{
					using (var repo = RepositoryEditor.Repository())
					{
						var v = repo.GetValue(repositoryVPath);
						if (v == null) return "";
						return v.ContentString ?? "";
					}
				}
			}
			return selfValue;
		}

		private void renameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeScenario.SelectedNode == null) return;

			if (treeScenario.SelectedNode != null && treeScenario.SelectedNode.Name.StartsWith("@"))
			{
				return;
			}

			var name = Interactor.Prompt("Введите новое имя узла", treeScenario.SelectedNode.Text);
			if (string.IsNullOrEmpty(name)) return;

			var b = treeScenario.SelectedNode.Tag as ScenarioNode;
			b.Name = name;
			treeScenario.SelectedNode.Text = name;
		}

		#region Source Values

		private void createSourceValueDeclarationView(string type)
		{
			var sourceValueDeclaration = SelectedNode.AddSourceValueDeclaration(type);
			var item = listSourceValues.Items.Add(sourceValueDeclaration.Name);
			item.Tag = sourceValueDeclaration;
			sourceValueDeclaration.NameChanged += x => item.Text = x;
			item.SubItems.Add(sourceValueDeclaration.HumanType);
			item.Selected = true;
		}

		private void menuItemSourceDataDeclarationCreateString_Click(object sender, EventArgs e)
		{
			createSourceValueDeclarationView("string");
		}

		private void menuItemSourceDataDeclarationCreateText_Click(object sender, EventArgs e)
		{
			createSourceValueDeclarationView("text");
		}

		private void menuItemSourceDataDeclarationCreateNumber_Click(object sender, EventArgs e)
		{
			createSourceValueDeclarationView("number");
		}

		private void menuItemSourceDataDeclarationCreatePercent_Click(object sender, EventArgs e)
		{
			createSourceValueDeclarationView("percent");
		}

		private void menuItemSourceDataDeclarationCreateDate_Click(object sender, EventArgs e)
		{
			createSourceValueDeclarationView("datetime");
		}

		private void menuItemSourceDataDeclarationCreateCurrency_Click(object sender, EventArgs e)
		{
			createSourceValueDeclarationView("currency");
		}

		private void menuItemSourceDataDeclarationEdit_Click(object sender, EventArgs e)
		{
			listSourceValues_DoubleClick(sender, e);
		}

		private void menuItemSourceDataDeclarationDelete_Click(object sender, EventArgs e)
		{
			if (listSourceValues.SelectedItems.Count != 1) return;

			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;
			listSourceValues.SelectedItems[0].Remove();
			sourceDataDeclaration.Model.Delete();
		}

		private void menuItemSourceDataDeclarationCopy_Click(object sender, EventArgs e)
		{
			if (listSourceValues.SelectedItems.Count != 1) return;

			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;
			var value = sourceDataDeclaration.Model;

			var html = string.Format("<span vpath='{1}' style='background-color:yellow' contenteditable='false'>&lt; {0} &gt;</span>", sourceDataDeclaration.Name, value.VPath.Path);
			Clipboard.SetText(html, TextDataFormat.Html);

			//MessageBox.Show(html);
		}

		private void UpdateCurrentSourceValueView()
		{
			if (listSourceValues.SelectedItems.Count != 1) return;

			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;
			listSourceValues.SelectedItems[0].SubItems[1].Text = sourceDataDeclaration.HumanType;
		}

		private void toolStripMenuItemSourceValueChangeTypeToString_Click(object sender, EventArgs e)
		{
			if (listSourceValues.SelectedItems.Count != 1) return;

			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;

			var token = sourceDataDeclaration.Type;
			if (token == "text")
			{
				sourceDataDeclaration.Type = "string";
			}
			if (token == "number")
			{
				sourceDataDeclaration.Type = "string";
			}
			if (token == "percent")
			{
				sourceDataDeclaration.Type = "string";
			}
			if (token == "datetime")
			{
				sourceDataDeclaration.Type = "string";
			}
			if (token == "currency")
			{
				sourceDataDeclaration.Type = "string";
			}
			UpdateCurrentSourceValueView();
		}

		private void toolStripMenuItemSourceValueChangeTypeToText_Click(object sender, EventArgs e)
		{
			if (listSourceValues.SelectedItems.Count != 1) return;

			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;

			var token = sourceDataDeclaration.Type;
			if (token == "string")
			{
				sourceDataDeclaration.Type = "text";
			}
			if (token == "text")
			{
				sourceDataDeclaration.Type = "text";
			}
			if (token == "number")
			{
				sourceDataDeclaration.Type = "text";
			}
			if (token == "percent")
			{
				sourceDataDeclaration.Type = "text";
			}
			if (token == "datetime")
			{
				sourceDataDeclaration.Type = "text";
			}
			if (token == "currency")
			{
				sourceDataDeclaration.Type = "text";
			}
			UpdateCurrentSourceValueView();
		}

		private void toolStripMenuItemSourceValueChangeTypeToNumber_Click(object sender, EventArgs e)
		{
			if (listSourceValues.SelectedItems.Count != 1) return;

			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;

			var token = sourceDataDeclaration.Type;
			if (token == "string")
			{
				sourceDataDeclaration.Type = "number";
			}
			if (token == "text")
			{
				sourceDataDeclaration.Type = "number";
			}
			if (token == "number")
			{
				sourceDataDeclaration.Type = "number";
			}
			if (token == "percent")
			{
				sourceDataDeclaration.Type = "number";
			}
			if (token == "datetime")
			{
				sourceDataDeclaration.Type = "number";
			}
			if (token == "currency")
			{
				sourceDataDeclaration.Type = "number";
			}
			UpdateCurrentSourceValueView();
		}

		private void toolStripMenuItemSourceValueChangeTypeToPercent_Click(object sender, EventArgs e)
		{
			if (listSourceValues.SelectedItems.Count != 1) return;

			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;

			var token = sourceDataDeclaration.Type;
			if (token == "string")
			{
				sourceDataDeclaration.Type = "percent";
			}
			if (token == "text")
			{
				sourceDataDeclaration.Type = "percent";
			}
			if (token == "number")
			{
				sourceDataDeclaration.Type = "percent";
			}
			if (token == "percent")
			{
				sourceDataDeclaration.Type = "percent";
			}
			if (token == "datetime")
			{
				sourceDataDeclaration.Type = "percent";
			}
			if (token == "currency")
			{
				sourceDataDeclaration.Type = "percent";
			}
			UpdateCurrentSourceValueView();
		}

		private void toolStripMenuItemSourceValueChangeTypeToDateTime_Click(object sender, EventArgs e)
		{
			if (listSourceValues.SelectedItems.Count != 1) return;

			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;

			var token = sourceDataDeclaration.Type;
			if (token == "string")
			{
				sourceDataDeclaration.Type = "datetime";
			}
			if (token == "text")
			{
				sourceDataDeclaration.Type = "datetime";
			}
			if (token == "number")
			{
				sourceDataDeclaration.Type = "datetime";
			}
			if (token == "percent")
			{
				sourceDataDeclaration.Type = "datetime";
			}
			if (token == "datetime")
			{
				sourceDataDeclaration.Type = "datetime";
			}
			if (token == "currency")
			{
				sourceDataDeclaration.Type = "datetime";
			}
			UpdateCurrentSourceValueView();
		}

		private void toolStripMenuItemSourceValueChangeTypeToCurrency_Click(object sender, EventArgs e)
		{
			if (listSourceValues.SelectedItems.Count != 1) return;

			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;

			var token = sourceDataDeclaration.Type;
			if (token == "string")
			{
				sourceDataDeclaration.Type = "currency";
			}
			if (token == "text")
			{
				sourceDataDeclaration.Type = "currency";
			}
			if (token == "number")
			{
				sourceDataDeclaration.Type = "currency";
			}
			if (token == "percent")
			{
				sourceDataDeclaration.Type = "currency";
			}
			if (token == "datetime")
			{
				sourceDataDeclaration.Type = "currency";
			}
			if (token == "currency")
			{
				sourceDataDeclaration.Type = "currency";
			}
			UpdateCurrentSourceValueView();
		}

		private void listSourceValues_SelectedIndexChanged(object sender, EventArgs e)
		{
			propertyGridSourceValue.SelectedObject = listSourceValues.SelectedItems.Count == 1 ? listSourceValues.SelectedItems[0].Tag : null;
		}

		private void listSourceValues_DoubleClick(object sender, EventArgs e)
		{
			if (listSourceValues.SelectedItems.Count != 1) return;
			var sourceDataDeclaration = listSourceValues.SelectedItems[0].Tag as SourceValueDeclaration;
			if (sourceDataDeclaration == null) return;

			var editor = SourceValueEditorBase.CreateEditorFromTypeToken(sourceDataDeclaration.Type);
			if (editor == null) return;

			editor.ValueDeclaration = sourceDataDeclaration;
			editor.Value = sourceDataDeclaration.ValueForTesting;
			editor.ValueVPath = ""; // sourceDataDeclaration.RepositoryValuePath;
			if (editor.ShowDialog(this) == DialogResult.OK)
			{
				sourceDataDeclaration.ValueForTesting = editor.Value;
				propertyGridSourceValue.SelectedObject = sourceDataDeclaration;
			}
		}

		#endregion

		#region Declarations

		private void createFormulaDeclarationView(string type)
		{
			if (Scenario == null) return;

			var name = Interactor.Prompt("Укажите наименование формулы", "");
			if (name == null) return;
			Application.DoEvents();

			var formulaDeclaration = SelectedNode.AddFormulaDeclaration(type);
			formulaDeclaration.Name = name;

			var editor = new FormulaEditor();
			Func<IBranch> branchSelector = () => ScenarionBrowser.SelectBranch(Scenario, SelectedNode.Model, formulaDeclaration.Model);
			Func<IBranch> nodeSelector = () => ScenarionBrowser.SelectNode(Scenario, SelectedNode.Model);
			editor.elfEditor.Ctx = new TillerIntegrationContext(Scenario.Cast<ScenarioNode>().First().Model, formulaDeclaration.Model, branchSelector, nodeSelector);
			editor.elfEditor.EnterLockedAssignmentMode(formulaDeclaration.Model.VPath.ToElfIdentifier() + " = ?");
			Application.DoEvents();
			if (editor.ShowDialog(this) == DialogResult.OK)
			{
				var code = editor.elfEditor.ElfCode.ToCanonicalElf().RenderCanonicalElfAsPublicText(editor.elfEditor.Ctx);
				formulaDeclaration.HumanText = code;
				formulaDeclaration.ElfCode = editor.elfEditor.ElfCode;
			}
			else
			{
				formulaDeclaration.Model.Delete();
				return;
			}

			var item = listDeclarations.Items.Add(formulaDeclaration.Name);
			item.Tag = formulaDeclaration;
			formulaDeclaration.NameChanged += x => item.Text = x;
			item.SubItems.Add(formulaDeclaration.HumanType);
			item.Selected = true;
		}

		private void menuItemDeclarationCreateString_Click(object sender, EventArgs e)
		{
			createFormulaDeclarationView("string");
		}

		private void menuItemDeclarationCreateText_Click(object sender, EventArgs e)
		{
			createFormulaDeclarationView("text");
		}

		private void menuItemDeclarationCreateNumber_Click(object sender, EventArgs e)
		{
			createFormulaDeclarationView("number");
		}

		private void menuItemDeclarationCreatePercent_Click(object sender, EventArgs e)
		{
			createFormulaDeclarationView("percent");
		}

		private void menuItemDeclarationCreateDate_Click(object sender, EventArgs e)
		{
			createFormulaDeclarationView("datetime");
		}

		private void menuItemDeclarationCreateCurrency_Click(object sender, EventArgs e)
		{
			createFormulaDeclarationView("currency");
		}

		private void menuItemDeclarationEdit_Click(object sender, EventArgs e)
		{
			listDeclarations_DoubleClick(sender, e);
		}

		private void UpdateCurrentDeclarationView()
		{
			if (listDeclarations.SelectedItems.Count != 1) return;

			var formulaDeclaration = listDeclarations.SelectedItems[0].Tag as FormulaDeclaration;
			if (formulaDeclaration == null) return;

			listDeclarations.SelectedItems[0].SubItems[1].Text = formulaDeclaration.HumanType;
		}

		private void toolStripMenuItemDeclarationChangeTypeToCurrency_Click(object sender, EventArgs e)
		{
			if (listDeclarations.SelectedItems.Count != 1) return;

			var formulaDeclaration = listDeclarations.SelectedItems[0].Tag as FormulaDeclaration;
			if (formulaDeclaration == null) return;

			var token = formulaDeclaration.Type;
			if (token == "string")
			{
				formulaDeclaration.Type = "currency";
			}
			if (token == "text")
			{
				formulaDeclaration.Type = "currency";
			}
			if (token == "number")
			{
				formulaDeclaration.Type = "currency";
			}
			if (token == "percent")
			{
				formulaDeclaration.Type = "currency";
			}
			if (token == "datetime")
			{
				formulaDeclaration.Type = "currency";
			}
			if (token == "currency")
			{
				formulaDeclaration.Type = "currency";
			}
			UpdateCurrentDeclarationView();
		}

		private void toolStripMenuItemDeclarationChangeTypeToNumber_Click(object sender, EventArgs e)
		{
			if (listDeclarations.SelectedItems.Count != 1) return;

			var formulaDeclaration = listDeclarations.SelectedItems[0].Tag as FormulaDeclaration;
			if (formulaDeclaration == null) return;

			var token = formulaDeclaration.Type;
			if (token == "string")
			{
				formulaDeclaration.Type = "number";
			}
			if (token == "text")
			{
				formulaDeclaration.Type = "number";
			}
			if (token == "number")
			{
				formulaDeclaration.Type = "number";
			}
			if (token == "percent")
			{
				formulaDeclaration.Type = "number";
			}
			if (token == "datetime")
			{
				formulaDeclaration.Type = "number";
			}
			if (token == "currency")
			{
				formulaDeclaration.Type = "number";
			}
			UpdateCurrentDeclarationView();
		}

		private void toolStripMenuItemDeclarationChangeTypeToPercent_Click(object sender, EventArgs e)
		{
			if (listDeclarations.SelectedItems.Count != 1) return;

			var formulaDeclaration = listDeclarations.SelectedItems[0].Tag as FormulaDeclaration;
			if (formulaDeclaration == null) return;

			var token = formulaDeclaration.Type;
			if (token == "string")
			{
				formulaDeclaration.Type = "percent";
			}
			if (token == "text")
			{
				formulaDeclaration.Type = "percent";
			}
			if (token == "number")
			{
				formulaDeclaration.Type = "percent";
			}
			if (token == "percent")
			{
				formulaDeclaration.Type = "percent";
			}
			if (token == "datetime")
			{
				formulaDeclaration.Type = "percent";
			}
			if (token == "currency")
			{
				formulaDeclaration.Type = "percent";
			}
			UpdateCurrentDeclarationView();
		}

		private void listDeclarations_DoubleClick(object sender, EventArgs e)
		{
			if (Scenario == null) return;
			if (listDeclarations.SelectedItems.Count != 1) return;

			var formulaDeclaration = listDeclarations.SelectedItems[0].Tag as FormulaDeclaration;
			if (formulaDeclaration == null) return;

			Application.DoEvents();

			var editor = new FormulaEditor();
			Func<IBranch> branchSelector = () => ScenarionBrowser.SelectBranch(Scenario, SelectedNode.Model, formulaDeclaration.Model);
			Func<IBranch> nodeSelector = () => ScenarionBrowser.SelectNode(Scenario, SelectedNode.Model);
			editor.elfEditor.Ctx = new TillerIntegrationContext(Scenario.Cast<ScenarioNode>().First().Model, formulaDeclaration.Model, branchSelector, nodeSelector);
			editor.elfEditor.EnterLockedAssignmentMode(formulaDeclaration.ElfCode);
			//editor.elfEditor.ElfCode = formulaDeclaration.ElfCode;

			Application.DoEvents();
			if (editor.ShowDialog(this) == DialogResult.OK)
			{
				var code = editor.elfEditor.ElfCode.ToCanonicalElf().RenderCanonicalElfAsPublicText(editor.elfEditor.Ctx);
				formulaDeclaration.HumanText = code;
				formulaDeclaration.ElfCode = editor.elfEditor.ElfCode;
				textFormulaView.Text = code;
			}
		}

		private void menuItemDeclarationDelete_Click(object sender, EventArgs e)
		{
			if (listDeclarations.SelectedItems.Count != 1) return;

			var formulaDeclaration = listDeclarations.SelectedItems[0].Tag as FormulaDeclaration;
			listDeclarations.SelectedItems[0].Remove();
			formulaDeclaration.Model.Delete();
		}

		private void menuItemDeclarationCopy_Click(object sender, EventArgs e)
		{
			if (listDeclarations.SelectedItems.Count != 1) return;

			var formulaDeclaration = listDeclarations.SelectedItems[0].Tag as FormulaDeclaration;
			var value = formulaDeclaration.Model;

			var html = string.Format("<span vpath='{1}' style='background-color:#ff9900' contenteditable='false'>&lt; {0} &gt;</span>", formulaDeclaration.Name, value.VPath.Path);
			Clipboard.SetText(html, TextDataFormat.Html);
		}

		private void listDeclarations_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listDeclarations.SelectedItems.Count == 1)
			{
				var formula = listDeclarations.SelectedItems[0].Tag as FormulaDeclaration;
				if (formula == null) return;
				textFormulaView.Text = formula.HumanText;
			}
			else
			{
				textFormulaView.Text = "";
			}
		}

		#endregion

		private void menuItemChildShouldBeSelected_Click(object sender, EventArgs e)
		{
			var conditionDeclaration = SelectedNode.AddConditionDeclaration();
			conditionDeclaration.Name = "Если текущий узел выбран, то нужно выбрать один дочерний узел";
			conditionDeclaration.Text =
				@"ЕСЛИ Выбран(ТекущийУзел) ТО
	Выбрать(ДочернийУзел)
ИНАЧЕ
	Ошибка(""При выборе данного узла обязателен выбор одного дочернего узла"")
КОНЕЦ
";

			var item = listConditions.Items.Add(conditionDeclaration.Name);
			item.Tag = conditionDeclaration;
			item.Selected = true;
		}

		private void menuItemDeleteCondition_Click(object sender, EventArgs e)
		{
			if (listConditions.SelectedItems.Count != 1) return;

			var conditionDeclaration = listConditions.SelectedItems[0].Tag as ConditionDeclaration;
			listConditions.SelectedItems[0].Remove();
			conditionDeclaration.Model.Delete();
		}

		private void listConditions_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listConditions.SelectedItems.Count == 1)
			{
				var cond = listConditions.SelectedItems[0].Tag as ConditionDeclaration;
				textConditionView.Text = cond.Text;
			}
			else
			{
				textConditionView.Text = "";
			}
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			if (Scenario == null) return;

			Func<IBranch, ScenarioNodeType> nodeType = b => b.GetValue("nodeType") == null
			                                                	? ScenarioNodeType.Default
			                                                	:
			                                                		(ScenarioNodeType) Enum.Parse(typeof (ScenarioNodeType), b.GetValue("nodeType").ContentString);
			var all = Scenario.Vault.GetBranchesRecursive().ToDictionary(b => b, nodeType);
			var forExport = all.FirstOrDefault(kvp => kvp.Value == ScenarioNodeType.ForExport).Key;

			if (forExport != null)
			{
				// todo. check whether repository editor is already open (e.g. in a neighbour MDI window)
				using (var repo = RepositoryEditor.Repository())
				{
					var now = DateTime.Now;
					var name = String.Format("{0:00}.{1:00}.{2:0000} {3:00}{4:00}{5:00}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Second);
					var root = repo.GetOrCreateBranch("Для выгрузки");
					var receiver = root.GetOrCreateBranch(name);
					receiver.ImportBranch(forExport);
					repo.Save();
				}
			}
		}

		private void menuItemReportGenerate_Click(object sender, EventArgs e)
		{
			var generateReportDialog = new GenerateReportDialog {Scenario = Scenario, CheckedIds = apprasialX.CheckedIds};
			//generateReportDialog.Tree = appraisalOne.treeAppraisal;
			generateReportDialog.ShowDialog(this);
		}

		private void menuItemReportExport_Click(object sender, EventArgs e)
		{
			
#if DEBUG && USE_TDISK
			var basePath = Path.Combine(@"T:\", Scenario.LastReportId.ToString());
#else
			var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Scenario.LastReportId.ToString());
#endif
			var reportFile = new FileInfo(basePath + ".doc");
			//var calcFile = new FileInfo(basePath + ".c.html");
			//var sourceDataFile = new FileInfo(basePath + ".s.html");

			if (!reportFile.Exists)
			{
				MessageBox.Show(this, "Для того, чтобы выгрузить отчет, последний должен быть сперва сгенерирован", "Модуль выгрузки данных", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
				return;
			}

			var fullPath = "";
			var ww = new WaitWindow();

			try
			{
				ww.Show(this);
				Func<IBranch, ScenarioNodeType> nodeType = b => b.GetValue("nodeType") == null
				                                                	? ScenarioNodeType.Default
				                                                	:
				                                                		(ScenarioNodeType) Enum.Parse(typeof (ScenarioNodeType), b.GetValue("nodeType").ContentString);
				var all = Scenario.Vault.GetBranchesRecursive().ToDictionary(b => b, nodeType);
				var forExport = all.Where(kvp => kvp.Value == ScenarioNodeType.ForExport).FirstOrDefault();

				Application.DoEvents();

				using (var repo = RepositoryEditor.Repository())
				{
					var now = DateTime.Now;
					var name = String.Format("{0:00}.{1:00}.{2:0000} {3:00}{4:00}{5:00}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Second);
					var root = repo.GetOrCreateBranch("Для выгрузки");
					var receiver = root.GetOrCreateBranch(name);
					fullPath = receiver.VPath.Path;

					if (!TryCreateFileStreamForTempCopy(reportFile, x => receiver.CreateValue("Отчет.doc", x).SetTypeToken2("binary"))) return;

					if (forExport.Key != null)
					{
						var node = new ScenarioNode(forExport.Key);

						//#warning Review this: eval sessions now lock the vault for reading

#if VAULT_EVAL_1
                    using (_evalSession1 = new Esath.Eval.Ver1.EvalSession(Scenario.Vault, repo))
                    {
#endif

#if VAULT_EVAL_2
						using (_evalSession2 = new Esath.Eval.Ver2.EvalSession(Scenario.Vault, repo, null))
						{
#endif

#if VAULT_EVAL_3
                    using (_evalSession3 = new Esath.Eval.Ver3.EvalSession(Scenario.VaultCompiler, repo))
                    {
#endif

							var source = Regex.Replace(node.Template, StartUp.VFIELD_PATTERN, vpathEvaluator, RegexOptions.Compiled);
							source = "<html><head></head><body>" + source + "</body></html>";
							receiver.CreateValue("Расчетные Данные.html", new TempHtmlFile(source).Content).SetTypeToken2("binary");
						}
					}

					var buf = new StringBuilder(32*1024);
					buf.Append("<html><head></head><body>");
					buf.Append("<table border=1>");
					buf.Append("<tr><th>Имя</th><th>Значение</th><th>Ед Изм</th><th>Тип</th></tr>");
					foreach (var sourceValueDeclaration in Scenario.AllSourceValueDeclarations)
					{
						buf.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
						                 sourceValueDeclaration.Name,
						                 sourceValueDeclaration.Value,
						                 sourceValueDeclaration.MeasurementUnit == "" ? "&nbsp;" : sourceValueDeclaration.MeasurementUnit,
						                 sourceValueDeclaration.HumanType
							);
					}
					buf.Append("</table>");
					buf.Append("</body></html>");
					receiver.CreateValue("Исходные Данные.html", new TempHtmlFile(buf.ToString()).Content).SetTypeToken2("binary");

					repo.Save();
				}
			}
			finally
			{
				ww.Hide();
				ww = null;
				Application.DoEvents();
			}
			MessageBox.Show(this, string.Format("Данные текущего отчета успешно выгружены в ветвь Репозитория '{0}'", fullPath), "Модуль выгрузки данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private bool TryCreateFileStreamForTempCopy(FileSystemInfo file, Action<Stream> action)
		{
			FileStream fs;
			while (true)
			{
				var target = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "." + file.Extension);
				try
				{
					File.Copy(file.FullName, target, true);
					fs = new FileStream(target, FileMode.Open, FileAccess.Read);
					break;
				}
				catch (IOException e)
				{
					if (MessageBox.Show(this, "Ошибка: " + e.Message, "Ошибка ввода-вывода", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Retry) return false;
				}
				catch
				{
					return false;
				}
			}
			action(fs);
			return true;
		}

		private void Scenarion_Load(object sender, EventArgs e)
		{
			if (MdiParent != null) MdiParent.Activate();
		}

		private const string SPECIAL_ROW_BG_COLOR = "#FFFF33";
		private const string SPECIAL_ROW_TITLE = "Агрегатная строка";
		private const string TOTAL_ROW_BG_COLOR = "#009999";
		private const string TOTAL_ROW_TITLE = "Строка 'Итого'";

		private const string OM_VARIANT_NAME = "OM__VARIATION";
		private const string OM_VARIATION_ROW_AGGREGATE = "OM_VARIATION_ROW_AGGREGATE";
		private const string OM_VARIATION_ROW_TOTAL = "OM_VARIATION_ROW_TOTAL";

		private void templateEditor_QueryRowIsSpecial(object sender, Microsoft.ConsultingServices.HtmlEditor.HtmlElementEventArgs e)
		{
			e.Flag = OM_VARIATION_ROW_AGGREGATE.Equals((e.DomElement as IHTMLElement).getAttribute(OM_VARIANT_NAME), StringComparison.InvariantCultureIgnoreCase);
		}

		private void templateEditor_QueryRowIsTotal(object sender, Microsoft.ConsultingServices.HtmlEditor.HtmlElementEventArgs e)
		{
			e.Flag = OM_VARIATION_ROW_TOTAL.Equals((e.DomElement as IHTMLElement).getAttribute(OM_VARIANT_NAME), StringComparison.InvariantCultureIgnoreCase);
		}

		private void templateEditor_ButtonRowSpecialClicked(object sender, Microsoft.ConsultingServices.HtmlEditor.HtmlElementEventArgs e)
		{
			var row = e.DomElement as IHTMLTableRow;
			var element = e.DomElement as IHTMLElement;
			if (OM_VARIATION_ROW_AGGREGATE.Equals(element.getAttribute(OM_VARIANT_NAME), StringComparison.InvariantCultureIgnoreCase))
			{
				row.bgColor = null;
				element.title = null;
				element.removeAttribute(OM_VARIANT_NAME);
			}
			else
			{
				row.bgColor = SPECIAL_ROW_BG_COLOR;
				element.title = SPECIAL_ROW_TITLE;
				element.setAttribute(OM_VARIANT_NAME, OM_VARIATION_ROW_AGGREGATE);
			}
			e.Flag = true;
		}

		private void templateEditor_ButtonRowTotalClicked(object sender, Microsoft.ConsultingServices.HtmlEditor.HtmlElementEventArgs e)
		{
			var row = e.DomElement as IHTMLTableRow;
			var element = e.DomElement as IHTMLElement;
			if (OM_VARIATION_ROW_TOTAL.Equals(element.getAttribute(OM_VARIANT_NAME), StringComparison.InvariantCultureIgnoreCase))
			{
				row.bgColor = null;
				element.title = null;
				element.removeAttribute(OM_VARIANT_NAME);
			}
			else
			{
				row.bgColor = TOTAL_ROW_BG_COLOR;
				element.title = TOTAL_ROW_TITLE;
				element.setAttribute(OM_VARIANT_NAME, OM_VARIATION_ROW_TOTAL);
			}
			e.Flag = true;
		}

		private void templateEditor_HtmlChanged(object sender, EventArgs e)
		{
			var s = templateEditor.InnerHtml;
			s = s.Replace("<o:p>&nbsp;</o:p>", "");
			s = Regex.Replace(s, @"<(SPAN)[^>]*></\1>", "", RegexOptions.Compiled);
			s = Regex.Replace(s, @"<(FONT)[^>]*></\1>", "", RegexOptions.Compiled);
			s = Regex.Replace(s, @"<(B)[^>]*></\1>", "", RegexOptions.Compiled);
			s = Regex.Replace(s, @"<(P)[^>]*></\1>", "", RegexOptions.Compiled);
			templateEditor.InnerHtml = s;
		}
	}
}