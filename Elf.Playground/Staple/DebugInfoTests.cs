using System;
using System.Reflection;
using Antlr.Runtime.Tree;
using Elf.Core.Runtime.Impl.Compiler;
using Elf.Helpers;
using Elf.Playground.Helpers;
using Elf.Syntax.Ast.Defs;
using Elf.Syntax.AstBuilders;
using NUnit.Framework;
using System.Linq;

namespace Elf.Playground.Staple
{
    [TestFixture]
    public class DebugInfoTests
    {
        [Test]
        public void TestTokenPositions()
        {
            var elfCode = ResourceHelper.ReadFromResource("Elf.Playground.Staple.Universal.elf");
            var antlrAst = (CommonTree)typeof(ElfAstBuilder).GetMethod("AcquireAntlrAst", 
                BindingFlags.Instance | BindingFlags.NonPublic).Invoke(new ElfAstBuilder(elfCode), null);

            Func<String, int, int, String> frag = (s, l, c) => {
                var lines = s.SelectLines();
                if (l < 1 || lines.Length <= l - 1) return "???";
                var line = lines[l - 1];
                if (c < 0 || line.Length <= c) return "???";
                return line.Substring(c, Math.Min(3, line.Length - c)); };

            var nodes = antlrAst.Flatten(node => node.Children.Cast<CommonTree>());
            var pewpew = nodes.Select(node => String.Format(
                "{0}:{1} (frg: {2}) -> {3}",
                node.Line, node.CharPositionInLine, 
                frag(elfCode, node.Line, node.CharPositionInLine),
                node.ToStringTree()));
            pewpew = (elfCode.InjectLineNumbers1() + Environment.NewLine).SelectLines().Concat(pewpew);

            var pepew = pewpew.StringJoin(Environment.NewLine);
            AssertHelper.AreEqualFromResource("Elf.Playground.Staple.DebugInfoTests.TokenPositions", pepew, @"d:\elf-tokenpos");
        }

        [Test]
        public void TestAntlrNodeBindings()
        {
            var elfCode = ResourceHelper.ReadFromResource("Elf.Playground.Staple.Universal.elf");
            var elfAst = new ElfAstBuilder(elfCode).BuildAst();

            var elfNodes = elfAst.Flatten(node => node.Children);
            elfNodes.ForEach(elfNode => Assert.IsNotNull(elfNode.AntlrNode, elfNode.ShortTPath));

            var pewpew = elfNodes.Select(node => String.Format(
                "{0} -> {1}", node.FullTPath, node.AntlrNode.ToStringTree()));

            var pepew = pewpew.StringJoin(Environment.NewLine);
            AssertHelper.AreEqualFromResource("Elf.Playground.Staple.DebugInfoTests.AntlrNodeBindings", pepew, @"d:\elf-antlrbind");
        }

        [Test]
        public void TestEviBindings()
        {
            var elfCode = ResourceHelper.ReadFromResource("Elf.Playground.Staple.Universal.elf");
            var elfAst = new ElfAstBuilder(elfCode).BuildAst();

            var evis = new DefaultElfCompiler().Compile((FuncDef)elfAst.Children.ElementAt(0).Children.ElementAt(1));
            evis.ForEach((evi, i) => Assert.IsNotNull(evi.AstNode, i + ": " + evi));

            var pewpew = evis.Select(evi => String.Format("{0} -> {1}", evi, evi.AstNode.FullTPath));
            var pepew = pewpew.StringJoin(Environment.NewLine);
            AssertHelper.AreEqualFromResource("Elf.Playground.Staple.DebugInfoTests.EviBindings", pepew, @"d:\elf-elfastbind");
        }
    }
}