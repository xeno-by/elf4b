using System;
using System.Globalization;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Elf.Interactive;
using NUnit.Framework;

namespace Esath.Playground
{
    [TestFixture]
    public class EsathTests
    {
        [TestFixtureSetUp]
        public void GlobalSetup()
        {
            AppDomain.CurrentDomain.Load("Esath.Data");
        }

        [Test]
        public void MainSuccessScenario()
        {
            // the stuff below tests everything new introduced to Elf:
            // * Deserialization from string with type tokens
            // * Varargs invocations support
            // * Mixed-in Esath data types

            var ei = new ElfInteractive();
            var res = ei.Eval("'[[currency]]2' ^ '[[percent]]200' * Срзнач('[[number]]5', '[[number]]7')");

            Assert.IsTrue(res.Retval is double);
            Assert.AreEqual(24, res.Retval);
        }

        [Test]
        public void TestEqualityOperators()
        {
            var ei = new ElfInteractive();
            var n2eqn7 = (bool)ei.Eval("'[[number]]2' == '[[number]]7'").Retval;
            var n2eqn2 = (bool)ei.Eval("'[[number]]2' == '[[number]]2'").Retval;
            var n2eqc2 = (bool)ei.Eval("'[[number]]2' == '[[currency]]2'").Retval;
            var c2eqc2 = (bool)ei.Eval("'[[currency]]2' == '[[currency]]2'").Retval;

            Assert.IsFalse(n2eqn7);
            Assert.IsTrue(n2eqn2);
            Assert.IsTrue(n2eqc2);
            Assert.IsTrue(c2eqc2);
        }

        [Test]
        public void TestArithmeticStuff()
        {
            var ei = new ElfInteractive();
            var n2pn7mc9 = (double)ei.Eval("'[[number]]2' + '[[number]]7' * '[[currency]]9'").Retval;
            Assert.AreEqual(n2pn7mc9, 65);
        }

        [Test]
        public void TestPercentStuff()
        {
            var ei = new ElfInteractive();
            var _100p5pct = (double)ei.Eval("'[[number]]100' + '[[percent]]5'").Retval;
            var _5pctp100 = (double)ei.Eval("'[[percent]]5' + '[[number]]100'").Retval;
            var _100m5pct = (double)ei.Eval("'[[number]]100' * '[[percent]]5'").Retval;
            var _5pctm100 = (double)ei.Eval("'[[percent]]5' * '[[number]]100'").Retval;
            var _5pctp10pct = (double)ei.Eval("'[[percent]]5' + '[[percent]]15'").Retval;
            Assert.AreEqual(105, _100p5pct);
            Assert.AreEqual(105, _5pctp100);
            Assert.AreEqual(5, _100m5pct);
            Assert.AreEqual(5, _5pctm100);
            Assert.AreEqual(0.20, _5pctp10pct);
        }

        [Test]
        public void TestLibraryFx()
        {
            var ei = new ElfInteractive();

            var _15round = (double)ei.Eval("Округл('[[number]]1.5')").Retval;
            var _05round = (double)ei.Eval("Округл('[[number]]0.5')").Retval;
            var _049round = (double)ei.Eval("Округл('[[number]]0.49')").Retval;
            Assert.AreEqual(2, _15round);
            Assert.AreEqual(1, _05round);
            Assert.AreEqual(0, _049round);

            var _todaytomorrow = (double)ei.Eval(
                "ДнейМеждуДатами('[[datetime]]23.02.2008', '[[datetime]]24.02.2008')").Retval;
            var _01012009to_01012010 = (double)ei.Eval(
                "ДнейМеждуДатами('[[datetime]]01.01.2009', '[[datetime]]01.01.2010')").Retval;
            Assert.AreEqual(1, _todaytomorrow);
            Assert.AreEqual(365, _01012009to_01012010);
        }

