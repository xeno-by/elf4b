namespace ObjectMeet.Tiller.Entities.Api
{
	using System;
	using System.IO;

	public interface IStorageUnit
	{
		FileInfo ContainingFile { get; }

		bool IsDirty { get; }

		Version StructureVersion { get; }
	}
}