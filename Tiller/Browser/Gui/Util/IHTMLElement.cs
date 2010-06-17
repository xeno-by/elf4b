// Copyright (c) 2008-2009 by ObjectMeet Ltd
//                        www.objectmeet.com
namespace Browser.Gui.Util
{
	using System.Runtime.InteropServices;

	[Guid("3050f1ff-98b5-11cf-bb82-00aa00bdce0b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	[ComImport]
	internal interface IHTMLElement
	{
		[DispId(-2147418043)]
		string title { get; set; }

		[DispId(-2147417610)]
		string getAttribute(string strAttributeName);

		[DispId(-2147417611)]
		void setAttribute(string strAttributeName, string attributeValue);

		[DispId(-2147417609)]
		bool removeAttribute(string strAttributeName);
	}
}