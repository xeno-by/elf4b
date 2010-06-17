using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Esath.Eval.Ver2;
using DataVault.Core.Api;

namespace Browser.Gui.Dialog
{
	using System.IO;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Text.RegularExpressions;
	using ObjectMeet.Appearance.Dialog;
	using ObjectMeet.Appearance.TabView;
	using Report;
	using Util;
	using Wordum.Interop;
	using Application=System.Windows.Forms.Application;

	public partial class GenerateReportDialog : Form
	{
		public GenerateReportDialog()
		{
			InitializeComponent();
		}

		internal const string Styles = @"
<style>
h1 {font-size:14pt}
h2 {font-size:14pt}
h3 {font-size:14pt}
h4 {font-size:14pt}
h5 {font-size:14pt}
</style>
";

		public ScenarioDepot Scenario { get; set; }
		public HashSet<Guid> CheckedIds { get; set; }


		private Dictionary<Guid, TreeNode> _map;


		private void CallBack(StringBuilder mainBuffer, StringBuilder appendinxBuffer)
		{
			webBrowser.DocumentText = "<html><head>" + Styles + "</head><body>" + mainBuffer + appendinxBuffer + "</body></html>";

			var main = mainBuffer.Replace("<hr>", "$$$newline$$$").ToString();
			main = main.Replace("<HR>", "$$$newline$$$");
			if (main.StartsWith("$$$newline$$$")) main = main.Substring(13);

			var appendix = appendinxBuffer.Replace("<hr>", "$$$newline$$$").ToString();
			appendix = appendix.Replace("<HR>", "$$$newline$$$");
			if (appendix.StartsWith("$$$newline$$$")) appendix = appendix.Substring(13);

			MainSource = "<html><head>" + Styles + "</head><body>" + main + "</body></html>";
			AppendixSource = "<html><head>" + Styles + "</head><body>" + appendix + "</body></html>";
		}

