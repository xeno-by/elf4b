namespace ObjectMeet.Tiller.Entities.Whit.Traits
{
	using System.Collections.Generic;

	internal static class ObjectTrait
	{
		public static IEnumerable<T> AsEnumerable<T>(this T source)
		{
			return new[] {source};
		}
	}
}