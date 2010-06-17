using System;
using System.IO;
using System.Linq;
using System.Text;
using Elf.Core;
using Elf.Core.Runtime;
using Elf.Core.Runtime.Impl.Compiler;
using Elf.Helpers;
using Elf.Playground.Helpers;
using Elf.Syntax.Ast.Defs;
using Elf.Syntax.AstBuilders;
using NUnit.Framework;

namespace Elf.Playground.Staple
{
    [TestFixture]
    public class VirtualMachineTests
    {
        private VirtualMachine _vm;
        private StringBuilder _toyLog;
        private IEntryPoint _ep;

        [SetUp]
        public void SetUp()
        {
            var codebase = Path.GetDirectoryName(GetType().Assembly.Location);
            var possiblePlugins = Directory.GetFiles(codebase, "*.dll");
            possiblePlugins.ForEach(file => { try
            {
                var asmBytes = File.ReadAllBytes(file);
                AppDomain.CurrentDomain.Load(asmBytes);
            } catch {/*ignore load failures*/} });

            var elfCode = ResourceHelper.ReadFromResource("Elf.Playground.Staple.Universal.elf");

            _vm = new VirtualMachine();
            _toyLog = new StringBuilder();
            _vm.Context["ToyLog"] = _toyLog;
            _vm.Load(elfCode);

            _ep = _vm.CreateEntryPoint("Script", "Main");
        }

        [Test]
        public void CompilationTest()
        {
            var elfCode = ResourceHelper.ReadFromResource("Elf.Playground.Staple.Universal.elf");
            var elfAst = new ElfAstBuilder(elfCode).BuildAst();

            var evis = new DefaultElfCompiler().Compile((FuncDef)elfAst.Children.ElementAt(0).Children.ElementAt(1));

            var pewpew = evis.Select(evi => evi.ToString()).StringJoin(Environment.NewLine);
            AssertHelper.AreEqualFromResource("Elf.Playground.Staple.VirtualMachineTests.Compilation", pewpew, @"d:\elf-vmcompile");
        }

        [Test]
        public void PlainExecutionTest()
        {
            _ep.RunTillEnd();
            AssertHelper.AreEqualFromResource("Elf.Playground.Staple.VirtualMachineTests.PlainExecution", 
                _toyLog.ToString(), @"d:\elf-vmexec");
        }

        [Test]
        public void RunStopAndDumpTest()
        {
            var counter = 13;
            var thread = _ep.GetEnumerator();
            while(thread.MoveNext()){ if (--counter == 0) break; }

            AssertHelper.AreEqualFromResource("Elf.Playground.Staple.VirtualMachineTests.RunStopAndDump",
                _vm.DumpAll(), @"d:\elf-vmdump");
        }
    }
}