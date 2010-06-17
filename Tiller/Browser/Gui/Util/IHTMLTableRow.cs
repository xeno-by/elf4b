namespace Browser.Gui.Util
{
	using System.Runtime.InteropServices;

	[Guid("3050f23c-98b5-11cf-bb82-00aa00bdce0b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	[ComImport]
	internal interface IHTMLTableRow
	{
		[DispId(-501)]
		string bgColor { get; set; }
	}
}