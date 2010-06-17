namespace ObjectMeet.Tiller.Entities.Whit
{
	using System;
	using System.Linq;
	using Api;
	using DataVault.Api;
	using Pocso;

	[AttributeUsage(AttributeTargets.Property)]
	public class MetaInfoAttribute : WhitAnnotation
	{
		private object _defaultValue;

		public object DefaultValue
		{
			get { return _defaultValue; }
			set
			{
				_defaultValue = value;
				IsDefaultValueInitialized = true;
			}
		}

		public string Alias { get; set; }

		public bool IsPrimaryKey { get; set; }

		public bool IsParentProperty { get; set; }

		internal bool IsDefaultValueInitialized { get; private set; }

		public string GetNameOrAlias(string name)
		{
			return string.IsNullOrEmpty(Alias) ? name : Alias;
		}
	}

	public abstract class WhitAnnotation : Attribute
	{
	}

	public class WhitIgnorableAttribute : WhitAnnotation
	{
	}

	internal abstract class Tezd
	{
		public IBranch Model { get; set; }

		public IQueryable<IScenarioNode> Nodes
		{
			get
			{
				return from branch in Model.GetBranches().AsQueryable()
				       select CreateChild(branch);
			}
		}

		private IScenarioNode CreateChild(IBranch branch)
		{
			var child = VaultWhit.New<ScenarioNode>(branch);
			child.Scenario = null;
			return child;
		}
	}
}