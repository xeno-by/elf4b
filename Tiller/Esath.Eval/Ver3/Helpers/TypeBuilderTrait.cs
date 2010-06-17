using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Esath.Eval.Ver3.Helpers
{
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
            var derived = source.DefineMethod(
                string.Format("{0}_{1}", parentMethod.Name, parentMethod.DeclaringType.ToShortString()), PFV,
                parentMethod.ReturnType,
                parentMethod.GetParameters().Select(x => x.ParameterType).ToArray());

            if (body != null) body(derived.GetILGenerator());

            source.DefineMethodOverride(derived, parentMethod);
            if (map != null) map[parentMethod] = derived;
            return derived;
        }

        private static string ToShortString(this Type source)
        {
            var buff = new StringBuilder(256);

            if (source.IsGenericType)
            {
                buff
                    .Append(source.Name.Substring(0, source.Name.IndexOf('`')))
                    .Append('<')
                    .Append(string.Join(",", source.GetGenericArguments().Select(x => x.ToShortString()).ToArray()))
                    .Append('>');
            }
            else if (source.HasElementType)
            {
                buff
                    .Append(source.GetElementType().ToShortString())
                    .Append("[]");
            }
            else
            {
                buff.Append(source.Name);
            }

            return buff.ToString();
        }
    }
}