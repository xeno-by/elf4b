namespace ObjectMeet.Tiller.Entities.Pocso
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.ConstrainedExecution;
	using Api;
	using Whit;
	using Whit.Internal;
	using Whit.Traits;

	internal abstract class Scenario : CriticalFinalizerObject, IScenario
	{
		public abstract Guid ScenarioId { get; set; }

		[MetaInfo(DefaultValue = 1)]
		public abstract int Revision { get; set; }

		public abstract IQueryable<IScenarioNode> ScenarioNodes { get; }
		public abstract int ScenarioNodeCount { get; set; }

		public abstract IQueryable<ISourceDatumDeclaration> SourceDatumDeclarations { get; }
		public abstract int SourceDatumDeclarationCount { get; set; }

		public abstract IQueryable<IFormulaDeclaration> FormulaDeclarations { get; }
		public abstract int FormulaDeclarationCount { get; set; }

		public abstract string Name { get; set; }

		public bool IsDirty { get; set; }

		public FileInfo ContainingFile { get; set; }


		internal abstract Guid CommonRootNodeId { get; set; }
		private ScenarioNode _commonRootNode;

		public IScenarioNode CommonRootNode
		{
			get
			{
				if (_commonRootNode == null)
				{
					_commonRootNode = VaultWhit.New<ScenarioNode>((this as ICreature).Model.GetBranch(@"ScenarioNodes\" + CommonRootNodeId));
					_commonRootNode.Scenario = this;
				}
				return _commonRootNode;
			}
		}

		internal abstract Guid ParticularRootNodeId { get; set; }
		private ScenarioNode _particularRootNode;

		public IScenarioNode ParticularRootNode
		{
			get
			{
				if (_particularRootNode == null)
				{
					_particularRootNode = VaultWhit.New<ScenarioNode>((this as ICreature).Model.GetBranch(@"ScenarioNodes\" + ParticularRootNodeId));
					_particularRootNode.Scenario = this;
				}
				return _particularRootNode;
			}
		}

		public event EventHandler<ScenarioEventArgs> NodeChanged;

		protected internal void OnNodeChanged(ScenarioEventArgs scenarioEventArgs)
		{
			if (NodeChanged != null) NodeChanged(this, scenarioEventArgs);
		}

		public abstract Version StructureVersion { get; set; }

		internal T Create<T>(Expression<Func<Scenario, IQueryable<T>>> marker, T entity) where T : class, ITillerEntity
		{
			T entityCreated = null;

			var memberExpression = marker.Body as MemberExpression;
			if (memberExpression == null) throw new ArgumentOutOfRangeException("marker", marker.Body.GetType(), "MemberExpression expected");
			var propertyInfo = memberExpression.Member as PropertyInfo;
			if (propertyInfo == null) throw new ArgumentOutOfRangeException("marker", memberExpression.Member.GetType(), "PropertyInfo in MemberExpression expected");

			MetaInfoAttribute metaInfoAttribute;
			var name = propertyInfo.HasAnnotation(out metaInfoAttribute) ? metaInfoAttribute.GetNameOrAlias(propertyInfo.Name) : propertyInfo.Name;

			var me = this as ICreature;
			if (me == null) throw new InvalidOperationException("Unable to use external inheritor of the Entitor class");

			var collectionBranch = me.Model.GetOrCreateBranch(name);
			var pk = Guid.NewGuid();
			var branch = collectionBranch.CreateBranch(pk.ToString());
			entityCreated = VaultWhit.New<T>(branch);
			if (entityCreated is TillerEntityBase) ((TillerEntityBase) (object) entityCreated).Scenario = this;
			(entityCreated as ICreature).Intersect(entity as ICreature);

			// TODO: refactor this ugly temporary auto counting code
			var countGet = GetType().GetMethod(string.Format("get_{0}Count", name.Slice(0, -1)), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			var countSet = GetType().GetMethod(string.Format("set_{0}Count", name.Slice(0, -1)), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if (countGet != null && countSet != null) countSet.Invoke(this, new object[] {((int) countGet.Invoke(this, new object[0])) + 1});

			// TODO: implement generic on created

			IsDirty = true;

			return entityCreated;
		}

		~Scenario()
		{
			var me = (ICreature) this;
			if (me.IsShadow) return;
			if (me.Model == null) return; // by some reasons dtor is called twice - additional investigation required
			var disposable = me.Model as IDisposable;
			if (disposable == null) return;
			disposable.Dispose();
		}
	}
}