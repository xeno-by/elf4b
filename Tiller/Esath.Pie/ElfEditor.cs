using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Elf.Core;
using Elf.Core.Reflection;
using Elf.Syntax.Light;
using Esath.Pie.Api;
using Esath.Pie.Contexts;
using Esath.Pie.Properties;
using Elf.Syntax.Ast;
using Elf.Syntax.Ast.Defs;
using Elf.Syntax.Ast.Expressions;
using System.Linq;
using Elf.Syntax.Ast.Statements;
using Elf.Helpers;
using Esath.Pie.AstRendering;
using Elf.Syntax.AstBuilders;
using Esath.Data;
using Esath.Data.Core;
using Esath.Pie.Helpers;

namespace Esath.Pie
{
    public partial class ElfEditor : UserControl, IElfEditor
    {
        public IElfEditorContext _rctx;
        public IElfEditorContext Ctx
        {
            get { return _rctx; } 
            set
            {
                _rctx = value;
                FillVarsList();
            }
        }

        private Block _root;
        private Expression[] Lines { get { return _root.Children.Cast<ExpressionStatement>().Select(es => es.Expression).ToArray(); } }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ElfCode
        {
            get { return ((Script)_root.Parent.Parent.Parent).RenderElfCode(_rctx); }
            set
            {
                var canon = value.ToCanonicalElf();
                var script = (Script)new ElfAstBuilder(canon).BuildAstAllowLoopholes();

                var root = script.Classes.Single().Funcs.Single().Body;
                ValidateIfLockedAssignment(root);
                _root = root;

                RebuildTopPanelContent();
                InitializeSelection();
            }
        }

        private void InitializeSelection()
        {
            SelectedLineIndex = _samode ? 0 : -1;
        }

        public event EventHandler ElfCodeChanged;

        public ElfEditor()
        {
            InitializeComponent();
            _topContainer.TopToolStripPanel.SizeChanged += (o, e) => RecalculateTotalHeight();
            _nowEditingStrip.Visible = false;
            _nodeButton.Visible = !(Ctx is ITillerIntegrationContext);

            ElfCode = null;
            Ctx = new DefaultEditorContext(new Dictionary<string, string>());

            RebuildTopPanelContent();
            FillCtypesList();
            FillFxtypesList();

            _lineOfCodeButton.Text = Resources.LineOfCode_NoCodeSelected;
            SelectedLine = null;
        }

        #region Filling in the combos

        private ToolStripItem _fxEq;

        private void FillCtypesList()
        {
            _ctypeButton.DropDownItems.Clear();
            DataUtilities.AllDataTypes.OrderBy(t => t.GetLocalization().TypeName).ForEach(t =>
            {
                if (t == typeof(EsathUndefined)) return;
                if (t == typeof(EsathText)) return;
                if (t == typeof(EsathString)) return;
                if (t == typeof(EsathBoolean)) return;

                var typen = t.GetLocalization().TypeName;
                var ddi = _ctypeButton.DropDownItems.Add(typen);
                ddi.Tag = t;
                ddi.Click += (o, e) => {
                    if (!this.Confirm()) return;

//                    var eo = ((Type)ddi.Tag).GetDefaultValue();
//                    ReplaceSubThenSelectItAndRedraw(_selectedSub, new LiteralExpression(eo.ToStorageString()));

                    _ctypeButton.Text = ((Type)ddi.Tag).GetLocalization().TypeName;
                    _constTextbox_TextChanged(this, EventArgs.Empty);
                    _constTextbox.Focus();
                };
            });
        }