        [Test]
        public void TestPmt()
        {
            var ei = new ElfInteractive();
            var _65_10757 = (double)ei.Eval("ПЛТ('[[percent]]6', '[[number]]10.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]0')").Retval;
            var _61_42224 = (double)ei.Eval("ПЛТ('[[percent]]6', '[[number]]10.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]1')").Retval;
            var _136_17748 = (double)ei.Eval("ПЛТ('[[percent]]6', '[[number]]5.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]0')").Retval;
            var _24_34718 = (double)ei.Eval("ПЛТ('[[percent]]6', '[[number]]10.00', '[[currency]]100.00', '[[currency]]-500.00', '[[number]]0')").Retval;
            var _70_00000 = (double)ei.Eval("ПЛТ('[[percent]]0', '[[number]]10.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]1')").Retval;
            var _140_00000 = (double)ei.Eval("ПЛТ('[[percent]]0', '[[number]]5.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]0')").Retval;
            var _40_00000 = (double)ei.Eval("ПЛТ('[[percent]]0', '[[number]]10.00', '[[currency]]100.00', '[[currency]]-500.00', '[[number]]0')").Retval;

            Assert.AreEqual("65.10757", _65_10757.ToString("00.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("61.42224", _61_42224.ToString("00.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("136.17748", _136_17748.ToString("000.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("24.34718", _24_34718.ToString("00.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("70.00000", _70_00000.ToString("00.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("140.00000", _140_00000.ToString("000.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("40.00000", _40_00000.ToString("00.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestPv()
        {
            var ei = new ElfInteractive();
            var _1751_21480 = (double)ei.Eval("ПС('[[percent]]6', '[[number]]10.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]0')").Retval;
            var _1839_53584 = (double)ei.Eval("ПС('[[percent]]6', '[[number]]10.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]1')").Retval;
            var _1216_10184 = (double)ei.Eval("ПС('[[percent]]6', '[[number]]5.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]0')").Retval;
            var _m456_81132 = (double)ei.Eval("ПС('[[percent]]6', '[[number]]10.00', '[[currency]]100.00', '[[currency]]-500.00', '[[number]]0')").Retval;
            var _2500_00000 = (double)ei.Eval("ПС('[[percent]]0', '[[number]]10.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]1')").Retval;
            var _1500_00000 = (double)ei.Eval("ПС('[[percent]]0', '[[number]]5.00', '[[currency]]-200.00', '[[currency]]-500.00', '[[number]]0')").Retval;
            var _m500_00000 = (double)ei.Eval("ПС('[[percent]]0', '[[number]]10.00', '[[currency]]100.00', '[[currency]]-500.00', '[[number]]0')").Retval;

            Assert.AreEqual("1751.21480", _1751_21480.ToString("0000.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("1839.53584", _1839_53584.ToString("0000.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("1216.10184", _1216_10184.ToString("0000.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("-456.81132", _m456_81132.ToString("000.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("2500.00000", _2500_00000.ToString("0000.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("1500.00000", _1500_00000.ToString("0000.00000", CultureInfo.InvariantCulture));
            Assert.AreEqual("-500.00000", _m500_00000.ToString("000.00000", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestParametersHaveInvalidTypes()
        {
            AssertDontSuit("ПЛТ('[[percent]]6', '[[number]]10.00', '[[currency]]-200.00', '[[number]]-500.00', '[[number]]0')");
            AssertDontSuit("'[[percent]]6' > '[[number]]10.00'");
            AssertDontSuit("'[[number]]6' < '[[number]]10.00'");
            AssertDontSuit("'[[currency]]6' >= '[[number]]10.00'");
            AssertDontSuit("'[[currency]]6' <= '[[currency]]10.00'");
//            AssertDontSuit("'[[number]]6' - '[[currency]]10.00'");
            AssertDontSuit("'[[number]]6' ^ '[[currency]]10.00'");
        }

        private void AssertDontSuit(String elf)
        {
            try
            {
                var ei = new ElfInteractive();
                ei.Eval(elf);
                Assert.Fail();
            }
            catch (ErroneousScriptRuntimeException uere)
            {
                Assert.AreEqual(ElfExceptionType.OperandsDontSuitMethod, uere.Type);
            }
        }

        [Test]
        public void TestRoundings()
        {
            var ei = new ElfInteractive();

            var _2_2 = (double)ei.Eval("Округл2('[[number]]2.15', '[[number]]1')").Retval;
            var _2_1 = (double)ei.Eval("Округл2('[[number]]2.149', '[[number]]1')").Retval;
            var _m1_48 = (double)ei.Eval("Округл2('[[number]]-1.475', '[[number]]2')").Retval;
            var _20 = (double)ei.Eval("Округл2('[[number]]21.5', '[[number]]-1')").Retval;

            var _4 = (double)ei.Eval("Округлвверх2('[[number]]3.2', '[[number]]0')").Retval;
            var _77 = (double)ei.Eval("Округлвверх2('[[number]]76.9', '[[number]]0')").Retval;
            var _3_142 = (double)ei.Eval("Округлвверх2('[[number]]3.14159', '[[number]]3')").Retval;
            var _m3_2 = (double)ei.Eval("Округлвверх2('[[number]]-3.14159', '[[number]]1')").Retval;
            var _31500 = (double)ei.Eval("Округлвверх2('[[number]]31415.92654', '[[number]]-2')").Retval;

            var _3 = (double)ei.Eval("Округлвниз2('[[number]]3.2', '[[number]]0')").Retval;
            var _76 = (double)ei.Eval("Округлвниз2('[[number]]76.9', '[[number]]0')").Retval;
            var _3_141 = (double)ei.Eval("Округлвниз2('[[number]]3.14159', '[[number]]3')").Retval;
            var _m3_1 = (double)ei.Eval("Округлвниз2('[[number]]-3.14159', '[[number]]1')").Retval;
            var _31400 = (double)ei.Eval("Округлвниз2('[[number]]31415.92654', '[[number]]-2')").Retval;

            Assert.AreEqual("2.2", _2_2.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("2.1", _2_1.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("-1.48", _m1_48.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("20", _20.ToString(CultureInfo.InvariantCulture));

            Assert.AreEqual("4", _4.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("77", _77.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("3.142", _3_142.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("-3.2", _m3_2.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("31500", _31500.ToString(CultureInfo.InvariantCulture));

            Assert.AreEqual("3", _3.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("76", _76.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("3.141", _3_141.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("-3.1", _m3_1.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("31400", _31400.ToString(CultureInfo.InvariantCulture));
        }
    }
}