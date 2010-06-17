using DataVault.Core.Api;

namespace Browser.Gui
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using Editor;
	using ObjectMeet.Appearance.Explorer;
	using ObjectMeet.Appearance.Tree;
	using Report;
	using Util;
	using System.Linq;

	public class ScenarioNode : SimpleTabbedDocument.INodeModel
	{
		public ScenarioNode(IBranch model)
		{
			if (model == null) throw new ArgumentNullException("model");
			Model = model;
		}

//		[Browsable(false)]
//		public TreeNode View { get; set; }

		[Browsable(false)]
		public IBranch Model { get; private set; }

		[Browsable(false)]
		public bool IsSystemNode { get { return Id.StartsWith("@"); } }

		private const string ID_NAME = "id";
		private const string ID_DEFAULT_VALUE = "";

		[Browsable(false)]
		[Obsolete("Since 1.0.2")]
		private string Id
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(ID_NAME)) != null ? v.ContentString : ID_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(ID_NAME)) == null)
				{
					if (ID_DEFAULT_VALUE != value) Model.CreateValue(ID_NAME, value);
				}
				else
				{
					if (ID_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}

		[Browsable(true)]
		[DisplayName("ИД")]
		[Description("Внутренний идентификатор узла")]
		[Category("Разработка сценария")]
		public string VPath { get { return Model.VPath.ToString(); } }

		[Browsable(false)]
		public string Name { get { return Model.GetOrCreateValue("name", "").ContentString; } set { Model.GetOrCreateValue("name", value).SetContent(value); } }


		private const string TITLE_NAME = "title";
		private const string TITLE_DEFAULT_VALUE = "";

		[Browsable(true)]
		[DisplayName("Наименование")]
		[Description("Используется при генерации отчета как заголовок соответствующего раздела")]
		[Category("Генерация отчета")]
		public string Title
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(TITLE_NAME)) != null ? v.ContentString : TITLE_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(TITLE_NAME)) == null)
				{
					if (TITLE_DEFAULT_VALUE != value) Model.CreateValue(TITLE_NAME, value);
				}
				else
				{
					if (TITLE_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}


// temporary removed from user input: always fase
// additional implementation logic is required when handling conditional nodes

//		[Browsable(!true)]
//		[DisplayName("Обязательный узел")]
//		[Description(@"Выбор Обязательного узла пользователь не может отменить. (Например, ""Титульный лист"")")]
//		[Category("Генерация отчета")]
//		[TypeConverter(typeof (BooleanToYesNoTypeConverter))]
//		public bool IsRequired
//		{
//			get { return Model.GetOrCreateValue("isRequired", "false").ContentString == "true"; }
//
//			set { Model.GetOrCreateValue("isRequired", value ? "true" : "false").SetContent(value ? "true" : "false"); }
//		}

		private int _level = 0;

		[Browsable(true)]
		[DisplayName("Уровень")]
		[Description("Уровень вложенности узла сценария")]
		[Category("Генерация отчета")]
		public int Level
		{
			get
			{
				if (_level == 0)
				{
					for (var parent = Model.Parent; parent != null; parent = parent.Parent) _level++;
					_level--; // ignore the Scenario node
				}
				return _level;
			}
		}


		private const string TEMPLATE_NAME = "template";
		private const string TEMPLATE_DEFAULT_VALUE = "";

		[Browsable(false)]
		public string Template
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(TEMPLATE_NAME)) != null ? v.ContentString : TEMPLATE_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(TEMPLATE_NAME)) == null)
				{
					if (TEMPLATE_DEFAULT_VALUE != value) Model.CreateValue(TEMPLATE_NAME, value);
				}
				else
				{
					if (TEMPLATE_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value);
				}
			}
		}


		private const string SORTING_WEIGHT_NAME = "sortingWeight";
		private const int SORTING_WEIGHT_DEFAULT_VALUE = 0;

		[Browsable(true)]
		[DisplayName("Вес сортировки")]
		[Description("Используется при генерации отчета, узлы с меньшими весами окажутся в отчете выше")]
		[Category("Генерация отчета")]
		public int SortingWeight
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(SORTING_WEIGHT_NAME)) != null ? int.Parse(v.ContentString) : SORTING_WEIGHT_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(SORTING_WEIGHT_NAME)) == null)
				{
					if (SORTING_WEIGHT_DEFAULT_VALUE != value) Model.CreateValue(SORTING_WEIGHT_NAME, value.ToString());
				}
				else
				{
					if (SORTING_WEIGHT_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value.ToString());
				}
			}
		}


		private const string NODE_TYPE_NAME = "nodeType";
		private const ScenarioNodeType NODE_TYPE_DEFAULT_VALUE = ScenarioNodeType.Default;

		[Browsable(true)]
		[DisplayName("Тип узла")]
		[Description("Указывает как системе обрабатывать данный узел")]
		[Category("Генерация отчета")]
		[TypeConverter(typeof (EnumTypeConverter))]
		public ScenarioNodeType NodeType
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(NODE_TYPE_NAME)) != null ? (ScenarioNodeType) Enum.Parse(typeof (ScenarioNodeType), v.ContentString) : NODE_TYPE_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(NODE_TYPE_NAME)) == null)
				{
					if (NODE_TYPE_DEFAULT_VALUE != value) Model.CreateValue(NODE_TYPE_NAME, value.ToString());
				}
				else
				{
					if (NODE_TYPE_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value.ToString());
				}
			}
		}