        private void FillFxtypesList()
        {
            _fxTypeButton.DropDownItems.Clear();

            var vm = new VirtualMachine();
            var fx = vm.Classes.SelectMany(c => c.Methods.OfType<ClrMethod>()).Concat(vm.HelperMethods);
            var fxn = fx.Select(m => m.Rtimpl.RtimplOf()).Distinct().OrderBy(n => n);
            new []{"="}.Concat(fxn).ForEach(fxnn =>
            {
                if (fxnn == "||" || fxnn == "&&" || fxnn == "!=" || fxnn == "==") return;

                var ddi = _fxTypeButton.DropDownItems.Add("pew");
                ddi.Text = fxnn;
                ddi.Click += (o, e) => {
                    var curr = _selectedSub.Children.Count();
                    var vargs = fxnn.GuessArgc() ?? curr;

                    if (vargs < curr && !this.Confirm()) return;

                    var head = _selectedSub.Children.Take(Math.Min(curr, vargs));
                    var tail = Enumerable.Range(0, Math.Max(0, vargs - curr))
                        .Select(i => new VariableExpression("?")).Cast<AstNode>();
                    var chi = head.Concat(tail).Cast<Expression>();

                    if (fxnn == "=")
                    {
                        var target = (VariableExpression)chi.ElementAt(0);
                        var expr = chi.ElementAt(1);
                        ReplaceSubThenSelectItAndRedraw(_selectedSub, new AssignmentExpression(target, expr));
                    }
                    else
                    {
                        ReplaceSubThenSelectItAndRedraw(_selectedSub, new InvocationExpression(fxnn, chi));
                    }

                    OnSelectFxPanel(); };
                if (fxnn == "=") _fxEq = ddi;
            });
        }

        private void FillVarsList()
        {
            _varSelectionCombo.DataSource = _rctx.Vars.ToArray();
        }

        #endregion

        private Expression _selectedLine;
        public Expression SelectedLine
        {
            get { return _selectedLine; }
            set
            {
                if (SelectedLabel != null)
                    SelectedLabel.Font = new Font(SelectedLabel.Font, FontStyle.Regular);

                _selectedLine = value;
                SyncTopPanelActions();

                if (_lineOfCodeButton.DropDownItems.Count > 0)
                {
                    var text = _lineOfCodeButton.DropDownItems[SelectedLineIndex + 1].Text;
                    _lineOfCodeButton.Text = SelectedLineIndex == -1 ? Resources.LineOfCode_NoCodeSelected :
                        String.Format(Resources.LineOfCode_SelectionFormat, text.Substring(0, text.IndexOf(":")));
                }

                if (SelectedLabel != null)
                    SelectedLabel.Font = new Font(SelectedLabel.Font, FontStyle.Bold);

                if (_samode && SelectedLineIndex == 0)
                {
                    SelectedSub = (Expression)SelectedLine.Children.ElementAt(1);
                }
                else
                {
                    SelectedSub = _selectedLine;
                }
            }
        }

        #region SelectedLine associates

        private int GetLineIndex(Expression exp)
        {
            return exp.Parents.Reverse().SkipWhile(an => !(an is Block)).Skip(1).First().ChildIndex;
        }

        private int SelectedLineIndex
        {
            get { return SelectedLine == null ? -1 : GetLineIndex(SelectedLine); }
            set { SelectedLine = value == -1 ? null : (Expression)_root.Children.ElementAt(value).Children.Single(); }
        }

        private TextBoxBase GetCodeLineTextBox(int index)
        {
            return (TextBoxBase)this.SearchRecursive("ttop " + index);
        }

        private Label GetCodeLineLabel(int index)
        {
            return (Label)this.SearchRecursive("ltop " + index);
        }

        private TextBoxBase SelectedTextBox { get { return SelectedLine == null ? null : GetCodeLineTextBox(SelectedLineIndex); } }
        private Label SelectedLabel { get { return SelectedLine == null ? null : GetCodeLineLabel(SelectedLineIndex); } }

        #endregion

