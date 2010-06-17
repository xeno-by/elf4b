using Elf.Core.TypeSystem;
using Elf.Core.Runtime.Contexts;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;

namespace Elf.Core.Runtime.Impl
{
    public class DefaultScopeResolver : IScopeResolver
    {
        public virtual void EnterScope(RuntimeContext ctx, IElfObject @this)
        {
            ctx.CallStack.Peek().Scopes.Push(new Scope());
        }

        public virtual void Declare(RuntimeContext ctx, string name, IElfObject @this)
        {
            var current = ctx.CallStack.Peek();
            foreach(var scope in current.Scopes)
            {
                if (scope.ContainsKey(name))
                {
                    throw new ErroneousScriptRuntimeException(ElfExceptionType.DuplicateVariableName, @this.VM);
                }
            }

            current.Scopes.Peek().Add(name, new ElfVoid());
        }

        public virtual void LeaveScope(RuntimeContext ctx, IElfObject @this)
        {
            ctx.CallStack.Peek().Scopes.Pop();
        }

        public virtual IElfObject Get(RuntimeContext ctx, string name, IElfObject @this)
        {
            var current = ctx.CallStack.Peek();
            foreach(var scope in current.Scopes)
            {
                if (scope.ContainsKey(name))
                {
                    return scope[name];
                }
            }

            throw new ErroneousScriptRuntimeException(ElfExceptionType.CannotResolveVariable, @this.VM);
        }

        public virtual void Set(RuntimeContext ctx, string name, IElfObject value, IElfObject @this)
        {
            var current = ctx.CallStack.Peek();
            foreach (var scope in current.Scopes)
            {
                if (scope.ContainsKey(name))
                {
                    scope[name] = value;
                    return;
                }
            }

            throw new ErroneousScriptRuntimeException(ElfExceptionType.CannotResolveVariable, @this.VM);
        }
    }
}