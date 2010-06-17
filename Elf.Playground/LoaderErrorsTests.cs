using Elf.Core;
using Elf.Exceptions;
using Elf.Exceptions.Loader;
using Elf.Helpers;
using NUnit.Framework;

namespace Elf.Playground
{
    [TestFixture]
    public class LoaderErrorsTests
    {
        [Test]
        [ExpectedException(typeof(ErroneousScriptLoaderException))]
        public void TestDuplicateClassLoaded()
        {
            var elf = @"def ToyScript rtimpl ToyScript end";

            try
            {
                new VirtualMachine().Load(elf);
            }
            catch (ErroneousScriptLoaderException e)
            {
                Assert.AreEqual(ElfExceptionType.DuplicateClassLoaded, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("ToyScript", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("(CLASS (DECL ToyScript (rtimpl ToyScript)))", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:ToyScript", e.ElfNode.FullTPath);

                Assert.AreEqual(
                    "Error loading script:\r\n" +
                    "[/s ->\r\n" +
                    "1: def ToyScript rtimpl ToyScript\r\n" +
                    "2: end]\r\n\r\n" +
                    "Could not load entity '/s/c:ToyScript'. Reason: 'DuplicateClassLoaded'.",
                    e.Message);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ErroneousScriptLoaderException))]
        public void TestClassRtimplNotFound()
        {
            var elf = @"def Script rtimpl ToyScript2 end";

            try
            {
                new VirtualMachine().Load(elf);
            }
            catch (ErroneousScriptLoaderException e)
            {
                Assert.AreEqual(ElfExceptionType.ClassRtimplNotFound, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("Script", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("(CLASS (DECL Script (rtimpl ToyScript2)))", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Script", e.ElfNode.FullTPath);

                Assert.AreEqual(
                    "Error loading script:\r\n" +
                    "[/s ->\r\n" +
                    "1: def Script rtimpl ToyScript2\r\n" +
                    "2: end]\r\n\r\n" +
                    "Could not load entity '/s/c:Script'. Reason: 'ClassRtimplNotFound'.",
                    e.Message);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ErroneousScriptLoaderException))]
        public void TestDuplicateFuncLoaded()
        {
            var elf = @"def Script rtimpl ToyScript def Fun() end def Fun() end end";

            try
            {
                new VirtualMachine().Load(elf);
            }
            catch (ErroneousScriptLoaderException e)
            {
                Assert.AreEqual(ElfExceptionType.DuplicateFuncLoaded, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual(Span.FromLength(46, 3), e.ErrorSpan);
                Assert.AreEqual("Fun", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("(FUNC (DECL Fun ARGS) BLOCK)", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Script/f:Fun", e.ElfNode.FullTPath);

                Assert.AreEqual(
                    "Error loading script:\r\n" +
                    "[/s ->\r\n" +
                    "1: def Script rtimpl ToyScript\r\n" +
                    "2:   def Fun()\r\n" +
                    "3: \r\n" +
                    "4:   end\r\n" +
                    "5: \r\n" +
                    "6:   def Fun()\r\n" +
                    "7: \r\n" +
                    "8:   end\r\n" +
                    "9: end]\r\n\r\n" +
                    "Could not load entity '/s/c:Script/f:Fun'. Reason: 'DuplicateFuncLoaded'.",
                    e.Message);

                throw;
            }
        }
    }
}