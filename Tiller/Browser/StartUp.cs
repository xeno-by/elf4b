using System.Linq;
using DataVault.Core.Api;
using DataVault.Core.Helpers;

#if VAULT_EVAL_3
    #error VAULT_EVAL_3 is more or less stable, but needs to be reviewed before it can be used in production.
    #error If you don't care about reviewing the 3rd version of the eval, and just want to get the application that works...
    #error ...try VAULT_EVAL_2 (this compilation switch should be defined in the properties of the Browser project).

    #warning If you've removed/commented previous lines, you must be sure that there're no more phantom bugs in the application
    #warning The very first place they've came from: getting ElfDeserializer for the EsathBoolean type (then mysteriously disappeared)
    #warning Another stuff that needs to be reviewed - ZipLib. I've seemingly fixed the sync bugs, but maybe they still exist
    #warning And the final thing to check is to review all the #warnings in the application.
#endif

#error At 22.03.2010 I performed some cleanup and had fixed a minor issue in Elf.
#error Please, verify that this works fine for Browser (sorry, I couldn't do that since I didn't managed to compile the project).

namespace Browser
{
	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.IO;
	using System.Reflection;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Windows.Forms;
	using DataVault.Core.Helpers.Assertions;
	using Gui;
	using Gui.Dialog;
	using ObjectMeet.Appearance.Dialog;

	internal static class StartUp
	{
		internal const string VERSION = "1.0.2.30";
#warning it seems like the pattern above is wrong, additional test required
		internal const string VFIELD_PATTERN = @"<SPAN[^>]+vpath=""(?<vpath>[^""]+)""[^>]*>(?<body>(?><SPAN(?<D>)|</SPAN(?<-D>)|.?)*(?(D)(?!)))</SPAN>";

		private static Form _splashForm;
		private static Form mainForm;

		private static EventHandler _mainShown;

		private const string FLUSH =
			@"
<P><FONT face=Verdana>all = </FONT><SPAN contentEditable=false style=""WIDTH: 294px; HEIGHT: 19px; BACKGROUND-COLOR: #ff9900"" vpath=""\Scenario\Common\4447dfd1_74c5_4101_ab40_be31156ef24f\_formulaDeclarations\9bfaae96_6a72_4448_9eb2_a4865a0ad4ea""><FONT face=Verdana>&lt; qwer &gt;</FONT></SPAN></P><P><FONT face=Verdana>all = </FONT>
<SPAN contentEditable=false style=""WIDTH: 294px; HEIGHT: 19px; BACKGROUND-COLOR: #ff9900"" vpath=""\Scenario\Common\4447dfd1_74c5_4101_ab40_be31156ef24f\_formulaDeclarations\9bfaae96_6a72_4448_9eb2_a4865a0ad4ea""><FONT face=Verdana>&lt; qwer &gt;</FONT></SPAN></P>
";

		private static void RegexpFlush()
		{
			//var source = Regex.Replace(FLUSH, @"<(SPAN)[^>]+vpath\s*=\s*""([^""]+)""[^>]*>([^&]*)(&[^;]+;)(.*)</\1>", "!!!", RegexOptions.Compiled);
			//var source = Regex.Replace(FLUSH, @"<SPAN\s*(vpath\s*=""(?<vpath>[^""]+)"")*\s*[^>]*>(?<body>(?><SPAN(?<D>)|</SPAN(?<-D>)|.?)*(?(D)(?!)))</SPAN>", "!!!", RegexOptions.Compiled);
			var source = Regex.Replace(FLUSH, @VFIELD_PATTERN, "!!!", RegexOptions.Compiled);
			Trace.WriteLine(source);
		}

		private static void MainJustScenarion(String fname)
		{
			var file = new FileInfo(Path.Combine(Application.StartupPath, fname));
			var scenarion = new Scenarion
			                	{
			                		Text = string.Format(file.Name),
			                		Scenario = new ScenarioDepot {Vault = VaultApi.OpenZip(file.FullName)},
			                	};
			Application.Run(scenarion);
		}