        private Expression _selectedSub;
        public Expression SelectedSub
        {
            get
            {
                return _selectedSub;
            }
            set
            {
                // every change ends with setting selected sub, so we raise the event here and only here
                if (ElfCodeChanged != null)
                    ElfCodeChanged(this, EventArgs.Empty);

                _selectedSub = value;
                SyncBottomPanelActions();

                var nowe = _nowEditingLabel.Text.Substring(0, _nowEditingLabel.Text.IndexOf(":") + 1) + " ";
                var subrend =_selectedSub == null ? Resources.NowEditing_NoExpressionSelected : _selectedSub.RenderPublicText(_rctx);
                _nowEditingLabel.Text = String.Format(nowe) + subrend;

                if (_selectedSub == null)
                {
                    _bottomContainer.TopToolStripPanelVisible = false;
                    _bottomNothingPanel.Visible = true;
                    _bottomNothingPanel.Dock = DockStyle.Fill;
                    _bottomFxPanel.Visible = false;
                    _bottomConstPanel.Visible = false;
                    _bottomVarPanel.Visible = false;
                    _bottomNodePanel.Visible = false;
                    RecalculateTotalHeight();
                }
                else if (_selectedSub.PieType() == PieType.Fx)
                {
                    _bottomContainer.TopToolStripPanelVisible = true;
                    _expTypeButton.Text = _fxButton.Text;
                    _bottomNothingPanel.Visible = false;
                    _bottomFxPanel.Visible = true;
                    _bottomFxPanel.Dock = DockStyle.Fill;
                    _bottomConstPanel.Visible = false;
                    _bottomVarPanel.Visible = false;
                    _bottomNodePanel.Visible = false;
                    OnSelectFxPanel();
                }
                else if (_selectedSub.PieType() == PieType.Const)
                {
                    _bottomContainer.TopToolStripPanelVisible = true;
                    _expTypeButton.Text = _constButton.Text;
                    _bottomNothingPanel.Visible = false;
                    _bottomFxPanel.Visible = false;
                    _bottomConstPanel.Visible = true;
                    _bottomConstPanel.Dock = DockStyle.Fill;
                    _bottomVarPanel.Visible = false;
                    _bottomNodePanel.Visible = false;
                    OnSelectLiteralPanel();
                }
                else if (_selectedSub.PieType() == PieType.Var)
                {
                    _bottomContainer.TopToolStripPanelVisible = true;
                    _expTypeButton.Text = _varButton.Text;
                    _bottomNothingPanel.Visible = false;
                    _bottomFxPanel.Visible = false;
                    _bottomConstPanel.Visible = false;
                    _bottomVarPanel.Visible = true;
                    _bottomNodePanel.Visible = false;
                    _bottomVarPanel.Dock = DockStyle.Fill;
                    OnSelectVariablePanel();
                }
                else if (_selectedSub.PieType() == PieType.Node)
                {
                    _bottomContainer.TopToolStripPanelVisible = true;
                    _expTypeButton.Text = _varButton.Text;
                    _bottomNothingPanel.Visible = false;
                    _bottomFxPanel.Visible = false;
                    _bottomConstPanel.Visible = false;
                    _bottomVarPanel.Visible = false;
                    _bottomNodePanel.Visible = true;
                    _bottomNodePanel.Dock = DockStyle.Fill;
                    OnSelectNodePanel();
                }
                else
                {
                    throw new NotSupportedException(_selectedSub.GetType().ToString());
                }
            }
        }

        private void SyncTopPanelActions()
        {
            if (!_samode)
            {
                _addButton.Enabled = true;
                _deleteButton.Enabled = SelectedLineIndex >= 0;
                _moveUpButton.Enabled = SelectedLineIndex > 0;
                _moveDownButton.Enabled = SelectedLineIndex >= 0 && SelectedLineIndex != _root.Children.Count() - 1;
            }
            else
            {
                _addButton.Enabled = true;
                _deleteButton.Enabled = SelectedLineIndex > 0;
                _moveUpButton.Enabled = SelectedLineIndex > 1;
                _moveDownButton.Enabled = SelectedLineIndex > 0 && SelectedLineIndex != _root.Children.Count() - 1;
            }
        }

