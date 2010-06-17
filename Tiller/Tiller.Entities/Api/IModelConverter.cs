namespace ObjectMeet.Tiller.Entities.Api
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	public interface IModelConverter
	{
		IEnumerable<string> FromVersions { get; }
		Version ToVersion { get; }

		bool Convert(FileInfo source, FileInfo destination);

		IScenarioService ScenarioService { get; set; }

	}
}