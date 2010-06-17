using Esath.Eval.Ver3;
using DataVault.Core.Api;

namespace Browser.Gui
{
	using System;
	using System.Collections;
	using System.Linq;
	using System.Collections.Generic;
	using ObjectMeet.Appearance.Tree;

	public class ScenarioDepot : IEnumerable<INodeModel>
	{
		public void Save()
		{
			if (Version == 0)
			{
				_commonPart = CommonPart;
				_partucilarPart = PartucilarPart;
			}

			//Vault.Backup(); //-- it's too slow to save this stuff twice
			Version++;
			Vault.Save();
		}

		private IVault _vault;

#if VAULT_EVAL_3
	    public IVaultCompiler VaultCompiler { get; private set; }
#endif

		public IVault Vault
		{
			get { return _vault; }
			set
			{
				_vault = value;
#if VAULT_EVAL_3
                VaultCompiler = new VaultCompiler(_vault);
#endif
			}
		}

		public IBranch Scenario
		{
			get
			{
				if (Vault == null) return null;
				return Vault.GetOrCreateBranch(new VPath("Scenario"));
			}
		}

		private IBranch _commonPart;

		private IBranch CommonPart
		{
			get
			{
				if (Scenario == null) return null;
				if (_commonPart == null)
				{
					_commonPart = Scenario.GetOrCreateBranch(new VPath("Common"));
					_commonPart.GetOrCreateValue(new VPath("name"), "Общие Характеристики");
					_commonPart.GetOrCreateValue(new VPath("id"), "@Common");
				}

				return _commonPart;
			}
		}

		private IBranch _partucilarPart;

		private IBranch PartucilarPart
		{
			get
			{
				if (Scenario == null) return null;
				if (_partucilarPart == null)
				{
					_partucilarPart = Scenario.GetOrCreateBranch(new VPath("Particular"));
					_partucilarPart.GetOrCreateValue(new VPath("name"), "Характеристики Объекта Оценки");
					_partucilarPart.GetOrCreateValue(new VPath("id"), "@Particular");
				}

				return _partucilarPart;
			}
		}

		public IEnumerable<SourceValueDeclaration> AllSourceValueDeclarations
		{
			get
			{
				if (Scenario == null) yield break;
				foreach (var branch in CommonPart.GetBranchesRecursive().Where(x => x.GetBranch("_sourceValueDeclarations") != null))
					foreach (var sourceValueDeclaration in new ScenarioNode(branch).SourceValueDeclarations)
						yield return sourceValueDeclaration;
				foreach (var branch in PartucilarPart.GetBranchesRecursive().Where(x => x.GetBranch("_sourceValueDeclarations") != null))
					foreach (var sourceValueDeclaration in new ScenarioNode(branch).SourceValueDeclarations)
						yield return sourceValueDeclaration;
			}
		}

		public IEnumerable<FormulaDeclaration> AllFormulaDeclarations
		{
			get
			{
				if (Scenario == null) yield break;
				foreach (var branch in CommonPart.GetBranchesRecursive().Where(x => x.GetBranch("_formulaDeclarations") != null))
					foreach (var sourceValueDeclaration in new ScenarioNode(branch).FormulaDeclarations)
						yield return sourceValueDeclaration;
				foreach (var branch in PartucilarPart.GetBranchesRecursive().Where(x => x.GetBranch("_formulaDeclarations") != null))
					foreach (var sourceValueDeclaration in new ScenarioNode(branch).FormulaDeclarations)
						yield return sourceValueDeclaration;
			}
		}

		public IEnumerable<ConditionDeclaration> AllConditionDeclarations
		{
			get
			{
				if (Scenario == null) yield break;
				foreach (var branch in CommonPart.GetBranchesRecursive().Where(x => x.GetBranch("_conditions") != null))
					foreach (var sourceValueDeclaration in new ScenarioNode(branch).ConditionDeclarations)
						yield return sourceValueDeclaration;
				foreach (var branch in PartucilarPart.GetBranchesRecursive().Where(x => x.GetBranch("_conditions") != null))
					foreach (var sourceValueDeclaration in new ScenarioNode(branch).ConditionDeclarations)
						yield return sourceValueDeclaration;
			}
		}

		public IEnumerable<ScenarioNode> AllScenarioNodes
		{
			get
			{
				if (Scenario == null) yield break;
				foreach (var branch in CommonPart.GetBranchesRecursive().Where(x => x.GetValue("sortingWeight") != null))
					yield return new ScenarioNode(branch);
				foreach (var branch in PartucilarPart.GetBranchesRecursive().Where(x => x.GetValue("sortingWeight") != null))
					yield return new ScenarioNode(branch);
			}
		}

		public int Version
		{
			get
			{
				if (Scenario == null) return 0;

				return int.Parse(Scenario.GetOrCreateValue("version", "0").ContentString);
			}
			set { Scenario.GetOrCreateValue("version", "0").SetContent(value.ToString()); }
		}

		public Guid Id
		{
			get
			{
				if (Scenario == null) return Guid.Empty;

				return new Guid(Scenario.GetOrCreateValue("id", Guid.NewGuid().ToString()).ContentString);
			}
			set { Scenario.GetOrCreateValue("id", "0").SetContent(value.ToString()); }
		}

		public Guid LastReportId
		{
			get
			{
				if (Scenario == null) return Guid.Empty;

				return new Guid(Scenario.GetOrCreateValue("lastReportId", Guid.Empty.ToString()).ContentString);
			}
			set { Scenario.GetOrCreateValue("lastReportId", "0").SetContent(value.ToString()); }
		}


		private ScenarioNode _common;
		private ScenarioNode _particular;

		IEnumerator<INodeModel> IEnumerable<INodeModel>.GetEnumerator()
		{
			yield return _common ?? (_common = new ScenarioNode(CommonPart));
			yield return _particular ?? (_particular = new ScenarioNode(PartucilarPart));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<INodeModel>) this).GetEnumerator();
		}
	}
}