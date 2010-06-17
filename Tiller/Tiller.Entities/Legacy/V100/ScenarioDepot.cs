using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectMeet.Tiller.Entities.Legacy.V100
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Collections.Generic;
	using DataVault.Api;

	[Obsolete("Any given program, when running, is obsolete")]
	internal class ScenarioDepot
	{
		public IVault Vault { get; set; }

		public IBranch Scenario
		{
			get
			{
				if (Vault == null) return null;
				return Vault.GetOrCreateBranch(new VPath("Scenario"));
			}
		}

		private IBranch _commonPart;

		public IBranch CommonPart
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

		public IBranch PartucilarPart
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
					foreach (var sourceValueDeclaration in new ScenarioNode { Model = branch }.SourceValueDeclarations)
						yield return sourceValueDeclaration;
				foreach (var branch in PartucilarPart.GetBranchesRecursive().Where(x => x.GetBranch("_sourceValueDeclarations") != null))
					foreach (var sourceValueDeclaration in new ScenarioNode { Model = branch }.SourceValueDeclarations)
						yield return sourceValueDeclaration;
			}
		}

		public int Version
		{
			get
			{
				if (Scenario == null) return 0;

				return int.Parse(Scenario.GetOrCreateValue("version", "0").ContentString);
			}
			set { Scenario.GetOrCreateValue("version", "0").UpdateContent(value.ToString()); }
		}

		public Guid Id
		{
			get
			{
				if (Scenario == null) return Guid.Empty;

				return new Guid(Scenario.GetOrCreateValue("id", Guid.NewGuid().ToString()).ContentString);
			}
			set { Scenario.GetOrCreateValue("id", "0").UpdateContent(value.ToString()); }
		}

		public Guid LastReportId
		{
			get
			{
				if (Scenario == null) return Guid.Empty;

				return new Guid(Scenario.GetOrCreateValue("lastReportId", Guid.Empty.ToString()).ContentString);
			}
			set { Scenario.GetOrCreateValue("lastReportId", "0").UpdateContent(value.ToString()); }
		}
	}
}
