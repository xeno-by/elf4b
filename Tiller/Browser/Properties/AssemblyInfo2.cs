using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Browser.Properties;
using DataVault.UI.Api.UIExtensionz;
using Browser;

[assembly : AssemblyVersion(StartUp.VERSION)]
[assembly : AssemblyFileVersion(StartUp.VERSION)]

#if EDITION_LIGHT

[assembly: AssemblyTitle("АРМОСТО")]
[assembly: AssemblyProduct("Esath Browser Light Edition")]

#else

[assembly : AssemblyTitle("АРМОСТО ПРО")]
[assembly : AssemblyProduct("Esath Browser")]
[assembly : SpalshInfo("Мастер Формул", "1.3.22", 1000)]

#endif

[assembly : SpalshInfo("Экспорт в Microsoft Word", "1.2.2", 8000)]
[assembly : SpalshInfo("Генератор Отчетов", "1.5.77", 5000)]
[assembly : SpalshInfo("Репозиторий", "1.9.93", 10000)]
[assembly: AssemblyDescription("Автоматизированное рабочее место оценщика для создания типовых отчетов и заключений об оценке. © 2008 - 2009 БелНИЦзем. partial © 2008 - 2010 ObjectMeet (Basis Platform)")]
[assembly : AssemblyConfiguration("TEST")]
[assembly : AssemblyCompany("БелНИЦзем")]
[assembly : AssemblyCopyright("© 2008 - 2009 БелНИЦзем")]
[assembly : AssemblyTrademark("")]
[assembly : AssemblyCulture("")]
[assembly : ComVisible(false)]
[assembly : DataVaultUIExtension]
[assembly : Guid("b5ae7f33-dfc1-4847-a3e2-ff1a5986e3eb")]

namespace Browser.Properties
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public class SpalshInfoAttribute : Attribute
	{
		public SpalshInfoAttribute(string componentName, string componentVersion, int siblingWeigth)
		{
			ComponentName = componentName;
			ComponentVersion = componentVersion;
			SiblingWeigth = siblingWeigth;
		}

		public int SiblingWeigth { get; private set; }

		public string ComponentVersion { get; private set; }

		public string ComponentName { get; private set; }

		public override string ToString()
		{
			return string.Format("{0} {1}", ComponentName, ComponentVersion);
		}
	}
}