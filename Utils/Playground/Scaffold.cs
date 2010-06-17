using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using System.Linq;
using Playground.Helpers;

namespace Playground
{
    [TestFixture]
    public class Scaffold
    {
        [Test]
        public void ValidFormulaeRegexTest()
        {
            Func<String, bool> match = s => Extractor.ValidFormulaeRegex.IsMatch(s);

            Assert.IsTrue(match("С16=С4+С8+С9+С7-Ф12+С15"));
            Assert.IsFalse(match("A9"));
            Assert.IsTrue(match("С6=округлФ6"));
            Assert.IsTrue(match("КУФ110=КУФ%110*С119"));
            Assert.IsFalse(match("КУФ%110"));
            Assert.IsTrue(match("КФИ%110=(1- ФИО%)/(1- ФИА%110)-1"));
        }

        [Test]
        public void FormulaeExtractorTest()
        {
            var input = ":0cm 1.5pt 0cm 1.5pt;height:10.5pt'>" +
  "<p class=MsoNormal align=center style='margin-top:2.8pt;margin-right:0cm;" +
  "margin-bottom:5.65pt;margin-left:0cm;text-align:center'><b><span lang=RU" +
  "style='font-family:\"Times New Roman\",\"serif\"'>КФИ%110=(1- ФИО%)/(1- " +
  "ФИА%110)-1*БЛАБЛАБЛА</span></b><span lang=RU style='font-size:10.0pt;line-height:115%'><o:p></o:p></span></p>" +
  "</td>" +
  "<td width=93 valign=top style='width:70.1pt;border-top:none;border-left:none;" +
  "border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:" +
  "solid black .75pt;mso-border-left-alt:solid black .75pt;mso-border-alt:solid black .75pt;" +
  "background:#CCFFFF;padding:0cm 1.5pt 0cm 1.5pt;height:10.5pt'>" +
  "<p class=MsoNormal align=center style='margin-top:2.8pt;margin-right:0cm;" +
  "margin-bottom:5.65pt;margin-left:0cm;text-align:center'><b><span lang=RU" +
  "style='font-family:\"Times New Roman\",\"serif\"'>КФИ%120=(1- ФИО%)/(1- ФИА%120)-" +
  "1*БЛАБЛАБЛА</span></b><span lang=RU style='font-size:10.0pt;line-height:115%'><o:p></o:p></span></p>" +
  "</td>";

            var output = Extractor.DetectFormulae(input).ToArray();

            Assert.AreEqual(2, output.Length);
            Assert.AreEqual("КФИ_110=(1-Parse(ФИО_))/(1-Parse(ФИА_110))-1*Parse(БЛАБЛАБЛА)", output[0]);
            Assert.AreEqual("КФИ_120=(1-Parse(ФИО_))/(1-Parse(ФИА_120))-1*Parse(БЛАБЛАБЛА)", output[1]);
        }

        [Test]
        public void ValidVariableRegexTest()
        {
            Func<String, bool> match = s => Extractor.ValidVariableRegex.IsMatch(s);

            Assert.IsTrue(match("С16"));
            Assert.IsFalse(match("-A12"));
            Assert.IsFalse(match("1,22"));
            Assert.IsTrue(match("КФИ%110"));
            Assert.IsTrue(match("С119"));
            Assert.IsFalse(match("  КУФ%110"));
        }

        [Test]
        public void VariableExtractorTest()
        {
            var input = "КФИ_120=(1-Parse(ФИО_))/(1-Parse(ФИА_120))-1*Parse(БЛАБЛАБЛА)";
            var output = Extractor.DetectVariablesInFormula(input).ToArray();

            Assert.AreEqual(4, output.Length);
            Assert.AreEqual("КФИ_120", output[0]);
            Assert.AreEqual("ФИО_", output[1]);
            Assert.AreEqual("ФИА_120", output[2]);
            Assert.AreEqual("БЛАБЛАБЛА", output[3]);
        }

        [Test]
        public void CSVariableExtractorTest()
        {
            var input = 
                "public static void FillCommon()" + Environment.NewLine +
                "{" + Environment.NewLine +
                "Repository.А1 = \"Склад\";" + Environment.NewLine +
                "Repository.А2 = \"г.Бобруйск, ул.Орловского, д.25Б, к.4\";" + Environment.NewLine;

            var output = Extractor.DetectVariablesInCS(input).ToArray();

            Assert.AreEqual(2, output.Length);
            Assert.AreEqual("А1", output[0]);
            Assert.AreEqual("А2", output[1]);
        }

        [Test]
        public void SpecialVariableExtractorTest()
        {
            var input = "mso-border-left-alt:solid black .75pt;mso-border-alt:solid black .75pt;" +
  "padding:0cm 1.5pt 0cm 1.5pt;height:12.0pt'>" +
  "<p class=MsoNormal align=center style='margin-top:2.8pt;margin-right:0cm;" +
  "margin-bottom:5.65pt;margin-left:0cm;text-align:center'><span" +
  "style='font-size:10.0pt;line-height:115%'>А17<o:p></o:p></span></p>" +
  "</td>" +
  "<td width=140 valign=top style='width:105.1pt;border-top:none;border-left:" +
  "none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;" +
  "mso-border-top-alt:solid black .75pt;mso-border-left-alt:solid black .75pt;" +
  "mso-border-alt:solid black .75pt;padding:0cm 1.5pt 0cm 1.5pt;height:12.0pt'>" +
  "<p class=MsoNormal align=center style='margin-top:2.8pt;margin-right:0cm;" +
  "margin-bottom:5.65pt;margin-left:0cm;text-align:center'><span" +
  "style='font-size:10.0pt;line-height:115%'>ёА17<o:p></o:p";

            var output = Extractor.DetectVariablesInDocument(input).ToArray();

            Assert.AreEqual(1, output.Length);
            Assert.AreEqual("А17", output[0]);
        }

