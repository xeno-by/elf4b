using System;
using System.Reflection;
using DataVault.Core.Api;
using Esath.Data;
using Esath.Data.Core;
using Esath.Eval.Ver3;
using Esath.Eval.Ver3.Core;
using Esath.Playground.Helpers;
using NUnit.Framework;
using System.Linq;
using Version=Esath.Eval.Ver3.Core.Version;

namespace Esath.Playground
{
    [TestFixture]
    public class VaultCompilerTests
    {
        [Test]
        public void TestBoilerplateAndStructure()
        {
            using (var scenario = "Esath.Playground.TestBoilerplateAndStructure.scenario".ExtractAndOpenVault())
            {
                var cache = new VaultCompiler(scenario).GetCompiledSync();
                Assert.IsNotNull(cache);

                var root = cache.RequestInstance();

                Func<ICustomAttributeProvider, Version> versionAttr = t =>
                    ((VersionAttribute)t.GetCustomAttributes(typeof(VersionAttribute), false)[0]).Version;
                Func<ICustomAttributeProvider, UInt64> revisionAttr = t =>
                    ((RevisionAttribute)t.GetCustomAttributes(typeof(RevisionAttribute), false)[0]).Revision;
                Func<ICustomAttributeProvider, VPath> vpathAttr = t =>
                    ((VPathAttribute)t.GetCustomAttributes(typeof(VPathAttribute), false)[0]).VPath;

                // the scenario itself
                var version = new Version(scenario.Id, scenario.Revision);
                Assert.AreEqual(version, root.Version);
                Assert.AreEqual(version, versionAttr(root.GetType()));

                // regular nodes
                VPath someVPath = @"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0";
                var someNode = root.Child(someVPath);
                Assert.AreEqual(root, someNode.Root);
                Assert.AreEqual("Root.Scenario.Характеристики Объекта Оценки.Определение постоянных расходов_rev0_seq0", someNode.Name);
                Assert.AreEqual("Root.Scenario.Характеристики Объекта Оценки.Определение постоянных расходов_rev0_seq0", someNode.GetType().FullName);
                Assert.AreEqual(someVPath, someNode.VPath);
                Assert.AreEqual(someVPath, vpathAttr(someNode.GetType()));
                Assert.AreEqual(scenario.Revision, revisionAttr(someNode.GetType()));

                // structure (reflection)
                var gend = someNode.GetType().GetProperties()
                    .Where(p => p.DeclaringType == someNode.GetType())
                    .Where(p => (p.GetGetMethod().Attributes & MethodAttributes.NewSlot) != 0);

                // children structure (reflection)
                var childrenReflection = gend.Where(p => typeof(ICompiledNode).IsAssignableFrom(p.PropertyType)).OrderBy(p => p.Name);
                Assert.AreEqual(5, childrenReflection.Count());
                Assert.AreEqual("Налог на землю", childrenReflection.ElementAt(0).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\0c142ad3_c339_499c_8b2d_6dd0361ce4bd", vpathAttr(childrenReflection.ElementAt(0)).ToString());
                Assert.AreEqual(scenario.Revision, revisionAttr(childrenReflection.ElementAt(0).PropertyType));
                Assert.AreEqual("Налог на недвижимость", childrenReflection.ElementAt(1).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\cbb2e893_052c_4856_9c17_fe946fe6f0e1", vpathAttr(childrenReflection.ElementAt(1)).ToString());
                Assert.AreEqual(scenario.Revision, revisionAttr(childrenReflection.ElementAt(1).PropertyType));
                Assert.AreEqual("Расходы на страхование", childrenReflection.ElementAt(2).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\9d5200aa_7e4a_41fe_a2ea_2eb2baa01815", vpathAttr(childrenReflection.ElementAt(2)).ToString());
                Assert.AreEqual(scenario.Revision, revisionAttr(childrenReflection.ElementAt(2).PropertyType));
                Assert.AreEqual("Таблица", childrenReflection.ElementAt(3).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\cb6f782e_65ef_4c11_a4b8_8756eaac9df5", vpathAttr(childrenReflection.ElementAt(3)).ToString());
                Assert.AreEqual(scenario.Revision, revisionAttr(childrenReflection.ElementAt(3).PropertyType));
                Assert.AreEqual("Таблица2", childrenReflection.ElementAt(4).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\f6dd5992_3ce2_42e9_a48c_e2b8677286f5", vpathAttr(childrenReflection.ElementAt(4)).ToString());
                Assert.AreEqual(scenario.Revision, revisionAttr(childrenReflection.ElementAt(4).PropertyType));

                // children structure (runtime)
                var childrenRuntime = someNode.Children.OrderBy(c => c.Name);
                Assert.AreEqual(5, childrenRuntime.Count());
                Assert.AreEqual("Root.Scenario.Характеристики Объекта Оценки.Определение постоянных расходов.Налог на землю_rev0_seq0", childrenRuntime.ElementAt(0).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\0c142ad3_c339_499c_8b2d_6dd0361ce4bd", childrenRuntime.ElementAt(0).VPath.ToString());
                Assert.AreEqual(someNode, childrenRuntime.ElementAt(0).Parent);
                Assert.AreEqual(root, childrenRuntime.ElementAt(0).Root);
                Assert.AreEqual(someNode.Child(childrenRuntime.ElementAt(0).VPath), childrenRuntime.ElementAt(0));
                Assert.AreEqual("Root.Scenario.Характеристики Объекта Оценки.Определение постоянных расходов.Налог на недвижимость_rev0_seq0", childrenRuntime.ElementAt(1).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\cbb2e893_052c_4856_9c17_fe946fe6f0e1", childrenRuntime.ElementAt(1).VPath.ToString());
                Assert.AreEqual(someNode, childrenRuntime.ElementAt(1).Parent);
                Assert.AreEqual(root, childrenRuntime.ElementAt(1).Root);
                Assert.AreEqual(someNode.Child(childrenRuntime.ElementAt(1).VPath), childrenRuntime.ElementAt(1));
                Assert.AreEqual("Root.Scenario.Характеристики Объекта Оценки.Определение постоянных расходов.Расходы на страхование_rev0_seq0", childrenRuntime.ElementAt(2).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\9d5200aa_7e4a_41fe_a2ea_2eb2baa01815", childrenRuntime.ElementAt(2).VPath.ToString());
                Assert.AreEqual(someNode, childrenRuntime.ElementAt(2).Parent);
                Assert.AreEqual(root, childrenRuntime.ElementAt(2).Root);
                Assert.AreEqual(someNode.Child(childrenRuntime.ElementAt(2).VPath), childrenRuntime.ElementAt(2));
                Assert.AreEqual("Root.Scenario.Характеристики Объекта Оценки.Определение постоянных расходов.Таблица_rev0_seq0", childrenRuntime.ElementAt(3).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\cb6f782e_65ef_4c11_a4b8_8756eaac9df5", childrenRuntime.ElementAt(3).VPath.ToString());
                Assert.AreEqual(someNode, childrenRuntime.ElementAt(3).Parent);
                Assert.AreEqual(root, childrenRuntime.ElementAt(3).Root);
                Assert.AreEqual(someNode.Child(childrenRuntime.ElementAt(3).VPath), childrenRuntime.ElementAt(3));
                Assert.AreEqual("Root.Scenario.Характеристики Объекта Оценки.Определение постоянных расходов.Таблица2_rev0_seq0", childrenRuntime.ElementAt(4).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\f6dd5992_3ce2_42e9_a48c_e2b8677286f5", childrenRuntime.ElementAt(4).VPath.ToString());
                Assert.AreEqual(someNode, childrenRuntime.ElementAt(4).Parent);
                Assert.AreEqual(root, childrenRuntime.ElementAt(4).Root);
                Assert.AreEqual(someNode.Child(childrenRuntime.ElementAt(4).VPath), childrenRuntime.ElementAt(4));

                // properties structure (reflection)
                var propsReflection = gend.Where(p => typeof(IEsathObject).IsAssignableFrom(p.PropertyType)).OrderBy(p => p.Name);
                Assert.AreEqual(2, propsReflection.Count());
                Assert.AreEqual("Годовые постоянные операционные расходы долларов", propsReflection.ElementAt(0).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\_formulaDeclarations\d4fd6639_4d10_4949_b69b_07f597a7fe9a", vpathAttr(propsReflection.ElementAt(0)).ToString());
                Assert.AreEqual("Прочие расходы", propsReflection.ElementAt(1).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\_sourceValueDeclarations\4fc36c2b_069e_4398_afbd_86cc6f09893d", vpathAttr(propsReflection.ElementAt(1)).ToString());

                // properties structure (runtime)
                var propsRuntime = someNode.Properties.OrderBy(p => p.Name);
                Assert.AreEqual(2, propsRuntime.Count());
                Assert.AreEqual("Годовые постоянные операционные расходы долларов", propsRuntime.ElementAt(0).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\_formulaDeclarations\d4fd6639_4d10_4949_b69b_07f597a7fe9a", propsRuntime.ElementAt(0).VPath.ToString());
                Assert.AreEqual(someNode, propsRuntime.ElementAt(0).Parent);
                Assert.AreEqual(someNode.Property(propsRuntime.ElementAt(0).VPath), propsRuntime.ElementAt(0));
                Assert.AreEqual("Прочие расходы", propsRuntime.ElementAt(1).Name);
                Assert.AreEqual(@"\Scenario\Particular\eff8210a_3b43_4b23_845c_b916b88a4ea0\_sourceValueDeclarations\4fc36c2b_069e_4398_afbd_86cc6f09893d", propsRuntime.ElementAt(1).VPath.ToString());
                Assert.AreEqual(someNode, propsRuntime.ElementAt(1).Parent);
                Assert.AreEqual(someNode.Property(propsRuntime.ElementAt(1).VPath), propsRuntime.ElementAt(1));

                // properties eval (reflection)
                Assert.AreEqual(new EsathCurrency(1337), propsReflection.ElementAt(0).GetValue(someNode, null));
                Assert.AreEqual(new EsathNumber(0), propsReflection.ElementAt(1).GetValue(someNode, null));

                // properties eval (runtime)
                Assert.AreEqual(new EsathCurrency(1337), propsRuntime.ElementAt(0).Eval());
                Assert.AreEqual(new EsathNumber(0), propsRuntime.ElementAt(1).Eval());
            }
        }

        [Test]
        public void TestSourceValueDeclarations()
        {
            using (var scenario = "Esath.Playground.TestSourceValueDeclarations.scenario".ExtractAndOpenVault())
            {
                using (var repository = "Esath.Playground.TestSourceValueDeclarations.repository".ExtractAndOpenVault())
                {
                    var cache = new VaultCompiler(scenario).GetCompiledSync();
                    Assert.IsNotNull(cache);

                    Func<ICompiledScenario, EsathString> stringSvd = root => (EsathString)root.Eval(@"\Scenario\Particular\9d427392_9775_45ab_a42a_03e2966a583d\_sourceValueDeclarations\7daa5549_49b4_4ed1_a513_4a1f9a7f81eb");
                    Func<ICompiledScenario, EsathText> textSvd = root => (EsathText)root.Eval(@"\Scenario\Particular\9d427392_9775_45ab_a42a_03e2966a583d\_sourceValueDeclarations\5cfda7b6_819f_4856_9b0a_c6bfdbfdab6c");
                    Func<ICompiledScenario, EsathNumber> numberSvd = root => (EsathNumber)root.Eval(@"\Scenario\Particular\9d427392_9775_45ab_a42a_03e2966a583d\_sourceValueDeclarations\cca58c07_3b11_4cdc_84ff_4dcb752f75de");
                    Func<ICompiledScenario, EsathPercent> percentSvd = root => (EsathPercent)root.Eval(@"\Scenario\Particular\9d427392_9775_45ab_a42a_03e2966a583d\_sourceValueDeclarations\672f7b9f_47a1_47d0_9f29_a2fd58a2ed3b");
                    Func<ICompiledScenario, EsathDateTime> dateTimeSvd = root => (EsathDateTime)root.Eval(@"\Scenario\Particular\9d427392_9775_45ab_a42a_03e2966a583d\_sourceValueDeclarations\5a6241a6_13ed_413f_a33a_fb3b5d700df1");
                    Func<ICompiledScenario, EsathCurrency> currencySvd = root => (EsathCurrency)root.Eval(@"\Scenario\Particular\9d427392_9775_45ab_a42a_03e2966a583d\_sourceValueDeclarations\08d25308_b260_4693_bb03_70fb666aa631");

                    Action<ICompiledScenario> assertDesignTime = root =>
                    {
                        Assert.AreEqual(new EsathString("Строка (design)"), stringSvd(root));
                        Assert.AreEqual(new EsathText("Текст (design)"), textSvd(root));
                        Assert.AreEqual(new EsathNumber(0), numberSvd(root));
                        Assert.AreEqual(new EsathPercent(0), percentSvd(root));
                        Assert.AreEqual(new EsathDateTime(new DateTime(2001, 1, 1)), dateTimeSvd(root));
                        Assert.AreEqual(new EsathCurrency(0), currencySvd(root));
                    };

                    Action<ICompiledScenario> assertRuntime = root =>
                    {
                        Assert.AreEqual(new EsathString("Строка (runtime)"), stringSvd(root));
                        Assert.AreEqual(new EsathText("Текст (runtime)"), textSvd(root));
                        Assert.AreEqual(new EsathNumber(-1), numberSvd(root));
                        Assert.AreEqual(new EsathPercent(-1), percentSvd(root));
                        Assert.AreEqual(new EsathDateTime(new DateTime(2009, 4, 14)), dateTimeSvd(root));
                        Assert.AreEqual(new EsathCurrency(-1), currencySvd(root));
                    };

                    var designTimeScenario = cache.RequestInstance();
                    assertDesignTime(designTimeScenario);
                    designTimeScenario.Repository = repository;
                    assertRuntime(designTimeScenario);

                    var runtimeScenario = cache.RequestInstance(repository);
                    assertRuntime(runtimeScenario);
                    runtimeScenario.Repository = null;
                    assertDesignTime(runtimeScenario);
                }
            }
        }

        [Test]
        public void TestFormulae()
        {
            using (var scenario = "Esath.Playground.TestFormulae.scenario".ExtractAndOpenVault())
            {
                var cache = new VaultCompiler(scenario).GetCompiledSync();
                Assert.IsNotNull(cache);

                var root = cache.RequestInstance();
                const string constsvd = @"\Scenario\Common\68650fab_818b_4e22_b575_ed2c088a85b7\_sourceValueDeclarations\";
                const string constflae = @"\Scenario\Common\68650fab_818b_4e22_b575_ed2c088a85b7\_formulaDeclarations\";
                const string testsvd = @"\Scenario\Common\62f0bf38_fd59_4ca6_b57d_5318bf59a82d\_sourceValueDeclarations\";
                const string testflae = @"\Scenario\Common\62f0bf38_fd59_4ca6_b57d_5318bf59a82d\_formulaDeclarations\";

                var now_byr = (EsathCurrency)root.Eval(testsvd + "6fad9d9f_6c3a_422f_afa4_177efdbb5ecd");
                var now_usd = (EsathCurrency)root.Eval(testflae + "5c68ed97_1147_4de2_83bd_9e23d6c5cbe0");
                var now_byr_spelled_out = (EsathString)root.Eval(testflae + "8c4148b5_f882_4be2_b23d_5bc0a69fa95d");
                var now_usd_spelled_out = (EsathString)root.Eval(testflae + "853686ac_939b_4614_bb21_09506fbf2bca");
                var byr_usd_inflation = (EsathPercent)root.Eval(constsvd + "b875c2d8_0de7_4f72_84da_befc552c5be1");
                var lose_roundtrip = (EsathPercent)root.Eval(constflae + "9d54df12_4208_4339_86d3_c5b67db3b634");
                var lose_percentage = (EsathPercent)root.Eval(testflae + "cc58fd87_1bdc_4d82_b699_58e9131d3752");
                var year_byr_byr = (EsathCurrency)root.Eval(testflae + "8e9963d8_1ba2_4931_90c4_50ca568c3b9d");
                var year_byr_usd = (EsathCurrency)root.Eval(testflae + "f769be84_122b_4244_bc8c_408b3f332fbd");
                var year_usd_byr = (EsathCurrency)root.Eval(testflae + "edc35fd9_37f0_4fc0_bdac_5abbdcc5379e");
                var year_usd_usd = (EsathCurrency)root.Eval(testflae + "dad4df96_d32a_4c34_957b_8fa723dc7b0f");

                // numbers below might slightly (+/- 10^-9) differ from those in Tiller cuz of different eval method (ver1 vs ver3)

                Assert.AreEqual(new EsathCurrency(1337000), now_byr);
                Assert.AreEqual(new EsathCurrency(469.12280701754383), now_usd);
                Assert.AreEqual(new EsathString("один миллион триста тридцать семь тысяч"), now_byr_spelled_out);
                Assert.AreEqual(new EsathString("четыреста шестьдесят девять"), now_usd_spelled_out);
                Assert.AreEqual(new EsathPercent(15), byr_usd_inflation);
                Assert.AreEqual(new EsathPercent(2.1505376344086002), lose_roundtrip);
                Assert.AreEqual(new EsathPercent(11.607142857142838), lose_percentage);
                Assert.AreEqual(new EsathCurrency(1671250), year_byr_byr);
                Assert.AreEqual(new EsathCurrency(509.92), year_byr_usd);
                Assert.AreEqual(new EsathCurrency(1685810.07), year_usd_byr);
                Assert.AreEqual(new EsathCurrency(525.42), year_usd_usd);
            }
        }

        // todo. async eval3 tests
        // 1) Check that flushcaches work as advertised: calc, runtime, calc, make sure that there's no cached shiz (manually)
        // 2) Check all the scenarios that cause partial regeneration of vault assembly
    }
}
