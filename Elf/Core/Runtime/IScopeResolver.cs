using System;
using Elf.Core.TypeSystem;
using Elf.Core.Runtime.Contexts;

namespace Elf.Core.Runtime
{
    public interface IScopeResolver
    {
        void EnterScope(RuntimeContext ctx, IElfObject @this);
        void Declare(RuntimeContext ctx, String name, IElfObject @this);
        void LeaveScope(RuntimeContext ctx, IElfObject @this);

        IElfObject Get(RuntimeContext ctx, String name, IElfObject @this);
        void Set(RuntimeContext ctx, String name, IElfObject @this, IElfObject value);
    }
}