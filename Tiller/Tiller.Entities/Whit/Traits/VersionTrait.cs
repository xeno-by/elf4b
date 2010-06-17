namespace ObjectMeet.Tiller.Entities.Whit.Traits
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal static class VersionTrait
	{
		public static bool Matches(this Version source, string version)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (version == null) throw new ArgumentNullException("version");

			if (version.Where(x => x == '.').Count() != 3) throw new FormatException("Version should be in the following format: n.n.n.n where n is a number or asterisk sign *");

			var pattern = version
				.Replace(".", @"\.")
				.Replace("*", @"\d+")
				;

			return Regex.IsMatch(source.ToString(4), pattern);
		}

	
	}
}