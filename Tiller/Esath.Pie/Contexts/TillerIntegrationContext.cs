using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using DataVault.Core.Api;
using Elf.Syntax.Ast.Expressions;
using Esath.Pie.Api;
using Elf.Helpers;
using System.Linq;
using Esath.Pie.Helpers;
using Esath.Pie.Properties;
using Esath.Pie.AstRendering;

namespace Esath.Pie.Contexts
{
    public class TillerIntegrationContext : ITillerIntegrationContext, IElfEditorContext
    {
        public IBranch Common { get; private set; }
        public IBranch FormulaBeingEdited { get; private set; }
        private Func<IBranch> BranchSelector { get; set; }
        private Func<IBranch> NodeSelector { get; set; }

        private Dictionary<VPath, String> _varNames = new Dictionary<VPath, String>();
        private Dictionary<VPath, String> _varTypes = new Dictionary<VPath, String>();
        private List<VarItem> _native = new List<VarItem>();
        private List<VarItem> _externalVars = new List<VarItem>();
        private List<VarItem> _externalNodes = new List<VarItem>();

        public TillerIntegrationContext(IBranch common, Func<IBranch> branchSelector, Func<IBranch> nodeSelector)
            : this(common, null, branchSelector, nodeSelector)
        {
        }

        public TillerIntegrationContext(IBranch common, IBranch activeParticle, Func<IBranch> branchSelector, Func<IBranch> nodeSelector)
        {
            Common = common;
            FormulaBeingEdited = activeParticle;
            BranchSelector = branchSelector;
            NodeSelector = nodeSelector;

            var svds = Enumerable.Empty<IBranch>();
            if (FormulaBeingEdited != null)
            {
                var particle = FormulaBeingEdited.Parent.Parent;
                var partDecl = particle.GetBranches().SingleOrDefault(b => b.Name == "_sourceValueDeclarations");
                var partFlae = particle.GetBranches().SingleOrDefault(b => b.Name == "_formulaDeclarations");

                if (partFlae != null) svds = partFlae.AsArray().Concat(svds);
                if (partDecl != null) svds = partDecl.AsArray().Concat(svds);
            }

            Func<IBranch, String> type = b => b.GetValue("type") == null ? null : b.GetValue("type").ContentString;
            svds.Distinct().SelectMany(svd => svd.GetBranches())
                .Where(var => var != FormulaBeingEdited && type(var) != "text" && type(var) != "string")
                .ForEach(var => ImportVar(var));

            var elf = activeParticle.GetValue("elfCode") == null ? null : activeParticle.GetValue("elfCode").ContentString;
            if (elf != null) elf.RenderLightElfAsPublicText(this); // as a side effect this will fill in the externals
        }

        private VarItem ImportVar(IBranch varBranch)
        {
            return ImportVar(varBranch, true);
        }

        private VarItem ImportVar(IBranch varBranch, bool native)
        {
            // protect ourselves from recursion
            if (varBranch == FormulaBeingEdited)
            {
                return null;
            }
            else
            {
                Func<String, String> attr = s => varBranch.GetValue(s) == null ? "" : varBranch.GetValue(s).ContentString;
                _varNames.Add(varBranch.VPath, attr("name"));
                _varTypes.Add(varBranch.VPath, attr("type"));
                var @newitem = new VarItem(varBranch, _varNames[varBranch.VPath], varBranch.VPath.ToElfIdentifier());

                if (native)
                {
                    _native.Add(@newitem);
                }
                else
                {
                    if (new VariableExpression(@newitem.InternalName).PieType() == PieType.Var)
                    {
                        _externalVars.Add(@newitem);
                    }
                    else
                    {
                        _externalNodes.Add(@newitem);
                    }
                }

                return @newitem;
            }
        }

        public CultureInfo Locale
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
        }

        public string GetDisplayName(string internalName)
        {
            // case 1. undefined var or node
            if (internalName == "?") return "?";
            if (internalName == "undefined$node") return "?";

            var key = internalName.FromElfIdentifier();

            // case 2. active particle
            if (FormulaBeingEdited != null && FormulaBeingEdited.VPath == key) 
                return FormulaBeingEdited.GetValue("name").ContentString;

            // case 3. local source values and formulae
            if (_varNames.ContainsKey(key)) return _varNames[key];

            // case 4. anything else
            var globallyFound = Common.Vault.GetBranch(key);
            if (globallyFound != null)
            {
                ImportVar(globallyFound, false);
                return GetDisplayName(internalName);
            }

            var branchRef = internalName.FromElfIdentifier();
            return _varNames.ContainsKey(branchRef) ? _varNames[branchRef] : "?";
        }

        public IEnumerable<VarItem> Vars
        {
            get
            {
                var sep = new VarItem(null, Resources.VarSelector_Separator, null);
                var lurk = new VarItem(null, Resources.VarSelector_Lurkmoar, null);
                var addendum = BranchSelector != null ? new[] { sep, lurk } : new VarItem[0];
                return _native.Concat(addendum).Concat(_externalVars);
            }
        }

        public VarItem SelectVarFromScenario()
        {
            var ext = BranchSelector == null ? null : BranchSelector();
            if (ext != null)
            {
                var existing = _native.Concat(_externalVars).SingleOrDefault(vi => vi.Branch == ext);
                return existing ?? ImportVar(ext, false);
            }
            else
            {
                return null;
            }
        }

        public VarItem SelectNodeFromScenario()
        {
            var node = NodeSelector == null ? null : NodeSelector();
            if (node != null)
            {
                var existing = _externalNodes.SingleOrDefault(vi => vi.Branch == node);
                return existing ?? ImportVar(node, false);
            }
            else
            {
                return null;
            }
        }
    }
}