		private static void MainImmediatelyActivatePreviewWithPoorPerformance(String fname)
		{
			var mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var file = new FileInfo(Path.Combine(mydocs, fname));

			var scenarion = new Scenarion {Text = string.Format(file.Name), Scenario = new ScenarioDepot {Vault = VaultApi.OpenZip(file.FullName)}};
			var vpath = @"\Scenario\Particular\a55eafda_7d4f_4e8d_98f9_6e0619b80e8d\92f6c511_91c0_4b0b_9917_ddb8a10f1693\1127d0c9_b2a1_4e29_8983_b79eaf1e0d49\458ca441_53fa_4c70_93be_8a98ab81f923";
			var branch = scenarion.Scenario.Vault.GetBranch(vpath).AssertNotNull();

			var allNodes = scenarion.treeScenario.Nodes.Cast<TreeNode>().Select(
				tn => tn.Flatten(tn2 => tn2.Nodes.Cast<TreeNode>())).Flatten();
			var node = allNodes.Single(tn => tn.Tag == branch);

			scenarion.treeScenario.SelectedNode = node;
			var @lock = 0;
			scenarion.AfterAfterSelect += (o, e) => { if (@lock++ == 0) scenarion.tabControlBranchOptions.SelectedTab = scenarion.previewTab; };

			Application.Run(scenarion);
		}

		private static void MainImmediatelyActivatePreviewWithVaultEval2CalcBug(String fname)
		{
			var mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var file = new FileInfo(Path.Combine(mydocs, fname));

			var scenarion = new Scenarion {Text = string.Format(file.Name), Scenario = new ScenarioDepot {Vault = VaultApi.OpenZip(file.FullName)}};
			var vpath = @"\Scenario\Particular\a55eafda_7d4f_4e8d_98f9_6e0619b80e8d\92f6c511_91c0_4b0b_9917_ddb8a10f1693\1127d0c9_b2a1_4e29_8983_b79eaf1e0d49\841dad60_5fbb_464c_94f8_2e65b100353e";
			var branch = scenarion.Scenario.Vault.GetBranch(vpath).AssertNotNull();

			var allNodes = scenarion.treeScenario.Nodes.Cast<TreeNode>().Select(
				tn => tn.Flatten(tn2 => tn2.Nodes.Cast<TreeNode>())).Flatten();
			var node = allNodes.Single(tn => tn.Tag == branch);

			scenarion.treeScenario.SelectedNode = node;
			var @lock = 0;
			scenarion.AfterAfterSelect += (o, e) => { if (@lock++ == 0) scenarion.tabControlBranchOptions.SelectedTab = scenarion.previewTab; };

			Application.Run(scenarion);
		}

		private static void MainImmediatelyActivatePreviewWithTestFormulaeScenario(String fname)
		{
			var mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var file = new FileInfo(Path.Combine(mydocs, fname));

			var scenarion = new Scenarion {Text = string.Format(file.Name), Scenario = new ScenarioDepot {Vault = VaultApi.OpenZip(file.FullName)}};
			var vpath = @"\Scenario\Common\62f0bf38_fd59_4ca6_b57d_5318bf59a82d";
			var branch = scenarion.Scenario.Vault.GetBranch(vpath).AssertNotNull();

			var allNodes = scenarion.treeScenario.Nodes.Cast<TreeNode>().Select(
				tn => tn.Flatten(tn2 => tn2.Nodes.Cast<TreeNode>())).Flatten();
			var node = allNodes.Single(tn => tn.Tag == branch);

			scenarion.treeScenario.SelectedNode = node;
			var @lock = 0;
			scenarion.AfterAfterSelect += (o, e) => { if (@lock++ == 0) scenarion.tabControlBranchOptions.SelectedTab = scenarion.previewTab; };

			Application.Run(scenarion);
		}

