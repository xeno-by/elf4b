namespace ObjectMeet.Tiller.Entities.Whit.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Reflection.Emit;

	internal class MetaProperty
	{
		public bool WhitIgnorable { get; set; }
		public bool IsPrimaryKey { get; set; }
		public string Name { get; set; }
		public object DefaultValue { get; set; }
		public TypeBuildingContext BuildingContext { get; set; }
		public FieldBuilder FieldBuilder { get; set; }
		public PropertyBuilder PropertyBuilder { get; set; }
		public MethodBuilder GetterBuilder { get; set; }
		public MethodBuilder SetterBuilder { get; set; }
		public Type ElementType { get; set; }
		public Type PropertyType { get; set; }
		public IEnumerable<PropertyInfo> Siblings { get; set; }

		public PropertyBuilder ShadowPropertyBuilder { get; set; }
		public MethodBuilder ShadowGetterBuilder { get; set; }
		public MethodBuilder ShadowSetterBuilder { get; set; }
	}
}