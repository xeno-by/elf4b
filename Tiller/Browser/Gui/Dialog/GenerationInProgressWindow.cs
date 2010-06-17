using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Browser.Gui.Dialog
{
	using System.Text.RegularExpressions;
	using global::DataVault.Core.Api;
	using Report;

	public partial class GenerationInProgressWindow : Form
	{
		public GenerationInProgressWindow()
		{
			InitializeComponent();
		}

		public ScenarioDepot Scenario { get; set; }
		public TreeView Tree { get; set; }
		public Action<StringBuilder, StringBuilder> CallBack { get; set; }

		private IEnumerable<TreeNode> ExpandTree()
		{
			_map = new Dictionary<Guid, TreeNode>(1024);
			var list = new List<TreeNode>();
			list.AddRange(ExpandTreeNode(Tree.Nodes[0].Nodes));
			list.AddRange(ExpandTreeNode(Tree.Nodes[1].Nodes));
			return list;
		}

		private IEnumerable<TreeNode> ExpandTreeNode(TreeNodeCollection collection)
		{
			var list = new List<TreeNode>();
			foreach (TreeNode node in collection)
			{
				_map[(node.Tag as IBranch).Id] = node;
				list.Add(node);
				list.AddRange(ExpandTreeNode(node.Nodes));
			}
			return list;
		}

		private Dictionary<Guid, TreeNode> _map;
		private Esath.Eval.Ver2.EvalSession _evalSession2;

		public void GenerateAll(IWin32Window parent, string infoText)
		{
			if (Scenario == null || Tree == null || CallBack == null) return;
			labelInfo.Text = infoText;
			Application.DoEvents();
			ShowDialog(parent);
		}

		private string vpathEvaluator(Match match)
		{
			if (Scenario == null) return "";

			var vpath = match.Groups[1].Value;

			var branch = Scenario.Vault.GetBranch(vpath);
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
							var scenarioNode = new ScenarioNode(treeNode.Tag as IBranch);
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
						eval = _evalSession2.Eval(branch, eval).ToString();
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

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			Action action = (() => progressBar.Value = e.ProgressPercentage);
			if (InvokeRequired) Invoke(action);
			else action();
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				return;
			}

			if (e.Result != null)
			{
				CallBack(((StringBuilder[]) e.Result)[0], ((StringBuilder[]) e.Result)[1]);
			}
			DialogResult = DialogResult.OK;
		}

		private void GenerationInProgressWindow_Shown(object sender, EventArgs e)
		{
			progressBar.Value = progressBar.Minimum;
			progressBar.Visible = buttonCancel.Visible = true;
			Application.DoEvents();
			backgroundWorker.RunWorkerAsync();
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			var levelage = new Levelage();

			var nodes = from node in ExpandTree()
			            let sn = new ScenarioNode(node.Tag as IBranch)
			            where node.Checked && !(string.IsNullOrEmpty(sn.Template) && string.IsNullOrEmpty(sn.Title))
			            orderby sn.SortingWeight
			            select sn;

			var max = nodes.Count();
			if (max == 0) return;
			var mainBuffer = new StringBuilder(300*1024);
			var appendinxBuffer = new StringBuilder(200*1024);


			var counter = 0;

			using (var repo = RepositoryEditor.Repository())
			using (_evalSession2 = new Esath.Eval.Ver2.EvalSession(Scenario.Vault, repo, null))
			{
				foreach (var branch in nodes)
				{
					if (e.Cancel) return;
					var source = Regex.Replace(branch.Template ?? "", @"<SPAN[^>]+vpath\s*=\s*""([^""]+)""[^>]*>[^<]+</SPAN>", vpathEvaluator, RegexOptions.Compiled);

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

					if (branch.IsAppendix)
						appendinxBuffer.Append(source);
					else
						mainBuffer.Append(source);
					backgroundWorker.ReportProgress((counter++)*100/max);
				}
			}
			e.Result = new[] {mainBuffer, appendinxBuffer};
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			buttonCancel.Visible = false;
			backgroundWorker.CancelAsync();
			DialogResult = DialogResult.Cancel;
		}
	}
}