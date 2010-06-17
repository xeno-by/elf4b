namespace ObjectMeet.Tiller.Entities.Whit.Traits
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;

	internal static class PropertyInfoTrait
	{
		public static bool IsAbstract(this PropertyInfo source, bool absolutely)
		{
			if (absolutely)
			{
				if (source.CanRead && !source.GetGetMethod(true).IsAbstract) return false;
				if (source.CanWrite && !source.GetSetMethod(true).IsAbstract) return false;
				return true;
			}
			if (source.CanRead && source.GetGetMethod(true).IsAbstract) return true;
			if (source.CanWrite && source.GetSetMethod(true).IsAbstract) return true;
			return false;
		}

		public static bool IsIndexer(this PropertyInfo source)
		{
			return source.GetIndexParameters().Length > 0;
		}

		public static IEnumerable<PropertyInfo> Declarations(this PropertyInfo source)
		{
			if (source == null) return new PropertyInfo[0];
			if (source.DeclaringType == null) return new PropertyInfo[0];
			if (source.DeclaringType.IsInterface) return new PropertyInfo[0];

			return (from accessor in source.GetAccessors(true)
			        from decl in MethodInfoTrait.Declarations(accessor)
			        let prop = decl.EnclosingProperty()
			        where prop != null
			        select prop).Distinct();
		}

		public static string ToShortString(this PropertyInfo source)
		{
			var buff = new StringBuilder(256);
			buff
				.Append(source.DeclaringType != null ? source.DeclaringType.ToShortString() : "none")
				.Append("::")
				.Append(source.Name);

			if (source.GetIndexParameters().Length > 0)
			{
				buff
					.Append('[')
					.Append(string.Join(", ", source.GetIndexParameters().Select(x => x.ParameterType.ToShortString()).ToArray()))
					.Append("]")
					;
			}
			buff
				.Append(": ")
				.Append(source.PropertyType.ToShortString())
				.Append(" { ")
				.Append(source.CanRead ? "get; " : "")
				.Append(source.CanWrite ? "set; " : "")
				.Append("}")
				;

			return buff.ToString();
		}
	}

	internal static class InterfaceMappingTrait
	{
		public static IDictionary<MethodInfo, MethodInfo> ToDictionary(this InterfaceMapping source)
		{
			var res = new Dictionary<MethodInfo, MethodInfo>();
			for (var i = 0; i < source.TargetMethods.Length; i++)
				res.Add(source.TargetMethods[i], source.InterfaceMethods[i]);
			return res;
		}
	}
}