using System;
using System.Linq;
using System.Windows.Forms;
using DataVault.Core.Api;

namespace Browser.Gui
{
	using System.IO;
	using Dialog;
	using ObjectMeet.Appearance.Dialog;

	public partial class MainForm : Form
	{
		public const string ScenarioDocumentExtension = ".iasto";
		public const string ApprasialDocumentExtension = ".aprep";

		public MainForm()
		{
			InitializeComponent();
			_lastMemCheck = DateTime.Now.Ticks;
			Application.Idle += Application_Idle;
		}

		private long _lastMemCheck;

		private void Application_Idle(object sender, EventArgs e)
		{
			if (new TimeSpan(DateTime.Now.Ticks - _lastMemCheck).Seconds < 1) return;
			_lastMemCheck = DateTime.Now.Ticks;
			toolStripStatusLabelMemoryUsed.Text = string.Format("{0} МБ", GC.GetTotalMemory(false)/1024/1024);
		}

		public bool InWork
		{
			set
			{
				if (menuWindow.Visible == value) return;
				menuWindow.Visible = value;
				//menuReport.Visible = value;
			}
		}

		private void menuItemExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void menuItemAbout_Click(object sender, EventArgs e)
		{
			new SplashForm(true).ShowDialog(this);
		}


		private void OpenScenarionWindow(string title, ScenarioDepot scenarioDepot)
		{
			if (InvokeRequired)
			{
				Invoke((Action) (() => OpenScenarionWindow(title, scenarioDepot)));
			}
			else
			{
				var scenarion = new Scenarion
				                	{
				                		MdiParent = this,
				                		Text = title,
				                		Scenario = scenarioDepot,
				                	};
				scenarion.Closed += (s, a) => InWork = MdiChildren.Length > 1;
				scenarion.Show();
			}
		}


