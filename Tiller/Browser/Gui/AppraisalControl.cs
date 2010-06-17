using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Esath.Eval.Ver2;
using Esath.Eval.Ver3;
using DataVault.Core.Api;

/*

namespace Browser.Gui
{
	using System.Text.RegularExpressions;
	using Dialog;
	using Editor;
	using Esath.Eval.Ver1;
	using Report;

	public partial class AppraisalControl : UserControl
	{
		private Dictionary<Guid, TreeNode> _map;

		public AppraisalControl()
		{
			InitializeComponent();
		}

		private IVault _scenarioVault;

		public IVault ScenarioVault
		{
			get { return _scenarioVault; }
			set
			{
				_map = new Dictionary<Guid, TreeNode>(1024);
				_scenarioVault = value;
				BuildView();
			}
		}

#if VAULT_EVAL_3
	    public IVaultCompiler ScenarioCompiler { get; set; }
#endif

		private bool _buildingView;

		private void BuildView()
		{
			if (ScenarioVault == null) return;
			_buildingView = true;

			try
			{
				treeAppraisal.Nodes.Clear();
				var scene = ScenarioVault.GetOrCreateBranch(new VPath("Scenario"));

				var common = scene.GetOrCreateBranch(new VPath("Common"));
				common.GetOrCreateValue(new VPath("name"), "Общие Характеристики");
				common.GetOrCreateValue(new VPath("id"), "@Common");

				LoadBranch(common, null);

				var particular = scene.GetOrCreateBranch(new VPath("Particular"));
				particular.GetOrCreateValue(new VPath("name"), "Объект Оценки №1");
				particular.GetOrCreateValue(new VPath("id"), "@Particular");

				LoadBranch(particular, null);
				treeAppraisal.SelectedNode = treeAppraisal.Nodes[0];
			}
			finally
			{
				_buildingView = false;
			}
		}

		private void LoadBranch(IBranch branch, TreeNode parent)
		{
			if (parent == null) // root
			{
				parent = new TreeNode
				         	{
				         		Text = branch.GetOrCreateValue(new VPath("name"), branch.Name).ContentString,
				         		Tag = branch,
				         		Name = branch.GetOrCreateValue(new VPath("id"), Guid.NewGuid().ToString()).ContentString
				         	};
				if (branch.GetOrCreateValue("isRequired", "false").ContentString == "true") parent.Checked = true;
				treeAppraisal.Nodes.Add(parent);
				_map[branch.Id] = parent;
			}
			foreach (var b in branch.GetBranches())
			{
				if (b.Name.StartsWith("_")) continue;
				var n = new TreeNode
				        	{
				        		Text = b.GetOrCreateValue("name", Guid.NewGuid().ToString().Replace('-', '_')).ContentString,
				        		Tag = b,
				        		Name = b.GetOrCreateValue(new VPath("id"), Guid.NewGuid().ToString().Replace('-', '_')).ContentString
				        	};
				parent.Nodes.Add(n);
				if (b.GetOrCreateValue("isRequired", "false").ContentString == "true") n.Checked = true;
				_map[b.Id] = n;
				IBranch cb;
				if ((cb = b.GetBranch("_conditions"))!=null && cb.GetBranches().Count()>0)n.NodeFont = new Font("Tahoma", 8, FontStyle.Underline);
				LoadBranch(b, n);
			}
		}

		public ScenarioNode SelectedNode
		{
			get { return new ScenarioNode { Model = treeAppraisal.SelectedNode.Tag as IBranch}; }
		}

		private void AppraisalControl_Load(object sender, EventArgs e)
		{
		}

		private bool _inAfterCheck;

		private void AfterCheck(ScenarioNode scenarioNode)
		{
			if (scenarioNode.ConditionDeclarations.Where(x => x.Handler == "").Count() > 0)
			{
				// default handler
				if (scenarioNode.View.Checked)
				{
					MessageBox.Show(this, "При выборе данного узла обязателен выбор одного дочернего узла", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					scenarioNode.View.Expand();
					var checkedCount = scenarioNode.View.Nodes.Cast<TreeNode>().Where(x => x.Checked).Count();
					if (checkedCount == 0 && scenarioNode.View.Nodes.Count > 0) scenarioNode.View.Nodes[0].Checked = true;
				}
				else
				{
					foreach (TreeNode node in scenarioNode.View.Nodes) node.Checked = false;
				}
				return;
			}
			var pn = new ScenarioNode {View = scenarioNode.View.Parent, Model = scenarioNode.View.Parent.Tag as IBranch};
			if (pn.ConditionDeclarations.Where(x => x.Handler == "").Count() > 0)
			{
				foreach (TreeNode node in pn.View.Nodes) node.Checked = false;
				scenarioNode.View.Checked = true;
				scenarioNode.View.Parent.Checked = true;
			}
		}

		private void treeAppraisal_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (_buildingView || UseWaitCursor) return;
			if (_inAfterCheck || e.Node.Parent == null /*|| e.Node.Parent.Parent == null/) return;
			_inAfterCheck = true;
			try
			{
				AfterCheck(new ScenarioNode {View = e.Node, Model = e.Node.Tag as IBranch});
			}
			catch (Exception opps)
			{
				MessageBox.Show(this, opps.Message);
			}
			_inAfterCheck = false;

			try
			{
				treeAppraisal_AfterSelect(treeAppraisal, new TreeViewEventArgs(treeAppraisal.SelectedNode));
			}
			catch (Exception opps)
			{
				MessageBox.Show(this, opps.Message);
			}
		}

		private void treeAppraisal_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (UseWaitCursor)return;
			UseWaitCursor = true;
			try
			{
				var node = SelectedNode;

				var tabTemplateVisible = false;
				var tabDataVisible = false;

				if (e.Node.Parent == null)
				{
					tabTemplateVisible = tabDataVisible = false;
				}
				else
				{
				}
				tabTemplateVisible = !string.IsNullOrEmpty(node.Template);
				tabDataVisible = node.SourceValueDeclarations.Count() > 0;


				if (tabTemplateVisible)
					if (((MainForm) TopLevelControl).optionsSlowPreview.Checked)
					{
						SlowPreview();
					}
					else
					{
						var topicNumber = "";
						var title = "";

						var tableCounter = 1;

#warning Review this: eval sessions now lock the vault for reading

#if VAULT_EVAL_1
                using (_evalSession1 = new Esath.Eval.Ver1.EvalSession(ScenarioVault))
                {
#endif

#if VAULT_EVAL_2
						using (_evalSession2 = new Esath.Eval.Ver2.EvalSession(ScenarioVault))
						{
#endif

#if VAULT_EVAL_3
                using (_evalSession3 = new Esath.Eval.Ver3.EvalSession(ScenarioCompiler))
                {
#endif


							var preview = Regex.Replace(node.Template, @"<SPAN[^>]+vpath\s*=\s*""([^""]+)""[^>]*>[^<]+</SPAN>", vpathEvaluator, RegexOptions.Compiled);
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

							webBrowser.DocumentText = preview;

#if VAULT_EVAL_1
                }
#endif

#if VAULT_EVAL_2
						}
#endif

#if VAULT_EVAL_3
                }
#endif
					}
				else
				{
					webBrowser.DocumentText = "";
				}

				if (tabDataVisible)
				{
					listSourceData.Items.Clear();
					foreach (var sourceValueDeclaration in node.SourceValueDeclarations.Where(x => !x.IsHidden))
					{
						var item = new ListViewItem {Text = sourceValueDeclaration.Name};
						item.SubItems.Add(sourceValueDeclaration.MeasurementUnit);
						item.SubItems.Add(sourceValueDeclaration.ValueForTesting);
						item.Tag = sourceValueDeclaration;
						listSourceData.Items.Add(item);
					}
				}
				else
				{
					listSourceData.Items.Clear();
				}

				if (tabDataVisible) tabData.Select();
				else if (tabTemplateVisible) tabTemplate.Select();
			}
			finally
			{
				UseWaitCursor = false;
			}
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
			var vpath = match.Groups[1].Value;

			var branch = ScenarioVault.GetBranch(vpath);
			if (branch == null) return "<span style='background-color:red'>Узел не существует</span>";

			var value = branch.GetValue("declarationType");
			if (value != null)
			{
				string eval = "";

				var type = value.ContentString;
				if (type == "formula")
				{
					var conditions = branch.Parent.Parent.GetBranch("_conditions");
					var isConditional = conditions != null && conditions.GetBranches().Length > 0;
					TreeNode treeNode;
					var index = branch.GetValue("elfCode").ContentString.IndexOf('=');
					if (isConditional && _map.TryGetValue(branch.Parent.Parent.Id, out treeNode) && index > 0)
					{
						treeNode = treeNode.Nodes.Cast<TreeNode>().Where(x => x.Checked).FirstOrDefault();
						if (treeNode != null)
						{
							var scenarioNode = new ScenarioNode {View = treeNode, Model = treeNode.Tag as IBranch};
							var varName = branch.GetValue("elfCode").ContentString.Substring(0, index);
							var realCode = scenarioNode.FormulaDeclarations.Where(x => x.ElfCode.IndexOf(varName) > 0).FirstOrDefault();
							if (realCode != null)
							{
								var lines = realCode.ElfCode.Split('\n');
								lines[lines.Length - 1] = "ret " + lines[lines.Length - 1];
								eval = string.Join(Environment.NewLine, lines);
							}
						}
					}
					try
					{
#if VAULT_EVAL_1
                        eval = _evalSession1.Eval(branch).ToString();
#endif

#if VAULT_EVAL_2
						eval = _evalSession2.Eval(branch, eval).ToString();
#endif

#if VAULT_EVAL_3
                        eval = _evalSession3.Eval(branch).ToString();
#endif
					}
					catch (Exception)
					{
						eval = string.Format("<div style='background-color:red'>Ошибка вычисления формулы</div>");
					}
					return eval;
				}
			}


			return branch.GetValue("valueForTesting").ContentString;
		}

		#region slow preview

		private Dictionary<Guid, TreeNode> _slowMap;

		private IEnumerable<TreeNode> ExpandTree()
		{
			_slowMap = new Dictionary<Guid, TreeNode>(1024);
			var list = new List<TreeNode>();
			list.AddRange(ExpandTreeNode(treeAppraisal.Nodes[0].Nodes));
			list.AddRange(ExpandTreeNode(treeAppraisal.Nodes[1].Nodes));
			return list;
		}

		private IEnumerable<TreeNode> ExpandTreeNode(TreeNodeCollection collection)
		{
			var list = new List<TreeNode>();
			foreach (TreeNode node in collection)
			{
				_slowMap[(node.Tag as IBranch).Id] = node;
				list.Add(node);
				list.AddRange(ExpandTreeNode(node.Nodes));
			}
			return list;
		}

		private void SlowPreview()
		{
			if (ScenarioVault == null) return;

			var levelage = new Levelage();
			var source = "";


			//#warning Review this: eval sessions now lock the vault for reading

#if VAULT_EVAL_1
            using (_evalSession1 = new Esath.Eval.Ver1.EvalSession(Scenario.Vault))
            {
#endif

#if VAULT_EVAL_2
			using (_evalSession2 = new Esath.Eval.Ver2.EvalSession(ScenarioVault))
			{
#endif

#if VAULT_EVAL_3
            using (_evalSession3 = new Esath.Eval.Ver3.EvalSession(Scenario.VaultCompiler))
            {
//#warning Review this: report generation now blocks until the scenario is compiled
                // reasoning for the decision described in the warning:
                // 1) report generation usually covers shitloads of pages => resorting to eval2 will lead to significant performance hit
                // todo. the hi-fi solution here would be gathering statistics and calculating what's better: wait+rush or just walk slowly
                // todo. by calculating i also mean asking worker how much time he has got left
                // todo. make sure that estimating amount of job to do (nodes in report) ain't longer than the job itself ))))
                // 2) usually, user will spend quite some time selecting necessary nodes for the report.
                //    while he/she is fucking around and not modifying the vault, compiler has a good chance to finish compilation in background

                // the line below is correct because we've locked the vault when entered the session
                // thus, after we wait for the the compiler to complete, noone can edit the vault and ruin our effort
                Scenario.VaultCompiler.GetCompiledSync();

#endif


				foreach (var branch in from node in ExpandTree()
				                       let sn = new ScenarioNode {Model = node.Tag as IBranch, View = node}
				                       where node.Checked || sn.IsRequired && !(string.IsNullOrEmpty(sn.Template) && string.IsNullOrEmpty(sn.Title)) || node == treeAppraisal.SelectedNode
				                       orderby sn.SortingWeight
				                       select sn)
				{
					Application.DoEvents();

					source = Regex.Replace(branch.Template ?? "", @"<SPAN[^>]+vpath\s*=\s*""([^""]+)""[^>]*>[^<]+</SPAN>", VpathEvaluatorSlow, RegexOptions.Compiled);

					var title = "";

					if (branch.Title != "")
					{
						title = branch.Title;
						switch (branch.NodeType)
						{
							case ScenarioNodeType.Topic:
								levelage.Enter(1);
								title = string.Format("<hr><h{1}>{2} {0}</h{1}>", title, "1", levelage);
								break;
							case ScenarioNodeType.Subtopic2:
								levelage.Enter(2);
								title = string.Format("<hr><h{1}>{2} {0}</h{1}>", title, "2", levelage);
								break;
							case ScenarioNodeType.Subtopic3:
								levelage.Enter(3);
								title = string.Format("<hr><h{1}>{2} {0}</h{1}>", title, "3", levelage);
								break;
							case ScenarioNodeType.Subtopic4:
								levelage.Enter(4);
								title = string.Format("<hr><h{1}>{2} {0}</h{1}>", title, "4", levelage);
								break;
							case ScenarioNodeType.Subtopic5:
								levelage.Enter(5);
								title = string.Format("<hr><h{1}>{2} {0}</h{1}>", title, "5", levelage);
								break;
							default:
								break;
						}
					}

					source = Regex.Replace(source, @"<table\s", match => string.Format(@"<div style='text-align:right'>Таблица {0}</div><table ", levelage.AddTable()), RegexOptions.Compiled | RegexOptions.IgnoreCase);
					if (title != "") source = title + source;

					if (branch.View == treeAppraisal.SelectedNode) break;
				}

#if VAULT_EVAL_1
                }
#endif

#if VAULT_EVAL_2
			}
#endif

#if VAULT_EVAL_3
            }
#endif


			webBrowser.DocumentText = "<html><head>" + GenerateReportDialog.Styles + "</head><body>" + source + "</body></html>";
		}

		private string VpathEvaluatorSlow(Match match)
		{
			var vpath = match.Groups[1].Value;

			var branch = ScenarioVault.GetBranch(vpath);
			if (branch == null) return "<span style='background-color:red'>Узел не существует</span>";

			var value = branch.GetValue("declarationType");
			if (value != null)
			{
				var type = value.ContentString;
				if (type == "formula")
				{
					string eval = null;

					var conditions = branch.Parent.Parent.GetBranch("_conditions");
					var isConditional = conditions != null && conditions.GetBranches().Length > 0;
					TreeNode treeNode;
					var index = branch.GetValue("elfCode").ContentString.IndexOf('=');
					if (isConditional && _map.TryGetValue(branch.Parent.Parent.Id, out treeNode) && index > 0)
					{
						treeNode = treeNode.Nodes.Cast<TreeNode>().Where(x => x.Checked).FirstOrDefault();
						if (treeNode != null)
						{
							var scenarioNode = new ScenarioNode {View = treeNode, Model = treeNode.Tag as IBranch};
							var varName = branch.GetValue("elfCode").ContentString.Substring(0, index);
							var realCode = scenarioNode.FormulaDeclarations.Where(x => x.ElfCode.IndexOf(varName) > 0).FirstOrDefault();
							if (realCode != null)
							{
								var lines = realCode.ElfCode.Split('\n');
								lines[lines.Length - 1] = "ret " + lines[lines.Length - 1];
								eval = string.Join(Environment.NewLine, lines);
							}
						}
					}

					try
					{
#if VAULT_EVAL_1
                        eval = _evalSession1.Eval(branch).ToString();
#endif

#if VAULT_EVAL_2
						eval = _evalSession2.Eval(branch, eval).ToString();
#endif

#if VAULT_EVAL_3
                        eval = _evalSession3.Eval(branch).ToString();
#endif
					}
					catch (Exception)
					{
						eval = string.Format("<div style='background-color:red'>Ошибка вычисления формулы</div>");
					}
					return eval;
				}
			}

			var selfValue = branch.GetValue("valueForTesting").ContentString;

			if (string.IsNullOrEmpty(selfValue))
			{
				var repositoryVPath = branch.GetValue("repositoryValue").ContentString;

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

		#endregion

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.TabPage == tabTemplate)
			{
				if (treeAppraisal.SelectedNode != null)
					treeAppraisal_AfterSelect(this, new TreeViewEventArgs(treeAppraisal.SelectedNode, TreeViewAction.Unknown));
			}
			else if (e.TabPage == tabData)
			{
			}
		}

		private void listSourceData_DoubleClick(object sender, EventArgs e)
		{
			if (listSourceData.SelectedIndices.Count == 1)
			{
				var item = listSourceData.SelectedItems[0];
				var sourceDataDeclaration = listSourceData.SelectedItems[0].Tag as SourceValueDeclaration;
				if (sourceDataDeclaration == null) return;

				var editor = SourceValueEditorBase.CreateEditorFromTypeToken(sourceDataDeclaration.Type);
				if (editor == null) return;

				editor.ValueDeclaration = sourceDataDeclaration;
				editor.Value = sourceDataDeclaration.ValueForTesting;
				editor.ValueVPath = sourceDataDeclaration.RepositoryValuePath;
				if (editor.ShowDialog(this) == DialogResult.OK)
				{
					sourceDataDeclaration.ValueForTesting = editor.Value;
					item.SubItems[2].Text = sourceDataDeclaration.ValueForTesting;
				}
			}
		}

		private void treeAppraisal_BeforeCheck(object sender, TreeViewCancelEventArgs e)
		{
			if (_buildingView) return;
			var branch = e.Node.Tag as IBranch;
			if (branch != null && branch.GetOrCreateValue("isRequired", "false").ContentString == "true")
			{
				//MessageBox.Show(this, "Данный узел является обязательным. Обязательные узлы всегда включаются в отчет.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				e.Cancel = true;
				return;
			}
		}
	}
}
*/