		private void buttonGenerate_Click(object sender, EventArgs e)
		{
			if (Scenario == null) return;
			if (CheckedIds == null) return;

			buttonGenerate.Visible = false;
			buttonExport.Visible = false;


			var levelage = new Levelage();
			var mainBuffer = new StringBuilder(300*1024);
			var appendinxBuffer = new StringBuilder(200*1024);

			var cwd = new CancellableWorkerDialog
			          	{
			          		Information = "Производится генерация документа, представляющего собой типовой отчет об оценке недвижимости. \n\nВ генерируемый документ попадут только выбранные ветви дерева сценария. Продолжительность данной процедуры зависит от количества выбранных ветвей дерева сценария.",
			          	};

			cwd.Worker = x =>
				{
					x.UpdateProgress(0, "Обработка выбранных узлов сценария");

					var nodes = from node in Scenario.Vault.GetBranchesRecursive()
					            let sn = new ScenarioNode(node)
					            where CheckedIds.Contains(node.Id) && !string.IsNullOrEmpty(sn.Template)
					            orderby sn.SortingWeight
					            select new {branch = sn, sn.Name};

					var total = 0;
					if ((total = nodes.Count()) == 0) return;
					var marker = 0;

					//#warning Review this: eval sessions now lock the vault for reading
					using (var repo = RepositoryEditor.Repository())
					using (_evalSession2 = new Esath.Eval.Ver2.EvalSession(Scenario.Vault, repo, CheckedIds))
					{
						foreach (var n in nodes)
						{
							x.UpdateProgress(++marker*99/total, n.Name);
							if (x.CancelPending) return;

							var source = Regex.Replace(n.branch.Template ?? "", @"<SPAN[^>]+vpath\s*=\s*""([^""]+)""[^>]*>[^<]+</SPAN>", vpathEvaluator, RegexOptions.Compiled);
							//var source = Regex.Replace(n.branch.Template ?? "", StartUp.VFIELD_PATTERN, vpathEvaluator, RegexOptions.Compiled);

							var title = "";

							if (n.branch.Title != "")
							{
								title = n.branch.Title;
								switch (n.branch.NodeType)
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

							if (n.branch.IsAppendix)
								appendinxBuffer.Append(source);
							else
								mainBuffer.Append(source);
						}

						x.UpdateProgress(100, "Почти все готово");

						var main = mainBuffer.ToString().Replace("<hr>", "$$$newline$$$");
						main = main.Replace("<HR>", "$$$newline$$$");
						if (main.StartsWith("$$$newline$$$")) main = main.Substring(13);

						var appendix = appendinxBuffer.ToString().Replace("<hr>", "$$$newline$$$");
						appendix = appendix.Replace("<HR>", "$$$newline$$$");
						if (appendix.StartsWith("$$$newline$$$")) appendix = appendix.Substring(13);

						MainSource = "<html><head>" + Styles + "</head><body>" + main + "</body></html>";
						AppendixSource = "<html><head>" + Styles + "</head><body>" + appendix + "</body></html>";
					}
				};

			if (cwd.ShowDialog(this) != DialogResult.OK)
			{
				buttonGenerate.Visible = true;
				buttonExport.Visible = false;
				return;
			}

			webBrowser.DocumentText = "<html><head>" + Styles + "</head><body>" + mainBuffer + appendinxBuffer + "</body></html>";
			Application.DoEvents();

			buttonGenerate.Visible = false;
			buttonExport.Visible = true;
		}

		public string MainSource { get; set; }

		public string AppendixSource { get; set; }

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

			var branch = Scenario.Vault.GetBranch(vpath);
			if (branch == null) return "<span style='background-color:red'>Узел не существует</span>";

			var eval = "";

			try
			{
				eval = _evalSession2.EvalToString(branch);
			}
			catch (Exception oops)
			{
				var message = oops.Message ?? "Ссылка не определена";
				eval = string.Format("<div style='background-color:red' onclick=with(this.children[0].style)display=display=='none'?'block':'none'>Ошибка вычисления формулы<div style='display:none'>{0}</div></div>", message.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br>"));
			}
			return eval;
		}

		private void buttonExport_Click(object sender, EventArgs e)
		{
	

			var cwd = new CancellableWorkerDialog
			          	{
										Information = "Производится экспорт документа, представляющего собой типовой отчет об оценке недвижимости, в Microsoft Word.\n\nРезультирующий документ будет открыт в новом окне Microsoft Word.",
										Cancellable = false,
			          	};

			cwd.Worker = x =>
				{
					x.UpdateProgress(10, "Инициализация Microsoft Word");
					var w = new Word() as IWord;
					if (w == null)
					{
						MessageBox.Show(this, "Не удается инициализировать Microsoft Word.\nУбедитесь, что он установлен.", "Модуль Экспорта Данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					Scenario.LastReportId = Guid.NewGuid();
					//var infileMain = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Guid.NewGuid() + ".main.html"));
					//var infileAppendix = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Guid.NewGuid() + ".apendix.html"));
					
					var outfile = new FileInfo(Path.Combine(
#if DEBUG && USE_TDISK
						@"T:\"
#else
						Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
#endif
						, Scenario.LastReportId + ".doc"));

					x.UpdateProgress(20, "Экспорт отчета");
					var infileMain = new TempHtmlFile(MainSource);

					x.UpdateProgress(40, "Экспорт приложений");
					var infileAppendix = new TempHtmlFile(AppendixSource);

					//using (var writer = new StreamWriter(File.OpenWrite(infileMain.FullName), Encoding.GetEncoding(0x4e3)))writer.Write(MainSource);
					//using (var writer = new StreamWriter(File.OpenWrite(infileAppendix.FullName), Encoding.GetEncoding(0x4e3)))writer.Write(AppendixSource);


					var doc = w.Documents.Add();
					doc.Range().InsertFile(infileMain.FullName);
					infileMain.Delete();

					var s = doc.Sections.Add();
					s.PageSetup.Orientation = PageOrientation.Landscape;
					s.Range.InsertFile(infileAppendix.FullName);
					infileAppendix.Delete();

					x.UpdateProgress(60, "Форматирование документа");
					doc.Range().Find.Execute("$$$newline$$$", false, false, false, false, false, false, false, null, "^m", 2);
					doc.SaveAs(outfile.FullName);

					x.UpdateProgress(90, "Почти все готово");

					w.Visible = true;

					Marshal.FinalReleaseComObject(s);
					Marshal.FinalReleaseComObject(doc);
					Marshal.FinalReleaseComObject(w);

					GC.Collect();
					GC.WaitForPendingFinalizers();
				};

			cwd.ShowDialog(this);
		}
	}
}