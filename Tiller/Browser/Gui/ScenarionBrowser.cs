using System;
using System.Drawing;
using System.Windows.Forms;
using Browser.Properties;
using DataVault.Core.Api;
using System.Linq;
using Elf.Helpers;

namespace Browser.Gui
{
    public partial class ScenarionBrowser : Form
    {
        public BrowserMode Mode { get; private set; }
        public IBranch SelectedEntity { get; private set; }

        private ScenarioDepot Scenario { get; set; }
        private IBranch InitialSelection { get; set; }
        private IBranch FormulaBeingEdited { get; set; }

        public static IBranch SelectNode(ScenarioDepot scenario, IBranch initialSelection)
        {
            var browser = new ScenarionBrowser(BrowserMode.SelectNode, scenario, initialSelection, null);
            return browser.ShowDialog() == DialogResult.OK ? browser.SelectedEntity : null;
        }

        public static IBranch SelectBranch(ScenarioDepot scenario, IBranch initialSelection, IBranch formulaBeingEdited)
        {
            var browser = new ScenarionBrowser(BrowserMode.SelectBranch, scenario, initialSelection, formulaBeingEdited);
            return browser.ShowDialog() == DialogResult.OK ? browser.SelectedEntity : null;
        }

        public ScenarionBrowser(BrowserMode mode, ScenarioDepot scenario, IBranch initialSelection)
            : this(mode, scenario, initialSelection, null)
        {
            
        }

        public ScenarionBrowser(BrowserMode mode, ScenarioDepot scenario, IBranch initialSelection, IBranch formulaBeingEdited)
        {
            InitializeComponent();

            Mode = mode;
            Text = mode == BrowserMode.SelectNode ? Resources.ScenarionBrowser_SelectNode : Resources.ScenarionBrowser_SelectBranch;

            Scenario = scenario;
            InitialSelection = initialSelection;
            FormulaBeingEdited = formulaBeingEdited;

						foreach (ScenarioNode root in Scenario)
						{
							LoadBranch(root, null);
						}


            var allNodes = treeScenario.Nodes.Cast<TreeNode>()
                .SelectMany(tn => tn.Flatten(n => n.Nodes.Cast<TreeNode>()));
            treeScenario.SelectedNode = allNodes.FirstOrDefault(n => n.Tag == initialSelection);

            tabControlBranchOptions.Visible = false;
            _emptySelectionTipRtb.Visible = true;
            splitContainer1.Panel2.Padding = new Padding(5, 5, 5, 5);
            ResyncOkAvailability();
        }

				private void LoadBranch(ScenarioNode scenarioNode, TreeNode parent)
				{
					if (parent == null) // root
					{
						parent = new TreeNode { Text = scenarioNode.Name, Tag = scenarioNode, NodeFont = new Font("Tahoma", 8, FontStyle.Bold), };
						treeScenario.Nodes.Add(parent);
					}
					foreach (ScenarioNode node in scenarioNode.GetChildren())
					{
						var n = new TreeNode { Text = node.Name, Tag = node, };
						parent.Nodes.Add(n);
						LoadBranch(node, n);
					}
				}

				public ScenarioNode SelectedNode
				{
					get { return treeScenario.SelectedNode == null ? null : (ScenarioNode)treeScenario.SelectedNode.Tag; }
				}

        private void treeScenario_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (SelectedNode.IsSystemNode)
            {
                tabControlBranchOptions.Visible = false;
                _emptySelectionTipRtb.Visible = true;
                splitContainer1.Panel2.Padding = new Padding(5, 5, 5, 5);
            }
            else
            {
                tabControlBranchOptions.Visible = true;
                tabControlBranchOptions.SelectedTab = sourceDataTab;
                SourceDataTabSelected(SelectedNode);
                _emptySelectionTipRtb.Visible = false;
                splitContainer1.Panel2.Padding = Padding.Empty;
            }

            ResyncOkAvailability();
        }

        private void tabControlBranchOptions_Selected(object sender, TabControlEventArgs e)
        {
            var node = SelectedNode;
            ResyncOkAvailability(e.TabPage);

            if (e.TabPage == sourceDataTab)
                if (SourceDataTabSelected(node)) return;

            if (e.TabPage == tabFormulas)
                if (FormulasTabSelected(node)) return;
        }

