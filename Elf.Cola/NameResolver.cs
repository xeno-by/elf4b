using Elf.Cola.Parameters;
using Elf.Core.Runtime.Contexts;
using Elf.Core.Runtime.Impl;
using Elf.Core.TypeSystem;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;

namespace Elf.Cola
{
    public class NameResolver : DefaultScopeResolver
    {
        public override void Declare(RuntimeContext ctx, string name, IElfObject @this)
        {
            if (name.Contains("."))
            {
                // todo. is this behavior really unacceptable?
                throw new ErroneousScriptRuntimeException(ElfExceptionType.BadVariableName, ctx.VM);
            }
            else
            {
                var node = (ColaNode)ctx.VM.Context["cc_node"];
                var qualifiedName = name.Contains(".") ? name : node.TPath + "." + name;

                var pbag = (ParametersValues)ctx.VM.Context["cc_values"];
                if (pbag.ContainsKey(qualifiedName))
                {
                    throw new ErroneousScriptRuntimeException(ElfExceptionType.DuplicateVariableName, ctx.VM);
                }
                else
                {
                    base.Declare(ctx, name, @this);
                }
            }
        }

        public override IElfObject Get(RuntimeContext ctx, string name, IElfObject @this)
        {
            var node = (ColaNode)ctx.VM.Context["cc_node"];
            var qualifiedName = name.Contains(".") ? name : node.TPath + "." + name;

            var pbag = (ParametersValues)ctx.VM.Context["cc_values"];
            if (pbag.ContainsKey(qualifiedName))
            {
                return @this.VM.Marshaller.Unmarshal(pbag[qualifiedName]);
            }

            return base.Get(ctx, name, @this);
        }

        public override void Set(RuntimeContext ctx, string name, IElfObject value, IElfObject @this)
        {
            var node = (ColaNode)ctx.VM.Context["cc_node"];
            var qualifiedName = name.Contains(".") ? name : node.TPath + "." + name;

            var pbag = (ParametersValues)ctx.VM.Context["cc_values"];
            if (pbag.ContainsKey(qualifiedName))
            {
                pbag[qualifiedName] = @this.VM.Marshaller.Marshal(value);
                return;
            }

            try
            {
                base.Set(ctx, name, value, @this);
            }
            catch (ErroneousScriptRuntimeException)
            {
                pbag.Add(new Parameter(qualifiedName), @this.VM.Marshaller.Marshal(value));
                return;
            }
        }
    }
}