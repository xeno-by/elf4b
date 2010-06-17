namespace ObjectMeet.Tiller.Entities
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using Pocso;
	using System.Linq;
	using Whit;
	using Whit.Internal;
	using Whit.Traits;


	internal abstract class Entitor 
	{
		public abstract IQueryable<Scenario> Scenarios { get; }


		public T Create<T>(Expression<Func<Entitor, IQueryable<T>>> marker, T entity) where T : class
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
			(entityCreated as ICreature).Intersect(entity as ICreature);
			// TODO implement generic on created

			return entityCreated;
		}
	}
}