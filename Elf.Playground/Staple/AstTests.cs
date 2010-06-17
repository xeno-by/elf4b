using System;
using System.Collections.Generic;
using System.Linq;
using Elf.Helpers;
using Elf.Playground.Helpers;
using Elf.Syntax.Ast;
using Elf.Syntax.AstBuilders;
using NUnit.Framework;

namespace Elf.Playground.Staple
{
    [TestFixture]
    public class AstTests
    {
        [Test]
        public void ContentTest()
        {
            var elfCode = ResourceHelper.ReadFromResource("Elf.Playground.Staple.Universal.elf");
            var ast = new ElfAstBuilder(elfCode).BuildAst();
            AssertHelper.AreEqualFromResource("Elf.Playground.Staple.AstTests.Content", ast.Content, @"d:\elf-astcontent");
        }

        [Test]
        public void AstBuilderTest()
        {
            var elfCode = ResourceHelper.ReadFromResource("Elf.Playground.Staple.Universal.elf");
            var allPaths = GetAllRootToLeafPaths(new ElfAstBuilder(elfCode).BuildAst()).StringJoin(Environment.NewLine);
            AssertHelper.AreEqualFromResource("Elf.Playground.Staple.AstTests.AstBuilder", allPaths, @"d:\elf-astbuilder");
        }

        [Test]
        public void LoopholeTest()
        {
            var elfCode = "def Script rtimpl ToyScript def Fun (a) ? = ?(2, ?); end end";
            var allPaths = GetAllRootToLeafPaths(new ElfAstBuilder(elfCode).BuildAstAllowLoopholes()).StringJoin(Environment.NewLine);
            AssertHelper.AreEqualFromResource("Elf.Playground.Staple.AstTests.Loopholes", allPaths, @"d:\elf-loopholes");
        }

        private List<String> GetAllRootToLeafPaths(AstNode root)
        {
            var ln = new List<String>();
            SeekRootToLeafPathsRecursive(root, ln);
            return ln;
        }

        private void SeekRootToLeafPathsRecursive(AstNode node, List<String> ln)
        {
            if (node.Children.Count() == 0) ln.Add(node.ShortTPath);
            node.Children.ForEach(child => SeekRootToLeafPathsRecursive(child, ln));
        }
    }
}