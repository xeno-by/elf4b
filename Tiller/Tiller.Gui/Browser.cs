namespace ObjectMeet.Tiller.Gui
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Entities.Api;
	using Entities.Service;
	using Telerik.WinControls;
	using Telerik.WinControls.Docking;
	using Telerik.WinControls.Primitives;
	using Telerik.WinControls.UI;
	using System.Linq;

	public partial class Browser : ShapedForm
	{
		public Browser()
		{
			InitializeComponent();
			EndInit();
			docManager.Visible = false;
			fakePanel.Visible = true;
			menuItemNewScenario.Click += MenuItemNewScenario_OnClick;
			quickItemNewScenario.Click += MenuItemNewScenario_OnClick;
			quickItemHelp.Click +=QuickItemHelp_OnClick;
		}

		private void QuickItemHelp_OnClick(object sender, EventArgs e)
		{
			var c = ribbon.ContextualTabGroups[0] as ContextualTabGroup;
			tabItem13.Visibility = ElementVisibility.Visible;
			c.Visibility=ElementVisibility.Visible;
//			ribbon.CommandTabs[2].ContextualTabGroup = c;
//			var ti = new TabItem();
//			c.TabItems.Add(ti);
//			c.Visibility = ElementVisibility.Visible;
		}

		private void MenuItemNewScenario_OnClick(object sender, EventArgs e)
		{
			//var s = new ScenarioBox {Text = "Сценарий №" + docManager.Documents.Count, Description = "краткое Опи"};
			var s = new ScenarioBox { Text = "Отчет о больных туберкулезом и саркоидозом", Description = "краткое Опи" };
			docManager.PrimarySite.SetDocument(s);
			var stw = new ScenarioTreeView();
			stw.Dock = DockStyle.Fill;
			var scenarioService = new ScenarioService { InteractionProvider = new Interactor(this) };
			var file = new FileInfo(@"c:\un2.scenario");
			IScenario scenario;
			if (scenarioService.LoadScenario(file, out scenario))
			{
				stw.Scenario = scenario;
			}
			panelExplorer.Controls.Add(stw);
		}

		private void EndInit()
		{
			ActOnItem<RadRibbonBarButtonGroup>(
				from commandTab in ribbon.CommandTabs.Cast<RadRibbonBarCommandTab>()
				from chunk in commandTab.Items.OfType<RadRibbonBarChunk>()
				from item in chunk.Items
				select item,
				x => x.Children.OfType<FillPrimitive>().First().ShouldPaint = false);
		}

		private void ActOnItem<T>(IEnumerable collection, Action<T> action) where T : RadElement
		{
			foreach (var item in collection)
			{
				var element = item as T;
				if (element != null) action(element);
				var itemsElement = item as IItemsElement;
				if (itemsElement == null) continue;
				ActOnItem(itemsElement.Items, action);
			}
		}

		private void docManager_DockingStateChanged(object sender, DockingChangedEventArgs e)
		{
			docManager.Visible = docManager.Documents.Count > 0;
			fakePanel.Visible = !docManager.Visible;
		}
	}
}