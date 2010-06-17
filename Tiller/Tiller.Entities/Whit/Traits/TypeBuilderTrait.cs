namespace ObjectMeet.Tiller.Entities.Whit.Traits
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Reflection.Emit;

	internal static class TypeBuilderTrait
	{
		private const MethodAttributes PFV = MethodAttributes.Final | MethodAttributes.Public | MethodAttributes.Virtual;

		public static MethodBuilder OverrideMethod(this TypeBuilder source, MethodInfo parentMethod)
		{
			return OverrideMethod(source, parentMethod, null);
		}

		public static MethodBuilder OverrideMethod(this TypeBuilder source, MethodInfo parentMethod, Func<ILGenerator, ILGenerator> body)
		{
			return OverrideMethod(source, parentMethod, body, null);
		}

		public static MethodBuilder OverrideMethod(this TypeBuilder source, MethodInfo parentMethod, Func<ILGenerator, ILGenerator> body, IDictionary<MethodInfo, MethodBuilder> map)
		{
			var derived = source.DefineMethod(string.Format("{0}_{1}", parentMethod.Name, parentMethod.DeclaringType.ToShortString()), PFV,
			                                  parentMethod.ReturnType,
			                                  parentMethod.GetParameters().Select(x => x.ParameterType).ToArray());

			if (body != null) body(derived.GetILGenerator());

			source.DefineMethodOverride(derived, parentMethod);
			if (map != null) map[parentMethod] = derived;
			return derived;
		}
	}
}