        private void SyncBottomPanelActions()
        {
            _expTypeButton.Visible = _selectedSub != null;
            _fxTypeButton.Visible = _selectedSub.PieType() == PieType.Fx;
            _fxTypeLabel.Visible = _selectedSub.PieType() == PieType.Fx;
            _argcTextbox.Visible = _selectedSub.PieType() == PieType.Fx;
            _argcLabel.Visible = _selectedSub.PieType() == PieType.Fx;
            _ctypeButton.Visible = _selectedSub.PieType() == PieType.Const;
            _ctypeLabel.Visible = _selectedSub.PieType() == PieType.Fx;
            toolStripSeparator3.Visible = _selectedSub.PieType() == PieType.Fx || _selectedSub.PieType() == PieType.Const;

            _upOneLevelButton.Enabled = _selectedSub != null && _selectedSub != _selectedLine;
            _wrapInFxButton.Enabled = _selectedSub != null && !(_selectedSub is AssignmentExpression);
            _extractFromFxButton.Enabled = _selectedSub != null && _selectedSub.Parent is Expression;

            if (_samode)
            {
                var isnewroot = _selectedSub != null && _selectedSub.Parent == Lines[0];
                _upOneLevelButton.Enabled &= !isnewroot;
                _extractFromFxButton.Enabled &= !isnewroot;
            }
        }

        private void _constButton_Click(object sender, EventArgs e)
        {
            if (_selectedSub.PieType() == PieType.Const) return;
            if (!this.Confirm()) return;
            ReplaceSubThenSelectItAndRedraw(_selectedSub, new LiteralExpression("[[?]]?"));

            _expTypeButton.Text = _constButton.Text;
            _fxTypeButton.Visible = false;
            _argcTextbox.Visible = false;
            _argcLabel.Visible = false;
            _ctypeButton.Visible = true;
            toolStripSeparator3.Visible = true;
            OnSelectLiteralPanel();
        }

        private void _varButton_Click(object sender, EventArgs e)
        {
            if (_selectedSub.PieType() == PieType.Var) return;
            if (!this.Confirm()) return;
            ReplaceSubThenSelectItAndRedraw(_selectedSub, new VariableExpression("?"));

            _expTypeButton.Text = _varButton.Text;
            _fxTypeButton.Visible = false;
            _argcTextbox.Visible = false;
            _argcLabel.Visible = false;
            _ctypeButton.Visible = false;
            toolStripSeparator3.Visible = false;
            OnSelectVariablePanel();
        }

        private void _fxButton_Click(object sender, EventArgs e)
        {
            if (_selectedSub.PieType() == PieType.Fx) return;
            if (!this.Confirm()) return;
            ReplaceSubThenSelectItAndRedraw(_selectedSub, new InvocationExpression("?", new Expression[0]));

            _expTypeButton.Text = _fxButton.Text;
            _fxTypeButton.Visible = true;
            _argcTextbox.Visible = true;
            _argcLabel.Visible = true;
            _ctypeButton.Visible = false;
            toolStripSeparator3.Visible = true;
            OnSelectFxPanel();
        }

        private void _nodeButton_Click(object sender, EventArgs e)
        {
            if (_selectedSub.PieType() == PieType.Node) return;
            if (!this.Confirm()) return;
            ReplaceSubThenSelectItAndRedraw(_selectedSub, new VariableExpression("undefined$node"));

            _expTypeButton.Text = _nodeButton.Text;
            _fxTypeButton.Visible = false;
            _argcTextbox.Visible = false;
            _argcLabel.Visible = false;
            _ctypeButton.Visible = false;
            toolStripSeparator3.Visible = false;
            OnSelectNodePanel();
        }

        private void OnSelectFxPanel()
        {
            var fxName = _selectedSub is AssignmentExpression ? "=" : ((InvocationExpression)_selectedSub).Name;
            _fxTypeButton.Text = fxName;
            _fxEq.Visible = _selectedSub == _selectedLine;
            _argcTextbox.Text = _selectedSub.Children.Count().ToLocalString(Thread.CurrentThread.CurrentUICulture);
            _argcTextbox.ReadOnly = fxName.CanFreezeArgc();

            RebuildBottomFxPanelContent();
            RecalculateTotalHeight();
        }