//		[Browsable(true)]
//		[DisplayName("Скрытый узел")]
//		[Description("Скрытый узел не видим пользователю, но участвует в генерации отчета")]
//		[Category("Разработка сценария")]
//		[TypeConverter(typeof (BooleanToYesNoTypeConverter))]
//		public bool IsHidden
//		{
//			get { return bool.Parse(Model.GetOrCreateValue("isHidden", "false").ContentString); }
//
//			set { Model.GetOrCreateValue("isHidden", value.ToString()).SetContent(value.ToString()); }
//		}

//		[Browsable(true)]
//		[DisplayName("Полный путь")]
//		[Description("Путь к текущему узлу от корня дерева сценария")]
//		[Category("Разработка сценария")]
//		public string FullPath
//		{
//			get { return View != null ? View.FullPath : ""; }
//		}


		private const string IS_APPENDIX_NAME = "isAppendix";
		private const bool IS_APPENDIX_DEFAULT_VALUE = false;

		[Browsable(true)]
		[DisplayName("Приложение")]
		[Description("Указывает системе, что этот узел попадет в Приложения к отчету при генерации документов")]
		[Category("Генерация отчета")]
		[TypeConverter(typeof (BooleanToYesNoTypeConverter))]
		public bool IsAppendix
		{
			get
			{
				IValue v;
				return (v = Model.GetValue(IS_APPENDIX_NAME)) != null ? bool.Parse(v.ContentString) : IS_APPENDIX_DEFAULT_VALUE;
			}

			set
			{
				IValue v;
				if ((v = Model.GetValue(IS_APPENDIX_NAME)) == null)
				{
					if (IS_APPENDIX_DEFAULT_VALUE != value) Model.CreateValue(IS_APPENDIX_NAME, value.ToString());
				}
				else
				{
					if (IS_APPENDIX_DEFAULT_VALUE == value) v.Delete();
					else v.SetContent(value.ToString());
				}
			}
		}

		public ScenarioNode AddChild(string name, Guid id)
		{
			var child = new ScenarioNode(Model.CreateBranch(id.ToString().Replace('-', '_'))) { Name = name, };

			return child;
		}

		private const string SOURCE_VALUE_DECLARATIONS_NAME = "_sourceValueDeclarations";

		[Browsable(false)]
		public IEnumerable<SourceValueDeclaration> SourceValueDeclarations
		{
			get
			{
				IBranch b;
				if ((b = Model.GetBranch(SOURCE_VALUE_DECLARATIONS_NAME)) == null)
					yield break;
				foreach (var branch in b.GetBranches())
					yield return new SourceValueDeclaration {Model = branch};
			}
		}

		public SourceValueDeclaration AddSourceValueDeclaration(string typeToken)
		{
			return AddSourceValueDeclaration(typeToken, Guid.NewGuid());
		}

		internal SourceValueDeclaration AddSourceValueDeclaration(string typeToken, Guid id)
		{
			return new SourceValueDeclaration
			       	{
			       		Model = Model.GetOrCreateBranch(SOURCE_VALUE_DECLARATIONS_NAME).CreateBranch(id.ToString().Replace('-', '_')),
			       		Type = typeToken,
			       		DeclarationType = "source",
			       		Name = SourceValueDeclaration.NAME_DEFAULT_VALUE,
			       	}
				;
		}

		private const string FORMULA_DECLARATIONS_NAME = "_formulaDeclarations";


		[Browsable(false)]
		public IEnumerable<FormulaDeclaration> FormulaDeclarations
		{
			get
			{
				IBranch b;
				if ((b = Model.GetBranch(FORMULA_DECLARATIONS_NAME)) == null)
					yield break;
				foreach (var branch in b.GetBranches())
					yield return new FormulaDeclaration {Model = branch};
			}
		}

		public FormulaDeclaration AddFormulaDeclaration(string typeToken)
		{
			return AddFormulaDeclaration(typeToken, Guid.NewGuid());
		}

		internal FormulaDeclaration AddFormulaDeclaration(string typeToken, Guid id)
		{
			return new FormulaDeclaration
			       	{
								Model = Model.GetOrCreateBranch(FORMULA_DECLARATIONS_NAME).CreateBranch(id.ToString().Replace('-', '_')),
			       		Type = typeToken,
			       		DeclarationType = "formula"
			       	}
				;
		}

		private const string CONDITION_DECLARATIONS_NAME = "_conditions";

		[Browsable(false)]
		public IEnumerable<ConditionDeclaration> ConditionDeclarations
		{
			get
			{
				IBranch b;
				if ((b = Model.GetBranch(CONDITION_DECLARATIONS_NAME)) == null)
					yield break;
				foreach (var branch in b.GetBranches())
					yield return new ConditionDeclaration {Model = branch};
			}
		}

		public ConditionDeclaration AddConditionDeclaration()
		{
			return new ConditionDeclaration
			       	{
			       		Model = Model.GetOrCreateBranch(CONDITION_DECLARATIONS_NAME).CreateBranch(Guid.NewGuid().ToString().Replace('-', '_')),
			       	};
		}

		public Guid GetId()
		{
			return Model.Id;
		}

		string INodeModel.GetCaption()
		{
			return Name;
		}

		string INodeModel.GetSuffix()
		{
			var svdc = SourceValueDeclarations.Count();
			var infoNumber = "";
			if (svdc > 0) infoNumber = string.Format(" ({0})", svdc);
			else if (IsMethodologyNode) infoNumber = " (n)";
			return string.Format("{0} {1}", infoNumber, string.IsNullOrEmpty(Template) ? "" : " *");
		}

		int INodeModel.GetSortingWeight()
		{
			return 0;
		}

		NodeModelTypes INodeModel.GetModelType()
		{
			if (IsSystemNode) return NodeModelTypes.Stone;
			if (ConditionDeclarations.Count() > 0) return NodeModelTypes.RadioOwner;
			return NodeModelTypes.Checkbox;
		}

		bool INodeModel.IsChecked()
		{
			return false;
		}

		bool INodeModel.IsEnabled()
		{
			return true;
		}

		public IEnumerable<INodeModel> GetChildren()
		{
			foreach (var branch in Model.GetBranches())
			{
				if (branch.Name.StartsWith("_")) continue; // internal folder but not a visible child
				yield return new ScenarioNode(branch);
			}
		}


		bool SimpleTabbedDocument.INodeModel.DoesProvideStaticContent()
		{
			return SourceValueDeclarations.Count() > 0 || IsMethodologyNode;
		}

		bool SimpleTabbedDocument.INodeModel.DoesProvideDynamicContent()
		{
			return !string.IsNullOrEmpty(Template);
		}

		Func<IEnumerable<Guid>, string> SimpleTabbedDocument.INodeModel.GetDynaDocAsyncGenerator()
		{
			return GeneratePreview;
		}

		private Esath.Eval.Ver2.EvalSession _evalSession2;

		private string GeneratePreview(IEnumerable<Guid> checkedNodes)
		{
			var levelage = new Levelage();
			var source = "";

			using (var repo = RepositoryEditor.Repository())
			using (_evalSession2 = new Esath.Eval.Ver2.EvalSession(Model.Vault, repo, checkedNodes))
			{
				source = Regex.Replace(Template ?? "", @"<SPAN[^>]+vpath\s*=\s*""([^""]+)""[^>]*>[^<]+</SPAN>", vpathEvaluator, RegexOptions.Compiled);
				//source = Regex.Replace(Template ?? "", StartUp.VFIELD_PATTERN, vpathEvaluator, RegexOptions.Compiled);

				var title = "";

				if (Title != "")
				{
					title = Title;
					switch (NodeType)
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
				_evalSession2 = null;
				source = Regex.Replace(source, @"<table\s", match => string.Format(@"<div style='text-align:right'>Таблица {0}</div><table ", levelage.AddTable()), RegexOptions.Compiled | RegexOptions.IgnoreCase);
				if (title != "") source = title + source;
			}

			return source;
		}

		private string vpathEvaluator(Match match)
		{
			var vpath = match.Groups[1].Value;

			var branch = Model.Vault.GetBranch(vpath);
			if (branch == null) return "<span style='background-color:red'>Узел не существует</span>";

			var eval = "";

			try
			{
				var res = _evalSession2.EvalToString(branch);
				eval = res.ToString();
			}
			catch (Exception oops)
			{
				var message = oops.Message ?? "Ссылка не определена";
#if DEBUG
				if (oops.InnerException != null) message = oops.InnerException.Message;
#endif
				eval = string.Format("<div style='background-color:red' onclick=with(this.children[0].style)display=display=='none'?'block':'none'>Ошибка вычисления формулы<div style='display:none'>{0}</div></div>", message.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br>"));
			}
			return eval;
		}

		private ListView _listView;

		Func<Control> SimpleTabbedDocument.INodeModel.GetStaticSyncDocGenerator()
		{
			return InitializeListView;
		}

		private bool IsMethodologyNode { get { return (new[] {"затратный метод", "доходный метод", "сравнительный метод"}).Contains(Name.ToLower()); } }

		private void LoadNodeMethodology(ScenarioNode x)
		{
			if (x.ConditionDeclarations.Count() > 0) return;
			if (x.SourceValueDeclarations.Count() > 0)
			{
				var group = _listView.Groups.Add(Id, x.Name);
				foreach (var sourceValueDeclaration in x.SourceValueDeclarations)
				{
					var item = new ListViewItem {Text = sourceValueDeclaration.Name};
					item.SubItems.Add(sourceValueDeclaration.MeasurementUnit);
					item.SubItems.Add(sourceValueDeclaration.ValueForTesting);
					item.Tag = sourceValueDeclaration;
					item.Group = group;
					_listView.Items.Add(item);
				}
			}
			foreach (var branch in x.Model.GetBranches())
			{
				if (branch.Name.StartsWith("_")) continue; // internal folder but not a visible child
				LoadNodeMethodology(new ScenarioNode(branch));
			}
		}

		private ListView InitializeListView()
		{
			if (_listView == null)
			{
				_listView = new ListView {Dock = DockStyle.Fill, View = View.Details, GridLines = true, FullRowSelect = true, Sorting = SortOrder.Ascending,};
				_listView.Columns.Add(new ColumnHeader {Text = "Наменование", Width = 300});
				_listView.Columns.Add(new ColumnHeader {Text = "Ед. изм.", Width = 70});
				_listView.Columns.Add(new ColumnHeader {Text = "Значение", Width = 300});

				_listView.DoubleClick += listSourceData_DoubleClick;
				_listView.KeyPress += _listView_KeyPress;
			}

			_listView.Items.Clear();
			if (IsMethodologyNode)
			{
				_listView.Groups.Clear();

				foreach (var branch in Model.GetBranches())
				{
					if (branch.Name.StartsWith("_")) continue; // internal folder but not a visible child
					LoadNodeMethodology(new ScenarioNode(branch));
				}
			}
			else
			{
				foreach (var sourceValueDeclaration in SourceValueDeclarations)
				{
					var item = new ListViewItem {Text = sourceValueDeclaration.Name};
					item.SubItems.Add(sourceValueDeclaration.MeasurementUnit);
					item.SubItems.Add(sourceValueDeclaration.ValueForTesting);
					item.Tag = sourceValueDeclaration;
					_listView.Items.Add(item);
				}
			}
			return _listView;
		}

		private void _listView_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				listSourceData_DoubleClick(this, EventArgs.Empty);
				e.Handled = true;
			}
		}


		private void listSourceData_DoubleClick(object sender, EventArgs e)
		{
			if (_listView.SelectedIndices.Count == 1)
			{
				var item = _listView.SelectedItems[0];
				var sourceDataDeclaration = _listView.SelectedItems[0].Tag as SourceValueDeclaration;
				if (sourceDataDeclaration == null) return;

				var editor = SourceValueEditorBase.CreateEditorFromTypeToken(sourceDataDeclaration.Type);
				if (editor == null) return;

				editor.ValueDeclaration = sourceDataDeclaration;
				editor.Value = sourceDataDeclaration.ValueForTesting;
				editor.ValueVPath = ""; // sourceDataDeclaration.RepositoryValuePath;
				if (editor.ShowDialog(_listView) == DialogResult.OK)
				{
					sourceDataDeclaration.ValueForTesting = editor.Value;
					item.SubItems[2].Text = sourceDataDeclaration.ValueForTesting;
				}
			}
		}
	}
}