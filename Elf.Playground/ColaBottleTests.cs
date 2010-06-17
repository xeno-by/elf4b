using Elf.Cola;
using Elf.Cola.Parameters;
using NUnit.Framework;
using System.Linq;
using Elf.Helpers;

namespace Elf.Playground
{
    [TestFixture]
    public class ColaBottleTests
    {
        private ColaBottle _cb;

        [SetUp]
        public void SetUp()
        {
            var a2 = new ColaNode("a2");
            var b1 = new ColaNode("b1");
            var a1 = new ColaNode("a1", a2, b1);
            _cb = new ColaBottle(a1);

            a1.ParamValues.Add(new Parameter("a11"), 11);
            a2.ParamValues.Add(new Parameter("a22"), 22);
            b1.ParamValues.Add(new Parameter("b12"), 12);
            b1.ParamValues.Add(new Parameter("b14"), 14);

            a1.Script = "a11 = a1.a2.a22 + 2; a11 = a11 - a1.b1.b12;";
            a2.Script = "a11 = a1.a2.a22 + 2 / a1.b1.b14";
            b1.Script = "b12 = 1; b14 = 2; a1.a2.a22 = a1.a2.a22 * b12 * b14";
        }

        [Test]
        public void TestParametersValues()
        {
            var values = _cb.ParamValues.OrderBy(kvp => kvp.Key.Name).ToArray();
            Assert.AreEqual(4, values.Length);
            Assert.AreEqual("[a1.a11, 11]", values[0].ToString());
            Assert.AreEqual("[a1.a2.a22, 22]", values[1].ToString());
            Assert.AreEqual("[a1.b1.b12, 12]", values[2].ToString());
            Assert.AreEqual("[a1.b1.b14, 14]", values[3].ToString());

            var modified = _cb.ParamValues;
            modified.Remove(_cb.Cap.ParamValues.Keys.Single());
            modified.Keys.ToArray().ForEach(p => modified[p] = ((int)modified[p]) * 2);
            _cb.ParamValues = modified;

            var mvalues = _cb.ParamValues.OrderBy(kvp => kvp.Key.Name).ToArray();
            Assert.AreEqual(3, mvalues.Length);
            Assert.AreEqual("[a1.a2.a22, 44]", mvalues[0].ToString());
            Assert.AreEqual("[a1.b1.b12, 24]", mvalues[1].ToString());
            Assert.AreEqual("[a1.b1.b14, 28]", mvalues[2].ToString());
        }

        [Test]
        public void TestParametersUsages()
        {
            var usages = _cb.ParamUsages.OrderBy(kvp => kvp.Key.Name).ToArray();
            Assert.AreEqual(5, usages.Length);
            Assert.AreEqual("[a1.a11, + = [], - = [a1]]", usages[0].Value.ToString());
            Assert.AreEqual("[a1.a2.a11, + = [], - = [a1.a2]]", usages[1].Value.ToString());
            Assert.AreEqual("[a1.a2.a22, + = [a1.a2, a1.b1, a1], - = []]", usages[2].Value.ToString());
            Assert.AreEqual("[a1.b1.b12, + = [a1], - = [a1.b1]]", usages[3].Value.ToString());
            Assert.AreEqual("[a1.b1.b14, + = [a1.a2], - = [a1.b1]]", usages[4].Value.ToString());
        }

        [Test]
        public void TestDependencyGraph()
        {
            var dg = _cb.DependencyGraph;

            Assert.AreEqual(3, dg.Vertices.Count());
            Assert.AreEqual("a1.a2", dg.Vertices.ElementAt(0).ToString());
            Assert.AreEqual("a1.b1", dg.Vertices.ElementAt(1).ToString());
            Assert.AreEqual("a1", dg.Vertices.ElementAt(2).ToString());

            Assert.AreEqual(2, dg.Edges.Count());
            Assert.AreEqual("a1.b1->a1.a2", dg.Edges.ElementAt(0).ToString());
            Assert.AreEqual("a1.b1->a1", dg.Edges.ElementAt(1).ToString());
        }

        [Test]
        public void TestExecutionPlan()
        {
            var ep = _cb.ExecutionPlan;

            Assert.AreEqual(3, ep.Count());
            Assert.AreEqual("a1.b1", ep.ElementAt(0).ToString());
            Assert.AreEqual("a1", ep.ElementAt(1).ToString());
            Assert.AreEqual("a1.a2", ep.ElementAt(2).ToString());
        }
    }
}