using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Browser.Wordum
{
	internal class Wordum
	{
	}

	namespace Interop
	{
		using System.Runtime.InteropServices;

		[Guid("000209FF-0000-0000-C000-000000000046")]
		[ComImport]
		[ClassInterface(ClassInterfaceType.None)]
		public class Word
		{
		}

		[Guid("00020970-0000-0000-C000-000000000046")]
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IWord
		{
			[DispId(0)]
			string Name { get; }

			[DispId(0x00000006)]
			IDocuments Documents { get; }

			[DispId(0x00000017)]
			bool Visible { get; set; }

			[DispId(0x000007d0)]
			IRange Range();

			[DispId(0x00000451)]
			void Quit();
		}

		[Guid("0002096C-0000-0000-C000-000000000046")]
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IDocuments
		{
			[DispId(0x0000000e)]
			IDocument Add();
		}

		[Guid("0002096B-0000-0000-C000-000000000046")]
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IDocument
		{
			[DispId(0)]
			string Name { get; }

			[DispId(0x0000000f)]
			ISections Sections { get; }

			[DispId(0x000007d0)]
			IRange Range();

			[DispId(0x00000178)]
			void SaveAs(string fileName);
		}

		[Guid("0002095A-0000-0000-C000-000000000046")]
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface ISections
		{
			[DispId(0x00000005)]
			ISection Add();
		}

		[Guid("00020959-0000-0000-C000-000000000046")]
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface ISection
		{
			[DispId(0)]
			IRange Range { get; }

			[DispId(0x0000044d)]
			IPageSetup PageSetup { get; set; }
		}

		[Guid("00020971-0000-0000-C000-000000000046")]
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IPageSetup
		{
			[DispId(0x0000006b)]
			PageOrientation Orientation { get; set; }
		}

		public enum PageOrientation
		{
			Portrait = 0,
			Landscape = 1,
		}

		[Guid("0002095E-0000-0000-C000-000000000046")]
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IRange
		{
			[DispId(0x0000007b)]
			void InsertFile(string fileName);

			[DispId(0x00000106)]
			IFind Find { get; }
		}

		[Guid("000209B0-0000-0000-C000-000000000046")]
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IFind
		{
			[DispId(0x000001bc)]
			bool Execute(
				string text,
				bool matchCase,
				bool matchWholeWord,
				bool matchWildcards,
				bool matchSoundsLike,
				bool matchAllWordForms,
				bool forward,
				bool wrap,
				object format,
				string replaceWith,
				int replace
				);
		}
	}
}