        private void OnSelectVariablePanel()
        {
            var vex = (VariableExpression)_selectedSub;

            var l = new List<VarItem>(_varSelectionCombo.Items.Cast<VarItem>());
            var def = l.FindIndex(vi => vi.InternalName == "?");
            var i = l.FindIndex(vi => vi.InternalName == vex.Name);
            if (i == -1) i = def;

            try
            {
                _varSelectionCombo.SelectedIndexChanged -= _varSelectionCombo_SelectedIndexChanged;
                _varSelectionCombo.SelectedIndex = _prevSelectedIndex = i;
            }
            finally
            {
                _varSelectionCombo.SelectedIndexChanged += _varSelectionCombo_SelectedIndexChanged;
            }

            RecalculateTotalHeight();
        }

        private int _prevSelectedIndex = -1;
        private void _varSelectionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_bottomVarPanel.Visible)
            {
                var item = (VarItem)_varSelectionCombo.SelectedItem;
                if (item != null)
                {
                    if (item.InternalName != null)
                    {
                        ReplaceSubThenSelectItAndRedraw(_selectedSub, new VariableExpression(item.InternalName));
                    }
                    else
                    {
                        if (item.PrettyText == Resources.VarSelector_Lurkmoar)
                        {
                            var tiller = Ctx as ITillerIntegrationContext;
                            if (tiller != null)
                            {
                                var ext = tiller.SelectVarFromScenario();
                                if (ext != null)
                                {
                                    try
                                    {
                                        _varSelectionCombo.SelectedIndexChanged -= _varSelectionCombo_SelectedIndexChanged;
                                        FillVarsList();
                                    }
                                    finally
                                    {
                                        _varSelectionCombo.SelectedIndexChanged += _varSelectionCombo_SelectedIndexChanged;
                                    }

                                    _varSelectionCombo.SelectedItem = ext;
                                }
                                else
                                {
                                    _varSelectionCombo.SelectedIndex = _prevSelectedIndex;
                                    return; // return is important here
                                }
                            }
                        }
                        else
                        {
                            ReplaceSubThenSelectItAndRedraw(_selectedSub, new VariableExpression("?"));
                        }
                    }
                }
                else
                {
                    ReplaceSubThenSelectItAndRedraw(_selectedSub, new VariableExpression("?"));
                }
            }

