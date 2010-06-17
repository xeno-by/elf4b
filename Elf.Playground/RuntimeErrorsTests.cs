using System;
using System.Text;
using Elf.Core;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Elf.Helpers;
using NUnit.Framework;

namespace Elf.Playground
{
    [TestFixture]
    public class RuntimeErrorsTests
    {
        private VirtualMachine _vm;
        private StringBuilder _toyLog;

        [SetUp]
        public void SetUp()
        {
            _vm = new VirtualMachine();
            _toyLog = new StringBuilder();
            _vm.Context["ToyLog"] = _toyLog;
        }

        [Test]
        [ExpectedException(typeof(UnexpectedRtimplRuntimeException))]
        public void TestRtimplLogicsError_Resolver()
        {
            var elf = @"def Script rtimpl ToyScript def Fun () ret a. && 2; end end";

            try
            {
                _vm.Load(elf);

                var ep = _vm.CreateEntryPoint("Script", "Fun");
                ep.RunTillEnd();
            }
            // back then all exceptions thrown by rtimpl classes were automatically
            // promoted to ErroneousScriptRuntimeException.
            // however, I decided to change it to unexpected one and here's why
            //
            // Point of erroneous script exceptions is that they're prognozable, i.e.
            // there's a fixed amount of reasons that can cause them, however unlike those
            // exceptions thrown by rtimpl can vary and be even caused by runtime programmer
            //
            // That's why every rtimpl should manually take care of exception handling and
            // explicitly throw ErroneousScriptRuntimeExceptions to indicate an error
            // generated solely by execution of a flawed script. This might even involve
            // introducing additional ExceptionTypes.
            catch (UnexpectedRtimplRuntimeException e)
            {
                Assert.IsTrue(e.InnerException is FormatException);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ErroneousScriptRuntimeException))]
        public void TestRtimplLogicsError_RtimplCall()
        {
            var elf = @"def Script rtimpl ToyScript def Fun () ret 2 / 0; end end";

            try
            {
                _vm.Load(elf);

                var ep = _vm.CreateEntryPoint("Script", "Fun");
                ep.RunTillEnd();
            }
            catch (ErroneousScriptRuntimeException e)
            {
                Assert.AreEqual(ElfExceptionType.DivisionByZero, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("/", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("(/ 2 0)", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Script/f:Fun/b/0:ret//", e.ElfNode.FullTPath);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ErroneousScriptRuntimeException))]
        public void TestUsingVoidValueAfterInvoke()
        {
            var elf = @"def Script rtimpl ToyScript def Void () end def Fun () ret Void(); end end";

            try
            {
                _vm.Load(elf);

                var ep = _vm.CreateEntryPoint("Script", "Fun");
                ep.RunTillEnd();
            }
            catch (ErroneousScriptRuntimeException e)
            {
                Assert.AreEqual(ElfExceptionType.UsingVoidValue, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("Void", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("(CALL Void ARGS)", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Script/f:Fun/b/0:ret/Void", e.ElfNode.FullTPath);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ErroneousScriptRuntimeException))]
        public void TestUsingVoidValueFromVariable()
        {
            var elf = @"def Script rtimpl ToyScript def Fun () var a; ret a; end end";

            try
            {
                _vm.Load(elf);

                var ep = _vm.CreateEntryPoint("Script", "Fun");
                ep.RunTillEnd();
            }
            catch (ErroneousScriptRuntimeException e)
            {
                Assert.AreEqual(ElfExceptionType.UsingVoidValue, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("a", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("a", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Script/f:Fun/b/1:ret/v:a", e.ElfNode.FullTPath);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ErroneousScriptRuntimeException))]
        public void TestConditionNotBoolean()
        {
            var elf = @"def Script rtimpl ToyScript def Fun () if 2 then ret; end end end";

            try
            {
                _vm.Load(elf);

                var ep = _vm.CreateEntryPoint("Script", "Fun");
                ep.RunTillEnd();
            }
            catch (ErroneousScriptRuntimeException e)
            {
                Assert.AreEqual(ElfExceptionType.ConditionNotBoolean, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("if", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("(if 2 (BLOCK ret))", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Script/f:Fun/b/0:if", e.ElfNode.FullTPath);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ErroneousScriptRuntimeException))]
        public void TestCannotResolveInvocation()
        {
            var elf = @"def Script rtimpl ToyScript def Fun () ret CannotResolve(); end end";

            try
            {
                _vm.Load(elf);

                var ep = _vm.CreateEntryPoint("Script", "Fun");
                ep.RunTillEnd();
            }
            catch (ErroneousScriptRuntimeException e)
            {
                Assert.AreEqual(ElfExceptionType.CannotResolveInvocation, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("CannotResolve", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("(CALL CannotResolve ARGS)", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Script/f:Fun/b/0:ret/CannotResolve", e.ElfNode.FullTPath);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ErroneousScriptRuntimeException))]
        public void TestCannotResolveVariable()
        {
            var elf = @"def Script rtimpl ToyScript def Fun () ret CannotResolve; end end";

            try
            {
                _vm.Load(elf);

                var ep = _vm.CreateEntryPoint("Script", "Fun");
                ep.RunTillEnd();
            }
            catch (ErroneousScriptRuntimeException e)
            {
                Assert.AreEqual(ElfExceptionType.CannotResolveVariable, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("CannotResolve", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("CannotResolve", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Script/f:Fun/b/0:ret/v:CannotResolve", e.ElfNode.FullTPath);

                throw;
            }
        }

        [Test]
        [ExpectedException(typeof(ErroneousScriptRuntimeException))]
        public void TestDuplicateVariableDeclared()
        {
            var elf = @"def Script rtimpl ToyScript def Fun () var Qualified.1; end end";

            try
            {
                _vm.Load(elf);

                var ep = _vm.CreateEntryPoint("Script", "Fun");
                ep.RunTillEnd();
            }
            catch(ErroneousScriptRuntimeException e)
            {
                Assert.AreEqual(ElfExceptionType.DuplicateVariableName, e.Type);
                Assert.AreEqual(elf, e.SourceCode);
                Assert.AreEqual("var", elf.Substring(e.ErrorSpan));
                Assert.AreEqual("(var Qualified.1)", e.AntlrNode.ToStringTree());
                Assert.AreEqual("/s/c:Script/f:Fun/b/0:var:Qualified.1", e.ElfNode.FullTPath);

                throw;
            }
        }
    }
}