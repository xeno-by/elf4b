namespace ObjectMeet.Tiller.Entities.Whit.Traits
{
	public static class StringTrait
	{
		public static string Slice(this string source, int start)
		{
			if (source == null) return string.Empty;

			if (start < 0) start += source.Length;
			if (start >= source.Length) return string.Empty;

			return source.Substring(start);
		}

		public static string Slice(this string source, int start, int end)
		{
			if (source == null) return string.Empty;

			if (start < 0) start += source.Length;
			if (end < 0) end += source.Length;

			if (start >= source.Length) return string.Empty;
			if (start >= end) return string.Empty;
			if (end >= source.Length) return source.Substring(start);

			return source.Substring(start, end - start);
		}
	}
}