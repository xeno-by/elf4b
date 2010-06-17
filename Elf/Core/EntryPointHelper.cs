using System;
using System.Linq;
using Elf.Core.Reflection;
using Elf.Core.Runtime;
using Elf.Core.Runtime.Impl;
using Elf.Core.TypeSystem;
using Elf.Exceptions.Runtime;
using Elf.Helpers;

namespace Elf.Core
{
    public static class EntryPointHelper
    {
        public static IEntryPoint CreateEntryPoint(this VirtualMachine vm, 
            String className, String methodName, params IElfObject[] args)
        {
            try
            {
                var @class = vm.Classes.Single(c => c.Name == className);
                var method = @class.Methods.OfType<NativeMethod>().Single(m => m.Name == methodName);
                return vm.CreateEntryPoint(method, args);
            }
            catch (Exception e)
            {
                if (e is UnexpectedElfRuntimeException) throw;
                throw new UnexpectedElfRuntimeException(vm, String.Format(
                    "Fatal error parsing entry point '{0}:{1}({2})'. " + "Reason: '{3}'",
                    className, methodName, args.StringJoin(), e.Message), e);
            }
        }

        public static IEntryPoint CreateEntryPoint(this VirtualMachine vm, 
            NativeMethod method, params IElfObject[] args)
        {
            try
            {
                var ctor = method.DeclaringType.Ctors.Single(ctor1 => ctor1.Rtimpl.GetParameters().Length == 0);
                var @this = new ElfScriptDefinedClassInstance(method.DeclaringType, Activator.CreateInstance(ctor.Rtimpl.DeclaringType));
                @this.Bind(vm);

                var ep = new DefaultEntryPoint(method, @this, args);
                ep.Bind(vm);

                return ep;
            }
            catch (Exception e)
            {
                if (e is UnexpectedElfRuntimeException) throw;
                throw new UnexpectedElfRuntimeException(vm, String.Format(
                    "Fatal error parsing entry point '{0}:{1}({2})'. " + "Reason: '{3}'",
                    method.DeclaringType.Name, method.Name, args.StringJoin(), e.Message), e);
            }
        }
    }
}