using System;
using Elf.Core.TypeSystem;
using Elf.Core.Reflection;
using Elf.Core.Runtime.Contexts;
using System.Linq;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Elf.Helpers;

namespace Elf.Core.Runtime.Impl
{
    public class DefaultInvocationResolver : IInvocationResolver
    {
        public static ElfMethod Resolve(VirtualMachine vm, String name, ElfClass thisClass, params ElfClass[] argClasses)
        {
            var firstArgClass = argClasses.Length == 0 ? new ElfClass(null, "aux", typeof(object)) : argClasses[0];

            Func<ElfMethod, int> isStaticOrCtor = m =>
            {
                if (!(m is ClrMethod)) return 0;
                if (m.DeclaringType != null && m.DeclaringType.Ctors.Contains((ClrMethod)m)) return 1;
                if (((ClrMethod)m).Rtimpl.IsStatic) return 1;
                return 0;
            };

            Func<ElfMethod, bool> argcOk = m =>
            {
                var sigArgs = m.DeclaringType == thisClass ? argClasses.Length : argClasses.Length + isStaticOrCtor(m) - 1;
                return m.IsVarargs ? m.Argc <= sigArgs : m.Argc == sigArgs;
            };

            var methodsOfThis = thisClass.Methods.Where(m => m.Name == name && argcOk(m)).ToArray();
            var ctorsOfName = vm.Classes.Where(c => c.Name == name).Select(c => c.Ctors.Where(m => argcOk(m))).Flatten().ToArray();
            var methodsOfFirstArg = firstArgClass.Methods.Where(m => m.Name == name && argcOk(m)).ToArray();
            var helperMethods = vm.HelperMethods.Where(m => m.Name == name && argcOk(m)).ToArray();

            if (methodsOfThis.Length != 0)
            {
                return methodsOfThis.Single();
            }
            else if (ctorsOfName.Length != 0)
            {
                return ctorsOfName.Single();
            }
            else if (methodsOfFirstArg.Length != 0)
            {
                return methodsOfFirstArg.Single();
            }
            else if (helperMethods.Length != 0)
            {
                return helperMethods.Single();
            }
            else
            {
                return null;
            }
        }

        public virtual void PrepareCallContext(RuntimeContext ctx, string name, IElfObject @this, params IElfObject[] args)
        {
            var resolved = Resolve(@this.VM, name, @this.Type, args.Select(arg => arg.Type).ToArray());
            if (resolved == null)
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.CannotResolveInvocation, ctx.VM);
            }
            else
            {
                if (resolved is NativeMethod)
                {
                    // this class, elf method
                    if (resolved.Argc == args.Length)
                    {
                        ctx.CallStack.Push(new NativeCallContext(
                            (NativeMethod)resolved, @this, args));
                    }
                    // other class, elf method as well
                    else
                    {
                        ctx.CallStack.Push(new NativeCallContext(
                            (NativeMethod)resolved, args[0], args.Skip(1).ToArray()));
                    }
                }
                else
                {
                    var mi = ((ClrMethod)resolved).Rtimpl;
                    if (mi.DeclaringType != @this.Type.ClrType)
                    {
                        if (mi.IsConstructor)
                        {
                            // other class, clr ctor
                            ctx.PendingClrCall = new ClrCallContext(
                                 (ClrMethod)resolved, null, args);
                        }
                        else if (mi.IsStatic)
                        {
                            // other class, static clr method
                            ctx.PendingClrCall = new ClrCallContext(
                                 (ClrMethod)resolved, args[0], args);
                        }
                        else
                        {
                            // other class, instance clr method
                            ctx.PendingClrCall = new ClrCallContext(
                                (ClrMethod)resolved, args[0], args.Skip(1).ToArray());
                        }
                    }
                    else
                    {
                        if (mi.IsStatic)
                        {
                            // this class, static clr method
                            ctx.PendingClrCall = new ClrCallContext(
                                 (ClrMethod)resolved, @this, @this.AsArray().Concat(args).ToArray());
                        }
                        else
                        {
                            // this class, instance clr method
                            ctx.PendingClrCall = new ClrCallContext(
                                (ClrMethod)resolved, @this, args);
                        }
                    }
                }
            }
        }
    }
}