		private static void MainImmediatelyActivatePreviewWithInvalidœÀ“(String fname)
		{
			var mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var file = new FileInfo(Path.Combine(mydocs, fname));

			var scenarion = new Scenarion {Text = string.Format(file.Name), Scenario = new ScenarioDepot {Vault = VaultApi.OpenZip(file.FullName)}};
			var vpath = @"\Scenario\Particular\a55eafda_7d4f_4e8d_98f9_6e0619b80e8d\c9f41723_4c0a_4774_ab23_def3bb07d9db\c0c4493e_bb13_425c_94e5_34611983db32\21d23925_d3ff_41cb_9097_46e86dcfaa35\4a82c26f_2047_4777_81b2_0f25af1199e8";
			var branch = scenarion.Scenario.Vault.GetBranch(vpath).AssertNotNull();

			var v2 = @"\Scenario\Particular\a55eafda_7d4f_4e8d_98f9_6e0619b80e8d\94b5624f_e6dc_4e23_b277_5f8e8db84b2e\847fb140_8919_47f8_8b29_a5d13429099d\_sourceValueDeclarations\3ba83618_f29e_4578_a809_a3b32129f756";
			var b2 = scenarion.Scenario.Vault.GetBranch(v2).AssertNotNull();

			var allNodes = scenarion.treeScenario.Nodes.Cast<TreeNode>().Select(
				tn => tn.Flatten(tn2 => tn2.Nodes.Cast<TreeNode>())).Flatten();
			var node = allNodes.Single(tn => tn.Tag == branch);

			scenarion.treeScenario.SelectedNode = node;
			var @lock = 0;
			scenarion.AfterAfterSelect += (o, e) => { if (@lock++ == 0) scenarion.tabControlBranchOptions.SelectedTab = scenarion.previewTab; };

			Application.Run(scenarion);
		}

		private static void FixGuidz()
		{
			const string file = @"T:\Demo test.scenario.iasto"; 
			//File.Copy(file,file+".back",true);
			var sd = new ScenarioDepot {Vault = VaultApi.OpenZip(file)};
			foreach(var i in sd.AllSourceValueDeclarations)
			{
				if(!i.Model.Name.Contains("-"))continue;
				//Console.Out.WriteLine("{1} = {0}", i.Name, i.Model.Name);
			}
			foreach (var i in sd.AllFormulaDeclarations)
			{
				//if (!i.Model.Name.Contains("-")) continue;
				if (!i.ElfCode.Contains("-")) continue;
				Console.Out.WriteLine("{1} = {0} {{{2}}}", i.Name, i.Model.Name, i.ElfCode);
			}
		}

		[STAThread]
		private static void Main()
		{
			//FixGuidz(); return;
			//RegexpFlush(); return;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");

#if!DEBUG
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
//            MainJustScenarion("repository.dat"); return;
//            MainImmediatelyActivatePreviewWithPoorPerformance("scenario.scenario"); return;
//            MainImmediatelyActivatePreviewWithVaultEval2CalcBug("scenario.scenario"); return;
//            MainImmediatelyActivatePreviewWithTestFormulaeScenario("TestFormulae.scenario"); return;
//            MainImmediatelyActivatePreviewWithInvalidœÀ“("scenarioMERGED.scenario"); return;
//            MainImmediatelyActivatePreviewWithPoorPerformance("scenarioMERGED.scenario"); return;

			_splashForm = new SplashForm();
			_splashForm.Show();
			Application.DoEvents();

			// force JIT under splash
			var repo = RepositoryEditor.Repository();
			var browser = new WebBrowser();
			browser.Navigate("about:blank");
#if VAULT_EVAL_2
			ForceJit(new Esath.Eval.Ver2.EvalSession(repo, repo, null));
#endif
			ForceJit(new Scenarion(), repo, new RepositoryEditor(), new GenerateReportDialog(), new CancellableWorkerDialog());


			_mainShown = form_Shown;

			mainForm = new MainForm();
			mainForm.Shown += _mainShown;

			var ta = (AssemblyTitleAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false)[0];
			var va = (AssemblyFileVersionAttribute) Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyFileVersionAttribute), false)[0];
			mainForm.Text = string.Format("{0} {1}", ta.Title, va.Version);

			Application.DoEvents();
			Application.Run(mainForm);
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is DivideByZeroException) Application.Exit();
		}

		private static void ForceJit(params object[] o)
		{
			foreach (var item in o)
			{
				var disposable = item as IDisposable;
				if (disposable == null) continue;
				try
				{
					disposable.Dispose();
				}
				catch
				{
					disposable = null;
				}
			}
		}

		private static void form_Shown(object sender, EventArgs e)
		{
			mainForm.Shown -= _mainShown;
			_mainShown = null;
			_splashForm.Hide();
			_splashForm = null;
			mainForm.Activate();
		}
	}
}