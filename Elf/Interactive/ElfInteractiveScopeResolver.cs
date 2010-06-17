using System;
using System.Collections.Generic;
using Elf.Core.Runtime.Contexts;
using Elf.Core.Runtime.Impl;
using Elf.Core.TypeSystem;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;

namespace Elf.Interactive
{
    public class ElfInteractiveScopeResolver : DefaultScopeResolver
    {
        public override void Declare(RuntimeContext ctx, string name, IElfObject @this)
        {
            var pbag = (PropertyBag)ctx.VM.Context["iactx"];
            if (pbag.ContainsKey(name))
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
            var pbag = (PropertyBag)ctx.VM.Context["iactx"];
            if (pbag.ContainsKey(name))
            {
                return @this.VM.Marshaller.Unmarshal(pbag[name]);
            }

            return base.Get(ctx, name, @this);
        }

        public override void Set(RuntimeContext ctx, string name, IElfObject value, IElfObject @this)
        {
            var pbag = (PropertyBag)ctx.VM.Context["iactx"];
            if (pbag.ContainsKey(name))
            {
                pbag[name] = @this.VM.Marshaller.Marshal(value);
                return;
            }

            try
            {
                base.Set(ctx, name, value, @this);
            }
            catch (ErroneousScriptRuntimeException e)
            {
                if (e.Type == ElfExceptionType.CannotResolveVariable)
                {
                    pbag.Add(name, @this.VM.Marshaller.Marshal(value));
                }
                else
                {
                    throw;
                }
            }
        }
    }
}