            _prevSelectedIndex = _varSelectionCombo.SelectedIndex;
        }

        private bool _disableReinitLiteralPanelUI;
        private void OnSelectLiteralPanel()
        {
            if (!_disableReinitLiteralPanelUI)
            {
                var eo = ((LiteralExpression)_selectedSub).AsEsathObject();
                _ctypeButton.Text = eo.GetType().GetLocalization().TypeName;

                _constTextbox.TextChanged -= _constTextbox_TextChanged;
                _constTextbox.Text = eo.ToUIString(Thread.CurrentThread.CurrentUICulture);
                _constValidationPanel.Visible = false;
                _constTextbox.TextChanged += _constTextbox_TextChanged;

                _constTextbox.Focus();
                RecalculateTotalHeight();
            }
        }

        private void _constTextbox_TextChanged(object sender, EventArgs e)
        {
            var tcurr = DataUtilities.AllDataTypes.Single(t =>
                t.GetLocalization().TypeName == _ctypeButton.Text);

            IEsathObject @new;

            try
            {
                @new = _constTextbox.Text.FromUIString(tcurr, Thread.CurrentThread.CurrentUICulture);
            }
            catch(Exception)
            {
                // todo. somehow make use of the fex to gief user moar info about the error
                _constValidationPanel.Visible = true;
//                ReplaceSubThenSelectItAndRedraw(_selectedSub, new LiteralExpression("[[?]]?"));
                return;
            }

            _constValidationPanel.Visible = false;
            _disableReinitLiteralPanelUI = true;
            try { ReplaceSubThenSelectItAndRedraw(_selectedSub, new LiteralExpression(@new.ToStorageString())); }
            finally { _disableReinitLiteralPanelUI = false; }
        }

        private void OnSelectNodePanel()
        {
            _nodeTextbox.Text = _selectedSub.RenderPublicText(Ctx);
        }

        private void _selectNodeButton_Click(object sender, EventArgs e)
        {
            var tiller = Ctx as ITillerIntegrationContext;
            if (tiller != null)
            {
                var node = tiller.SelectNodeFromScenario();
                if (node != null)
                {
                    _nodeTextbox.Text = node.PrettyText;
                    ReplaceSubThenSelectItAndRedraw(_selectedSub, new VariableExpression(node.InternalName));
                }
            }
        }

        private List<Panel> _topPanels = new List<Panel>();
        private void RebuildTopPanelContent()
        {
            _elfCodePanel.Controls.Clear();

            _lineOfCodeButton.DropDownItems.Clear();
            var ddr = _lineOfCodeButton.DropDownItems.Add(Resources.LineOfCode_NoCodeSelected);
            ddr.Click += (o, args) => SelectedLine = null;

            _topPanels = new List<Panel>();
            var labels = new List<Label>();

            Lines.ForEach((e, i) =>
            {
                var index = (i + 1).ToString(new string('0', (_root.Children.Count() - 1).ToString().Length)) + ":";
                var text = e.RenderPublicText(_rctx);

                var p = new Panel();
                p.Padding = new Padding(3, 3, 3, 3);
                var l = new Label();
                l.Name = "ltop " + i;
                l.Text = index;
                l.Dock = DockStyle.Left;
//                l.Padding = new Padding(3, 3, 0, 0);
                l.DoubleClick += (o, args) => { SelectedLineIndex = i; };
                l.KeyDown += (o, args) => { if (args.KeyCode == Keys.Enter) { SelectedLineIndex = i; } };
                labels.Add(l);
                var t = new RichTextBox();
                t.ReadOnly = true;
                t.Name = "ttop " + i;
                t.Dock = DockStyle.Fill;
                t.WordWrap = true;
                t.AutoSize = true;
                t.BorderStyle = BorderStyle.None;
                t.Text = text;
                t.DoubleClick += (o, args) => { SelectedLineIndex = i; };
                t.Select(0, 0);
                t.Width = this.Width - 25; // scrollbar
                var pt = t.GetPositionFromCharIndex(text.Length - 1);
                p.Controls.Add(t);
                p.Controls.Add(l);
                p.Dock = DockStyle.Top;
                p.Height = pt.Y + 30 + 5;
                _topPanels.Add(p);

                var ddi = _lineOfCodeButton.DropDownItems.Add(index + " " + text);
                ddi.Click += (o, args) => SelectedLine = e;
            });

            ((IEnumerable<Panel>)_topPanels).Reverse().ForEach(p => _elfCodePanel.Controls.Add(p));
            RecalculateTotalHeight();

            var width = labels.Count == 0 ? 0 : labels.Max(l => l.PreferredWidth);
            labels.ForEach(l => { l.AutoSize = false; l.Width = width + 2; }); // 2 extra pixels for bold
        }

        private List<Panel> _bottomPanels = new List<Panel>();
        private void RebuildBottomFxPanelContent()
        {
            _fxArgumentsPanel.Controls.Clear();
            _noParamsLabel.Visible = _selectedSub.Children.Count() == 0;

            if (_selectedSub.Children.Count() == 0)
            {
                _fxArgumentsPanel.Controls.Add(_noParamsLabel);
            }
            else
            {
                _bottomPanels = new List<Panel>();
                var labels = new List<Label>();
                _selectedSub.Children.Cast<Expression>().ForEach((e, i) =>
                {
                    var index = String.Format(Resources.ParameterFormat, i + 1);
                    var text = e.RenderPublicText(_rctx);

                    var p = new Panel();
                    p.Padding = new Padding(3, 3, 3, 3);
                    var l = new Label();
                    l.Name = "lbot " + i;
                    l.Width = 57;
                    l.Text = index;
                    l.Dock = DockStyle.Left;
//                    l.Padding = new Padding(3, 3, 0, 0);
                    l.AutoSize = false;
                    l.DoubleClick += (o, args) => SelectedSub = e;
                    l.KeyDown += (o, args) => { if (args.KeyCode == Keys.Enter) { SelectedSub = e; } };
                    labels.Add(l);
                    var t = new RichTextBox();
                    t.ReadOnly = true;
                    t.Name = "tbot " + i;
                    t.Dock = DockStyle.Fill;
                    t.Height = 25;
                    t.WordWrap = true;
                    t.AutoSize = true;
                    t.Text = text;
                    t.BorderStyle = BorderStyle.None;
                    t.DoubleClick += (o, args) => SelectedSub = e;
                    t.Select(0, 0);
                    t.Width = this.Width - 25; // scrollbar
                    var pt = t.GetPositionFromCharIndex(text.Length - 1);
                    p.Controls.Add(t);
                    p.Controls.Add(l);
                    p.Dock = DockStyle.Top;
                    p.Height = pt.Y + 30 + 5;
                    _bottomPanels.Add(p);
                });

                ((IEnumerable<Panel>)_bottomPanels).Reverse().ForEach(p => _fxArgumentsPanel.Controls.Add(p));

                var width = labels.Count == 0 ? 0 : labels.Max(l => l.PreferredWidth);
                labels.ForEach(l => { l.AutoSize = false; l.Width = width; });
            }

            RecalculateTotalHeight();
        }

        private void RecalculateTotalHeight()
        {
            var bar_top = _topContainer.TopToolStripPanel.Height;
            var bar_bottom = _bottomContainer.TopToolStripPanel.Height;

            var h_top = _topPanels.Select(p => p.Height).Sum();
            h_top = Math.Min(Math.Max(h_top, 25), 450);

            var h_bottom = _bottomPanels.Select(p => p.Height).Sum();
            h_bottom = Math.Min(Math.Max(h_bottom, 45), 550);

            // carefully apply sizes since order of operation does matter
            // SplitContainer property setters are full of weird side effects
            var panel1MinSize = bar_top + h_top + 10;
            var panel2MinSize = bar_bottom + h_bottom;
            var splitterHeight = panel1MinSize + panel2MinSize + 25;
            var formHeight = splitterHeight + 80;

            var masterForm = this.Parents().OfType<Form>().LastOrDefault();
            if (masterForm != null) masterForm.Height = formHeight;
            _splitter.Height = splitterHeight;

            _splitter.Panel1MinSize = bar_top + h_top + 10;
            _splitter.Panel2MinSize = bar_bottom + h_bottom;
            _splitter.SplitterDistance = panel1MinSize + 5;
        }

        private void _addButton_Click(object sender, EventArgs e)
        {
            var expression = new AssignmentExpression(new VariableExpression("?"), new LiteralExpression("[[?]]?"));

            var oldIndex = SelectedLineIndex;
            var head = _root.Children.Take(oldIndex + 1);
            var @new = new ExpressionStatement(expression);
            var tail = _root.Children.Skip(oldIndex + 1);
            var newchi = head.Concat(@new.AsArray()).Concat(tail).Cast<Statement>();

            _root = (Block)_root.ReplaceMeWith(() => new Block(newchi));
            RebuildTopPanelContent();
            SelectedLineIndex = oldIndex + 1;
        }

        private void _deleteButton_Click(object sender, EventArgs e)
        {
            if (!this.Confirm()) return;

            var oldIndex = SelectedLineIndex;
            var head = _root.Children.Take(SelectedLineIndex);
            var tail = _root.Children.Skip(SelectedLineIndex + 1);

            _root = (Block)_root.ReplaceMeWith(() => new Block(head.Concat(tail).Cast<Statement>()));
            RebuildTopPanelContent();
            SelectedLineIndex = oldIndex - 1;
        }

        private void _moveUpButton_Click(object sender, EventArgs e)
        {
            var oldIndex = SelectedLineIndex;
            var head = _root.Children.Take(SelectedLineIndex - 1);
            var swap = _root.Children.ElementAt(SelectedLineIndex - 1);
            var me = _root.Children.ElementAt(SelectedLineIndex);
            var tail = _root.Children.Skip(SelectedLineIndex + 1);

            _root = new Block(head.Concat(me.AsArray())
                .Concat(swap.AsArray()).Concat(tail).Cast<Statement>());
            RebuildTopPanelContent();
            SelectedLineIndex = oldIndex - 1;
        }

        private void _moveDownButton_Click(object sender, EventArgs e)
        {
            var oldIndex = SelectedLineIndex;
            var head = _root.Children.Take(SelectedLineIndex);
            var me = _root.Children.ElementAt(SelectedLineIndex);
            var swap = _root.Children.ElementAt(SelectedLineIndex + 1);
            var tail = _root.Children.Skip(SelectedLineIndex + 2);

            _root = new Block(head.Concat(swap.AsArray())
                .Concat(me.AsArray()).Concat(tail).Cast<Statement>());
            RebuildTopPanelContent();
            SelectedLineIndex = oldIndex + 1;
        }

        private void _upOneLevelButton_Click(object sender, EventArgs e)
        {
            SelectedSub = (Expression)_selectedSub.Parent;
        }

        private void ReplaceSubThenSelectItAndRedraw(AstNode old, AstNode @new)
        {
            ReplaceSubThenSelectItAndRedraw(old, () => @new);
        }

        private void ReplaceSubThenSelectItAndRedraw(AstNode old, Func<AstNode> newf)
        {
            var subIsLine = _selectedSub == _selectedLine;
            var @new = (Expression)old.ReplaceMeWith(newf);

            SelectedSub = @new;
            if (subIsLine) SelectedLine = @new;

            SelectedTextBox.Text = SelectedLine.RenderPublicText(_rctx);
            RebuildTopPanelContent();
        }

        private void _wrapInFxButton_Click(object sender, EventArgs e)
        {
            ReplaceSubThenSelectItAndRedraw(_selectedSub, () => new InvocationExpression("?", _selectedSub.AsArray()));
        }

        private void _extractFromFxButton_Click(object sender, EventArgs e)
        {
            if (_selectedSub.Parent.Parent.Children.Count() > 0 && !this.Confirm()) return;
            ReplaceSubThenSelectItAndRedraw(_selectedSub.Parent, _selectedSub);
        }

        private void _argcTextbox_Leave(object sender, EventArgs e)
        {
            if (!TryApplyArgcChanges())
            {
                _argcTextbox.Text = _selectedSub == null ? null :
                    _selectedSub.Children.Count().ToString(Thread.CurrentThread.CurrentUICulture);
            }
        }

        private void _argcTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Alt && !e.Control && !e.Shift)
            {
                if (!TryApplyArgcChanges())
                {
                    _argcTextbox.Text = _selectedSub == null ? null :
                        _selectedSub.Children.Count().ToString(Thread.CurrentThread.CurrentUICulture);
                }
            }
        }

        private bool TryApplyArgcChanges()
        {
            var curr = _selectedSub.Children.Count();
            int changed; if (!int.TryParse(_argcTextbox.Text, out changed))
            {
                return false;
            }
            else
            {
                if (curr == changed)
                {
                    return true;
                }
                else
                {
                    if (changed < curr)
                    {
                        if (!this.Confirm())
                        {
                            return false;
                        }
                    }

                    var head = _selectedSub.Children.Take(Math.Min(curr, changed));
                    var tail = Enumerable.Range(0, Math.Max(0, changed - curr))
                        .Select(i => new VariableExpression("?")).Cast<AstNode>();
                    var newchi = head.Concat(tail).Cast<Expression>();

                    var ie = (InvocationExpression)_selectedSub;
                    ReplaceSubThenSelectItAndRedraw(_selectedSub,
                        () => new InvocationExpression(ie.Name, newchi));
                    OnSelectFxPanel();

                    return true;
                }
            }
        }

        private bool _samode;

        public void EnterLockedAssignmentMode(string elfCode)
        {
            _samode = true;
            ElfCode = elfCode;
        }

        public void LeaveLockedAssignmentMode()
        {
            _samode = false;
            var ssub = SelectedSub;
            SelectedLine = SelectedLine;
            SelectedSub = ssub;
        }

        private void ValidateIfLockedAssignment(Block block)
        {
            if (_samode)
            {
                if (!(((ExpressionStatement)block.Children.First()).Expression is AssignmentExpression))
                {
                    throw new ArgumentException("Cannot enter locked assignment mode for a non-assignment expression.");
                }
            }
        }
    }
}
