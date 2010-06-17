namespace ObjectMeet.Tiller.Entities.Whit.Traits
{
	using System;
	using System.IO;

	internal static class FileInfoTrait
	{
		public static FileInfo MoveAndAddSuffix(this FileInfo source, string suffixBeingAddedToFileName)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (suffixBeingAddedToFileName == null) throw new ArgumentNullException("suffixBeingAddedToFileName");

			var file = new FileInfo(source.FullName.Slice(0, -source.Extension.Length) + suffixBeingAddedToFileName + source.Extension);
			if (file.Exists)
				for (var i = 1; file.Exists; i++)
					file = new FileInfo(string.Format("{0}{1} ({2}){3}", source.FullName.Slice(0, -source.Extension.Length), suffixBeingAddedToFileName, i, source.Extension));

			File.Move(source.FullName, file.FullName);

			return file;
		}
	}
}