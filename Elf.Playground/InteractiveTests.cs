using Elf.Core.TypeSystem;
using Elf.Interactive;
using NUnit.Framework;
using Elf.Helpers;

namespace Elf.Playground
{
    [TestFixture]
    public class InteractiveTests
    {
        [Test]
        public void TestInteractiveAndChangeSets()
        {
            var ei = new ElfInteractive();

            var eval2Plus2 = ei.Eval("2 + 2");
            Assert.IsTrue(eval2Plus2.SideEffects.Empty);
            Assert.AreEqual(4.0, eval2Plus2.Retval);

            ei.Ctx.Add("x", 4);
            var evalXPlus2 = ei.Eval("x * 10 + 2");
            Assert.IsTrue(evalXPlus2.SideEffects.Empty);
            Assert.AreEqual(42.0, evalXPlus2.Retval);

            var evalXEq2 = ei.Eval("x = 2;");
            Assert.IsFalse(evalXEq2.SideEffects.Empty);
            Assert.IsTrue(evalXEq2.SideEffects.Admixture.IsNullOrEmpty());
            Assert.AreEqual(1, evalXEq2.SideEffects.Changes.Count);
            Assert.AreEqual(new ElfVoid(), evalXEq2.Retval);

            var evalComplex = ei.Eval(@"y = 5; if (x >= 2) then ret y * x + 22; end");
            Assert.IsFalse(evalComplex.SideEffects.Empty);
            Assert.IsFalse(ei.Ctx.ContainsKey("y"));
            Assert.IsTrue(evalComplex.SideEffects.Changes.IsNullOrEmpty());
            Assert.AreEqual(1, evalComplex.SideEffects.Admixture.Count);
            Assert.AreEqual(42.0, evalComplex.Retval);

            evalComplex.SideEffects.Accept();
            evalXEq2.SideEffects.Accept();
            Assert.AreEqual(2.0, ei.Ctx["x"]);
            Assert.AreEqual(5.0, ei.Ctx["y"]);

            var evalFin = ei.Eval(@"y * x * x * x + 2");
            Assert.IsTrue(evalFin.SideEffects.Empty);
            Assert.AreEqual(42.0, evalFin.Retval);
        }
    }
}