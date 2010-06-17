using System;
using Elf.Core.Runtime.Contexts;
using Elf.Core.Runtime.Impl;
using Elf.Core.TypeSystem;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;

namespace Elf.Playground.ToyScript
{
    public class ToyScriptScopeResolver : DefaultScopeResolver
    {
        public override void Declare(RuntimeContext ctx, string name, IElfObject @this)
        {
            if (name.Contains("."))
            {
                throw new ErroneousScriptRuntimeException(ElfExceptionType.DuplicateVariableName, ctx.VM);
            }
            else
            {
                base.Declare(ctx, name, @this);
            }
        }

        public override IElfObject Get(RuntimeContext ctx, string name, IElfObject @this)
        {
            var script = (ToyScript)@this.VM.Marshaller.Marshal(@this);
            if (name.Contains("."))
            {
                // release code should feature type check here
                return @this.VM.Marshaller.Unmarshal((double)script[name]);
            }

            return base.Get(ctx, name, @this);
        }

        public override void Set(RuntimeContext ctx, string name, IElfObject value, IElfObject @this)
        {
            var script = (ToyScript)@this.VM.Marshaller.Marshal(@this);
            if (script[name] != null)
            {
                // release code should feature type check here
                script[name] = (double)@this.VM.Marshaller.Marshal(value);
                return;
            }

            base.Set(ctx, name, value, @this);
        }
    }
}