        private const string WorkingDir = @"D:\Elf\Utils\DataSource\";
        private const string InputHtml = WorkingDir + @"\scenario-iDed.htm";
//        private const string InputHtml = WorkingDir + @"\fullScenario.htm";
        private const string RepoUserCS = WorkingDir + @"\FillSampleRepository.cs";

        [Test]
        public void SuiteTest()
        {
            var text = File.ReadAllText(InputHtml, Encoding.GetEncoding(1251));
            text = text.Replace("&nbsp;", "");
            if (text.Contains("span class=GramE") || text.Contains("span class=SpellE"))
                throw new NotSupportedException("Input HTML file contains grammar/spelling error markers. Please, resave input w/o them.");

            var formulae = Extractor.DetectFormulae(text).ToArray();
            Trace.WriteLine(String.Empty);
            Trace.WriteLine(formulae.StringJoin(Environment.NewLine).InjectLineNumbers1());
            File.WriteAllText(WorkingDir + "formulae", formulae.StringJoin(Environment.NewLine), Encoding.UTF8);

            var repoUser = File.ReadAllText(RepoUserCS, Encoding.UTF8);
            Trace.WriteLine(String.Empty);
            Trace.WriteLine(repoUser.InjectLineNumbers1());

            var initializedVars = Extractor.DetectVariablesInCS(repoUser).ToArray();
//            Trace.WriteLine(String.Empty);
//            Trace.WriteLine(initializedVars.StringJoin());

            var ёvars = Extractor.DetectVariablesInDocument(text).ToArray();
//            Trace.WriteLine(String.Empty);
//            Trace.WriteLine(ёvars.StringJoin());

            var code = Codifier.CodifyFormulae(formulae, initializedVars, ёvars);
            Trace.WriteLine(String.Empty);
            Trace.WriteLine(code.InjectLineNumbers1());
            File.WriteAllText(WorkingDir + "repository.cs", code, Encoding.UTF8);

            var cscsi = new ProcessStartInfo {
                FileName = @"c:\WINDOWS\Microsoft.NET\Framework\v3.5\Csc.exe",
                Arguments = "/target:library /out:repository.dll " +
                    "repository.cs FillSampleRepository.cs " +
                    "/utf8output " + 
                    "/reference:\"C:\\Program Files\\Reference Assemblies\\Microsoft\\Framework\\v3.5\\System.Core.dll\"",
                WorkingDirectory = WorkingDir,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                UseShellExecute = false };

            var cscp = Process.Start(cscsi);
            Trace.WriteLine(String.Empty);
            Trace.Write(cscp.StandardOutput.ReadToEnd());
            cscp.WaitForExit();

            // ensure that this shit compiles finely
            Assert.AreEqual(0, cscp.ExitCode);

            File.Copy(WorkingDir + "repository.dll", "repository.dll", true);
            var asm = AppDomain.CurrentDomain.Load("repository");
            var repo = asm.GetType("Repository");

            // ensure that all ёvars can be read finely
            ёvars.ForEach(ёvar => Assert.AreEqual(String.Empty, repo.GetProperty(ёvar).GetValue(null, null)));

            // ensure that getting the default value of any property registered here returns "", but not crashes the repo
            var actualVarProps = repo.GetProperties(BindingFlags.Public | BindingFlags.Static);
            var defaultValues = actualVarProps.Select(av => (String)av.GetValue(null, null));
            AssertHelper.SequenceEqual(Enumerable.Repeat(String.Empty, defaultValues.Count()), defaultValues);

            // ensure that read-write combo works
            var aux = Guid.NewGuid().ToString();
            Func<String, String> getValue = name => (String)repo.GetMethod("GetValue").Invoke(null, name.AsArray());
            Action<String, String> setValue = (name, val) => repo.GetMethod("SetValue").Invoke(null, new object[]{name, val});
            foreach (var name in actualVarProps.Where(p => p.CanWrite).Select(p => p.Name))
            {
                setValue(name, aux);
                Assert.AreEqual(aux, getValue(name));
            }

            // ensure that the repository doesn't have any non-russian property name
            var actualVars = repo.GetProperties(BindingFlags.Public | BindingFlags.Static).Select(p => p.Name);
            var nonrusVar = actualVars.FirstOrDefault(av => !Regex.IsMatch(av, @"^[_а-яА-ЯёЁ][_0-9а-яА-ЯёЁ]*$"));
            Assert.IsNull(nonrusVar, nonrusVar == null ? null : GetEscapeSequence(nonrusVar));
        }

        private String GetEscapeSequence(String s)
        {
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                sb.AppendFormat("\\u{0:x4}", (int)c);
            }

            return sb.ToString();
        }
    }
}
