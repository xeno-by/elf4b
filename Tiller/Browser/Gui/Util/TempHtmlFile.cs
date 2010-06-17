namespace Browser.Gui.Util
{
	using System;
	using System.IO;
	using System.Text;

	internal class TempHtmlFile
	{
		private readonly string _fullName;

		public TempHtmlFile(string content)
		{
			_fullName = Path.Combine(Path.GetTempPath(), string.Format("{0}.html", Guid.NewGuid()));
			using (var writer = new StreamWriter(File.OpenWrite(FullName), Encoding.GetEncoding(0x4e3)))
				writer.Write(content);
		}

		public Stream Content
		{
			get { return new FileStream(FullName, FileMode.Open, FileAccess.Read); }
		}

		public string FullName
		{
			get { return _fullName; }
		}

		public bool Delete()
		{
			try
			{
				File.Delete(FullName);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}