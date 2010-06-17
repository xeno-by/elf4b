using System.Linq;
using System.Windows.Forms;

namespace Browser.Gui.Dialog
{
	using System;
	using System.Collections.Generic;
	using ObjectMeet.Appearance.Dialog;

	public partial class ScenarioPropertiesDialog : Form
	{
		public ScenarioPropertiesDialog()
		{
			InitializeComponent();
		}

		public ScenarioDepot ScenarioDepot
		{
			set
			{
				if (value == null) return;

				var cwd = new CancellableWorkerDialog
				          	{
				          		Information = "Собирается статистическая информация о текущем сценарии.\n\n",
				          		Cancellable = false,
				          	};

				var dict = new Dictionary<string, object>(128);

				cwd.Worker = arg =>
					{
						dict["Версия\\common"] = value.Version;
						dict["Идентификатор\\common"] = value.Id.ToString().ToUpper();
						arg.UpdateProgress(10, "Исходные данные, формулы и условия");
						dict["Исходных данных\\common"] = value.AllSourceValueDeclarations.Count();
						dict["Формул\\common"] = value.AllFormulaDeclarations.Count();
						dict["Условий\\common"] = value.AllConditionDeclarations.Count();
						arg.UpdateProgress(30, "Узлы сценария");
						dict["Узлов сценария\\common"] = value.AllScenarioNodes.Count();
						dict["Элементов приложений\\common"] = value.AllScenarioNodes.Where(x => x.IsAppendix).Count();
						dict["Разделов 1-го уровня\\common"] = value.AllScenarioNodes.Where(x => x.NodeType == ScenarioNodeType.Topic).Count();
						dict["Подразделов 2-го уровня\\common"] = value.AllScenarioNodes.Where(x => x.NodeType == ScenarioNodeType.Subtopic2).Count();
						dict["Подразделов 3-го уровня\\common"] = value.AllScenarioNodes.Where(x => x.NodeType == ScenarioNodeType.Subtopic3).Count();
						dict["Подразделов 4-го уровня\\common"] = value.AllScenarioNodes.Where(x => x.NodeType == ScenarioNodeType.Subtopic4).Count();
						dict["Подразделов 5-го уровня\\common"] = value.AllScenarioNodes.Where(x => x.NodeType == ScenarioNodeType.Subtopic5).Count();
						dict["Размер заголовков\\bytes"] = string.Format("{0:N} байт", value.AllScenarioNodes.Where(x => !string.IsNullOrEmpty(x.Title)).Aggregate(0, (total, node) => total + node.Title.Length));
						arg.UpdateProgress(40, "Анализ шаблонов");
						dict["Шаблонов\\common"] = value.AllScenarioNodes.Where(x => !string.IsNullOrEmpty(x.Template)).Count();
						dict["Размер шаблонов\\bytes"] = string.Format("{0:N} байт", value.AllScenarioNodes.Where(x => !string.IsNullOrEmpty(x.Template)).Aggregate(0, (total, node) => total + node.Template.Length));
						dict["Объем шаблонов\\bytes"] = string.Format("{0:N} байт", Math.Floor(value.AllScenarioNodes.Where(x => !string.IsNullOrEmpty(x.Template)).Aggregate(0, (total, node) => total + node.Template.Length) * 1.70));
						arg.UpdateProgress(60, "Анализ параметров хранилища");
						dict["Узлов хранения\\common"] = value.Scenario.GetBranchesRecursive().Count();
						dict["Размер узлов\\bytes"] = string.Format("{0:N} байт", value.Scenario.GetBranchesRecursive().Aggregate(0, (total, branch) => total + branch.Name.Length));
						dict["Переменных хранения\\common"] = value.Scenario.GetValuesRecursive().Count();
						dict["Размер переменных\\bytes"] = string.Format("{0:N} байт", value.Scenario.GetValuesRecursive().Aggregate(0, (total, prop) => total + prop.Name.Length));
						arg.UpdateProgress(100, "Готово");
					};
				
				if(cwd.ShowDialog(this)!=DialogResult.OK)return;

				foreach (var k in dict.Keys)
				{
					var info = k.Split('\\');
					var li = new ListViewItem {Text = info[0]};
					li.SubItems.Add(dict[k].ToString());
					li.Group = listView.Groups[info[1]];
					listView.Items.Add(li);
				}

				listView.Columns[1].Width = listView.Width - listView.Columns[0].Width-25;
			}
		}
	}
}