namespace ObjectMeet.Tiller.Entities.Whit
{
	using System;

	[AttributeUsage(AttributeTargets.Interface)]
	public class DefaultImplementationAttribute : WhitAnnotation
	{
		public DefaultImplementationAttribute(Type implementor)
		{
			Implementator = implementor;
		}

		public Type Implementator { get; private set; }
	}
}