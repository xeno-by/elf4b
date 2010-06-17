namespace ObjectMeet.Tiller.Entities.Whit.Traits
{
	using System.Reflection.Emit;

	internal static class MethodBuilderTrait
	{
		public static void ImplementByDefault(this MethodBuilder source)
		{
			source.GetILGenerator().lddefault(source.ReturnParameter.ParameterType).ret();
		}
	}
}