        private bool SourceDataTabSelected(ScenarioNode node)
        {
            listSourceValues.Items.Clear();
            propertyGridSourceValue.SelectedObject = null;
            foreach (var sourceValueDeclaration in SelectedNode.SourceValueDeclarations
                .Where(sv => sv.Type != "text" && sv.Type != "string"))
            {
                var item = listSourceValues.Items.Add(sourceValueDeclaration.Name);
                item.Tag = sourceValueDeclaration;
                sourceValueDeclaration.NameChanged += x => item.Text = x;
                item.SubItems.Add(sourceValueDeclaration.HumanType);
            }
            return true;
        }

        private bool FormulasTabSelected(ScenarioNode node)
        {
            listDeclarations.Items.Clear();
            textFormulaView.Text = "";
            foreach (var declaration in SelectedNode.FormulaDeclarations
                .Where(f => f.Type != "text" && f.Type != "string" && f.Model != FormulaBeingEdited))
            {
                var item = listDeclarations.Items.Add(declaration.Name);
                item.Tag = declaration;
                declaration.NameChanged += x => item.Text = x;
                item.SubItems.Add(declaration.HumanType);
            }
            return true;
        }

        private void listSourceValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listSourceValues.SelectedItems.Count == 1)
            {
                propertyGridSourceValue.SelectedObject = listSourceValues.SelectedItems[0].Tag;
            }
            else
            {
                propertyGridSourceValue.SelectedObject = null;
            }

            ResyncOkAvailability();
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

            ResyncOkAvailability();
        }

        private void ResyncOkAvailability()
        {
            ResyncOkAvailability(tabControlBranchOptions.SelectedTab);
        }

        private void ResyncOkAvailability(TabPage tab)
        {
            if (Mode == BrowserMode.SelectBranch)
            {
                if (!tabControlBranchOptions.Visible)
                {
                    _okButton.Enabled = false;
                }
                else
                {
                    if (tab == sourceDataTab)
                    {
                        _okButton.Enabled = listSourceValues.SelectedItems.Count == 1;
                    }
                    else if (tab == tabFormulas)
                    {
                        _okButton.Enabled = listDeclarations.SelectedItems.Count == 1;
                    }
                    else
                    {
                        _okButton.Enabled = false;
                    }
                }
            }
            else if (Mode == BrowserMode.SelectNode)
            {
                _okButton.Enabled = treeScenario.SelectedNode != null;
            }
            else
            {
                throw new NotSupportedException(Mode.ToString());
            }
        }

        private void _okButton_Click(object sender, EventArgs e)
        {
            if (Mode == BrowserMode.SelectBranch)
            {
                if (!tabControlBranchOptions.Visible)
                {
                    SelectedEntity = null;
                }
                else
                {
                    if (tabControlBranchOptions.SelectedTab == sourceDataTab)
                    {
                        SelectedEntity = ((SourceValueDeclaration)listSourceValues.SelectedItems[0].Tag).Model;
                    }
                    else if (tabControlBranchOptions.SelectedTab == tabFormulas)
                    {
                        SelectedEntity = ((FormulaDeclaration)listDeclarations.SelectedItems[0].Tag).Model;
                    }
                    else
                    {
                        SelectedEntity = null;
                    }
                }
            }
            else if (Mode == BrowserMode.SelectNode)
            {
                SelectedEntity = treeScenario.SelectedNode.Tag as IBranch;
            }
            else
            {
                throw new NotSupportedException(Mode.ToString());
            }

            DialogResult = SelectedEntity == null ? DialogResult.Cancel : DialogResult.OK;
            Close();
        }

        private void ScenarionBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Cancel && !e.Control && !e.Alt && !e.Shift)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void listSourceValues_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Mode == BrowserMode.SelectBranch)
            {
                _okButton_Click(this, EventArgs.Empty);
            }
        }

        private void listDeclarations_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Mode == BrowserMode.SelectBranch)
            {
                _okButton_Click(this, EventArgs.Empty);
            }
        }

        private void treeScenario_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Mode == BrowserMode.SelectNode)
            {
                if (treeScenario.GetNodeAt(e.Location) != null)
                {
                    _okButton_Click(this, EventArgs.Empty);
                }
            }
        }
    }
}