		private void menuItemOpen_Click(object sender, EventArgs e)
		{
#if EDITION_LIGHT
			openFileDialog.Filter = "Отчеты|*"+ApprasialDocumentExtension;
			openFileDialog.Title = "Открыть файл отчета";
#else
			openFileDialog.Filter = "Сценарии|*" + ScenarioDocumentExtension + "|Отчеты|*" + ApprasialDocumentExtension;
#endif

#if DEBUG && USE_TDISK
			openFileDialog.InitialDirectory = @"T:\";
#endif
			if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;
			var file = new FileInfo(openFileDialog.FileName);

			// already open?
			foreach (var form in MdiChildren)
				if (form is Scenarion)
				{
					if (new FileInfo(((Scenarion) form).Scenario.Vault.Uri).FullName != file.FullName) continue;
					ActivateMdiChild(form);
					form.Activate();
					return;
				}

			if (file.Extension == ScenarioDocumentExtension)
			{
				var cwd = new CancellableWorkerDialog
				          	{
				          		Information = "Производится загрузка файла сценария. Продолжительность этого процесса зависит от размера самого файла и количества ветвей дерева сценария, хранимых в нем.\n\nНа медленных компьютерах эта процедура может занять некоторое время.",
				          		Cancellable = false,
				          		UnknownProgress = true,
				          	};
				cwd.Worker = x => OpenScenarionWindow(string.Format("Сценарий \"{0}\"", file.Name), new ScenarioDepot {Vault = VaultApi.OpenZip(file.FullName)});

				if (cwd.ShowDialog(this) == DialogResult.Abort) return;
				InWork = true;
			}
			if (file.Extension == ApprasialDocumentExtension)
			{
				var cwd = new CancellableWorkerDialog
				          	{
				          		Information = "Производится загрузка файла отчета. Продолжительность этого процесса зависит от размера самого файла и количества ветвей дерева сценария, хранимых в нем.\n\nНа медленных компьютерах эта процедура может занять некоторое время.",
				          		Cancellable = false,
				          		UnknownProgress = true,
				          	};

				cwd.Worker = x => OpenScenarionWindow(string.Format("Отчет \"{0}\"", file.Name), new ScenarioDepot {Vault = VaultApi.OpenZip(file.FullName)});

				if (cwd.ShowDialog(this) == DialogResult.Abort) return;

				InWork = true;
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			menuStrip.MdiWindowListItem = menuWindow;
#if EDITION_LIGHT
			menuItemNewScenario.Visible = false;
			menuBreak001.Visible = false;
			menuItemProperties.Visible = false;
			openFileDialog.Filter = "Отчеты|*" + ApprasialDocumentExtension;
#endif
		}


		private void menuItemNewAppraisal_Click(object sender, EventArgs e)
		{
			openScenarioDialog.Title = "Выберите сценарий для создаваемого отчета";
			openScenarioDialog.Filter = "Сценарии|*" + ScenarioDocumentExtension;
#if DEBUG && USE_TDISK
			openScenarioDialog.InitialDirectory = @"T:\";
#endif
			if (openScenarioDialog.ShowDialog(this) != DialogResult.OK) return;

			saveAsDialog.Title = "Введите имя файла отчета";
			saveAsDialog.Filter = "Отчеты|*" + ApprasialDocumentExtension;
#if DEBUG && USE_TDISK
			saveAsDialog.InitialDirectory = @"T:\";
#endif
			if (saveAsDialog.ShowDialog(this) != DialogResult.OK) return;

			var report = new FileInfo(saveAsDialog.FileName);
			if (report.Exists) report.Delete();

			var nameTo = report.FullName;
			if (!nameTo.EndsWith(ApprasialDocumentExtension)) nameTo += ApprasialDocumentExtension;

			var scenario = new FileInfo(openScenarioDialog.FileName);
			var nameFrom = scenario.FullName;
			if (!nameFrom.EndsWith(ScenarioDocumentExtension)) nameFrom += ScenarioDocumentExtension;

			var cwd = new CancellableWorkerDialog
			          	{
			          		Information = "Производится создание файла отчета, интеграция дерева сценария и предварительная оптимизация плана расчета. Продолжительность этого процесса зависит от количества ветвей дерева сценария.\n\nНа медленных компьютерах эта процедура может занять продолжительное время.",
			          		Cancellable = false,
			          		UnknownProgress = true,
			          	};

			cwd.Worker = x =>
				{
					x.UpdateProgress(0, "Создание файла отчета");
					File.Copy(nameFrom, nameTo);
					x.UpdateProgress(0, "Интеграция дерева сценариев");
					var scenarioDepot = new ScenarioDepot {Vault = VaultApi.OpenZip(nameTo),};
					x.UpdateProgress(0, "Оптимизация плана расчета");
					scenarioDepot.Save();
					x.UpdateProgress(0, "Почти все готово");
					OpenScenarionWindow(nameTo, scenarioDepot);
				};

			if (cwd.ShowDialog(this) != DialogResult.OK) return;
			InWork = true;
		}

		private void menuItemNewScenario_Click(object sender, EventArgs e)
		{
			saveAsDialog.Title = "Введите имя файла сценария";
			saveAsDialog.Filter = "Сценарии|*" + ScenarioDocumentExtension;
#if DEBUG && USE_TDISK
			saveAsDialog.InitialDirectory = @"T:\";
#endif
			if (saveAsDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			var file = new FileInfo(saveAsDialog.FileName);
			if (file.Exists) file.Delete();

			var name = file.FullName;
			if (!name.EndsWith(ScenarioDocumentExtension)) name += ScenarioDocumentExtension;

			// this operation takes a short time
			OpenScenarionWindow(file.Name, new ScenarioDepot {Vault = VaultApi.OpenZip(name),});

			InWork = true;
		}

		private void menuItemServiceRepositoryEditor_Click(object sender, EventArgs e)
		{
			InWork = true;

			foreach (var form in MdiChildren)
				if (form is RepositoryEditor)
				{
					ActivateMdiChild(form);
					form.Activate();
					return;
				}

			var editor = new RepositoryEditor {MdiParent = this, Text = string.Format("Редактор Репозитория")};
			editor.Closed += (s, a) => InWork = MdiChildren.Length > 1;
			editor.Show();
		}

		private void menuItemSave_Click(object sender, EventArgs e)
		{
			var scenarion = ActiveMdiChild as Scenarion;
			if (scenarion == null) return;

#if !EDITION_LIGHT
			// if template is being edited it's required to sync the current content with its owner before saving
			if (scenarion.treeScenario.SelectedNode != null)
				scenarion.SelectedNode.Template = scenarion.templateEditor.InnerHtml;
#endif

			while (true)
			{
				try
				{
					var cwd = new CancellableWorkerDialog
					          	{
					          		Information = "Производится сохранение и оптимизация файла для ускорения расчетов. Продолжительность этого процесса зависит от размера самого файла и количества ветвей дерева сценария, хранимых в нем.\n\nНа медленных компьютерах эта процедура может занять продолжительное время.",
					          		Cancellable = false,
					          		UnknownProgress = true,
					          	};
					cwd.Worker = x => scenarion.Scenario.Save();

					if (cwd.ShowDialog(this) == DialogResult.Abort) throw cwd.Error;

					break;
				}
				catch (IOException oops)
				{
					if (MessageBox.Show(this, "Ошибка: " + oops.Message, "Ошибка ввода-вывода", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Retry) return;
				}
				catch
				{
					return;
				}
			}

#if !EDITION_LIGHT
			scenarion.apprasialX.DefineModel(scenarion.Scenario);
			//scenarion.appraisalOne.ScenarioVault = scenarion.Scenario.Vault;
#endif
		}

		private void menuItemProperties_Click(object sender, EventArgs e)
		{
			var scenarion = ActiveMdiChild as Scenarion;
			if (scenarion == null) return;
			new ScenarioPropertiesDialog {ScenarioDepot = scenarion.Scenario,}.ShowDialog(this);
		}

		private void menuFile_DropDownOpening(object sender, EventArgs e)
		{
			menuItemProperties.Enabled = menuItemSave.Enabled = ActiveMdiChild as Scenarion != null;
		}

		private void menuItemServiceDocumentConverter_Click(object sender, EventArgs e)
		{
			openScenarioDialog.Title = "Выберите сценарий, созданный в предыдущей версии системы";
			openScenarioDialog.Filter = "Сценарии предыдущих версий|*.scenario";
			if (openScenarioDialog.ShowDialog(this) != DialogResult.OK) return;

			var scenario = new FileInfo(openScenarioDialog.FileName);
			var nameFrom = scenario.FullName;
			if (!nameFrom.EndsWith(".scenario")) nameFrom += ".scenario";

			var nameTo = Path.Combine(scenario.DirectoryName, scenario.Name + ScenarioDocumentExtension);
			if (!nameTo.EndsWith(ScenarioDocumentExtension)) nameTo += ScenarioDocumentExtension;

			var cwd = new CancellableWorkerDialog
			          	{
			          		Information = "Производится конвертация файла сценария, созданного в более ранней версии системы, в наиболее оптимальный формат. Продолжительность этого процесса зависит от количества ветвей дерева сценария.\n\nНа медленных компьютерах эта процедура может занять продолжительное время.",
			          		Cancellable = false,
										UnknownProgress = true,
			          	};

			cwd.Worker = x =>
				{
					x.UpdateProgress(0, "Изучение файла сценария");
					var prev = new ScenarioDepot {Vault = VaultApi.OpenZip(nameFrom)};
					var curr = new ScenarioDepot {Vault = VaultApi.OpenZip(nameTo)};

					var a = prev.Cast<ScenarioNode>();
					var b = curr.Cast<ScenarioNode>();

					x.UpdateProgress(0, "Конвертация Общих Характеристик");
					MergeNodes(a.ElementAt(0), b.ElementAt(0));

					x.UpdateProgress(0, "Конвертация Характеристик Объекта");
					MergeNodes(a.ElementAt(1), b.ElementAt(1));

					x.UpdateProgress(99, "Создание нового документа");
					curr.Save();
					x.UpdateProgress(0, "Почти все готово");
				};

			if (cwd.ShowDialog(this) != DialogResult.OK) return;
		}

		private void MergeNodes(ScenarioNode source, ScenarioNode dest)
		{
			foreach (ScenarioNode child in source.GetChildren())
			{
				var @new = dest.AddChild(child.Name, new Guid(child.Model.Name.Replace('_','-')));
				foreach (var declaration in child.SourceValueDeclarations)
				{
					var newDeclaration = @new.AddSourceValueDeclaration(declaration.Type, new Guid(declaration.Model.Name.Replace('_', '-')));
					newDeclaration.Name = declaration.Name;
					newDeclaration.Comment = declaration.Comment;
					newDeclaration.DefaultValue = declaration.DefaultValue;
					newDeclaration.MeasurementUnit = declaration.MeasurementUnit;
					//newDeclaration.RepositoryValuePath = declaration.RepositoryValuePath;
					newDeclaration.ValueForTesting = declaration.ValueForTesting;
				}
				foreach (var declaration in child.FormulaDeclarations)
				{
					var newDeclaration = @new.AddFormulaDeclaration(declaration.Type, new Guid(declaration.Model.Name.Replace('_', '-')));
					newDeclaration.Name = declaration.Name;
					newDeclaration.ElfCode = declaration.ElfCode;
					newDeclaration.HumanText = declaration.HumanText;
				}
				foreach (var declaration in child.ConditionDeclarations)
				{
					var newDeclaration = @new.AddConditionDeclaration();
					newDeclaration.Name = declaration.Name;
					newDeclaration.Handler = declaration.Handler;
					newDeclaration.Model = declaration.Model;
				}
				@new.NodeType = child.NodeType;
				@new.IsAppendix = child.IsAppendix;
				@new.SortingWeight = child.SortingWeight;
				@new.Template = child.Template;
				@new.Title = child.Title;
				MergeNodes(child, @new);
			}
		}

	}
}