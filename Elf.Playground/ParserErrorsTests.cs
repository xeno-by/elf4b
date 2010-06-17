using Elf.Exceptions;
using Elf.Exceptions.Parser;
using Elf.Helpers;
using Elf.Syntax.AstBuilders;
using NUnit.Framework;

namespace Elf.Playground
{
    [TestFixture]
    public class ParserErrorsTests
    {
        [Test]
        [ExpectedException(typeof(SyntaxErrorException))]
        public void TestPrematureEndOfScript()
        {
            var elf = @"def Script rtimpl ToyScript def Fun (a) ret a && 2; end end blargh";

            try
            {
                new ElfAstBuilder(elf).BuildAst();
            }
            // todo. this should be a separate type of exception or we should diversify the ElfExceptionType.SyntaxError
            // todo. into say PrematureEndOfScript, LexerError, ParserError
            catch (SyntaxErrorException e)
            {
                Assert.AreEqual(ElfExceptionType.PrematureEndOfScript, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual(Span.FromLength(60, 0), e.ErrorSpan);
                Assert.AreEqual(null, e.AntlrNode);
                Assert.AreEqual(null, e.ElfNode);

                Assert.AreEqual(
                    "Error parsing script:\r\n" +
                    "1: def Script rtimpl ToyScript def Fun (a) ret a && 2; end end [ERROR>>>]blargh\r\n\r\n" +
                    "Reason: recognition exception 'PrematureEndOfScriptException' occurred at " +
                    "1:60 (index is 60). ANTLR reported: premature end of script (expected end of file right after the SCRIPT token).",
                    e.Message);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(SyntaxErrorException))]
        public void TestLexerError()
        {
            var elf = @"def Script rtimpl ToyScript def Fun (a) ret a & 2; end end";

            try
            {
                new ElfAstBuilder(elf).BuildAst();
            }
            catch (SyntaxErrorException e)
            {
                // todo. perform thorough analysis of RecognitionExceptions and optimize error reporting
                // e.g. in this very case we'd better report user the missing token as well

                Assert.AreEqual(ElfExceptionType.SyntaxError, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual(Span.FromLength(47, 0), e.ErrorSpan);
                Assert.AreEqual(null, e.AntlrNode);
                Assert.AreEqual(null, e.ElfNode);

                Assert.AreEqual(
                    "Error parsing script:\r\n" +
                    "1: def Script rtimpl ToyScript def Fun (a) ret a &[ERROR>>>] 2; end end\r\n\r\n" +
                    "Reason: recognition exception 'MismatchedTokenException(32!=38)' occurred at " + 
                    "1:47 (index is 47). ANTLR reported: line 1:47 mismatched character ' ' expecting '&'.",
                    e.Message);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(SyntaxErrorException))]
        public void TestParserError()
        {
            var elf = @"def Script ToyScript def Fun (a) ret a && 2; end end";

            try
            {
                new ElfAstBuilder(elf).BuildAst();
            }
            catch(SyntaxErrorException e)
            {
                // todo. perform thorough analysis of RecognitionExceptions and optimize error reporting
                // e.g. in this very case we'd better report user the missing token instead of mismatched one

                Assert.AreEqual(ElfExceptionType.SyntaxError, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("ToyScript", elf.Substring(e.ErrorSpan));
                Assert.AreEqual(null, e.AntlrNode);
                Assert.AreEqual(null, e.ElfNode);

                Assert.AreEqual(
                    "Error parsing script:\r\n" + 
                    "1: def Script [ERROR>>> ToyScript] def Fun (a) ret a && 2; end end\r\n\r\n" + 
                    "Reason: recognition exception 'MissingTokenException(at ToyScript)' occurred at " +
                    "1:11 (index is 4). ANTLR reported: line 1:11 missing RTIMPL at 'ToyScript'.",
                    e.Message);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(SemanticErrorException))]
        public void TestInvalidAssignmentLhs()
        {
            var elf = @"def Script rtimpl ToyScript def Fun (a) a + 2 = a; end end";

            try
            {
                new ElfAstBuilder(elf).BuildAst();
            }
            catch (SemanticErrorException e)
            {
                Assert.AreEqual(ElfExceptionType.InvalidAssignmentLhs, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("=", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("(= (+ a 2) a)", e.AntlrNode.ToStringTree());
                Assert.AreEqual(null, e.ElfNode);

                Assert.AreEqual(
                    "Error parsing script:\r\n" +
                    "1: def Script rtimpl ToyScript def Fun (a) a + 2 [ERROR>>> =] a; end end\r\n\r\n" +
                    "Reason: semantic error 'InvalidAssignmentLhs' occurred when parsing '(= (+ a 2) a)'.",
                    e.Message);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(SemanticErrorException))]
        public void TestLoopholesAreNowDisallowed()
        {
            var elf = @"def Script rtimpl ToyScript def Fun (a) a = ?(5, ?); end end";

            try
            {
                new ElfAstBuilder(elf).BuildAst();
            }
            catch (SemanticErrorException e)
            {
                Assert.AreEqual(ElfExceptionType.LoopholesAreNowDisallowed, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("?", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("?", e.AntlrNode.ToStringTree());
                Assert.AreEqual(null, e.ElfNode);

                Assert.AreEqual(
                    "Error parsing script:\r\n" +
                    "1: def Script rtimpl ToyScript def Fun (a) a = ?(5, [ERROR>>> ?]); end end\r\n\r\n" +
                    "Reason: semantic error 'LoopholesAreNowDisallowed' occurred when parsing '?'.",
                    e.Message);

                throw;
            }
        }
    }
}
