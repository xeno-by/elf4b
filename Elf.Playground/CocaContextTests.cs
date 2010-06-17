using Elf.Cola;
using Elf.Cola.Analysis;
using Elf.Cola.Exceptions;
using Elf.Cola.Facta;
using Elf.Cola.Parameters;
using Elf.Exceptions;
using Elf.Helpers;
using Elf.Playground.Helpers;
using Elf.Syntax.Light;
using NUnit.Framework;
using System.Linq;

namespace Elf.Playground
{
    [TestFixture]
    public class CocaContextTests
    {
        [Test]
        public void TestInspectComposition()
        {
            var a2 = new ColaNode("a2");
            var b1 = new ColaNode("b1");
            var a1 = new ColaNode("a1", a2, b1);

            a1.Script = "a11 = a1.a2.a22 + 2; a11 *= a11;";
            a2.Script = "a11 = a1.a2.a22 + 2; a1.b1.b12 = a11";
            b1.Script = "a1.a2.a22 = b12 * b14";

            var cc = new CocaContext(a1);
            var findings = cc.InspectComposition();
            Assert.IsTrue(findings.AreFatal);

            Assert.AreEqual(2, findings.Count);
            Assert.IsTrue(findings[0] is ScriptIsErroneousFactum);
            Assert.IsTrue(findings[1] is DependencyGraphHasLoopFactum);

            var esf = findings[0] as ScriptIsErroneousFactum;
            Assert.AreEqual("[a1.a2, a1.b1, a1]", esf.Bottle.ToString());
            Assert.AreEqual("a1", esf.Node.ToString());
            var e = esf.Exception;
            var lightCode = e.SourceCode.RecalculateSourceCode();
            var lightSpan = e.ErrorSpan.RecalculateErrorSpan(e.SourceCode);
            Assert.AreEqual(ElfExceptionType.SyntaxError, e.Type);
            Assert.AreEqual("a11 = a1.a2.a22 + 2; a11 *= a11;", lightCode);
            Assert.AreEqual(Span.FromLength(26, 1), lightSpan);
            Assert.AreEqual("=", lightCode.Substring(lightSpan));
            Assert.AreEqual(null, e.AntlrNode);
            Assert.AreEqual(null, e.ElfNode);

            var dghlf = findings[1] as DependencyGraphHasLoopFactum;
            Assert.AreEqual("[a1.a2, a1.b1, a1]", esf.Bottle.ToString());
            AssertHelper.SequenceIsomorphic(new[] { a2, b1 }, dghlf.Loop);
        }

        [Test]
        [ExpectedException(typeof(FatalFindingsException))]
        public void TestInspectParameters()
        {
            var a2 = new ColaNode("a2");
            var b1 = new ColaNode("b1");
            var a1 = new ColaNode("a1", a2, b1);

            a1.Script = "a11 = a1.a2.a22 + 2; a11 = a11;";
            a2.Script = "a11 = a1.a2.a22 + 2; a1.a11 = a11;";
            b1.Script = "a1.a2.a22 = b12 * b14";

            a2.ParamValues.Add(new Parameter("a22"), 22);

            var cc = new CocaContext(a1);
            var findings = cc.InspectCompositionAndParameters();
            ValidateInspectParametersFindings(findings);

            try
            {
                cc.Eval();
            }
            catch (FatalFindingsException ffe)
            {
                ValidateInspectParametersFindings(ffe.Findings);
                throw;
            }
        }

        private void ValidateInspectParametersFindings(Findings findings)
        {
            Assert.IsTrue(findings.AreFatal);

            Assert.AreEqual(4, findings.Count);
            Assert.IsTrue(findings[0] is ParameterValueIsMissingFactum);
            Assert.IsTrue(findings[1] is ParameterValueIsMissingFactum);
            Assert.IsTrue(findings[2] is ParameterValueIsNeverUsedFactum);
            Assert.IsTrue(findings[3] is ParameterIsMutatedSeveralTimesFactum);

            var pvim1 = findings[0] as ParameterValueIsMissingFactum;
            Assert.AreEqual("[a1.a2, a1.b1, a1]", pvim1.Bottle.ToString());
            Assert.AreEqual("a1.b1.b12", pvim1.Param.ToString());
            Assert.AreEqual("a1.b1", pvim1.Nodes.Single().ToString());

            var pvim2 = findings[1] as ParameterValueIsMissingFactum;
            Assert.AreEqual("[a1.a2, a1.b1, a1]", pvim2.Bottle.ToString());
            Assert.AreEqual("a1.b1.b14", pvim2.Param.ToString());
            Assert.AreEqual("a1.b1", pvim2.Nodes.Single().ToString());

            var pvinuf = findings[2] as ParameterValueIsNeverUsedFactum;
            Assert.AreEqual("[a1.a2, a1.b1, a1]", pvinuf.Bottle.ToString());
            Assert.AreEqual("a1.a2.a22", pvinuf.Param.ToString());

            var pimstf = findings[3] as ParameterIsMutatedSeveralTimesFactum;
            Assert.AreEqual("[a1.a2, a1.b1, a1]", pimstf.Bottle.ToString());
            Assert.AreEqual("a1.a11", pimstf.Param.ToString());
            AssertHelper.SequenceIsomorphic(new[]{"a1", "a1.a2"}, pimstf.Nodes.Select(n => n.ToString()));
        }

