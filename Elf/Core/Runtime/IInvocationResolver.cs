using System;
using Elf.Core.TypeSystem;
using Elf.Core.Runtime.Contexts;

namespace Elf.Core.Runtime
{
    public interface IInvocationResolver
    {
        void PrepareCallContext(RuntimeContext ctx, String name, IElfObject @this, params IElfObject[] args);
    }
}