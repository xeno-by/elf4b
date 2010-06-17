namespace ObjectMeet.Tiller.Entities.Whit.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Reflection.Emit;

	internal class TypeBuildingContext
	{
		public int Counter { get; set; }
		public Type SourceType { get; set; }
		public TypeBuilder TypeBuilder { get; set; }
		
		public HashSet<MethodInfo> MethodsLeftAbstract { get; set; }
		public ConstructorBuilder DefaultCtor { get; set; }

		public TypeBuilder ShadowBuilder { get; set; }
		public ConstructorBuilder ShadowDefaultCtor { get; set; }

		public MethodBuilder IntersectWithShadowBuilder { get; set; }
		public FieldBuilder IntersectingField { get; set; }

	}
}