        [Test]
        [ExpectedException(typeof(FatalFindingsException))]
        public void TestEvalFailDivisionByZero()
        {
            var cc = new CocaContext(new ColaNode("a1"){Script = "2 / 0"});

            try
            {
                cc.Eval();
            }
            catch (FatalFindingsException ffe)
            {
                Assert.IsTrue(ffe.Findings.AreFatal);
                Assert.AreEqual(1, ffe.Findings.Count);
                Assert.IsTrue(ffe.Findings[0] is ScriptIsErroneousFactum);

                var esf = ffe.Findings[0] as ScriptIsErroneousFactum;
                Assert.AreEqual("[a1]", esf.Bottle.ToString());
                Assert.AreEqual("a1", esf.Node.ToString());
                var e = esf.Exception;
                var lightCode = e.SourceCode.RecalculateSourceCode();
                var lightSpan = e.ErrorSpan.RecalculateErrorSpan(e.SourceCode);
                Assert.AreEqual(ElfExceptionType.DivisionByZero, e.Type);
                Assert.AreEqual("2 / 0", lightCode);
                Assert.AreEqual(Span.FromLength(2, 1), lightSpan);
                Assert.AreEqual("/", lightCode.Substring(lightSpan));
                Assert.AreEqual("(/ 2 0)", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Host", e.ElfNode.FullTPath.Substring(0, 9));
                Assert.AreEqual("/f:Main/b/0:/", e.ElfNode.FullTPath.Substring(42));

                throw;
            }
        }

        [Test]
        public void TestEvalSuccess()
        {
            // construction test coca
            var a2 = new ColaNode("a2");
            var b1 = new ColaNode("b1");
            var a1 = new ColaNode("a1", a2, b1);
            a1.Script = "a11 = a1.a2.a22 + 2; a11 = a11;";
            a2.Script = "a11 = a1.a2.a22 + 2; a1.a11 = a11;";
            b1.Script = "a1.a2.a22 = b12 * b14";
            a2.ParamValues.Add(new Parameter("a22"), 22);
            b1.ParamValues.Add(new Parameter("b12"), 12);
            b1.ParamValues.Add(new Parameter("b14"), 14);
            var cc = new CocaContext(a1);

            // checking out the findings
            var findings = cc.InspectCompositionAndParameters();
            Assert.IsFalse(findings.AreFatal);
            Assert.AreEqual(2, findings.Count);
            Assert.IsTrue(findings[0] is ParameterValueIsNeverUsedFactum);
            Assert.IsTrue(findings[1] is ParameterIsMutatedSeveralTimesFactum);
            var pvinuf = findings[0] as ParameterValueIsNeverUsedFactum;
            Assert.AreEqual("[a1.a2, a1.b1, a1]", pvinuf.Bottle.ToString());
            Assert.AreEqual("a1.a2.a22", pvinuf.Param.ToString());
            var pimstf = findings[1] as ParameterIsMutatedSeveralTimesFactum;
            Assert.AreEqual("[a1.a2, a1.b1, a1]", pimstf.Bottle.ToString());
            Assert.AreEqual("a1.a11", pimstf.Param.ToString());
            AssertHelper.SequenceIsomorphic(new[] { "a1", "a1.a2" }, pimstf.Nodes.Select(n => n.ToString()));

            // ensuring that values and execution plan are fine
            Assert.AreEqual("[a1.a2.a22 = 22, a1.b1.b12 = 12, a1.b1.b14 = 14]", cc.ParamValues.ToString());
            Assert.AreEqual("a1.b1, a1, a1.a2", cc.ExecutionPlan.StringJoin());

            // run the calculations and check their results
            var cs = cc.Eval();
            Assert.AreEqual("[* = [a1.a2.a22 = 168], + = [a1.a11 = 170, a1.a2.a11 = 170], - = <empty>, " +
                "base = [a1.a2.a22 = 22, a1.b1.b12 = 12, a1.b1.b14 = 14]]", cs.ToString());

            // ensure that only after being accepted the results get saved to the coca
            Assert.AreEqual("[a1.a2.a22 = 22, a1.b1.b12 = 12, a1.b1.b14 = 14]", cc.ParamValues.ToString());
            cs.Accept();
            Assert.AreEqual("[a1.a2.a22 = 168, a1.a2.a11 = 170, " +
                "a1.b1.b12 = 12, a1.b1.b14 = 14, a1.a11 = 170]", cc.ParamValues.ToString());
        }
    }
}